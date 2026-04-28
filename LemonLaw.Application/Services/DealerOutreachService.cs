using LemonLaw.Application.Interfaces;
using LemonLaw.Application.Repositories;
using LemonLaw.Core.Entities;
using LemonLaw.Core.Enums;
using LemonLaw.Shared.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LemonLaw.Application.Services;

public class DealerOutreachService(
    IApplicationRepository applicationRepository,
    IGenericRepository<DealerOutreach> outreachRepository,
    IGenericRepository<DealerResponse> responseRepository,
    IGenericRepository<CaseEvent> eventRepository,
    IGenericRepository<Correspondence> correspondenceRepository,
    IGenericRepository<ApplicationDocument> documentRepository,
    IEmailService emailService,
    IConfiguration configuration,
    ILogger<DealerOutreachService> logger) : IDealerOutreachService
{
    public async Task<CommonResponseDto<bool>> CreateOutreachAsync(CreateOutreachDto dto, string staffId)
    {
        try
        {
            var tokenCleartext = GenerateSecureToken();
            var tokenHash = HashToken(tokenCleartext);

            var outreach = new DealerOutreach
            {
                ApplicationId = dto.ApplicationId,
                DealerEmail = dto.DealerEmail,
                DealerName = dto.DealerName,
                OutreachType = OutreachType.INITIAL,
                SentByStaffId = staffId,
                TemplateUsed = dto.TemplateCode,
                TokenHash = tokenHash,
                TokenCreatedAt = DateTime.UtcNow,
                TokenExpiresAt = DateTime.UtcNow.AddDays(30),
                ResponseDeadline = dto.ResponseDeadline,
                Status = OutreachStatus.PENDING
            };

            await outreachRepository.AddAsync(outreach);

            await eventRepository.AddAsync(new CaseEvent
            {
                ApplicationId = dto.ApplicationId,
                EventType = CaseEventType.DEALER_NOTIFIED,
                ActorType = ActorType.STAFF,
                ActorId = staffId,
                ActorDisplayName = staffId,
                Description = $"Dealer outreach created. Deadline: {dto.ResponseDeadline}."
            });

            await outreachRepository.SaveChangesAsync();

            var application = await applicationRepository.GetWithFullDetailsAsync(dto.ApplicationId);
            var portalLink = $"{configuration["DealerPortal:BaseUrl"]}?token={tokenCleartext}&outreachId={outreach.Id}";
            await emailService.SendFromTemplateAsync(
                outreach.DealerEmail, outreach.DealerName,
                "DEALER_INITIAL_OUTREACH",
                new Dictionary<string, string>
                {
                    ["caseNumber"] = application?.CaseNumber ?? "",
                    ["consumerName"] = application?.Applicant != null
                        ? $"{application.Applicant.FirstName} {application.Applicant.LastName}"
                        : "",
                    ["vehicleYMM"] = application?.Vehicle != null
                        ? $"{application.Vehicle.VehicleYear} {application.Vehicle.VehicleMake} {application.Vehicle.VehicleModel}"
                        : "",
                    ["vin"] = application?.Vehicle?.VIN ?? "",
                    ["dealerName"] = outreach.DealerName ?? "",
                    ["responseDeadline"] = outreach.ResponseDeadline.ToString("MMMM d, yyyy"),
                    ["dealerPortalLink"] = portalLink,
                    ["ocabrContactName"] = "OCABR Lemon Law Program",
                    ["ocabrContactPhone"] = "(617) 973-8787",
                    ["ocabrContactEmail"] = "ocabr@mass.gov"
                });

            // Mark outreach as sent and update status
            outreach.SentAt = DateTime.UtcNow;
            outreach.Status = OutreachStatus.SENT;
            outreachRepository.Update(outreach);

            // Log correspondence record per spec §3.3.6
            await correspondenceRepository.AddAsync(new Correspondence
            {
                ApplicationId = dto.ApplicationId,
                Direction = CorrespondenceDirection.OUTBOUND,
                RecipientType = CorrespondenceRecipientType.DEALER,
                RecipientEmail = outreach.DealerEmail,
                Subject = $"OCABR Lemon Law Notice — Response Required by {outreach.ResponseDeadline:MMMM d, yyyy}",
                TemplateUsed = "DEALER_INITIAL_OUTREACH",
                SentAt = DateTime.UtcNow,
                SentByStaffId = staffId
            });
            await correspondenceRepository.SaveChangesAsync();

            logger.LogInformation("Dealer outreach created for application {Id}", dto.ApplicationId);

            return new CommonResponseDto<bool> { Success = true, Data = true };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating dealer outreach for {Id}", dto.ApplicationId);
            return Fail("Failed to create dealer outreach.");
        }
    }

    public async Task<CommonResponseDto<bool>> SendFollowUpAsync(
        Guid outreachId, SendFollowUpDto dto, string staffId)
    {
        try
        {
            var outreach = await outreachRepository.GetByIdAsync(outreachId);
            if (outreach == null)
                return Fail("Outreach record not found.");

            if (!Enum.TryParse<OutreachType>(dto.OutreachType, out var outreachType))
                return Fail("Invalid outreach type.");

            // Generate a fresh token for the follow-up link per spec §4.1 security model
            var freshToken = GenerateSecureToken();
            var freshHash = HashToken(freshToken);
            outreach.TokenHash = freshHash;
            outreach.TokenCreatedAt = DateTime.UtcNow;
            outreach.TokenExpiresAt = DateTime.UtcNow.AddDays(30);

            switch (outreachType)
            {
                case OutreachType.FOLLOW_UP_1:
                    outreach.FollowUp1SentAt = DateTime.UtcNow;
                    break;
                case OutreachType.FOLLOW_UP_2:
                    outreach.FollowUp2SentAt = DateTime.UtcNow;
                    break;
                case OutreachType.FINAL_NOTICE:
                    outreach.FinalNoticeSentAt = DateTime.UtcNow;
                    break;
            }

            outreach.OutreachType = outreachType;
            outreachRepository.Update(outreach);
            await outreachRepository.SaveChangesAsync();

            if (outreach.ApplicationId == null)
                return new CommonResponseDto<bool> { Success = false, Data = false };

            var followUpApplication = await applicationRepository.GetWithFullDetailsAsync(outreach.ApplicationId.Value);
            // Include the fresh token in the portal link so the dealer can actually access it
            var followUpPortalLink = $"{configuration["DealerPortal:BaseUrl"]}?outreachId={outreach.Id}&token={freshToken}";
            await emailService.SendFromTemplateAsync(
                outreach.DealerEmail, outreach.DealerName,
                $"DEALER_{outreachType}",
                new Dictionary<string, string>
                {
                    ["caseNumber"] = followUpApplication?.CaseNumber ?? "",
                    ["consumerName"] = followUpApplication?.Applicant != null
                        ? $"{followUpApplication.Applicant.FirstName} {followUpApplication.Applicant.LastName}"
                        : "",
                    ["vehicleYMM"] = followUpApplication?.Vehicle != null
                        ? $"{followUpApplication.Vehicle.VehicleYear} {followUpApplication.Vehicle.VehicleMake} {followUpApplication.Vehicle.VehicleModel}"
                        : "",
                    ["vin"] = followUpApplication?.Vehicle?.VIN ?? "",
                    ["dealerName"] = outreach.DealerName ?? "",
                    ["responseDeadline"] = outreach.ResponseDeadline.ToString("MMMM d, yyyy"),
                    ["dealerPortalLink"] = followUpPortalLink,
                    ["ocabrContactName"] = "OCABR Lemon Law Program",
                    ["ocabrContactPhone"] = "(617) 973-8787",
                    ["ocabrContactEmail"] = "ocabr@mass.gov"
                });

            // Log correspondence record per spec §3.3.6
            await correspondenceRepository.AddAsync(new Correspondence
            {
                ApplicationId = outreach.ApplicationId.Value,
                Direction = CorrespondenceDirection.OUTBOUND,
                RecipientType = CorrespondenceRecipientType.DEALER,
                RecipientEmail = outreach.DealerEmail,
                Subject = $"OCABR Lemon Law Follow-Up — Response Required by {outreach.ResponseDeadline:MMMM d, yyyy}",
                TemplateUsed = $"DEALER_{outreachType}",
                SentAt = DateTime.UtcNow,
                SentByStaffId = staffId
            });
            await correspondenceRepository.SaveChangesAsync();

            return new CommonResponseDto<bool> { Success = true, Data = true };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error sending follow-up for outreach {Id}", outreachId);
            return Fail("Failed to send follow-up.");
        }
    }

    public async Task<CommonResponseDto<DealerCaseSummaryDto>> GetCaseSummaryForDealerAsync(Guid outreachId)
    {
        try
        {
            var outreach = await outreachRepository.GetByIdAsync(outreachId);
            if (outreach == null)
                return new CommonResponseDto<DealerCaseSummaryDto>
                {
                    Success = false,
                    Message = "Outreach record not found."
                };

            if (outreach.ApplicationId == null)
                return new CommonResponseDto<DealerCaseSummaryDto>
                {
                    Success = false,
                    Message = "Application not found."
                };

            var application = await applicationRepository.GetWithFullDetailsAsync(outreach.ApplicationId.Value);
            if (application == null)
                return new CommonResponseDto<DealerCaseSummaryDto>
                {
                    Success = false,
                    Message = "Application not found."
                };

            return new CommonResponseDto<DealerCaseSummaryDto>
            {
                Success = true,
                Data = new DealerCaseSummaryDto
                {
                    CaseNumber = application.CaseNumber,
                    ConsumerName = application.Applicant != null
                        ? $"{application.Applicant.FirstName} {application.Applicant.LastName}"
                        : string.Empty,
                    VehicleYMM = application.Vehicle != null
                        ? $"{application.Vehicle.VehicleYear} {application.Vehicle.VehicleMake} {application.Vehicle.VehicleModel}"
                        : string.Empty,
                    VIN = application.Vehicle?.VIN ?? string.Empty,
                    ResponseDeadline = outreach.ResponseDeadline,
                    NarrativeStatement = application.NarrativeStatement ?? string.Empty,
                    Defects = application.Defects.Select(d => new DefectDto
                    {
                        DefectId = d.Id,
                        DefectDescription = d.DefectDescription,
                        DefectCategory = d.DefectCategory,
                        FirstOccurrenceDate = d.FirstOccurrenceDate,
                        IsOngoing = d.IsOngoing,
                        SortOrder = d.SortOrder
                    }).ToList(),
                    RepairAttempts = application.RepairAttempts.Select(r => new RepairAttemptDto
                    {
                        RepairAttemptId = r.Id,
                        RepairDate = r.RepairDate,
                        RepairFacilityName = r.RepairFacilityName,
                        RepairFacilityAddr = r.RepairFacilityAddr,
                        RoNumber = r.RoNumber,
                        MileageAtRepair = r.MileageAtRepair,
                        DefectsAddressed = r.DefectsAddressed,
                        RepairSuccessful = r.RepairSuccessful,
                        DaysOutOfService = r.DaysOutOfService,
                        SortOrder = r.SortOrder
                    }).OrderBy(r => r.SortOrder).ToList()
                }
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting dealer case summary for outreach {Id}", outreachId);
            return new CommonResponseDto<DealerCaseSummaryDto>
            {
                Success = false,
                Message = "Failed to retrieve case summary."
            };
        }
    }

    public async Task<CommonResponseDto<bool>> SubmitDealerResponseAsync(
        Guid outreachId, DealerResponseSubmitDto dto, string ipAddress)
    {
        try
        {
            var outreach = await outreachRepository.GetByIdAsync(outreachId);
            if (outreach == null)
                return Fail("Outreach record not found.");

            if (!Enum.TryParse<DealerPosition>(dto.DealerPosition, out var position))
                return Fail("Invalid dealer position.");

            var response = new DealerResponse
            {
                OutreachId = outreachId,
                ApplicationId = outreach.ApplicationId,
                ResponderName = dto.ResponderName,
                ResponderTitle = dto.ResponderTitle,
                ResponderEmail = dto.ResponderEmail,
                ResponderPhone = dto.ResponderPhone,
                DealerPosition = position,
                ResponseNarrative = dto.ResponseNarrative,
                RepairHistoryNotes = dto.RepairHistoryNotes,
                SettlementOffer = dto.SettlementOffer,
                SettlementDetails = dto.SettlementDetails,
                CertificationAccepted = dto.CertificationAccepted,
                CertifierFullName = dto.CertifierFullName,
                SubmittedAt = DateTime.UtcNow,
                SubmissionIpAddress = ipAddress
            };

            await responseRepository.AddAsync(response);

            outreach.Status = OutreachStatus.RESPONDED;
            outreachRepository.Update(outreach);

            if (outreach.ApplicationId == null)
                return new CommonResponseDto<bool> { Success = false, Data = false };

            await eventRepository.AddAsync(new CaseEvent
            {
                ApplicationId = outreach.ApplicationId.Value,
                EventType = CaseEventType.DEALER_RESPONDED,
                ActorType = ActorType.DEALER,
                ActorDisplayName = dto.ResponderName,
                Description = $"Dealer submitted response. Position: {position}."
            });

            // Log correspondence record
            await correspondenceRepository.AddAsync(new Correspondence
            {
                ApplicationId = outreach.ApplicationId.Value,
                Direction = CorrespondenceDirection.INBOUND,
                RecipientType = CorrespondenceRecipientType.INTERNAL,
                RecipientEmail = "ocabr@mass.gov",
                Subject = $"Dealer Response Received",
                SentAt = DateTime.UtcNow
            });

            await responseRepository.SaveChangesAsync();

            // Auto-transition application status to DEALER_RESPONDED per spec §3.4
            var application = await applicationRepository.GetWithFullDetailsAsync(outreach.ApplicationId.Value);
            if (application != null && application.Status == ApplicationStatus.ACCEPTED)
            {
                application.Status = ApplicationStatus.DEALER_RESPONDED;
                application.LastActivityAt = DateTime.UtcNow;
                applicationRepository.Update(application);
                await applicationRepository.SaveChangesAsync();

                // Notify consumer that dealer has responded per spec §5.1
                if (application.Applicant != null && !string.IsNullOrWhiteSpace(application.Applicant.EmailAddress))
                {
                    try
                    {
                        var portalBaseUrl = configuration["Portal:BaseUrl"] ?? configuration["Cors:PortalOrigin"] ?? "http://localhost:5173";
                        var consumerName = $"{application.Applicant.FirstName} {application.Applicant.LastName}".Trim();
                        // Generate a fresh token for the consumer's portal link
                        var freshToken = GenerateSecureToken();
                        var freshHash = HashToken(freshToken);
                        if (application.Token != null)
                        {
                            application.Token.TokenHash = freshHash;
                            application.Token.ExpiresAt = DateTime.UtcNow.AddYears(2);
                            applicationRepository.Update(application);
                            await applicationRepository.SaveChangesAsync();
                        }
                        var statusLink = $"{portalBaseUrl}/status?caseNumber={Uri.EscapeDataString(application.CaseNumber)}&token={freshToken}";

                        await emailService.SendFromTemplateAsync(
                            application.Applicant.EmailAddress, consumerName,
                            "CONSUMER_DEALER_RESPONDED",
                            new Dictionary<string, string>
                            {
                                ["consumerName"] = consumerName,
                                ["caseNumber"] = application.CaseNumber,
                                ["portalStatusLink"] = statusLink
                            });

                        await correspondenceRepository.AddAsync(new Correspondence
                        {
                            ApplicationId = outreach.ApplicationId.Value,
                            Direction = CorrespondenceDirection.OUTBOUND,
                            RecipientType = CorrespondenceRecipientType.CONSUMER,
                            RecipientEmail = application.Applicant.EmailAddress,
                            Subject = $"Dealer Response Received — Case {application.CaseNumber}",
                            TemplateUsed = "CONSUMER_DEALER_RESPONDED",
                            SentAt = DateTime.UtcNow
                        });
                        await correspondenceRepository.SaveChangesAsync();
                    }
                    catch (Exception emailEx)
                    {
                        logger.LogError(emailEx, "Failed to send dealer-responded notification for application {Id}", outreach.ApplicationId.Value);
                    }
                }
            }

            return new CommonResponseDto<bool> { Success = true, Data = true };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error submitting dealer response for outreach {Id}", outreachId);
            return Fail("Failed to submit dealer response.");
        }
    }

    private static string GenerateSecureToken()
    {
        var bytes = new byte[64];
        System.Security.Cryptography.RandomNumberGenerator.Fill(bytes);
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }

    private static string HashToken(string token)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(token);
        var hash = System.Security.Cryptography.SHA256.HashData(bytes);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    private static CommonResponseDto<bool> Fail(string message) =>
        new() { Success = false, Message = message };

    /// <summary>
    /// Validates a dealer access token against the outreach record per spec §4.1 / §8.1.
    /// </summary>
    public async Task<bool> ValidateDealerTokenAsync(Guid outreachId, string token)
    {
        try
        {
            var tokenHash = HashToken(token);
            var outreach = await outreachRepository.FindOneAsync(o =>
                o.Id == outreachId &&
                o.TokenHash == tokenHash &&
                o.TokenExpiresAt > DateTime.UtcNow);
            return outreach != null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error validating dealer token for outreach {Id}", outreachId);
            return false;
        }
    }

    /// <summary>
    /// Returns documents flagged isVisible_Dealer = true for the dealer portal per spec §7.5.
    /// </summary>
    public async Task<CommonResponseDto<List<DocumentListItemDto>>> GetDealerDocumentsAsync(Guid outreachId)
    {
        try
        {
            var outreach = await outreachRepository.GetByIdAsync(outreachId);
            if (outreach == null || outreach.ApplicationId == null)
                return new CommonResponseDto<List<DocumentListItemDto>>
                {
                    Success = false,
                    Message = "Outreach record not found."
                };

            var docs = await documentRepository.FindAsync(d =>
                d.ApplicationId == outreach.ApplicationId.Value && d.IsVisible_Dealer);

            var dtos = docs.OrderByDescending(d => d.UploadedAt).Select(d => new DocumentListItemDto
            {
                DocumentId = d.Id,
                DocumentType = d.DocumentType.ToString(),
                FileName = d.FileName,
                FileSizeBytes = d.FileSizeBytes,
                Status = d.Status.ToString(),
                UploadedAt = d.UploadedAt,
                UploadedByRole = d.UploadedByRole.ToString()
            }).ToList();

            return new CommonResponseDto<List<DocumentListItemDto>> { Success = true, Data = dtos };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting dealer documents for outreach {Id}", outreachId);
            return new CommonResponseDto<List<DocumentListItemDto>>
            {
                Success = false,
                Message = "Failed to retrieve documents."
            };
        }
    }
}

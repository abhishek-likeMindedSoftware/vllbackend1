using LemonLaw.Application.Interfaces;
using LemonLaw.Application.Repositories;
using LemonLaw.Core.Entities;
using LemonLaw.Core.Enums;
using LemonLaw.Shared.DTOs;
using Microsoft.Extensions.Logging;

using AppEntity = LemonLaw.Core.Entities.Application;

namespace LemonLaw.Application.Services;

public class ApplicationService(
    IApplicationRepository applicationRepository,
    IGenericRepository<CaseEvent> eventRepository,
    IGenericRepository<CaseNote> noteRepository,
    IGenericRepository<Hearing> hearingRepository,
    IGenericRepository<Decision> decisionRepository,
    IGenericRepository<ApplicationDocument> documentRepository,
    IGenericRepository<Applicant> applicantRepository,
    IGenericRepository<Vehicle> vehicleRepository,
    IGenericRepository<Defect> defectRepository,
    IGenericRepository<RepairAttempt> repairRepository,
    IGenericRepository<Expense> expenseRepository,
    ILogger<ApplicationService> logger) : IApplicationService
{
    // ── Create Application ────────────────────────────────────────────────────

    public async Task<CommonResponseDto<CreateApplicationResponseDto>> CreateApplicationAsync(
        CreateApplicationRequestDto dto, string ipAddress)
    {
        try
        {
            var caseNumber = await applicationRepository.GenerateCaseNumberAsync();
            var tokenCleartext = GenerateSecureToken();
            var tokenHash = HashToken(tokenCleartext);

            var application = new AppEntity
            {
                CaseNumber = caseNumber,
                ApplicationType = dto.ApplicationType,
                Status = ApplicationStatus.SUBMITTED,
                SubmittedAt = DateTime.UtcNow,
                LastActivityAt = DateTime.UtcNow,
                SubmissionIpAddress = ipAddress,
                Token = new ApplicationToken
                {
                    TokenType = TokenType.CONSUMER,
                    TokenHash = tokenHash,
                    ExpiresAt = DateTime.UtcNow.AddYears(2)
                }
            };

            await applicationRepository.AddAsync(application);
            await applicationRepository.SaveChangesAsync();

            return new CommonResponseDto<CreateApplicationResponseDto>
            {
                Success = true,
                Message = "Application created.",
                Data = new CreateApplicationResponseDto
                {
                    ApplicationId = application.Id,
                    CaseNumber = caseNumber,
                    AccessToken = tokenCleartext
                }
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating application");
            return new CommonResponseDto<CreateApplicationResponseDto>
            {
                Success = false,
                Message = "Failed to create application.",
                ExceptionMessage = ex.Message
            };
        }
    }

    // ── Save Wizard Step ──────────────────────────────────────────────────────

    public async Task<CommonResponseDto<bool>> SaveStepAsync(
        Guid applicationId, int step, object stepData)
    {
        try
        {
            var application = await applicationRepository.GetWithFullDetailsAsync(applicationId);
            if (application == null)
                return Fail<bool>("Application not found.");

            application.LastActivityAt = DateTime.UtcNow;
            applicationRepository.Update(application);
            await applicationRepository.SaveChangesAsync();

            return new CommonResponseDto<bool> { Success = true, Data = true };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error saving step {Step} for application {Id}", step, applicationId);
            return Fail<bool>("Failed to save step.");
        }
    }

    public async Task<CommonResponseDto<bool>> SaveStep1Async(Guid applicationId, Step1ConsumerInfoDto dto)
    {
        try
        {
            var application = await applicationRepository.GetWithFullDetailsAsync(applicationId);
            if (application == null) return Fail<bool>("Application not found.");

            if (application.Applicant == null)
            {
                application.Applicant = new Applicant { ApplicationId = applicationId };
                await applicantRepository.AddAsync(application.Applicant);
            }

            var a = application.Applicant;
            a.FirstName = dto.FirstName;
            a.MiddleName = dto.MiddleName;
            a.LastName = dto.LastName;
            a.EmailAddress = dto.EmailAddress;
            a.PhoneNumber = dto.PhoneNumber;
            a.PhoneType = dto.PhoneType;
            a.AddressLine1 = dto.AddressLine1;
            a.AddressLine2 = dto.AddressLine2;
            a.City = dto.City;
            a.AddressState = dto.State;
            a.ZipCode = dto.ZipCode;
            a.GooglePlaceId = dto.PlaceId;
            a.PreferredContact = dto.PreferredContactMethod;

            application.LastActivityAt = DateTime.UtcNow;
            applicationRepository.Update(application);
            await applicationRepository.SaveChangesAsync();

            return new CommonResponseDto<bool> { Success = true, Data = true };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error saving step 1 for {Id}", applicationId);
            return Fail<bool>("Failed to save consumer information.");
        }
    }

    public async Task<CommonResponseDto<bool>> SaveStep2Async(Guid applicationId, Step2VehicleInfoDto dto)
    {
        try
        {
            var application = await applicationRepository.GetWithFullDetailsAsync(applicationId);
            if (application == null) return Fail<bool>("Application not found.");

            if (application.Vehicle == null)
            {
                application.Vehicle = new Vehicle { ApplicationId = applicationId };
                await vehicleRepository.AddAsync(application.Vehicle);
            }

            var v = application.Vehicle;
            v.VIN = dto.VIN;
            v.VinConfirmed = dto.VinConfirmed;
            v.VehicleYear = dto.VehicleYear;
            v.VehicleMake = dto.VehicleMake;
            v.VehicleModel = dto.VehicleModel;
            v.VehicleColor = dto.VehicleColor;
            v.LicensePlate = dto.LicensePlate;
            v.LicensePlateState = dto.LicensePlateState;
            v.PurchaseDate = dto.PurchaseDate;
            v.PurchasePrice = dto.PurchasePrice;
            v.MileageAtPurchase = dto.MileageAtPurchase;
            v.CurrentMileage = dto.CurrentMileage;
            v.DealerName = dto.DealerName;
            v.DealerAddressLine1 = dto.DealerAddressLine1;
            v.DealerAddressLine2 = dto.DealerAddressLine2;
            v.DealerCity = dto.DealerCity;
            v.DealerState = dto.DealerState;
            v.DealerZip = dto.DealerZip;
            v.DealerPhone = dto.DealerPhone;
            v.DealerEmail = dto.DealerEmail;
            v.DealerGooglePlaceId = dto.DealerPlaceId;
            v.ManufacturerName = dto.ManufacturerName;
            v.WarrantyType = dto.WarrantyType;
            v.WarrantyStartDate = dto.WarrantyStartDate;
            v.WarrantyExpiryDate = dto.WarrantyExpiryDate;

            application.LastActivityAt = DateTime.UtcNow;
            applicationRepository.Update(application);
            await applicationRepository.SaveChangesAsync();

            return new CommonResponseDto<bool> { Success = true, Data = true };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error saving step 2 for {Id}", applicationId);
            return Fail<bool>("Failed to save vehicle information.");
        }
    }

    public async Task<CommonResponseDto<bool>> SaveStep3Async(Guid applicationId, Step3DefectRepairDto dto)
    {
        try
        {
            var application = await applicationRepository.GetWithFullDetailsAsync(applicationId);
            if (application == null) return Fail<bool>("Application not found.");

            // Replace defects
            foreach (var existing in application.Defects.ToList())
                defectRepository.Delete(existing);

            foreach (var d in dto.Defects)
            {
                await defectRepository.AddAsync(new Defect
                {
                    ApplicationId = applicationId,
                    DefectDescription = d.DefectDescription,
                    DefectCategory = d.DefectCategory,
                    FirstOccurrenceDate = d.FirstOccurrenceDate,
                    IsOngoing = d.IsOngoing,
                    SortOrder = d.SortOrder
                });
            }

            // Replace repair attempts
            foreach (var existing in application.RepairAttempts.ToList())
                repairRepository.Delete(existing);

            foreach (var r in dto.RepairAttempts)
            {
                await repairRepository.AddAsync(new RepairAttempt
                {
                    ApplicationId = applicationId,
                    RepairDate = r.RepairDate,
                    RepairFacilityName = r.RepairFacilityName,
                    RepairFacilityAddr = r.RepairFacilityAddr,
                    RoNumber = r.RoNumber,
                    MileageAtRepair = r.MileageAtRepair,
                    DefectsAddressed = r.DefectsAddressed,
                    RepairSuccessful = r.RepairSuccessful,
                    DaysOutOfService = r.DaysOutOfService,
                    SortOrder = r.SortOrder
                });
            }

            // Replace expenses
            foreach (var existing in application.Expenses.ToList())
                expenseRepository.Delete(existing);

            foreach (var e in dto.Expenses)
            {
                await expenseRepository.AddAsync(new Expense
                {
                    ApplicationId = applicationId,
                    ExpenseType = e.ExpenseType,
                    ExpenseDate = e.ExpenseDate,
                    Amount = e.Amount,
                    Description = e.Description
                });
            }

            application.LastActivityAt = DateTime.UtcNow;
            applicationRepository.Update(application);
            await applicationRepository.SaveChangesAsync();

            return new CommonResponseDto<bool> { Success = true, Data = true };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error saving step 3 for {Id}", applicationId);
            return Fail<bool>("Failed to save defect and repair information.");
        }
    }

    public async Task<CommonResponseDto<bool>> SaveStep4Async(Guid applicationId, Step4NarrativeDto dto)
    {
        try
        {
            var application = await applicationRepository.GetByIdAsync(applicationId);
            if (application == null) return Fail<bool>("Application not found.");

            application.NarrativeStatement = dto.NarrativeStatement;
            application.PriorContactDealer = dto.PriorContactWithDealer;
            application.PriorContactMfr = dto.PriorContactWithMfr;
            application.PriorContactNotes = dto.PriorContactNotes;
            application.DesiredResolution = dto.DesiredResolution;
            application.LastActivityAt = DateTime.UtcNow;

            applicationRepository.Update(application);
            await applicationRepository.SaveChangesAsync();

            return new CommonResponseDto<bool> { Success = true, Data = true };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error saving step 4 for {Id}", applicationId);
            return Fail<bool>("Failed to save narrative statement.");
        }
    }

    // ── Submit Application ────────────────────────────────────────────────────

    public async Task<CommonResponseDto<bool>> SubmitApplicationAsync(
        Guid applicationId, Step6SubmitDto dto, string ipAddress)
    {
        try
        {
            var application = await applicationRepository.GetWithFullDetailsAsync(applicationId);
            if (application == null)
                return Fail<bool>("Application not found.");

            application.CertificationAccepted = dto.CertificationAccepted;
            application.SignatureFullName = dto.SignatureFullName;
            application.SignatureTimestamp = DateTime.UtcNow;
            application.EmailVerified = true;
            application.Status = ApplicationStatus.SUBMITTED;
            application.LastActivityAt = DateTime.UtcNow;

            await eventRepository.AddAsync(new CaseEvent
            {
                ApplicationId = applicationId,
                EventType = CaseEventType.STATUS_CHANGE,
                ActorType = ActorType.CONSUMER,
                ActorDisplayName = "Consumer",
                Description = "Application submitted by consumer.",
                NewValue = ApplicationStatus.SUBMITTED.ToString()
            });

            applicationRepository.Update(application);
            await applicationRepository.SaveChangesAsync();

            return new CommonResponseDto<bool> { Success = true, Data = true, Message = "Application submitted." };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error submitting application {Id}", applicationId);
            return Fail<bool>("Failed to submit application.");
        }
    }

    // ── Full one-shot submission (FILIR pattern) ──────────────────────────────

    public async Task<CommonResponseDto<bool>> SubmitFullApplicationAsync(
        Guid applicationId, FullSubmitDto dto, string ipAddress)
    {
        try
        {
            // Save all steps in a single operation
            await SaveStep1Async(applicationId, dto.ConsumerInfo);
            await SaveStep2Async(applicationId, dto.VehicleInfo);
            await SaveStep3Async(applicationId, dto.DefectsAndRepairs);
            await SaveStep4Async(applicationId, dto.Narrative);

            // Final submission
            return await SubmitApplicationAsync(applicationId, new Step6SubmitDto
            {
                EmailVerificationCode = dto.EmailVerificationCode,
                CertificationAccepted = dto.CertificationAccepted,
                SignatureFullName = dto.SignatureFullName
            }, ipAddress);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in full submission for {Id}", applicationId);
            return Fail<bool>("Failed to submit application.");
        }
    }

    // ── Portal Status ─────────────────────────────────────────────────────────

    public async Task<CommonResponseDto<PortalStatusDto>> GetPortalStatusAsync(Guid applicationId)
    {
        try
        {
            var application = await applicationRepository.GetWithFullDetailsAsync(applicationId);
            if (application == null)
                return Fail<PortalStatusDto>("Application not found.");

            var milestones = BuildMilestones(application);

            return new CommonResponseDto<PortalStatusDto>
            {
                Success = true,
                Data = new PortalStatusDto
                {
                    ApplicationId = application.Id,
                    CaseNumber = application.CaseNumber,
                    Status = application.Status.ToString(),
                    ApplicationType = application.ApplicationType.ToString(),
                    SubmittedAt = application.SubmittedAt,
                    Milestones = milestones
                }
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting portal status for {Id}", applicationId);
            return Fail<PortalStatusDto>("Failed to retrieve status.");
        }
    }

    // ── Case List ─────────────────────────────────────────────────────────────

    public async Task<PagedResponseDto<List<CaseListItemDto>>> GetCaseListAsync(CaseListFilterDto filter)
    {
        try
        {
            var (items, total) = await applicationRepository.GetPagedAsync(filter);
            var today = DateTime.UtcNow.Date;

            var dtos = items.Select(a => new CaseListItemDto
            {
                ApplicationId = a.Id,
                CaseNumber = a.CaseNumber,
                ApplicationType = a.ApplicationType.ToString(),
                ApplicantName = a.Applicant != null
                    ? $"{a.Applicant.FirstName} {a.Applicant.LastName}"
                    : string.Empty,
                VehicleYMM = a.Vehicle != null
                    ? $"{a.Vehicle.VehicleYear} {a.Vehicle.VehicleMake} {a.Vehicle.VehicleModel}"
                    : string.Empty,
                VIN = a.Vehicle?.VIN ?? string.Empty,
                Status = a.Status.ToString(),
                AssignedTo = a.AssignedToName,
                SubmittedAt = a.SubmittedAt,
                LastActivityAt = a.LastActivityAt,
                AgingDays = (today - a.SubmittedAt.Date).Days,
                DealerNotified = a.DealerOutreaches.Any(o => o.SentAt.HasValue)
            }).ToList();

            return new PagedResponseDto<List<CaseListItemDto>>
            {
                Success = true,
                Data = dtos,
                TotalRecords = total,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting case list");
            return new PagedResponseDto<List<CaseListItemDto>>
            {
                Success = false,
                Message = "Failed to retrieve cases."
            };
        }
    }

    // ── Case Detail ───────────────────────────────────────────────────────────

    public async Task<CommonResponseDto<CaseDetailDto>> GetCaseDetailAsync(Guid applicationId)
    {
        try
        {
            var a = await applicationRepository.GetWithFullDetailsAsync(applicationId);
            if (a == null)
                return Fail<CaseDetailDto>("Case not found.");

            return new CommonResponseDto<CaseDetailDto>
            {
                Success = true,
                Data = new CaseDetailDto
                {
                    ApplicationId = a.Id,
                    CaseNumber = a.CaseNumber,
                    ApplicationType = a.ApplicationType.ToString(),
                    Status = a.Status.ToString(),
                    AssignedToStaffId = a.AssignedToStaffId,
                    AssignedToName = a.AssignedToName,
                    SubmittedAt = a.SubmittedAt,
                    LastActivityAt = a.LastActivityAt,
                    NarrativeStatement = a.NarrativeStatement,
                    DesiredResolution = a.DesiredResolution?.ToString(),
                    Applicant = a.Applicant == null ? null : MapApplicant(a.Applicant),
                    Vehicle = a.Vehicle == null ? null : MapVehicle(a.Vehicle),
                    Defects = a.Defects.Select(d => new DefectDto
                    {
                        DefectId = d.Id,
                        DefectDescription = d.DefectDescription,
                        DefectCategory = d.DefectCategory,
                        FirstOccurrenceDate = d.FirstOccurrenceDate,
                        IsOngoing = d.IsOngoing,
                        SortOrder = d.SortOrder
                    }).ToList(),
                    RepairAttempts = a.RepairAttempts.Select(r => new RepairAttemptDto
                    {
                        RepairAttemptId = r.Id,
                        RepairDate = r.RepairDate,
                        RepairFacilityName = r.RepairFacilityName,
                        DefectsAddressed = r.DefectsAddressed,
                        RepairSuccessful = r.RepairSuccessful,
                        DaysOutOfService = r.DaysOutOfService,
                        SortOrder = r.SortOrder
                    }).ToList()
                }
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting case detail for {Id}", applicationId);
            return Fail<CaseDetailDto>("Failed to retrieve case.");
        }
    }

    // ── Status Transition ─────────────────────────────────────────────────────

    public async Task<CommonResponseDto<bool>> TransitionStatusAsync(
        Guid applicationId, StatusTransitionDto dto, string staffId, string staffName)
    {
        try
        {
            var application = await applicationRepository.GetByIdAsync(applicationId);
            if (application == null)
                return Fail<bool>("Application not found.");

            if (!Enum.TryParse<ApplicationStatus>(dto.NewStatus, out var newStatus))
                return Fail<bool>("Invalid status value.");

            var previousStatus = application.Status;
            application.Status = newStatus;
            application.LastActivityAt = DateTime.UtcNow;

            await eventRepository.AddAsync(new CaseEvent
            {
                ApplicationId = applicationId,
                EventType = CaseEventType.STATUS_CHANGE,
                ActorType = ActorType.STAFF,
                ActorId = staffId,
                ActorDisplayName = staffName,
                Description = $"Status changed to {newStatus}. {dto.Reason}",
                PreviousValue = previousStatus.ToString(),
                NewValue = newStatus.ToString()
            });

            applicationRepository.Update(application);
            await applicationRepository.SaveChangesAsync();

            return new CommonResponseDto<bool> { Success = true, Data = true };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error transitioning status for {Id}", applicationId);
            return Fail<bool>("Failed to transition status.");
        }
    }

    // ── Assign Case ───────────────────────────────────────────────────────────

    public async Task<CommonResponseDto<bool>> AssignCaseAsync(
        Guid applicationId, AssignCaseDto dto, string assignedByStaffId)
    {
        try
        {
            var application = await applicationRepository.GetByIdAsync(applicationId);
            if (application == null)
                return Fail<bool>("Application not found.");

            application.AssignedToStaffId = dto.StaffId;
            application.AssignedToName = dto.StaffName;
            application.AssignedAt = DateTime.UtcNow;
            application.AssignedByStaffId = assignedByStaffId;
            application.LastActivityAt = DateTime.UtcNow;

            await eventRepository.AddAsync(new CaseEvent
            {
                ApplicationId = applicationId,
                EventType = CaseEventType.ASSIGNMENT_CHANGED,
                ActorType = ActorType.STAFF,
                ActorId = assignedByStaffId,
                ActorDisplayName = assignedByStaffId,
                Description = $"Case assigned to {dto.StaffName}.",
                NewValue = dto.StaffId
            });

            applicationRepository.Update(application);
            await applicationRepository.SaveChangesAsync();

            return new CommonResponseDto<bool> { Success = true, Data = true };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error assigning case {Id}", applicationId);
            return Fail<bool>("Failed to assign case.");
        }
    }

    // ── Notes ─────────────────────────────────────────────────────────────────

    public async Task<CommonResponseDto<CaseNoteDto>> AddNoteAsync(
        Guid applicationId, CreateNoteDto dto, string staffId, string staffName)
    {
        try
        {
            var note = new CaseNote
            {
                ApplicationId = applicationId,
                NoteText = dto.NoteText,
                CreatedByStaffId = staffId,
                CreatedByName = staffName,
                CreatedAt = DateTime.UtcNow
            };

            await noteRepository.AddAsync(note);

            await eventRepository.AddAsync(new CaseEvent
            {
                ApplicationId = applicationId,
                EventType = CaseEventType.NOTE_ADDED,
                ActorType = ActorType.STAFF,
                ActorId = staffId,
                ActorDisplayName = staffName,
                Description = "Internal note added."
            });

            await noteRepository.SaveChangesAsync();

            return new CommonResponseDto<CaseNoteDto>
            {
                Success = true,
                Data = new CaseNoteDto
                {
                    NoteId = note.Id,
                    NoteText = note.NoteText,
                    CreatedByName = note.CreatedByName,
                    CreatedAt = note.CreatedAt,
                    IsPinned = note.IsPinned
                }
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error adding note to {Id}", applicationId);
            return Fail<CaseNoteDto>("Failed to add note.");
        }
    }

    public async Task<CommonResponseDto<bool>> UpdateNotePinAsync(Guid noteId, UpdateNotePinDto dto)
    {
        try
        {
            var note = await noteRepository.GetByIdAsync(noteId);
            if (note == null)
                return Fail<bool>("Note not found.");

            note.IsPinned = dto.IsPinned;
            noteRepository.Update(note);
            await noteRepository.SaveChangesAsync();

            return new CommonResponseDto<bool> { Success = true, Data = true };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating note pin {Id}", noteId);
            return Fail<bool>("Failed to update note.");
        }
    }

    // ── Hearing ───────────────────────────────────────────────────────────────

    public async Task<CommonResponseDto<HearingDto>> CreateHearingAsync(
        Guid applicationId, CreateHearingDto dto, string staffId)
    {
        try
        {
            if (!Enum.TryParse<HearingFormat>(dto.HearingFormat, out var format))
                return Fail<HearingDto>("Invalid hearing format.");

            var hearing = new Hearing
            {
                ApplicationId = applicationId,
                HearingDate = dto.HearingDate,
                HearingFormat = format,
                HearingLocation = dto.HearingLocation,
                ArbitratorName = dto.ArbitratorName,
                Outcome = HearingOutcome.PENDING
            };

            await hearingRepository.AddAsync(hearing);

            await eventRepository.AddAsync(new CaseEvent
            {
                ApplicationId = applicationId,
                EventType = CaseEventType.HEARING_SCHEDULED,
                ActorType = ActorType.STAFF,
                ActorId = staffId,
                ActorDisplayName = staffId,
                Description = $"Hearing scheduled for {dto.HearingDate:yyyy-MM-dd} ({dto.HearingFormat})."
            });

            await hearingRepository.SaveChangesAsync();

            return new CommonResponseDto<HearingDto>
            {
                Success = true,
                Data = new HearingDto
                {
                    HearingId = hearing.Id,
                    HearingDate = hearing.HearingDate,
                    HearingFormat = hearing.HearingFormat.ToString(),
                    HearingLocation = hearing.HearingLocation,
                    ArbitratorName = hearing.ArbitratorName,
                    Outcome = hearing.Outcome.ToString()
                }
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating hearing for {Id}", applicationId);
            return Fail<HearingDto>("Failed to create hearing.");
        }
    }

    public async Task<CommonResponseDto<bool>> UpdateHearingOutcomeAsync(
        Guid hearingId, UpdateHearingOutcomeDto dto)
    {
        try
        {
            var hearing = await hearingRepository.GetByIdAsync(hearingId);
            if (hearing == null)
                return Fail<bool>("Hearing not found.");

            if (Enum.TryParse<HearingOutcome>(dto.Outcome, out var outcome))
                hearing.Outcome = outcome;

            hearing.OutcomeNotes = dto.OutcomeNotes;
            hearing.ContinuedTo = dto.ContinuedTo;

            hearingRepository.Update(hearing);
            await hearingRepository.SaveChangesAsync();

            return new CommonResponseDto<bool> { Success = true, Data = true };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating hearing outcome {Id}", hearingId);
            return Fail<bool>("Failed to update hearing.");
        }
    }

    // ── Decision ──────────────────────────────────────────────────────────────

    public async Task<CommonResponseDto<bool>> IssueDecisionAsync(
        Guid applicationId, IssueDecisionDto dto, string staffId)
    {
        try
        {
            if (!Enum.TryParse<DecisionType>(dto.DecisionType, out var decisionType))
                return Fail<bool>("Invalid decision type.");

            var decision = new Decision
            {
                ApplicationId = applicationId,
                DecisionType = decisionType,
                DecisionDate = dto.DecisionDate,
                RefundAmount = dto.RefundAmount,
                ComplianceDeadline = dto.ComplianceDeadline,
                DecisionDocumentId = dto.DecisionDocumentId,
                DecisionIssuedById = staffId
            };

            await decisionRepository.AddAsync(decision);

            await eventRepository.AddAsync(new CaseEvent
            {
                ApplicationId = applicationId,
                EventType = CaseEventType.DECISION_ISSUED,
                ActorType = ActorType.STAFF,
                ActorId = staffId,
                ActorDisplayName = staffId,
                Description = $"Decision issued: {decisionType}."
            });

            await decisionRepository.SaveChangesAsync();

            return new CommonResponseDto<bool> { Success = true, Data = true };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error issuing decision for {Id}", applicationId);
            return Fail<bool>("Failed to issue decision.");
        }
    }

    // ── Consumer Documents ────────────────────────────────────────────────────

    public async Task<CommonResponseDto<List<DocumentListItemDto>>> GetConsumerDocumentsAsync(Guid applicationId)
    {
        try
        {
            var docs = await documentRepository.FindAsync(d =>
                d.ApplicationId == applicationId && d.IsVisible_Consumer);
            var dtos = docs.OrderByDescending(d => d.UploadedAt).Select(MapDocumentListItem).ToList();
            return new CommonResponseDto<List<DocumentListItemDto>> { Success = true, Data = dtos };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting consumer documents for {Id}", applicationId);
            return Fail<List<DocumentListItemDto>>("Failed to retrieve documents.");
        }
    }

    public async Task<CommonResponseDto<List<DocumentListItemDto>>> GetPortalDocumentsAsync(Guid applicationId)
        => await GetConsumerDocumentsAsync(applicationId);

    // ── Case Events ───────────────────────────────────────────────────────────

    public async Task<CommonResponseDto<List<CaseEventDto>>> GetCaseEventsAsync(Guid applicationId)
    {
        try
        {
            var events = await eventRepository.FindAsync(e => e.ApplicationId == applicationId);
            var dtos = events.OrderByDescending(e => e.EventTimestamp).Select(e => new CaseEventDto
            {
                CaseEventId = e.CaseEventId,
                EventType = e.EventType.ToString(),
                EventTimestamp = e.EventTimestamp,
                ActorType = e.ActorType.ToString(),
                ActorDisplayName = e.ActorDisplayName,
                Description = e.Description,
                PreviousValue = e.PreviousValue,
                NewValue = e.NewValue
            }).ToList();
            return new CommonResponseDto<List<CaseEventDto>> { Success = true, Data = dtos };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting events for {Id}", applicationId);
            return Fail<List<CaseEventDto>>("Failed to retrieve events.");
        }
    }

    // ── Case Documents ────────────────────────────────────────────────────────

    public async Task<CommonResponseDto<List<DocumentListItemDto>>> GetCaseDocumentsAsync(Guid applicationId)
    {
        try
        {
            var docs = await documentRepository.FindAsync(d => d.ApplicationId == applicationId);
            var dtos = docs.OrderByDescending(d => d.UploadedAt).Select(MapDocumentListItem).ToList();
            return new CommonResponseDto<List<DocumentListItemDto>> { Success = true, Data = dtos };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting documents for {Id}", applicationId);
            return Fail<List<DocumentListItemDto>>("Failed to retrieve documents.");
        }
    }

    // ── Document Review ───────────────────────────────────────────────────────

    public async Task<CommonResponseDto<bool>> UpdateDocumentStatusAsync(
        Guid documentId, DocumentReviewDto dto, string staffId)
    {
        try
        {
            var doc = await documentRepository.GetByIdAsync(documentId);
            if (doc == null)
                return Fail<bool>("Document not found.");

            if (Enum.TryParse<DocumentStatus>(dto.Status, out var status))
                doc.Status = status;

            if (!string.IsNullOrWhiteSpace(dto.StaffNotes))
                doc.StaffNotes = dto.StaffNotes;

            if (dto.IsVisible_Dealer.HasValue)
                doc.IsVisible_Dealer = dto.IsVisible_Dealer.Value;

            documentRepository.Update(doc);
            await documentRepository.SaveChangesAsync();

            return new CommonResponseDto<bool> { Success = true, Data = true };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating document {Id}", documentId);
            return Fail<bool>("Failed to update document.");
        }
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static DocumentListItemDto MapDocumentListItem(ApplicationDocument d) => new()
    {
        DocumentId = d.Id,
        DocumentType = d.DocumentType.ToString(),
        FileName = d.FileName,
        FileSizeBytes = d.FileSizeBytes,
        Status = d.Status.ToString(),
        UploadedAt = d.UploadedAt,
        UploadedByRole = d.UploadedByRole.ToString()
    };

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

    private static CommonResponseDto<T> Fail<T>(string message) =>
        new() { Success = false, Message = message };

    private static ApplicantDto MapApplicant(Applicant a) => new()
    {
        FirstName = a.FirstName,
        MiddleName = a.MiddleName,
        LastName = a.LastName,
        EmailAddress = a.EmailAddress,
        PhoneNumber = a.PhoneNumber,
        PhoneType = a.PhoneType.ToString(),
        AddressLine1 = a.AddressLine1,
        AddressLine2 = a.AddressLine2,
        City = a.City,
        State = a.AddressState,
        ZipCode = a.ZipCode,
        PreferredContact = a.PreferredContact.ToString()
    };

    private static VehicleDto MapVehicle(Vehicle v) => new()
    {
        VIN = v.VIN,
        VehicleYear = v.VehicleYear,
        VehicleMake = v.VehicleMake,
        VehicleModel = v.VehicleModel,
        VehicleColor = v.VehicleColor,
        DealerName = v.DealerName,
        ManufacturerName = v.ManufacturerName,
        WarrantyType = v.WarrantyType.ToString(),
        PurchaseDate = v.PurchaseDate,
        MileageAtPurchase = v.MileageAtPurchase,
        CurrentMileage = v.CurrentMileage
    };

    private static List<MilestoneDto> BuildMilestones(AppEntity a)
    {
        var statuses = new[]
        {
            ApplicationStatus.SUBMITTED,
            ApplicationStatus.ACCEPTED,
            ApplicationStatus.DEALER_RESPONDED,
            ApplicationStatus.HEARING_SCHEDULED,
            ApplicationStatus.HEARING_COMPLETE,
            ApplicationStatus.DECISION_ISSUED,
            ApplicationStatus.CLOSED
        };

        var currentIndex = Array.IndexOf(statuses, a.Status);

        return statuses.Select((s, i) => new MilestoneDto
        {
            Label = s.ToString().Replace("_", " "),
            Completed = i <= currentIndex
        }).ToList();
    }
}

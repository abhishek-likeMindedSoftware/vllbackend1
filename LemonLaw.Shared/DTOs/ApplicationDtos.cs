using LemonLaw.Core.Enums;

namespace LemonLaw.Shared.DTOs;

// ── Consumer Portal DTOs ──────────────────────────────────────────────────────

public class ApplicationTypeDto
{
    public string Value { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string EligibilitySummary { get; set; } = string.Empty;
}

public class Step1ConsumerInfoDto
{
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public PhoneType PhoneType { get; set; }
    public string AddressLine1 { get; set; } = string.Empty;
    public string? AddressLine2 { get; set; }
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string? PlaceId { get; set; }
    public PreferredContactMethod PreferredContactMethod { get; set; }
}

public class Step2VehicleInfoDto
{
    public string VIN { get; set; } = string.Empty;
    public bool VinConfirmed { get; set; }
    public short VehicleYear { get; set; }
    public string VehicleMake { get; set; } = string.Empty;
    public string VehicleModel { get; set; } = string.Empty;
    public string? VehicleColor { get; set; }
    public string? LicensePlate { get; set; }
    public string? LicensePlateState { get; set; }
    public DateOnly PurchaseDate { get; set; }
    public decimal? PurchasePrice { get; set; }
    public int MileageAtPurchase { get; set; }
    public int CurrentMileage { get; set; }
    public string DealerName { get; set; } = string.Empty;
    public string DealerAddressLine1 { get; set; } = string.Empty;
    public string? DealerAddressLine2 { get; set; }
    public string DealerCity { get; set; } = string.Empty;
    public string DealerState { get; set; } = string.Empty;
    public string DealerZip { get; set; } = string.Empty;
    public string? DealerPhone { get; set; }
    public string? DealerEmail { get; set; }
    public string? DealerPlaceId { get; set; }
    public string ManufacturerName { get; set; } = string.Empty;
    public WarrantyType WarrantyType { get; set; }
    public DateOnly? WarrantyStartDate { get; set; }
    public DateOnly? WarrantyExpiryDate { get; set; }
}

public class DefectDto
{
    public Guid? DefectId { get; set; }
    public string DefectDescription { get; set; } = string.Empty;
    public DefectCategory DefectCategory { get; set; }
    public DateOnly FirstOccurrenceDate { get; set; }
    public bool IsOngoing { get; set; }
    public int SortOrder { get; set; }
}

public class RepairAttemptDto
{
    public Guid? RepairAttemptId { get; set; }
    public DateOnly RepairDate { get; set; }
    public string RepairFacilityName { get; set; } = string.Empty;
    public string? RepairFacilityAddr { get; set; }
    public string? RepairFacilityPlaceId { get; set; }
    public string? RoNumber { get; set; }
    public int? MileageAtRepair { get; set; }
    public string DefectsAddressed { get; set; } = string.Empty;
    public bool RepairSuccessful { get; set; }
    public int? DaysOutOfService { get; set; }
    public int SortOrder { get; set; }
}

public class ExpenseDto
{
    public Guid? ExpenseId { get; set; }
    public ExpenseType ExpenseType { get; set; }
    public DateOnly? ExpenseDate { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public bool ReceiptUploaded { get; set; }
}

public class Step3DefectRepairDto
{
    public List<DefectDto> Defects { get; set; } = new();
    public List<RepairAttemptDto> RepairAttempts { get; set; } = new();
    public List<ExpenseDto> Expenses { get; set; } = new();
}

public class Step4NarrativeDto
{
    public string NarrativeStatement { get; set; } = string.Empty;
    public bool PriorContactWithDealer { get; set; }
    public bool PriorContactWithMfr { get; set; }
    public string? PriorContactNotes { get; set; }
    public DesiredResolution DesiredResolution { get; set; }
}

public class Step6SubmitDto
{
    public string EmailVerificationCode { get; set; } = string.Empty;
    public bool CertificationAccepted { get; set; }
    public string SignatureFullName { get; set; } = string.Empty;
}

public class CreateApplicationRequestDto
{
    public ApplicationType ApplicationType { get; set; }
    public bool EligibilityAcknowledged { get; set; }
}

public class CreateApplicationResponseDto
{
    public Guid ApplicationId { get; set; }
    public string CaseNumber { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
}

// ── Portal Status ─────────────────────────────────────────────────────────────

public class PortalStatusDto
{
    public Guid ApplicationId { get; set; }
    public string CaseNumber { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string ApplicationType { get; set; } = string.Empty;
    public DateTime SubmittedAt { get; set; }
    public List<MilestoneDto> Milestones { get; set; } = new();
    public List<NotificationSummaryDto> Notifications { get; set; } = new();
}

public class MilestoneDto
{
    public string Label { get; set; } = string.Empty;
    public bool Completed { get; set; }
    public DateTime? CompletedAt { get; set; }
}

public class NotificationSummaryDto
{
    public string Type { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public DateTime SentAt { get; set; }
    public string DeliveryStatus { get; set; } = string.Empty;
}

// ── Staff / Workbench DTOs ────────────────────────────────────────────────────

public class CaseListItemDto
{
    public Guid ApplicationId { get; set; }
    public string CaseNumber { get; set; } = string.Empty;
    public string ApplicationType { get; set; } = string.Empty;
    public string ApplicantName { get; set; } = string.Empty;
    public string VehicleYMM { get; set; } = string.Empty;
    public string VIN { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? AssignedTo { get; set; }
    public DateTime SubmittedAt { get; set; }
    public DateTime LastActivityAt { get; set; }
    public int AgingDays { get; set; }
    public bool DocumentsComplete { get; set; }
    public bool DealerNotified { get; set; }
}

public class CaseListFilterDto
{
    public string? ApplicationType { get; set; }
    public string? Status { get; set; }
    public string? AssignedToStaffId { get; set; }
    public string? VIN { get; set; }
    public DateTime? SubmittedFrom { get; set; }
    public DateTime? SubmittedTo { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? SortBy { get; set; }
    public bool SortDescending { get; set; } = true;
}

public class CaseDetailDto
{
    public Guid ApplicationId { get; set; }
    public string CaseNumber { get; set; } = string.Empty;
    public string ApplicationType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? AssignedToStaffId { get; set; }
    public string? AssignedToName { get; set; }
    public DateTime SubmittedAt { get; set; }
    public DateTime LastActivityAt { get; set; }
    public ApplicantDto? Applicant { get; set; }
    public VehicleDto? Vehicle { get; set; }
    public List<DefectDto> Defects { get; set; } = new();
    public List<RepairAttemptDto> RepairAttempts { get; set; } = new();
    public List<ExpenseDto> Expenses { get; set; } = new();
    public string? NarrativeStatement { get; set; }
    public string? DesiredResolution { get; set; }
}

public class ApplicantDto
{
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string PhoneType { get; set; } = string.Empty;
    public string AddressLine1 { get; set; } = string.Empty;
    public string? AddressLine2 { get; set; }
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string PreferredContact { get; set; } = string.Empty;
}

public class VehicleDto
{
    public string VIN { get; set; } = string.Empty;
    public short VehicleYear { get; set; }
    public string VehicleMake { get; set; } = string.Empty;
    public string VehicleModel { get; set; } = string.Empty;
    public string? VehicleColor { get; set; }
    public string DealerName { get; set; } = string.Empty;
    public string ManufacturerName { get; set; } = string.Empty;
    public string WarrantyType { get; set; } = string.Empty;
    public DateOnly PurchaseDate { get; set; }
    public int MileageAtPurchase { get; set; }
    public int CurrentMileage { get; set; }
}

public class StatusTransitionDto
{
    public string NewStatus { get; set; } = string.Empty;
    public string? Reason { get; set; }
}

public class AssignCaseDto
{
    public string StaffId { get; set; } = string.Empty;
    public string StaffName { get; set; } = string.Empty;
}

public class DocumentReviewDto
{
    public string Status { get; set; } = string.Empty;
    public string? StaffNotes { get; set; }
    public bool? IsVisible_Dealer { get; set; }
}

public class CaseNoteDto
{
    public Guid NoteId { get; set; }
    public string NoteText { get; set; } = string.Empty;
    public string CreatedByName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsPinned { get; set; }
}

public class CreateNoteDto
{
    public string NoteText { get; set; } = string.Empty;
}

public class UpdateNotePinDto
{
    public bool IsPinned { get; set; }
}

public class HearingDto
{
    public Guid HearingId { get; set; }
    public DateTime HearingDate { get; set; }
    public string HearingFormat { get; set; } = string.Empty;
    public string? HearingLocation { get; set; }
    public string? ArbitratorName { get; set; }
    public string Outcome { get; set; } = string.Empty;
    public string? OutcomeNotes { get; set; }
}

public class CreateHearingDto
{
    public DateTime HearingDate { get; set; }
    public string HearingFormat { get; set; } = string.Empty;
    public string? HearingLocation { get; set; }
    public string? ArbitratorName { get; set; }
}

public class UpdateHearingOutcomeDto
{
    public string Outcome { get; set; } = string.Empty;
    public string? OutcomeNotes { get; set; }
    public DateTime? ContinuedTo { get; set; }
}

public class IssueDecisionDto
{
    public string DecisionType { get; set; } = string.Empty;
    public DateOnly DecisionDate { get; set; }
    public decimal? RefundAmount { get; set; }
    public DateOnly? ComplianceDeadline { get; set; }
    public Guid? DecisionDocumentId { get; set; }
}

// ── VIN ───────────────────────────────────────────────────────────────────────

public class VinLookupResponseDto
{
    public bool IsValid { get; set; }
    public short? Year { get; set; }
    public string? Make { get; set; }
    public string? Model { get; set; }
    public string? Trim { get; set; }
    public bool IsDuplicate { get; set; }
    public List<string> DuplicateCaseNumbers { get; set; } = new();
    public string? ErrorMessage { get; set; }
}

// ── Dealer Portal DTOs ────────────────────────────────────────────────────────

public class DealerCaseSummaryDto
{
    public string CaseNumber { get; set; } = string.Empty;
    public string ConsumerName { get; set; } = string.Empty;
    public string VehicleYMM { get; set; } = string.Empty;
    public string VIN { get; set; } = string.Empty;
    public DateOnly ResponseDeadline { get; set; }
    public string NarrativeStatement { get; set; } = string.Empty;
    public List<DefectDto> Defects { get; set; } = new();
    public List<RepairAttemptDto> RepairAttempts { get; set; } = new();
}

public class DealerResponseSubmitDto
{
    public string ResponderName { get; set; } = string.Empty;
    public string? ResponderTitle { get; set; }
    public string ResponderEmail { get; set; } = string.Empty;
    public string? ResponderPhone { get; set; }
    public string DealerPosition { get; set; } = string.Empty;
    public string ResponseNarrative { get; set; } = string.Empty;
    public string? RepairHistoryNotes { get; set; }
    public decimal? SettlementOffer { get; set; }
    public string? SettlementDetails { get; set; }
    public bool CertificationAccepted { get; set; }
    public string CertifierFullName { get; set; } = string.Empty;
}

// ── Outreach ──────────────────────────────────────────────────────────────────

public class CreateOutreachDto
{
    public Guid ApplicationId { get; set; }
    public string DealerEmail { get; set; } = string.Empty;
    public string TemplateCode { get; set; } = string.Empty;
    public DateOnly ResponseDeadline { get; set; }
}

public class SendFollowUpDto
{
    public string OutreachType { get; set; } = string.Empty;
}

// ── Verification ──────────────────────────────────────────────────────────────

public class SendVerificationCodeDto
{
    public string EmailAddress { get; set; } = string.Empty;
}

public class ConfirmVerificationCodeDto
{
    public string EmailAddress { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}

public class VerificationResultDto
{
    public bool Verified { get; set; }
    public string? PreSubmissionToken { get; set; }
}

// ── Document Upload ───────────────────────────────────────────────────────────

public class DocumentUploadResultDto
{
    public Guid DocumentId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string StoragePath { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

// ── Reporting ─────────────────────────────────────────────────────────────────

public class OpenCasesByStatusDto
{
    public string Status { get; set; } = string.Empty;
    public int Count { get; set; }
}

public class AgingReportItemDto
{
    public string Bucket { get; set; } = string.Empty;
    public int Count { get; set; }
}

public class VolumeReportItemDto
{
    public int Year { get; set; }
    public int Month { get; set; }
    public string ApplicationType { get; set; } = string.Empty;
    public int Count { get; set; }
}

public class DealerResponseRateDto
{
    public int TotalOutreachSent { get; set; }
    public int ResponsesWithin15Days { get; set; }
    public double ResponseRatePercent { get; set; }
}

public class StaffWorkloadDto
{
    public string StaffId { get; set; } = string.Empty;
    public string StaffName { get; set; } = string.Empty;
    public int OpenCaseCount { get; set; }
}

public class DecisionSummaryDto
{
    public string DecisionType { get; set; } = string.Empty;
    public int Count { get; set; }
}

// ── Case Event ────────────────────────────────────────────────────────────────

public class CaseEventDto
{
    public Guid CaseEventId { get; set; }
    public string EventType { get; set; } = string.Empty;
    public DateTime EventTimestamp { get; set; }
    public string ActorType { get; set; } = string.Empty;
    public string? ActorDisplayName { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? PreviousValue { get; set; }
    public string? NewValue { get; set; }
}

// ── Document List ─────────────────────────────────────────────────────────────

public class DocumentListItemDto
{
    public Guid DocumentId { get; set; }
    public string DocumentType { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public long FileSizeBytes { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; }
    public string UploadedByRole { get; set; } = string.Empty;
}
// ── Document Upload Form ──────────────────────────────────────────────────────

public class DocumentUploadDto
{
    public string DocumentType { get; set; } = string.Empty;
}

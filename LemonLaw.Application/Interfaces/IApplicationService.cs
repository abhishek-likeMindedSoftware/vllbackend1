using LemonLaw.Shared.DTOs;

namespace LemonLaw.Application.Interfaces;

public interface IApplicationService
{
    Task<CommonResponseDto<CreateApplicationResponseDto>> CreateApplicationAsync(
        CreateApplicationRequestDto dto, string ipAddress);

    Task<CommonResponseDto<bool>> SaveStepAsync(
        Guid applicationId, int step, object stepData);

    Task<CommonResponseDto<bool>> SaveStep1Async(Guid applicationId, Step1ConsumerInfoDto dto);
    Task<CommonResponseDto<bool>> SaveStep2Async(Guid applicationId, Step2VehicleInfoDto dto);
    Task<CommonResponseDto<bool>> SaveStep3Async(Guid applicationId, Step3DefectRepairDto dto);
    Task<CommonResponseDto<bool>> SaveStep4Async(Guid applicationId, Step4NarrativeDto dto);

    Task<CommonResponseDto<bool>> SubmitApplicationAsync(
        Guid applicationId, Step6SubmitDto dto, string ipAddress);

    /// <summary>
    /// One-shot submission — accepts all wizard data at once (FILIR pattern).
    /// Saves steps 1-4 and submits in a single transaction.
    /// </summary>
    Task<CommonResponseDto<bool>> SubmitFullApplicationAsync(
        Guid applicationId, FullSubmitDto dto, string ipAddress);

    Task<CommonResponseDto<PortalStatusDto>> GetPortalStatusAsync(Guid applicationId);

    Task<PagedResponseDto<List<CaseListItemDto>>> GetCaseListAsync(CaseListFilterDto filter);

    Task<CommonResponseDto<CaseDetailDto>> GetCaseDetailAsync(Guid applicationId);

    Task<CommonResponseDto<bool>> TransitionStatusAsync(
        Guid applicationId, StatusTransitionDto dto, string staffId, string staffName);

    Task<CommonResponseDto<bool>> AssignCaseAsync(
        Guid applicationId, AssignCaseDto dto, string assignedByStaffId);

    Task<CommonResponseDto<CaseNoteDto>> AddNoteAsync(
        Guid applicationId, CreateNoteDto dto, string staffId, string staffName);

    Task<CommonResponseDto<bool>> UpdateNotePinAsync(
        Guid noteId, UpdateNotePinDto dto);

    Task<CommonResponseDto<HearingDto>> CreateHearingAsync(
        Guid applicationId, CreateHearingDto dto, string staffId);

    Task<CommonResponseDto<bool>> UpdateHearingOutcomeAsync(
        Guid hearingId, UpdateHearingOutcomeDto dto);

    Task<CommonResponseDto<bool>> IssueDecisionAsync(
        Guid applicationId, IssueDecisionDto dto, string staffId);

    Task<CommonResponseDto<bool>> UpdateDocumentStatusAsync(
        Guid documentId, DocumentReviewDto dto, string staffId);

    Task<CommonResponseDto<List<DocumentListItemDto>>> GetConsumerDocumentsAsync(Guid applicationId);

    Task<CommonResponseDto<List<CaseEventDto>>> GetCaseEventsAsync(Guid applicationId);

    Task<CommonResponseDto<List<DocumentListItemDto>>> GetCaseDocumentsAsync(Guid applicationId);
    Task<CommonResponseDto<List<DocumentListItemDto>>> GetPortalDocumentsAsync(Guid applicationId);
}

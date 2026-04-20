using LemonLaw.Application.Interfaces;
using LemonLaw.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LemonLaw.API.Controllers;

/// <summary>Agency workbench case management endpoints.</summary>
[Route("api/cases")]
[Authorize]
public class CasesController(
    IApplicationService applicationService,
    IReportingService reportingService) : BaseController
{
    /// <summary>Get paginated, filtered case list.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponseDto<List<CaseListItemDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetList([FromQuery] CaseListFilterDto filter)
    {
        var result = await applicationService.GetCaseListAsync(filter);
        return Ok(result);
    }

    /// <summary>Get full case detail.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CommonResponseDto<CaseDetailDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDetail(Guid id)
    {
        var result = await applicationService.GetCaseDetailAsync(id);
        return Ok(result);
    }

    /// <summary>Transition case status.</summary>
    [HttpPut("{id:guid}/status")]
    [ProducesResponseType(typeof(CommonResponseDto<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> TransitionStatus(Guid id, [FromBody] StatusTransitionDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var result = await applicationService.TransitionStatusAsync(id, dto, StaffId, StaffName);
        return Ok(result);
    }

    /// <summary>Assign case to a staff member.</summary>
    [HttpPut("{id:guid}/assign")]
    [ProducesResponseType(typeof(CommonResponseDto<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Assign(Guid id, [FromBody] AssignCaseDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var result = await applicationService.AssignCaseAsync(id, dto, StaffId);
        return Ok(result);
    }

    /// <summary>Update document review status.</summary>
    [HttpPut("{id:guid}/documents/{docId:guid}")]
    [ProducesResponseType(typeof(CommonResponseDto<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateDocument(Guid id, Guid docId, [FromBody] DocumentReviewDto dto)
    {
        var result = await applicationService.UpdateDocumentStatusAsync(docId, dto, StaffId);
        return Ok(result);
    }

    /// <summary>Get all documents for a case.</summary>
    [HttpGet("{id:guid}/documents")]
    [ProducesResponseType(typeof(CommonResponseDto<List<DocumentListItemDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDocuments(Guid id)
    {
        var result = await applicationService.GetCaseDocumentsAsync(id);
        return Ok(result);
    }

    /// <summary>Add an internal note.</summary>
    [HttpPost("{id:guid}/notes")]
    [ProducesResponseType(typeof(CommonResponseDto<CaseNoteDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddNote(Guid id, [FromBody] CreateNoteDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var result = await applicationService.AddNoteAsync(id, dto, StaffId, StaffName);
        return Ok(result);
    }

    /// <summary>Pin or unpin a note.</summary>
    [HttpPut("{id:guid}/notes/{noteId:guid}")]
    [ProducesResponseType(typeof(CommonResponseDto<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateNotePin(Guid id, Guid noteId, [FromBody] UpdateNotePinDto dto)
    {
        var result = await applicationService.UpdateNotePinAsync(noteId, dto);
        return Ok(result);
    }

    /// <summary>Schedule a hearing.</summary>
    [HttpPost("{id:guid}/hearings")]
    [ProducesResponseType(typeof(CommonResponseDto<HearingDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateHearing(Guid id, [FromBody] CreateHearingDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var result = await applicationService.CreateHearingAsync(id, dto, StaffId);
        return Ok(result);
    }

    /// <summary>Update hearing outcome.</summary>
    [HttpPut("{id:guid}/hearings/{hId:guid}")]
    [ProducesResponseType(typeof(CommonResponseDto<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateHearing(Guid id, Guid hId, [FromBody] UpdateHearingOutcomeDto dto)
    {
        var result = await applicationService.UpdateHearingOutcomeAsync(hId, dto);
        return Ok(result);
    }

    /// <summary>Get full case event timeline.</summary>
    [HttpGet("{id:guid}/events")]
    [ProducesResponseType(typeof(CommonResponseDto<List<CaseEventDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEvents(Guid id)
    {
        var result = await applicationService.GetCaseEventsAsync(id);
        return Ok(result);
    }

    /// <summary>Issue a decision.</summary>
    [HttpPost("{id:guid}/decisions")]
    [ProducesResponseType(typeof(CommonResponseDto<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> IssueDecision(Guid id, [FromBody] IssueDecisionDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var result = await applicationService.IssueDecisionAsync(id, dto, StaffId);
        return Ok(result);
    }
}

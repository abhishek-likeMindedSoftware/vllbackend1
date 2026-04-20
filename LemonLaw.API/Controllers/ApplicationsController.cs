using LemonLaw.Application.Interfaces;
using LemonLaw.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LemonLaw.API.Controllers;

/// <summary>Consumer portal application endpoints.</summary>
[Route("api/applications")]
public class ApplicationsController(
    IApplicationService applicationService,
    IDocumentService documentService) : BaseController
{
    /// <summary>Create a new application (consumer portal).</summary>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(CommonResponseDto<CreateApplicationResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create([FromBody] CreateApplicationRequestDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var result = await applicationService.CreateApplicationAsync(dto, ClientIpAddress);
        return Ok(result);
    }

    /// <summary>Save step 1 — consumer info.</summary>
    [HttpPut("{id:guid}/step/1")]
    [AllowAnonymous]
    public async Task<IActionResult> SaveStep1(Guid id, [FromBody] Step1ConsumerInfoDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var result = await applicationService.SaveStep1Async(id, dto);
        return Ok(result);
    }

    /// <summary>Save step 2 — vehicle info.</summary>
    [HttpPut("{id:guid}/step/2")]
    [AllowAnonymous]
    public async Task<IActionResult> SaveStep2(Guid id, [FromBody] Step2VehicleInfoDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var result = await applicationService.SaveStep2Async(id, dto);
        return Ok(result);
    }

    /// <summary>Save step 3 — defects, repairs, expenses.</summary>
    [HttpPut("{id:guid}/step/3")]
    [AllowAnonymous]
    public async Task<IActionResult> SaveStep3(Guid id, [FromBody] Step3DefectRepairDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var result = await applicationService.SaveStep3Async(id, dto);
        return Ok(result);
    }

    /// <summary>Save step 4 — narrative statement.</summary>
    [HttpPut("{id:guid}/step/4")]
    [AllowAnonymous]
    public async Task<IActionResult> SaveStep4(Guid id, [FromBody] Step4NarrativeDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var result = await applicationService.SaveStep4Async(id, dto);
        return Ok(result);
    }

    /// <summary>Final submission of the application.</summary>
    [HttpPost("{id:guid}/submit")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(CommonResponseDto<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Submit(Guid id, [FromBody] Step6SubmitDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var result = await applicationService.SubmitApplicationAsync(id, dto, ClientIpAddress);
        return Ok(result);
    }

    /// <summary>Upload a document for an application.</summary>
    [HttpPost("{id:guid}/documents")]
    [AllowAnonymous]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(CommonResponseDto<DocumentUploadResultDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UploadDocument(Guid id, IFormFile file, [FromForm] string documentType)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file provided.");

        await using var stream = file.OpenReadStream();
        var result = await documentService.UploadDocumentAsync(
            id,
            documentType,
            file.FileName,
            file.ContentType,
            file.Length,
            stream,
            "CONSUMER");

        return Ok(result);
    }

    /// <summary>Delete a pending document for an application.</summary>
    [HttpDelete("{id:guid}/documents/{docId:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(CommonResponseDto<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteDocument(Guid id, Guid docId)
    {
        var result = await documentService.DeleteDocumentAsync(id, docId);
        return Ok(result);
    }
}

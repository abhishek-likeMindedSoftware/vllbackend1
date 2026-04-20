namespace LemonLaw.Shared.DTOs;

/// <summary>
/// Single DTO that carries all wizard data for a one-shot submission.
/// The frontend collects everything locally and sends it all at once.
/// </summary>
public class FullSubmitDto
{
    // ── Step 1 — Consumer Info ────────────────────────────────────────────────
    public Step1ConsumerInfoDto ConsumerInfo { get; set; } = new();

    // ── Step 2 — Vehicle Info ─────────────────────────────────────────────────
    public Step2VehicleInfoDto VehicleInfo { get; set; } = new();

    // ── Step 3 — Defects, Repairs, Expenses ──────────────────────────────────
    public Step3DefectRepairDto DefectsAndRepairs { get; set; } = new();

    // ── Step 4 — Narrative ────────────────────────────────────────────────────
    public Step4NarrativeDto Narrative { get; set; } = new();

    // ── Step 6 — Certification & Signature ───────────────────────────────────
    public string EmailVerificationCode { get; set; } = string.Empty;
    public bool CertificationAccepted { get; set; }
    public string SignatureFullName { get; set; } = string.Empty;
}

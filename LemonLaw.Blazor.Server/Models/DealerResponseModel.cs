namespace LemonLaw.Blazor.Server.Models
{
    public class DealerResponseModel
    {
        public string ResponderName { get; set; } = string.Empty;
        public string? ResponderTitle { get; set; }
        public string ResponderEmail { get; set; } = string.Empty;
        public string? ResponderPhone { get; set; }
        public string? DealerPosition { get; set; }
        public string ResponseNarrative { get; set; } = string.Empty;
        public string? RepairHistoryNotes { get; set; }
        public decimal? SettlementOffer { get; set; }
        public string? SettlementDetails { get; set; }
        public bool CertificationAccepted { get; set; }
        public string CertifierFullName { get; set; } = string.Empty;
    }
}

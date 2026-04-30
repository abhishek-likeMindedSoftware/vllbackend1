using DevExpress.ExpressApp.DC;
using LemonLaw.Core.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LemonLaw.Module.BusinessObjects
{
    /// <summary>
    /// Non-persistent popup form for "Issue Decision" per spec §3.6.
    /// Staff select the decision type and optionally enter a refund amount,
    /// compliance deadline, and notes before the API call is made.
    /// </summary>
    [DomainComponent]
    [XafDisplayName("Issue Decision")]
    public class IssueDecisionInput : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void Notify(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private DecisionType _decisionType = DecisionType.CLAIM_DENIED;

        [XafDisplayName("Decision Type")]
        [Required]
        public DecisionType DecisionType
        {
            get => _decisionType;
            set { if (_decisionType != value) { _decisionType = value; Notify(nameof(DecisionType)); } }
        }

        private DateTime _decisionDate = DateTime.UtcNow;

        [XafDisplayName("Decision Date")]
        [Required]
        public DateTime DecisionDate
        {
            get => _decisionDate;
            set { if (_decisionDate != value) { _decisionDate = value; Notify(nameof(DecisionDate)); } }
        }

        private decimal? _refundAmount;

        [XafDisplayName("Refund / Reimbursement Amount ($)")]
        public decimal? RefundAmount
        {
            get => _refundAmount;
            set { if (_refundAmount != value) { _refundAmount = value; Notify(nameof(RefundAmount)); } }
        }

        private DateTime? _complianceDeadline;

        [XafDisplayName("Compliance Deadline")]
        public DateTime? ComplianceDeadline
        {
            get => _complianceDeadline;
            set { if (_complianceDeadline != value) { _complianceDeadline = value; Notify(nameof(ComplianceDeadline)); } }
        }
    }
}

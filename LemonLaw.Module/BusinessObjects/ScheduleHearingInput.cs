using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using LemonLaw.Core.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LemonLaw.Module.BusinessObjects
{
    /// <summary>
    /// Non-persistent object used as the popup form when staff click
    /// "Schedule Hearing". Collects the four fields required by the API:
    ///   POST /api/cases/{id}/hearings
    ///
    /// Per spec §3.5:
    ///   - hearingDate   — required, must be in the future
    ///   - hearingFormat — required (IN_PERSON | TELEPHONE | VIDEO)
    ///   - hearingLocation — optional; physical address, dial-in number, or URL
    ///   - arbitratorName  — optional; assigned arbitrator
    /// </summary>
    [DomainComponent]
    [XafDisplayName("Schedule Hearing")]
    public class ScheduleHearingInput : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private void Notify(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private DateTime _hearingDate = DateTime.UtcNow.Date.AddDays(1);

        [XafDisplayName("Hearing Date & Time")]
        [Required]
        public DateTime HearingDate
        {
            get => _hearingDate;
            set { if (_hearingDate != value) { _hearingDate = value; Notify(nameof(HearingDate)); } }
        }

        private HearingFormat _hearingFormat = HearingFormat.TELEPHONE;

        [XafDisplayName("Format")]
        [Required]
        public HearingFormat HearingFormat
        {
            get => _hearingFormat;
            set { if (_hearingFormat != value) { _hearingFormat = value; Notify(nameof(HearingFormat)); } }
        }

        private string? _hearingLocation;

        [XafDisplayName("Location / Dial-in / URL")]
        public string? HearingLocation
        {
            get => _hearingLocation;
            set { if (_hearingLocation != value) { _hearingLocation = value; Notify(nameof(HearingLocation)); } }
        }

        private string? _arbitratorName;

        [XafDisplayName("Arbitrator Name")]
        public string? ArbitratorName
        {
            get => _arbitratorName;
            set { if (_arbitratorName != value) { _arbitratorName = value; Notify(nameof(ArbitratorName)); } }
        }
    }
}

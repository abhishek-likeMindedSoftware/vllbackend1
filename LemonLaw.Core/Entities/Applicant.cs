using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using LemonLaw.Core.Enums;
using System.ComponentModel;

namespace LemonLaw.Core.Entities;

[DefaultProperty(nameof(FullName))]
[XafDisplayName("Applicant")]
public class Applicant : AuditDetails
{
    [Browsable(false)]
    [VisibleInListView(false)]
    public virtual Guid Id { get; set; }

    [Browsable(false)]
    public virtual Guid ApplicationId { get; set; }

    [Browsable(false)]
    public virtual Application? Application { get; set; }

    // ── Name ──────────────────────────────────────────────────────────────────

    private string _firstName = string.Empty;
    [XafDisplayName("First Name")]
    public virtual string FirstName
    {
        get => _firstName;
        set { if (_firstName != value) { RaisePropertyChanging(nameof(FirstName)); _firstName = value; RaisePropertyChanged(nameof(FirstName)); RaisePropertyChanged(nameof(FullName)); } }
    }

    private string? _middleName;
    [XafDisplayName("Middle Name")]
    [VisibleInListView(false)]
    public virtual string? MiddleName
    {
        get => _middleName;
        set { if (_middleName != value) { RaisePropertyChanging(nameof(MiddleName)); _middleName = value; RaisePropertyChanged(nameof(MiddleName)); } }
    }

    private string _lastName = string.Empty;
    [XafDisplayName("Last Name")]
    public virtual string LastName
    {
        get => _lastName;
        set { if (_lastName != value) { RaisePropertyChanging(nameof(LastName)); _lastName = value; RaisePropertyChanged(nameof(LastName)); RaisePropertyChanged(nameof(FullName)); } }
    }

    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    [XafDisplayName("Full Name")]
    [VisibleInListView(true)]
    public string FullName => $"{FirstName} {LastName}".Trim();

    // ── Contact ───────────────────────────────────────────────────────────────

    private string _emailAddress = string.Empty;
    [XafDisplayName("Email Address")]
    public virtual string EmailAddress
    {
        get => _emailAddress;
        set { if (_emailAddress != value) { RaisePropertyChanging(nameof(EmailAddress)); _emailAddress = value; RaisePropertyChanged(nameof(EmailAddress)); } }
    }

    [XafDisplayName("Phone Number")]
    public virtual string PhoneNumber { get; set; } = string.Empty;

    [XafDisplayName("Phone Type")]
    [VisibleInListView(false)]
    public virtual PhoneType PhoneType { get; set; }

    [XafDisplayName("Preferred Contact")]
    [VisibleInListView(false)]
    public virtual PreferredContactMethod PreferredContact { get; set; } = PreferredContactMethod.EMAIL;

    // ── Address ───────────────────────────────────────────────────────────────

    [XafDisplayName("Address Line 1")]
    [VisibleInListView(false)]
    public virtual string AddressLine1 { get; set; } = string.Empty;

    [XafDisplayName("Address Line 2")]
    [VisibleInListView(false)]
    public virtual string? AddressLine2 { get; set; }

    [XafDisplayName("City")]
    [VisibleInListView(false)]
    public virtual string City { get; set; } = string.Empty;

    [XafDisplayName("State")]
    [VisibleInListView(false)]
    public virtual string State { get; set; } = string.Empty;

    [XafDisplayName("ZIP Code")]
    [VisibleInListView(false)]
    public virtual string ZipCode { get; set; } = string.Empty;

    [Browsable(false)]
    public virtual string? GooglePlaceId { get; set; }
}

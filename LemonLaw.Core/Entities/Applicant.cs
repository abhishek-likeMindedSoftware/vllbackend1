using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using LemonLaw.Core.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LemonLaw.Core.Entities;

[DefaultProperty(nameof(FullName))]
[XafDisplayName("Applicant")]
public class Applicant : AuditDetails, INotifyPropertyChanged, IObjectSpaceLink
{
    #region XAF
    public new event PropertyChangedEventHandler PropertyChanged;
    protected new void RaisePropertyChanged(string propertyName)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    private IObjectSpace _objectSpace;

    [NotMapped, Browsable(false)]
    public virtual IObjectSpace ObjectSpace
    {
        get => _objectSpace;
        set
        {
            if (_objectSpace != value)
            {
                _objectSpace = value;
                RaisePropertyChanged(nameof(ObjectSpace));
            }
        }
    }
    #endregion

    public Applicant() { }

    private Guid _id = Guid.NewGuid();

    [Key]
    [XafDisplayName("ID")]
    [VisibleInDetailView(false), VisibleInListView(false)]
    public virtual Guid Id
    {
        get => _id;
        set
        {
            if (_id != value)
            {
                RaisePropertyChanging(nameof(Id));
                _id = value;
                RaisePropertyChanged(nameof(Id));
            }
        }
    }

    #region Application Relationship (M:1)
    private Guid _applicationId;

    [Browsable(false)]
    public virtual Guid ApplicationId
    {
        get => _applicationId;
        set
        {
            if (_applicationId != value)
            {
                RaisePropertyChanging(nameof(ApplicationId));
                _applicationId = value;
                RaisePropertyChanged(nameof(ApplicationId));
            }
        }
    }

    private Application? _application;

    [ForeignKey(nameof(ApplicationId))]
    [Browsable(false)]
    public virtual Application? Application
    {
        get => _application;
        set
        {
            if (_application != value)
            {
                RaisePropertyChanging(nameof(Application));
                _application = value;
                ApplicationId = value?.Id ?? Guid.Empty;
                RaisePropertyChanged(nameof(Application));
            }
        }
    }
    #endregion

    #region Name

    private string _firstName = string.Empty;

    [XafDisplayName("First Name")]
    public virtual string FirstName
    {
        get => _firstName;
        set
        {
            if (_firstName != value)
            {
                RaisePropertyChanging(nameof(FirstName));
                _firstName = value;
                RaisePropertyChanged(nameof(FirstName));
                RaisePropertyChanged(nameof(FullName));
            }
        }
    }

    private string? _middleName;

    [XafDisplayName("Middle Name")]
    [VisibleInListView(false)]
    public virtual string? MiddleName
    {
        get => _middleName;
        set
        {
            if (_middleName != value)
            {
                RaisePropertyChanging(nameof(MiddleName));
                _middleName = value;
                RaisePropertyChanged(nameof(MiddleName));
            }
        }
    }

    private string _lastName = string.Empty;

    [XafDisplayName("Last Name")]
    public virtual string LastName
    {
        get => _lastName;
        set
        {
            if (_lastName != value)
            {
                RaisePropertyChanging(nameof(LastName));
                _lastName = value;
                RaisePropertyChanged(nameof(LastName));
                RaisePropertyChanged(nameof(FullName));
            }
        }
    }

    [NotMapped]
    [XafDisplayName("Full Name")]
    [VisibleInListView(true)]
    public string FullName => $"{FirstName} {LastName}".Trim();

    #endregion

    #region Contact

    private string _emailAddress = string.Empty;

    [XafDisplayName("Email Address")]
    public virtual string EmailAddress
    {
        get => _emailAddress;
        set
        {
            if (_emailAddress != value)
            {
                RaisePropertyChanging(nameof(EmailAddress));
                _emailAddress = value;
                RaisePropertyChanged(nameof(EmailAddress));
            }
        }
    }

    private string _phoneNumber = string.Empty;

    [XafDisplayName("Phone Number")]
    public virtual string PhoneNumber
    {
        get => _phoneNumber;
        set
        {
            if (_phoneNumber != value)
            {
                RaisePropertyChanging(nameof(PhoneNumber));
                _phoneNumber = value;
                RaisePropertyChanged(nameof(PhoneNumber));
            }
        }
    }

    private PhoneType _phoneType;

    [XafDisplayName("Phone Type")]
    [VisibleInListView(false)]
    public virtual PhoneType PhoneType
    {
        get => _phoneType;
        set
        {
            if (_phoneType != value)
            {
                RaisePropertyChanging(nameof(PhoneType));
                _phoneType = value;
                RaisePropertyChanged(nameof(PhoneType));
            }
        }
    }

    private PreferredContactMethod _preferredContact = PreferredContactMethod.EMAIL;

    [XafDisplayName("Preferred Contact")]
    [VisibleInListView(false)]
    public virtual PreferredContactMethod PreferredContact
    {
        get => _preferredContact;
        set
        {
            if (_preferredContact != value)
            {
                RaisePropertyChanging(nameof(PreferredContact));
                _preferredContact = value;
                RaisePropertyChanged(nameof(PreferredContact));
            }
        }
    }

    #endregion

    #region Address

    private string _addressLine1 = string.Empty;

    [XafDisplayName("Address Line 1")]
    [VisibleInListView(false)]
    public virtual string AddressLine1
    {
        get => _addressLine1;
        set
        {
            if (_addressLine1 != value)
            {
                RaisePropertyChanging(nameof(AddressLine1));
                _addressLine1 = value;
                RaisePropertyChanged(nameof(AddressLine1));
            }
        }
    }

    private string? _addressLine2;

    [XafDisplayName("Address Line 2")]
    [VisibleInListView(false)]
    public virtual string? AddressLine2
    {
        get => _addressLine2;
        set
        {
            if (_addressLine2 != value)
            {
                RaisePropertyChanging(nameof(AddressLine2));
                _addressLine2 = value;
                RaisePropertyChanged(nameof(AddressLine2));
            }
        }
    }

    private string _city = string.Empty;

    [XafDisplayName("City")]
    [VisibleInListView(false)]
    public virtual string City
    {
        get => _city;
        set
        {
            if (_city != value)
            {
                RaisePropertyChanging(nameof(City));
                _city = value;
                RaisePropertyChanged(nameof(City));
            }
        }
    }

    private string _addressState = string.Empty;

    [XafDisplayName("State")]
    [VisibleInListView(false)]
    public virtual string AddressState
    {
        get => _addressState;
        set
        {
            if (_addressState != value)
            {
                RaisePropertyChanging(nameof(AddressState));
                _addressState = value;
                RaisePropertyChanged(nameof(AddressState));
            }
        }
    }

    private string _zipCode = string.Empty;

    [XafDisplayName("ZIP Code")]
    [VisibleInListView(false)]
    public virtual string ZipCode
    {
        get => _zipCode;
        set
        {
            if (_zipCode != value)
            {
                RaisePropertyChanging(nameof(ZipCode));
                _zipCode = value;
                RaisePropertyChanged(nameof(ZipCode));
            }
        }
    }

    private string? _googlePlaceId;

    [Browsable(false)]
    public virtual string? GooglePlaceId
    {
        get => _googlePlaceId;
        set
        {
            if (_googlePlaceId != value)
            {
                RaisePropertyChanging(nameof(GooglePlaceId));
                _googlePlaceId = value;
                RaisePropertyChanged(nameof(GooglePlaceId));
            }
        }
    }

    #endregion
}

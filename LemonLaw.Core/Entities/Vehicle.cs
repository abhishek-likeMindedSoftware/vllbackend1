using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using LemonLaw.Core.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LemonLaw.Core.Entities;

[DefaultProperty(nameof(VehicleDisplay))]
[XafDisplayName("Vehicle")]
public class Vehicle : AuditDetails, INotifyPropertyChanged, IObjectSpaceLink
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

    public Vehicle() { }

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

    #region VIN

    private string _vin = string.Empty;

    [XafDisplayName("VIN")]
    public virtual string VIN
    {
        get => _vin;
        set
        {
            if (_vin != value)
            {
                RaisePropertyChanging(nameof(VIN));
                _vin = value;
                RaisePropertyChanged(nameof(VIN));
            }
        }
    }

    private bool _vinConfirmed;

    [XafDisplayName("VIN Confirmed")]
    [VisibleInListView(false)]
    public virtual bool VinConfirmed
    {
        get => _vinConfirmed;
        set
        {
            if (_vinConfirmed != value)
            {
                RaisePropertyChanging(nameof(VinConfirmed));
                _vinConfirmed = value;
                RaisePropertyChanged(nameof(VinConfirmed));
            }
        }
    }

    private short? _vinDecodedYear;

    [XafDisplayName("Decoded Year")]
    [VisibleInListView(false)]
    public virtual short? VinDecoded_Year
    {
        get => _vinDecodedYear;
        set
        {
            if (_vinDecodedYear != value)
            {
                RaisePropertyChanging(nameof(VinDecoded_Year));
                _vinDecodedYear = value;
                RaisePropertyChanged(nameof(VinDecoded_Year));
            }
        }
    }

    private string? _vinDecodedMake;

    [XafDisplayName("Decoded Make")]
    [VisibleInListView(false)]
    public virtual string? VinDecoded_Make
    {
        get => _vinDecodedMake;
        set
        {
            if (_vinDecodedMake != value)
            {
                RaisePropertyChanging(nameof(VinDecoded_Make));
                _vinDecodedMake = value;
                RaisePropertyChanged(nameof(VinDecoded_Make));
            }
        }
    }

    private string? _vinDecodedModel;

    [XafDisplayName("Decoded Model")]
    [VisibleInListView(false)]
    public virtual string? VinDecoded_Model
    {
        get => _vinDecodedModel;
        set
        {
            if (_vinDecodedModel != value)
            {
                RaisePropertyChanging(nameof(VinDecoded_Model));
                _vinDecodedModel = value;
                RaisePropertyChanged(nameof(VinDecoded_Model));
            }
        }
    }

    private string? _vinDecodedTrim;

    [XafDisplayName("Decoded Trim")]
    [VisibleInListView(false)]
    public virtual string? VinDecoded_Trim
    {
        get => _vinDecodedTrim;
        set
        {
            if (_vinDecodedTrim != value)
            {
                RaisePropertyChanging(nameof(VinDecoded_Trim));
                _vinDecodedTrim = value;
                RaisePropertyChanged(nameof(VinDecoded_Trim));
            }
        }
    }

    #endregion

    #region Vehicle Details

    private short _vehicleYear;

    [XafDisplayName("Year")]
    public virtual short VehicleYear
    {
        get => _vehicleYear;
        set
        {
            if (_vehicleYear != value)
            {
                RaisePropertyChanging(nameof(VehicleYear));
                _vehicleYear = value;
                RaisePropertyChanged(nameof(VehicleYear));
                RaisePropertyChanged(nameof(VehicleDisplay));
            }
        }
    }

    private string _vehicleMake = string.Empty;

    [XafDisplayName("Make")]
    public virtual string VehicleMake
    {
        get => _vehicleMake;
        set
        {
            if (_vehicleMake != value)
            {
                RaisePropertyChanging(nameof(VehicleMake));
                _vehicleMake = value;
                RaisePropertyChanged(nameof(VehicleMake));
                RaisePropertyChanged(nameof(VehicleDisplay));
            }
        }
    }

    private string _vehicleModel = string.Empty;

    [XafDisplayName("Model")]
    public virtual string VehicleModel
    {
        get => _vehicleModel;
        set
        {
            if (_vehicleModel != value)
            {
                RaisePropertyChanging(nameof(VehicleModel));
                _vehicleModel = value;
                RaisePropertyChanged(nameof(VehicleModel));
                RaisePropertyChanged(nameof(VehicleDisplay));
            }
        }
    }

    private string? _vehicleColor;

    [XafDisplayName("Color")]
    [VisibleInListView(false)]
    public virtual string? VehicleColor
    {
        get => _vehicleColor;
        set
        {
            if (_vehicleColor != value)
            {
                RaisePropertyChanging(nameof(VehicleColor));
                _vehicleColor = value;
                RaisePropertyChanged(nameof(VehicleColor));
            }
        }
    }

    private string? _licensePlate;

    [XafDisplayName("License Plate")]
    [VisibleInListView(false)]
    public virtual string? LicensePlate
    {
        get => _licensePlate;
        set
        {
            if (_licensePlate != value)
            {
                RaisePropertyChanging(nameof(LicensePlate));
                _licensePlate = value;
                RaisePropertyChanged(nameof(LicensePlate));
            }
        }
    }

    private string? _licensePlateState;

    [XafDisplayName("Plate State")]
    [VisibleInListView(false)]
    public virtual string? LicensePlateState
    {
        get => _licensePlateState;
        set
        {
            if (_licensePlateState != value)
            {
                RaisePropertyChanging(nameof(LicensePlateState));
                _licensePlateState = value;
                RaisePropertyChanged(nameof(LicensePlateState));
            }
        }
    }

    [NotMapped]
    [XafDisplayName("Vehicle")]
    [VisibleInListView(true)]
    public string VehicleDisplay => $"{VehicleYear} {VehicleMake} {VehicleModel}".Trim();

    #endregion

    #region Purchase

    private DateOnly _purchaseDate;

    [XafDisplayName("Purchase Date")]
    [VisibleInListView(false)]
    public virtual DateOnly PurchaseDate
    {
        get => _purchaseDate;
        set
        {
            if (_purchaseDate != value)
            {
                RaisePropertyChanging(nameof(PurchaseDate));
                _purchaseDate = value;
                RaisePropertyChanged(nameof(PurchaseDate));
            }
        }
    }

    private decimal? _purchasePrice;

    [XafDisplayName("Purchase Price")]
    [VisibleInListView(false)]
    public virtual decimal? PurchasePrice
    {
        get => _purchasePrice;
        set
        {
            if (_purchasePrice != value)
            {
                RaisePropertyChanging(nameof(PurchasePrice));
                _purchasePrice = value;
                RaisePropertyChanged(nameof(PurchasePrice));
            }
        }
    }

    private int _mileageAtPurchase;

    [XafDisplayName("Mileage at Purchase")]
    [VisibleInListView(false)]
    public virtual int MileageAtPurchase
    {
        get => _mileageAtPurchase;
        set
        {
            if (_mileageAtPurchase != value)
            {
                RaisePropertyChanging(nameof(MileageAtPurchase));
                _mileageAtPurchase = value;
                RaisePropertyChanged(nameof(MileageAtPurchase));
            }
        }
    }

    private int _currentMileage;

    [XafDisplayName("Current Mileage")]
    [VisibleInListView(false)]
    public virtual int CurrentMileage
    {
        get => _currentMileage;
        set
        {
            if (_currentMileage != value)
            {
                RaisePropertyChanging(nameof(CurrentMileage));
                _currentMileage = value;
                RaisePropertyChanged(nameof(CurrentMileage));
            }
        }
    }

    #endregion

    #region Dealer

    private string _dealerName = string.Empty;

    [XafDisplayName("Dealer Name")]
    public virtual string DealerName
    {
        get => _dealerName;
        set
        {
            if (_dealerName != value)
            {
                RaisePropertyChanging(nameof(DealerName));
                _dealerName = value;
                RaisePropertyChanged(nameof(DealerName));
            }
        }
    }

    private string _dealerAddressLine1 = string.Empty;

    [XafDisplayName("Dealer Address")]
    [VisibleInListView(false)]
    public virtual string DealerAddressLine1
    {
        get => _dealerAddressLine1;
        set
        {
            if (_dealerAddressLine1 != value)
            {
                RaisePropertyChanging(nameof(DealerAddressLine1));
                _dealerAddressLine1 = value;
                RaisePropertyChanged(nameof(DealerAddressLine1));
            }
        }
    }

    private string? _dealerAddressLine2;

    [VisibleInListView(false)]
    public virtual string? DealerAddressLine2
    {
        get => _dealerAddressLine2;
        set
        {
            if (_dealerAddressLine2 != value)
            {
                RaisePropertyChanging(nameof(DealerAddressLine2));
                _dealerAddressLine2 = value;
                RaisePropertyChanged(nameof(DealerAddressLine2));
            }
        }
    }

    private string _dealerCity = string.Empty;

    [XafDisplayName("Dealer City")]
    [VisibleInListView(false)]
    public virtual string DealerCity
    {
        get => _dealerCity;
        set
        {
            if (_dealerCity != value)
            {
                RaisePropertyChanging(nameof(DealerCity));
                _dealerCity = value;
                RaisePropertyChanged(nameof(DealerCity));
            }
        }
    }

    private string _dealerState = string.Empty;

    [XafDisplayName("Dealer State")]
    [VisibleInListView(false)]
    public virtual string DealerState
    {
        get => _dealerState;
        set
        {
            if (_dealerState != value)
            {
                RaisePropertyChanging(nameof(DealerState));
                _dealerState = value;
                RaisePropertyChanged(nameof(DealerState));
            }
        }
    }

    private string _dealerZip = string.Empty;

    [XafDisplayName("Dealer ZIP")]
    [VisibleInListView(false)]
    public virtual string DealerZip
    {
        get => _dealerZip;
        set
        {
            if (_dealerZip != value)
            {
                RaisePropertyChanging(nameof(DealerZip));
                _dealerZip = value;
                RaisePropertyChanged(nameof(DealerZip));
            }
        }
    }

    private string? _dealerPhone;

    [XafDisplayName("Dealer Phone")]
    [VisibleInListView(false)]
    public virtual string? DealerPhone
    {
        get => _dealerPhone;
        set
        {
            if (_dealerPhone != value)
            {
                RaisePropertyChanging(nameof(DealerPhone));
                _dealerPhone = value;
                RaisePropertyChanged(nameof(DealerPhone));
            }
        }
    }

    private string? _dealerEmail;

    [XafDisplayName("Dealer Email")]
    [VisibleInListView(false)]
    public virtual string? DealerEmail
    {
        get => _dealerEmail;
        set
        {
            if (_dealerEmail != value)
            {
                RaisePropertyChanging(nameof(DealerEmail));
                _dealerEmail = value;
                RaisePropertyChanged(nameof(DealerEmail));
            }
        }
    }

    private string? _dealerGooglePlaceId;

    [Browsable(false)]
    public virtual string? DealerGooglePlaceId
    {
        get => _dealerGooglePlaceId;
        set
        {
            if (_dealerGooglePlaceId != value)
            {
                RaisePropertyChanging(nameof(DealerGooglePlaceId));
                _dealerGooglePlaceId = value;
                RaisePropertyChanged(nameof(DealerGooglePlaceId));
            }
        }
    }

    #endregion

    #region Warranty

    private string _manufacturerName = string.Empty;

    [XafDisplayName("Manufacturer")]
    public virtual string ManufacturerName
    {
        get => _manufacturerName;
        set
        {
            if (_manufacturerName != value)
            {
                RaisePropertyChanging(nameof(ManufacturerName));
                _manufacturerName = value;
                RaisePropertyChanged(nameof(ManufacturerName));
            }
        }
    }

    private WarrantyType _warrantyType;

    [XafDisplayName("Warranty Type")]
    [VisibleInListView(false)]
    public virtual WarrantyType WarrantyType
    {
        get => _warrantyType;
        set
        {
            if (_warrantyType != value)
            {
                RaisePropertyChanging(nameof(WarrantyType));
                _warrantyType = value;
                RaisePropertyChanged(nameof(WarrantyType));
            }
        }
    }

    private DateOnly? _warrantyStartDate;

    [XafDisplayName("Warranty Start")]
    [VisibleInListView(false)]
    public virtual DateOnly? WarrantyStartDate
    {
        get => _warrantyStartDate;
        set
        {
            if (_warrantyStartDate != value)
            {
                RaisePropertyChanging(nameof(WarrantyStartDate));
                _warrantyStartDate = value;
                RaisePropertyChanged(nameof(WarrantyStartDate));
            }
        }
    }

    private DateOnly? _warrantyExpiryDate;

    [XafDisplayName("Warranty Expiry")]
    [VisibleInListView(false)]
    public virtual DateOnly? WarrantyExpiryDate
    {
        get => _warrantyExpiryDate;
        set
        {
            if (_warrantyExpiryDate != value)
            {
                RaisePropertyChanging(nameof(WarrantyExpiryDate));
                _warrantyExpiryDate = value;
                RaisePropertyChanged(nameof(WarrantyExpiryDate));
            }
        }
    }

    #endregion
}

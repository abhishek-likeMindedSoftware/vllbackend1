using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using LemonLaw.Core.Enums;
using System.ComponentModel;

namespace LemonLaw.Core.Entities;

[DefaultProperty(nameof(VehicleDisplay))]
[XafDisplayName("Vehicle")]
public class Vehicle : AuditDetails
{
    [Browsable(false)]
    [VisibleInListView(false)]
    public virtual Guid Id { get; set; }

    [Browsable(false)]
    public virtual Guid ApplicationId { get; set; }

    [Browsable(false)]
    public virtual Application? Application { get; set; }

    // ── VIN ───────────────────────────────────────────────────────────────────

    private string _vin = string.Empty;
    [XafDisplayName("VIN")]
    public virtual string VIN
    {
        get => _vin;
        set { if (_vin != value) { RaisePropertyChanging(nameof(VIN)); _vin = value; RaisePropertyChanged(nameof(VIN)); } }
    }

    [XafDisplayName("VIN Confirmed")]
    [VisibleInListView(false)]
    public virtual bool VinConfirmed { get; set; } = false;

    [XafDisplayName("Decoded Year")]
    [VisibleInListView(false)]
    public virtual short? VinDecoded_Year { get; set; }

    [XafDisplayName("Decoded Make")]
    [VisibleInListView(false)]
    public virtual string? VinDecoded_Make { get; set; }

    [XafDisplayName("Decoded Model")]
    [VisibleInListView(false)]
    public virtual string? VinDecoded_Model { get; set; }

    [XafDisplayName("Decoded Trim")]
    [VisibleInListView(false)]
    public virtual string? VinDecoded_Trim { get; set; }

    // ── Vehicle details ───────────────────────────────────────────────────────

    private short _vehicleYear;
    [XafDisplayName("Year")]
    public virtual short VehicleYear
    {
        get => _vehicleYear;
        set { if (_vehicleYear != value) { RaisePropertyChanging(nameof(VehicleYear)); _vehicleYear = value; RaisePropertyChanged(nameof(VehicleYear)); RaisePropertyChanged(nameof(VehicleDisplay)); } }
    }

    private string _vehicleMake = string.Empty;
    [XafDisplayName("Make")]
    public virtual string VehicleMake
    {
        get => _vehicleMake;
        set { if (_vehicleMake != value) { RaisePropertyChanging(nameof(VehicleMake)); _vehicleMake = value; RaisePropertyChanged(nameof(VehicleMake)); RaisePropertyChanged(nameof(VehicleDisplay)); } }
    }

    private string _vehicleModel = string.Empty;
    [XafDisplayName("Model")]
    public virtual string VehicleModel
    {
        get => _vehicleModel;
        set { if (_vehicleModel != value) { RaisePropertyChanging(nameof(VehicleModel)); _vehicleModel = value; RaisePropertyChanged(nameof(VehicleModel)); RaisePropertyChanged(nameof(VehicleDisplay)); } }
    }

    [XafDisplayName("Color")]
    [VisibleInListView(false)]
    public virtual string? VehicleColor { get; set; }

    [XafDisplayName("License Plate")]
    [VisibleInListView(false)]
    public virtual string? LicensePlate { get; set; }

    [XafDisplayName("Plate State")]
    [VisibleInListView(false)]
    public virtual string? LicensePlateState { get; set; }

    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    [XafDisplayName("Vehicle")]
    [VisibleInListView(true)]
    public string VehicleDisplay => $"{VehicleYear} {VehicleMake} {VehicleModel}".Trim();

    // ── Purchase ──────────────────────────────────────────────────────────────

    [XafDisplayName("Purchase Date")]
    [VisibleInListView(false)]
    public virtual DateOnly PurchaseDate { get; set; }

    [XafDisplayName("Purchase Price")]
    [VisibleInListView(false)]
    public virtual decimal? PurchasePrice { get; set; }

    [XafDisplayName("Mileage at Purchase")]
    [VisibleInListView(false)]
    public virtual int MileageAtPurchase { get; set; }

    [XafDisplayName("Current Mileage")]
    [VisibleInListView(false)]
    public virtual int CurrentMileage { get; set; }

    // ── Dealer ────────────────────────────────────────────────────────────────

    [XafDisplayName("Dealer Name")]
    public virtual string DealerName { get; set; } = string.Empty;

    [XafDisplayName("Dealer Address")]
    [VisibleInListView(false)]
    public virtual string DealerAddressLine1 { get; set; } = string.Empty;

    [VisibleInListView(false)]
    public virtual string? DealerAddressLine2 { get; set; }

    [XafDisplayName("Dealer City")]
    [VisibleInListView(false)]
    public virtual string DealerCity { get; set; } = string.Empty;

    [XafDisplayName("Dealer State")]
    [VisibleInListView(false)]
    public virtual string DealerState { get; set; } = string.Empty;

    [XafDisplayName("Dealer ZIP")]
    [VisibleInListView(false)]
    public virtual string DealerZip { get; set; } = string.Empty;

    [XafDisplayName("Dealer Phone")]
    [VisibleInListView(false)]
    public virtual string? DealerPhone { get; set; }

    [XafDisplayName("Dealer Email")]
    [VisibleInListView(false)]
    public virtual string? DealerEmail { get; set; }

    [Browsable(false)]
    public virtual string? DealerGooglePlaceId { get; set; }

    // ── Warranty ──────────────────────────────────────────────────────────────

    [XafDisplayName("Manufacturer")]
    public virtual string ManufacturerName { get; set; } = string.Empty;

    [XafDisplayName("Warranty Type")]
    [VisibleInListView(false)]
    public virtual WarrantyType WarrantyType { get; set; }

    [XafDisplayName("Warranty Start")]
    [VisibleInListView(false)]
    public virtual DateOnly? WarrantyStartDate { get; set; }

    [XafDisplayName("Warranty Expiry")]
    [VisibleInListView(false)]
    public virtual DateOnly? WarrantyExpiryDate { get; set; }
}

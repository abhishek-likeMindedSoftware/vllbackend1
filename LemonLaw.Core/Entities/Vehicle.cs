using LemonLaw.Core.Enums;

namespace LemonLaw.Core.Entities;

public class Vehicle : AuditDetails
{
    public virtual Guid Id { get; set; }
    public virtual Guid ApplicationId { get; set; }
    public virtual Application? Application { get; set; }

    public virtual string VIN { get; set; } = string.Empty;
    public virtual short VehicleYear { get; set; }
    public virtual string VehicleMake { get; set; } = string.Empty;
    public virtual string VehicleModel { get; set; } = string.Empty;
    public virtual string? VehicleColor { get; set; }
    public virtual string? LicensePlate { get; set; }
    public virtual string? LicensePlateState { get; set; }
    public virtual DateOnly PurchaseDate { get; set; }
    public virtual decimal? PurchasePrice { get; set; }
    public virtual int MileageAtPurchase { get; set; }
    public virtual int CurrentMileage { get; set; }

    // VIN Decode
    public virtual short? VinDecoded_Year { get; set; }
    public virtual string? VinDecoded_Make { get; set; }
    public virtual string? VinDecoded_Model { get; set; }
    public virtual string? VinDecoded_Trim { get; set; }
    public virtual bool VinConfirmed { get; set; } = false;

    // Dealer
    public virtual string DealerName { get; set; } = string.Empty;
    public virtual string DealerAddressLine1 { get; set; } = string.Empty;
    public virtual string? DealerAddressLine2 { get; set; }
    public virtual string DealerCity { get; set; } = string.Empty;
    public virtual string DealerState { get; set; } = string.Empty;
    public virtual string DealerZip { get; set; } = string.Empty;
    public virtual string? DealerPhone { get; set; }
    public virtual string? DealerEmail { get; set; }
    public virtual string? DealerGooglePlaceId { get; set; }

    // Manufacturer & Warranty
    public virtual string ManufacturerName { get; set; } = string.Empty;
    public virtual WarrantyType WarrantyType { get; set; }
    public virtual DateOnly? WarrantyStartDate { get; set; }
    public virtual DateOnly? WarrantyExpiryDate { get; set; }
}

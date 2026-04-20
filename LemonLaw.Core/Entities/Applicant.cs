using LemonLaw.Core.Enums;

namespace LemonLaw.Core.Entities;

public class Applicant : AuditDetails
{
    public virtual Guid Id { get; set; }
    public virtual Guid ApplicationId { get; set; }
    public virtual Application? Application { get; set; }

    public virtual string FirstName { get; set; } = string.Empty;
    public virtual string? MiddleName { get; set; }
    public virtual string LastName { get; set; } = string.Empty;
    public virtual string EmailAddress { get; set; } = string.Empty;
    public virtual string PhoneNumber { get; set; } = string.Empty;
    public virtual PhoneType PhoneType { get; set; }
    public virtual string AddressLine1 { get; set; } = string.Empty;
    public virtual string? AddressLine2 { get; set; }
    public virtual string City { get; set; } = string.Empty;
    public virtual string State { get; set; } = string.Empty;
    public virtual string ZipCode { get; set; } = string.Empty;
    public virtual string? GooglePlaceId { get; set; }
    public virtual PreferredContactMethod PreferredContact { get; set; } = PreferredContactMethod.EMAIL;
}

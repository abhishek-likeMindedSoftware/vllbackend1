using System.ComponentModel;
using DevExpress.ExpressApp.DC;

namespace LemonLaw.Core.Enums;

public enum ApplicationType
{
    [XafDisplayName("New Car")] [Description("New Car")] NEW_CAR,
    [XafDisplayName("Used Car")] [Description("Used Car")] USED_CAR,
    [XafDisplayName("Leased")] [Description("Leased")] LEASED
}

public enum ApplicationStatus
{
    [XafDisplayName("Submitted")] [Description("Submitted")] SUBMITTED,
    [XafDisplayName("Incomplete")] [Description("Incomplete")] INCOMPLETE,
    [XafDisplayName("Accepted")] [Description("Accepted")] ACCEPTED,
    [XafDisplayName("Dealer Responded")] [Description("Dealer Responded")] DEALER_RESPONDED,
    [XafDisplayName("Hearing Scheduled")] [Description("Hearing Scheduled")] HEARING_SCHEDULED,
    [XafDisplayName("Hearing Complete")] [Description("Hearing Complete")] HEARING_COMPLETE,
    [XafDisplayName("Decision Issued")] [Description("Decision Issued")] DECISION_ISSUED,
    [XafDisplayName("Withdrawn")] [Description("Withdrawn")] WITHDRAWN,
    [XafDisplayName("Closed")] [Description("Closed")] CLOSED
}

public enum PhoneType
{
    [XafDisplayName("Mobile")] [Description("Mobile")] MOBILE,
    [XafDisplayName("Home")] [Description("Home")] HOME,
    [XafDisplayName("Work")] [Description("Work")] WORK
}

public enum PreferredContactMethod
{
    [XafDisplayName("Email")] [Description("Email")] EMAIL,
    [XafDisplayName("Phone")] [Description("Phone")] PHONE
}

public enum WarrantyType
{
    [XafDisplayName("Manufacturer's Warranty")] [Description("Manufacturer's Warranty")] MANUFACTURERS_WARRANTY,
    [XafDisplayName("Implied Warranty")] [Description("Implied Warranty")] IMPLIED_WARRANTY,
    [XafDisplayName("Extended Warranty")] [Description("Extended Warranty")] EXTENDED_WARRANTY
}

public enum DefectCategory
{
    [XafDisplayName("Safety Defect")] [Description("Safety Defect")] SAFETY_DEFECT,
    [XafDisplayName("Major Defect")] [Description("Major Defect")] MAJOR_DEFECT,
    [XafDisplayName("Minor Defect")] [Description("Minor Defect")] MINOR_DEFECT,
    [XafDisplayName("Brakes")] [Description("Brakes")] BRAKES,
    [XafDisplayName("Steering")] [Description("Steering")] STEERING,
    [XafDisplayName("Engine")] [Description("Engine")] ENGINE,
    [XafDisplayName("Electrical")] [Description("Electrical")] ELECTRICAL,
    [XafDisplayName("Transmission")] [Description("Transmission")] TRANSMISSION,
    [XafDisplayName("Other")] [Description("Other")] OTHER
}

public enum ExpenseType
{
    [XafDisplayName("Rental Car")] [Description("Rental Car")] RENTAL_CAR,
    [XafDisplayName("Towing")] [Description("Towing")] TOWING,
    [XafDisplayName("Repair Cost")] [Description("Repair Cost")] REPAIR_COST,
    [XafDisplayName("Financing Cost")] [Description("Financing Cost")] FINANCING_COST,
    [XafDisplayName("Other")] [Description("Other")] OTHER
}

public enum DesiredResolution
{
    [XafDisplayName("Refund")] [Description("Refund")] REFUND,
    [XafDisplayName("Replacement")] [Description("Replacement")] REPLACEMENT,
    [XafDisplayName("Reimbursement")] [Description("Reimbursement")] REIMBURSEMENT,
    [XafDisplayName("Arbitration")] [Description("Arbitration")] ARBITRATION,
    [XafDisplayName("Unsure")] [Description("Unsure")] UNSURE
}

public enum DocumentType
{
    [XafDisplayName("Purchase Contract")] [Description("Purchase Contract")] PURCHASE_CONTRACT,
    [XafDisplayName("Lease Agreement")] [Description("Lease Agreement")] LEASE_AGREEMENT,
    [XafDisplayName("RMV-1 Registration")] [Description("RMV-1 Registration")] RMV1_REGISTRATION,
    [XafDisplayName("Repair Records")] [Description("Repair Records")] REPAIR_RECORDS,
    [XafDisplayName("Warranty Document")] [Description("Warranty Document")] WARRANTY_DOCUMENT,
    [XafDisplayName("Manufacturer Correspondence")] [Description("Manufacturer Correspondence")] MANUFACTURER_CORRESPONDENCE,
    [XafDisplayName("Dealer Correspondence")] [Description("Dealer Correspondence")] DEALER_CORRESPONDENCE,
    [XafDisplayName("Expense Receipt")] [Description("Expense Receipt")] EXPENSE_RECEIPT,
    [XafDisplayName("Repair Order")] [Description("Repair Order")] REPAIR_ORDER,
    [XafDisplayName("Dealer Statement")] [Description("Dealer Statement")] DEALER_STATEMENT,
    [XafDisplayName("Purchase Record")] [Description("Purchase Record")] PURCHASE_RECORD,
    [XafDisplayName("Other")] [Description("Other")] OTHER
}

public enum DocumentStatus
{
    [XafDisplayName("Pending Review")] [Description("Pending Review")] PENDING_REVIEW,
    [XafDisplayName("Accepted")] [Description("Accepted")] ACCEPTED,
    [XafDisplayName("Rejected")] [Description("Rejected")] REJECTED,
    [XafDisplayName("Requested")] [Description("Requested")] REQUESTED
}

public enum VirusScanResult
{
    [XafDisplayName("Pending")] [Description("Pending")] PENDING,
    [XafDisplayName("Clean")] [Description("Clean")] CLEAN,
    [XafDisplayName("Infected")] [Description("Infected")] INFECTED
}

public enum UploadedByRole
{
    [XafDisplayName("Consumer")] [Description("Consumer")] CONSUMER,
    [XafDisplayName("Staff")] [Description("Staff")] STAFF,
    [XafDisplayName("Dealer")] [Description("Dealer")] DEALER
}

public enum TokenType
{
    [XafDisplayName("Consumer")] [Description("Consumer")] CONSUMER,
    [XafDisplayName("Dealer")] [Description("Dealer")] DEALER
}

public enum EmailDeliveryStatus
{
    [XafDisplayName("Pending")] [Description("Pending")] PENDING,
    [XafDisplayName("Delivered")] [Description("Delivered")] DELIVERED,
    [XafDisplayName("Opened")] [Description("Opened")] OPENED,
    [XafDisplayName("Bounced")] [Description("Bounced")] BOUNCED,
    [XafDisplayName("Failed")] [Description("Failed")] FAILED
}

public enum CaseEventType
{
    [XafDisplayName("Status Change")] [Description("Status Change")] STATUS_CHANGE,
    [XafDisplayName("Document Uploaded")] [Description("Document Uploaded")] DOCUMENT_UPLOADED,
    [XafDisplayName("Document Accepted")] [Description("Document Accepted")] DOCUMENT_ACCEPTED,
    [XafDisplayName("Document Rejected")] [Description("Document Rejected")] DOCUMENT_REJECTED,
    [XafDisplayName("Document Requested")] [Description("Document Requested")] DOCUMENT_REQUESTED,
    [XafDisplayName("Dealer Notified")] [Description("Dealer Notified")] DEALER_NOTIFIED,
    [XafDisplayName("Dealer Responded")] [Description("Dealer Responded")] DEALER_RESPONDED,
    [XafDisplayName("Note Added")] [Description("Note Added")] NOTE_ADDED,
    [XafDisplayName("Assignment Changed")] [Description("Assignment Changed")] ASSIGNMENT_CHANGED,
    [XafDisplayName("Hearing Scheduled")] [Description("Hearing Scheduled")] HEARING_SCHEDULED,
    [XafDisplayName("Decision Issued")] [Description("Decision Issued")] DECISION_ISSUED,
    [XafDisplayName("Consumer Accessed Portal")] [Description("Consumer Accessed Portal")] CONSUMER_ACCESSED_PORTAL
}

public enum ActorType
{
    [XafDisplayName("Staff")] [Description("Staff")] STAFF,
    [XafDisplayName("Consumer")] [Description("Consumer")] CONSUMER,
    [XafDisplayName("Dealer")] [Description("Dealer")] DEALER,
    [XafDisplayName("System")] [Description("System")] SYSTEM
}

public enum HearingFormat
{
    [XafDisplayName("In Person")] [Description("In Person")] IN_PERSON,
    [XafDisplayName("Telephone")] [Description("Telephone")] TELEPHONE,
    [XafDisplayName("Video")] [Description("Video")] VIDEO
}

public enum HearingOutcome
{
    [XafDisplayName("Pending")] [Description("Pending")] PENDING,
    [XafDisplayName("Settled")] [Description("Settled")] SETTLED,
    [XafDisplayName("Decision For Consumer")] [Description("Decision For Consumer")] DECISION_FOR_CONSUMER,
    [XafDisplayName("Decision For Dealer")] [Description("Decision For Dealer")] DECISION_FOR_DEALER,
    [XafDisplayName("No Jurisdiction")] [Description("No Jurisdiction")] NO_JURISDICTION,
    [XafDisplayName("Withdrawn")] [Description("Withdrawn")] WITHDRAWN
}

public enum DecisionType
{
    [XafDisplayName("Refund Ordered")] [Description("Refund Ordered")] REFUND_ORDERED,
    [XafDisplayName("Replacement Ordered")] [Description("Replacement Ordered")] REPLACEMENT_ORDERED,
    [XafDisplayName("Reimbursement Ordered")] [Description("Reimbursement Ordered")] REIMBURSEMENT_ORDERED,
    [XafDisplayName("Claim Denied")] [Description("Claim Denied")] CLAIM_DENIED,
    [XafDisplayName("Settled Prior")] [Description("Settled Prior")] SETTLED_PRIOR,
    [XafDisplayName("Withdrawn")] [Description("Withdrawn")] WITHDRAWN
}

public enum OutreachType
{
    [XafDisplayName("Initial")] [Description("Initial")] INITIAL,
    [XafDisplayName("Follow Up 1")] [Description("Follow Up 1")] FOLLOW_UP_1,
    [XafDisplayName("Follow Up 2")] [Description("Follow Up 2")] FOLLOW_UP_2,
    [XafDisplayName("Final Notice")] [Description("Final Notice")] FINAL_NOTICE
}

public enum OutreachStatus
{
    [XafDisplayName("Pending")] [Description("Pending")] PENDING,
    [XafDisplayName("Sent")] [Description("Sent")] SENT,
    [XafDisplayName("Opened")] [Description("Opened")] OPENED,
    [XafDisplayName("Responded")] [Description("Responded")] RESPONDED,
    [XafDisplayName("Overdue")] [Description("Overdue")] OVERDUE,
    [XafDisplayName("Closed")] [Description("Closed")] CLOSED
}

public enum DealerPosition
{
    [XafDisplayName("Disputes Claim")] [Description("Disputes Claim")] DISPUTES_CLAIM,
    [XafDisplayName("Acknowledges Defect")] [Description("Acknowledges Defect")] ACKNOWLEDGES_DEFECT,
    [XafDisplayName("Offers Settlement")] [Description("Offers Settlement")] OFFERS_SETTLEMENT,
    [XafDisplayName("Resolved Prior")] [Description("Resolved Prior")] RESOLVED_PRIOR,
    [XafDisplayName("Vehicle Not Purchased Here")] [Description("Vehicle Not Purchased Here")] VEHICLE_NOT_PURCHASED_HERE,
    [XafDisplayName("Other")] [Description("Other")] OTHER
}

public enum CorrespondenceDirection
{
    [XafDisplayName("Outbound")] [Description("Outbound")] OUTBOUND,
    [XafDisplayName("Inbound")] [Description("Inbound")] INBOUND
}

public enum CorrespondenceRecipientType
{
    [XafDisplayName("Consumer")] [Description("Consumer")] CONSUMER,
    [XafDisplayName("Dealer")] [Description("Dealer")] DEALER,
    [XafDisplayName("Manufacturer")] [Description("Manufacturer")] MANUFACTURER,
    [XafDisplayName("Internal")] [Description("Internal")] INTERNAL
}

public enum StaffRole
{
    [XafDisplayName("OCABR Admin")] [Description("OCABR Admin")] OCABR_ADMIN,
    [XafDisplayName("Case Manager")] [Description("Case Manager")] CASE_MANAGER,
    [XafDisplayName("Reviewer")] [Description("Reviewer")] REVIEWER,
    [XafDisplayName("Supervisor")] [Description("Supervisor")] SUPERVISOR
}

public enum NotificationType
{
    [XafDisplayName("Submission Confirmation")] [Description("Submission Confirmation")] SUBMISSION_CONFIRMATION,
    [XafDisplayName("Email Verification")] [Description("Email Verification")] EMAIL_VERIFICATION,
    [XafDisplayName("Application Incomplete")] [Description("Application Incomplete")] APPLICATION_INCOMPLETE,
    [XafDisplayName("Document Rejected")] [Description("Document Rejected")] DOCUMENT_REJECTED,
    [XafDisplayName("Document Requested")] [Description("Document Requested")] DOCUMENT_REQUESTED,
    [XafDisplayName("Application Accepted")] [Description("Application Accepted")] APPLICATION_ACCEPTED,
    [XafDisplayName("Dealer Responded")] [Description("Dealer Responded")] DEALER_RESPONDED,
    [XafDisplayName("Hearing Scheduled")] [Description("Hearing Scheduled")] HEARING_SCHEDULED,
    [XafDisplayName("Hearing Reminder")] [Description("Hearing Reminder")] HEARING_REMINDER,
    [XafDisplayName("Decision Issued")] [Description("Decision Issued")] DECISION_ISSUED,
    [XafDisplayName("Case Closed")] [Description("Case Closed")] CASE_CLOSED
}

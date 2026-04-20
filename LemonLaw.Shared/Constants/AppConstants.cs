namespace LemonLaw.Shared.Constants;

public static class AppConstants
{
    public static class CaseNumber
    {
        public const string Prefix = "LL";
        public const int SequenceLength = 5;
    }

    public static class Token
    {
        public const int ConsumerTokenBytes = 64;
        public const int ConsumerExpiryYears = 2;
        public const int DealerExpiryDays = 30;
    }

    public static class Verification
    {
        public const int CodeExpiryMinutes = 15;
        public const int MaxResendAttempts = 3;
        public const int CodeLength = 6;
    }

    public static class Document
    {
        public const long MaxFileSizeBytes = 25 * 1024 * 1024; // 25 MB
        public static readonly string[] AllowedMimeTypes =
        [
            "application/pdf",
            "image/jpeg",
            "image/png",
            "image/tiff"
        ];
    }

    public static class Narrative
    {
        public const int MinLength = 50;
        public const int MaxLength = 5000;
    }

    public static class Pagination
    {
        public const int DefaultPageSize = 20;
        public const int MaxPageSize = 100;
    }

    public static class Aging
    {
        public const int GreenDays = 15;
        public const int AmberDays = 30;
    }

    public static class VinCache
    {
        public const int CacheHours = 24;
    }

    public static class TemplateCode
    {
        public const string DealerInitialOutreach = "DEALER_INITIAL_OUTREACH";
        public const string DealerFollowUp1 = "DEALER_FOLLOW_UP_1";
        public const string DealerFollowUp2 = "DEALER_FOLLOW_UP_2";
        public const string DealerFinalNotice = "DEALER_FINAL_NOTICE";
        public const string ConsumerSubmissionConfirmation = "CONSUMER_SUBMISSION_CONFIRMATION";
        public const string ConsumerEmailVerification = "CONSUMER_EMAIL_VERIFICATION";
        public const string ConsumerIncomplete = "CONSUMER_APPLICATION_INCOMPLETE";
        public const string ConsumerAccepted = "CONSUMER_APPLICATION_ACCEPTED";
        public const string ConsumerHearingScheduled = "CONSUMER_HEARING_SCHEDULED";
        public const string ConsumerDecisionIssued = "CONSUMER_DECISION_ISSUED";
    }
}

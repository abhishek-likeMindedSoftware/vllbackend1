using LemonLaw.Core.Enums;

namespace LemonLaw.Application.Services;

/// <summary>
/// Validates status transitions per spec §3.4.
/// Enforces the allowed transition matrix.
/// </summary>
public static class StatusTransitionValidator
{
    private static readonly Dictionary<ApplicationStatus, HashSet<ApplicationStatus>> AllowedTransitions = new()
    {
        [ApplicationStatus.SUBMITTED] = new()
        {
            ApplicationStatus.INCOMPLETE,
            ApplicationStatus.ACCEPTED,
            ApplicationStatus.WITHDRAWN
        },
        [ApplicationStatus.INCOMPLETE] = new()
        {
            ApplicationStatus.SUBMITTED, // Auto when consumer provides requested docs
            ApplicationStatus.WITHDRAWN
        },
        [ApplicationStatus.ACCEPTED] = new()
        {
            ApplicationStatus.DEALER_RESPONDED,
            ApplicationStatus.HEARING_SCHEDULED,
            ApplicationStatus.WITHDRAWN
        },
        [ApplicationStatus.DEALER_RESPONDED] = new()
        {
            ApplicationStatus.HEARING_SCHEDULED,
            ApplicationStatus.WITHDRAWN
        },
        [ApplicationStatus.HEARING_SCHEDULED] = new()
        {
            ApplicationStatus.HEARING_COMPLETE,
            ApplicationStatus.WITHDRAWN
        },
        [ApplicationStatus.HEARING_COMPLETE] = new()
        {
            ApplicationStatus.DECISION_ISSUED
        },
        [ApplicationStatus.DECISION_ISSUED] = new()
        {
            ApplicationStatus.CLOSED
        },
        [ApplicationStatus.WITHDRAWN] = new()
        {
            // Terminal state
        },
        [ApplicationStatus.CLOSED] = new()
        {
            // Terminal state
        }
    };

    public static bool IsTransitionAllowed(ApplicationStatus from, ApplicationStatus to)
    {
        if (!AllowedTransitions.TryGetValue(from, out var allowed))
            return false;
        return allowed.Contains(to);
    }

    public static string GetTransitionError(ApplicationStatus from, ApplicationStatus to)
    {
        return $"Cannot transition from {from} to {to}. This transition is not permitted by the workflow rules.";
    }
}

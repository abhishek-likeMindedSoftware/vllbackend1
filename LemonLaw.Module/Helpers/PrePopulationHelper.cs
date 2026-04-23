
using LemonLaw.Core.Entities;
using LemonLaw.Core.Entities.Faq;
using AppEntity = LemonLaw.Core.Entities.Application;

namespace LemonLaw.Module.Helpers
{
    public static class PrePopulationHelper
    {
        public static void Apply(object parent, object child)
        {
            if (parent == null || child == null)
                return;

            ApplyApplication(parent, child);
            ApplyFaq(parent, child);
        }

        private static void ApplyApplication(object parent, object child)
        {
            if (parent is not AppEntity application)
                return;

            switch (child)
            {
                case Defect defect:
                    defect.ApplicationId = application.Id;
                    break;
                case RepairAttempt repairAttempt:
                    repairAttempt.ApplicationId = application.Id;
                    break;
                case Expense expense:
                    expense.ApplicationId = application.Id;
                    break;
                case ApplicationDocument document:
                    document.ApplicationId = application.Id;
                    break;
                case CaseNote note:
                    note.ApplicationId = application.Id;
                    break;
                case Correspondence correspondence:
                    correspondence.ApplicationId = application.Id;
                    break;
                case DealerOutreach outreach:
                    outreach.ApplicationId = application.Id;
                    break;
                case Hearing hearing:
                    hearing.ApplicationId = application.Id;
                    break;
                case Decision decision:
                    decision.ApplicationId = application.Id;
                    break;
                case CaseEvent caseEvent:
                    caseEvent.ApplicationId = application.Id;
                    break;
            }
        }

        private static void ApplyFaq(object parent, object child)
        {
            if (parent is not FaqQuestion faq)
                return;

            if (child is FaqAnswer answer)
            {
                answer.FaqQuestionId = faq.Id;
            }
        }
    }

}

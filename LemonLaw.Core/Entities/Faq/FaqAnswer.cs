using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LemonLaw.Core.Entities.Faq
{
    [DefaultProperty(nameof(AnswerText))]
    // No [DefaultClassOptions] — answers are child records of FaqQuestion,
    // not a standalone navigation item. Access them via the FAQ question detail view.
    [XafDisplayName("Answer")]
    public class FaqAnswer : AuditDetails, INotifyPropertyChanged, IObjectSpaceLink
    {
        #region XAF
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propertyName)
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

        public FaqAnswer() { }

        private Guid _id = Guid.NewGuid();

        [System.ComponentModel.DataAnnotations.Key]
        [XafDisplayName("ID")]
        [VisibleInDetailView(false), VisibleInListView(false)]
        public virtual Guid Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    RaisePropertyChanged(nameof(Id));
                }
            }
        }

        #region Question Relationship (M:1)
        private Guid? _faqQuestionId;

        [Browsable(false)]
        public virtual Guid? FaqQuestionId
        {
            get => _faqQuestionId;
            set
            {
                if (_faqQuestionId != value)
                {
                    _faqQuestionId = value;
                    RaisePropertyChanged(nameof(FaqQuestionId));
                }
            }
        }

        private FaqQuestion? _faqQuestion;

        [ForeignKey("FaqQuestionId")]
        [DevExpress.Xpo.Association("FaqQuestion-Answers")]
        public virtual FaqQuestion? FaqQuestion
        {
            get => _faqQuestion;
            set
            {
                if (_faqQuestion != value)
                {
                    _faqQuestion = value;
                    FaqQuestionId = value?.Id;
                    RaisePropertyChanged(nameof(FaqQuestion));
                }
            }
        }
        #endregion

        #region Answer Text
        private string? _answerText;

        [XafDisplayName("Answer")]
        [Size(SizeAttribute.Unlimited)]
        [ModelDefault("RowCount", "4")]
        //[Translatable]
        public virtual string? AnswerText
        {
            get => _answerText;
            set
            {
                if (_answerText != value)
                {
                    _answerText = value;
                    RaisePropertyChanged(nameof(AnswerText));
                }
            }
        }
        #endregion
    }
}

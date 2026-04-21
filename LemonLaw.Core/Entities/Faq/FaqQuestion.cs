using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Xpo;

namespace LemonLaw.Core.Entities.Faq
{
    [DefaultClassOptions]
    [DefaultProperty(nameof(QuestionText))]
    [NavigationItem("Utility")]
    [XafDisplayName("FAQ")]
    public class FaqQuestion : AuditDetails,
        INotifyPropertyChanging, INotifyPropertyChanged, IObjectSpaceLink
    {
        #region XAF & INotify
        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanging(string propertyName) =>
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));

        protected void RaisePropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private IObjectSpace _objectSpace;

        [NotMapped, Browsable(false)]
        public virtual IObjectSpace ObjectSpace
        {
            get => _objectSpace;
            set
            {
                if (_objectSpace != value)
                {
                    RaisePropertyChanging(nameof(ObjectSpace));
                    _objectSpace = value;
                    RaisePropertyChanged(nameof(ObjectSpace));
                }
            }
        }
        #endregion

        public FaqQuestion() { }

        private Guid _id = Guid.NewGuid();

        [System.ComponentModel.DataAnnotations.Key, XafDisplayName("ID"), VisibleInDetailView(false), VisibleInListView(false)]
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

        //#region Category Lookup (M:1 to LookUps)
        //private Guid? _categoryId;

        //[Browsable(false)]
        //public virtual Guid? CategoryId
        //{
        //    get => _categoryId;
        //    set
        //    {
        //        if (_categoryId != value)
        //        {
        //            RaisePropertyChanging(nameof(CategoryId));
        //            _categoryId = value;
        //            RaisePropertyChanged(nameof(CategoryId));
        //        }
        //    }
        //}

        //private LookUps? _category;

        //[ForeignKey("CategoryId")]
        //[XafDisplayName("Category")]
        //[DataSourceCriteria("Category.Name = 'FAQCategory' AND State = 0")]
        //public virtual LookUps? Category
        //{
        //    get => _category;
        //    set
        //    {
        //        if (_category != value)
        //        {
        //            RaisePropertyChanging(nameof(Category));
        //            _category = value;
        //            RaisePropertyChanged(nameof(Category));
        //        }
        //    }
        //}
        //#endregion

        #region Question Text
        private string? _questionText;

        [XafDisplayName("Question")]
        [Size(SizeAttribute.Unlimited)]
        [ModelDefault("RowCount", "2")]
        //[Translatable]
        public virtual string? QuestionText
        {
            get => _questionText;
            set
            {
                if (_questionText != value)
                {
                    RaisePropertyChanging(nameof(QuestionText));
                    _questionText = value;
                    RaisePropertyChanged(nameof(QuestionText));
                }
            }
        }
        #endregion

        #region Answers Relationship (1:N)
        private readonly ObservableCollection<FaqAnswer> _answers = new();

        [XafDisplayName("Answers")]
        [DevExpress.Xpo.Association("FaqQuestion-Answers")]
        [ModelDefault("Index", "10")]
        public virtual ICollection<FaqAnswer> Answers
        {
            get => _answers;
            set
            {
                if (!ReferenceEquals(_answers, value))
                {
                    RaisePropertyChanging(nameof(Answers));
                    _answers.Clear();

                    if (value != null)
                    {
                        foreach (var item in value)
                            _answers.Add(item);
                    }

                    RaisePropertyChanged(nameof(Answers));
                }
            }
        }
        #endregion
    }
}

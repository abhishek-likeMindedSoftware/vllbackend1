using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using LemonLaw.Core.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace LemonLaw.Core.Entities
{
    /// <summary>
    /// Base class for all auditable entities.
    /// Implements both INotifyPropertyChanging and INotifyPropertyChanged
    /// as required by XAF's change tracking proxies.
    /// </summary>
    public abstract class AuditDetails : INotifyPropertyChanging, INotifyPropertyChanged
    {
        public event PropertyChangingEventHandler? PropertyChanging;
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void RaisePropertyChanging(string propertyName) =>
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));

        protected void RaisePropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        // ── Created ───────────────────────────────────────────────────────────

        private DateTime _createdDate;
        [XafDisplayName("Created Date")]
        [ReadOnly(true)]
        [ModelDefault("AllowEdit", "False")]
        [ModelDefault("DisplayFormat", "{0:MM/dd/yyyy hh:mm tt}")]
        [ModelDefault("EditMask", "MM/dd/yyyy hh:mm tt")]
        [VisibleInDetailView(false), VisibleInListView(false)]
        public virtual DateTime CreatedDate
        {
            get => _createdDate;
            set
            {
                if (_createdDate != value)
                {
                    RaisePropertyChanging(nameof(CreatedDate));
                    _createdDate = value;
                    RaisePropertyChanged(nameof(CreatedDate));
                }
            }
        }

        private Guid? _createdById;
        [Browsable(false)]
        public virtual Guid? CreatedById
        {
            get => _createdById;
            set
            {
                if (_createdById != value)
                {
                    RaisePropertyChanging(nameof(CreatedById));
                    _createdById = value;
                    RaisePropertyChanged(nameof(CreatedById));
                }
            }
        }

        [ForeignKey(nameof(CreatedById))]
        [XafDisplayName("Created By")]
        [ReadOnly(true)]
        [ModelDefault("AllowEdit", "False")]
        [VisibleInDetailView(false), VisibleInListView(false)]
        public virtual ApplicationUser? CreatedBy { get; set; }

        // ── Modified ──────────────────────────────────────────────────────────

        private DateTime? _modifiedDate;
        [XafDisplayName("Modified Date")]
        [ReadOnly(true)]
        [ModelDefault("AllowEdit", "False")]
        [ModelDefault("DisplayFormat", "{0:MM/dd/yyyy hh:mm tt}")]
        [ModelDefault("EditMask", "MM/dd/yyyy hh:mm tt")]
        [VisibleInDetailView(false), VisibleInListView(false)]
        public virtual DateTime? ModifiedDate
        {
            get => _modifiedDate;
            set
            {
                if (_modifiedDate != value)
                {
                    RaisePropertyChanging(nameof(ModifiedDate));
                    _modifiedDate = value;
                    RaisePropertyChanged(nameof(ModifiedDate));
                }
            }
        }

        private Guid? _modifiedById;
        [Browsable(false)]
        public virtual Guid? ModifiedById
        {
            get => _modifiedById;
            set
            {
                if (_modifiedById != value)
                {
                    RaisePropertyChanging(nameof(ModifiedById));
                    _modifiedById = value;
                    RaisePropertyChanged(nameof(ModifiedById));
                }
            }
        }

        [ForeignKey(nameof(ModifiedById))]
        [XafDisplayName("Modified By")]
        [ReadOnly(true)]
        [ModelDefault("AllowEdit", "False")]
        [VisibleInDetailView(false), VisibleInListView(false)]
        public virtual ApplicationUser? ModifiedBy { get; set; }

        // ── Soft delete ───────────────────────────────────────────────────────

        private bool _isDeleted;
        [XafDisplayName("Is Deleted")]
        [ReadOnly(true)]
        [ModelDefault("AllowEdit", "False")]
        [VisibleInDetailView(false)]
        public virtual bool IsDeleted
        {
            get => _isDeleted;
            set
            {
                if (_isDeleted != value)
                {
                    RaisePropertyChanging(nameof(IsDeleted));
                    _isDeleted = value;
                    RaisePropertyChanged(nameof(IsDeleted));
                }
            }
        }

        // ── Lifecycle state ───────────────────────────────────────────────────

        private LifeCycleState _state = LifeCycleState.Active;
        [Browsable(false)]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public virtual LifeCycleState State
        {
            get => _state;
            set
            {
                if (_state != value)
                {
                    RaisePropertyChanging(nameof(State));
                    _state = value;
                    RaisePropertyChanged(nameof(State));
                }
            }
        }
    }
}

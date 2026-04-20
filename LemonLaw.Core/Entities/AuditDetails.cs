using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using LemonLaw.Core.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace LemonLaw.Core.Entities
{
    public abstract class AuditDetails : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void AuditRaisePropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private DateTime _createdDate;
        [XafDisplayName("Created Date")]
        [ReadOnly(true)]
        [ModelDefault("AllowEdit", "False")]
        [ModelDefault("DisplayFormat", "{0:MM/dd/yyyy hh:mm tt}")]
        [ModelDefault("EditMask", "MM/dd/yyyy hh:mm tt")]
        public virtual DateTime CreatedDate
        {
            get => _createdDate;
            set
            {
                if (_createdDate != value)
                {
                    _createdDate = value;
                    AuditRaisePropertyChanged(nameof(CreatedDate));
                }
            }
        }

        private LifeCycleState _state = LifeCycleState.Active;
        [XafDisplayName("State")]
        [Browsable(false)]
        public virtual LifeCycleState State
        {
            get => _state;
            set
            {
                if (_state != value)
                {
                    _state = value;
                    AuditRaisePropertyChanged(nameof(State));
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
                    _createdById = value;
                    AuditRaisePropertyChanged(nameof(CreatedById));
                }
            }
        }

        [ForeignKey(nameof(CreatedById))]
        [XafDisplayName("Created By")]
        [ModelDefault("DisplayFormat", "{0:FullName} ({0:UserName})")]
        [ModelDefault("EditMask", "{0:FullName} ({0:UserName})")]
        [ReadOnly(true)]
        [ModelDefault("AllowEdit", "False")]
        public virtual ApplicationUser? CreatedBy { get; set; }

        private DateTime? _modifiedDate;
        [XafDisplayName("Modified Date")]
        [ReadOnly(true)]
        [ModelDefault("AllowEdit", "False")]
        [ModelDefault("DisplayFormat", "{0:MM/dd/yyyy hh:mm tt}")]
        [ModelDefault("EditMask", "MM/dd/yyyy hh:mm tt")]
        public virtual DateTime? ModifiedDate
        {
            get => _modifiedDate;
            set
            {
                if (_modifiedDate != value)
                {
                    _modifiedDate = value;
                    AuditRaisePropertyChanged(nameof(ModifiedDate));
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
                    _modifiedById = value;
                    AuditRaisePropertyChanged(nameof(ModifiedById));
                }
            }
        }

        [ForeignKey(nameof(ModifiedById))]
        [XafDisplayName("Modified By")]
        [ModelDefault("DisplayFormat", "{0:FullName} ({0:UserName})")]
        [ModelDefault("EditMask", "{0:FullName} ({0:UserName})")]
        [ReadOnly(true)]
        [ModelDefault("AllowEdit", "False")]
        public virtual ApplicationUser? ModifiedBy { get; set; }

        private bool _isDeleted;
        [XafDisplayName("Is Deleted")]
        [ReadOnly(true)]
        [ModelDefault("AllowEdit", "False")]
        public virtual bool IsDeleted
        {
            get => _isDeleted;
            set
            {
                if (_isDeleted != value)
                {
                    _isDeleted = value;
                    AuditRaisePropertyChanged(nameof(IsDeleted));
                }
            }
        }
    }
    public abstract class OrganizationDetails : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private Guid? _organizationId;
        [Browsable(false)]
        public virtual Guid? OrganizationId
        {
            get => _organizationId;
            set
            {
                if (_organizationId != value)
                {
                    _organizationId = value;
                    RaisePropertyChanged(nameof(OrganizationId));
                }
            }
        }        
    }
}

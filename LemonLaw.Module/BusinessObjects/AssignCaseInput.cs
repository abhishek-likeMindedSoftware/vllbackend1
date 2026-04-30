using DevExpress.ExpressApp.DC;
using LemonLaw.Core.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LemonLaw.Module.BusinessObjects
{
    /// <summary>
    /// Non-persistent popup form for "Assign Case".
    /// The SelectedUser property is a reference to ApplicationUser — XAF renders
    /// this as a searchable lookup dropdown showing all users in the system.
    /// Only CASE_MANAGER users are shown (filtered in the controller).
    /// </summary>
    [DomainComponent]
    [XafDisplayName("Assign Case")]
    public class AssignCaseInput : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void Notify(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private ApplicationUser? _selectedUser;

        /// <summary>
        /// The staff member to assign the case to.
        /// XAF renders this as a lookup (dropdown/search) against ApplicationUser.
        /// </summary>
        [XafDisplayName("Assign To")]
        [Required]
        public ApplicationUser? SelectedUser
        {
            get => _selectedUser;
            set
            {
                if (_selectedUser != value)
                {
                    _selectedUser = value;
                    Notify(nameof(SelectedUser));
                }
            }
        }
    }
}

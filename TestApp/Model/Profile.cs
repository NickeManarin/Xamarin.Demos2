using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TestApp.Helpers;

namespace TestApp.Model
{
    public class Profile : BindableBase
    {
        #region Constants

        public const int Corporative = 1;
        public const int Personal = 2;

        #endregion

        #region Variables

        private string _email;
        private string _name;
        private int _typeOfProfile = 0;
        private string _about;

        #endregion

        [Key, Required]
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public int TypeOfProfile
        {
            get => _typeOfProfile;
            set
            {
                SetProperty(ref _typeOfProfile, value);
                OnPropertyChanged(nameof(IsCorporative));
                OnPropertyChanged(nameof(IsPersonal));
            }
        }
        
        public string About
        {
            get => _about;
            set => SetProperty(ref _about, value);
        }

        #region One profile to many...

        /// <summary>
        /// A profile must have at least some images.
        /// </summary>
        public IList<Image> Images { get; set; }

        /// <summary>
        /// A profile may have many interests.
        /// </summary>
        public IList<Interest> Interests { get; set; }

        #endregion


        #region Transients

        [NotMapped]
        public bool IsCorporative => TypeOfProfile == 1;

        [NotMapped]
        public bool IsPersonal => TypeOfProfile == 2;

        #endregion
    }
}
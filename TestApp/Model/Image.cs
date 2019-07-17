using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using TestApp.Helpers;
using Xamarin.Forms;

namespace TestApp.Model
{
    public class Image : BindableBase
    {
        private long _id;
        private long _externalId;
        private int _position;
        private byte[] _content;
        private string _hash;

        public long Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        /// <summary>
        /// External Id, the one used on the API. 
        /// </summary>
        public long ExternalId
        {
            get => _externalId;
            set => SetProperty(ref _externalId, value);
        }

        /// <summary>
        /// The order of the image on the list. 
        /// </summary>
        public int Position
        {
            get => _position;
            set => SetProperty(ref _position, value);
        }

        /// <summary>
        /// A maximum resolution of 640 x 640, with 24 bits of depth (RGB), so a maximum length of the byte array of 1.228.800.
        /// A minimum resolution of 200 x 200?
        /// </summary>
        public byte[] Content //TODO: Is it going to be saved?
        {
            get => _content;
            set
            {
                SetProperty(ref _content, value);
                OnPropertyChanged(nameof(ImageSource));
                OnPropertyChanged(nameof(HasImage));
            }
        }

        /// <summary>
        /// The hash of the image. 
        /// </summary>
        public string Hash
        {
            get => _hash;
            set => SetProperty(ref _hash, value);
        }

        /// <summary>
        /// A profile is able to have multiple images.
        /// </summary>
        public Profile Profile { get; set; }


        /// <summary>
        /// The image as an image source. Returns a placeholder when no image was provided. 
        /// </summary>
        [NotMapped]
        public ImageSource ImageSource => Content != null ? ImageSource.FromStream(() => new MemoryStream(Content)) : null;
        //ImageSource.FromResource("MatchingApp.Resources.Images.AddPlaceholder.png", typeof(Model.Image).GetTypeInfo().Assembly)

        /// <summary>
        /// True if this object has an image.
        /// </summary>
        [NotMapped]
        public bool HasImage => !Content.IsNullOrEmpty();
    }
}
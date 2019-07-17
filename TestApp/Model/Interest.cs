using System.ComponentModel.DataAnnotations.Schema;
using TestApp.Helpers;

namespace TestApp.Model
{
    public class Interest : BindableBase
    {
        private long _id;
        private string _content;
        private string _language;
        private bool _wasBanned;

        public long ExternalId { get; set; }

        public long Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        public string Language
        {
            get => _language;
            set => SetProperty(ref _language, value);
        }

        public Profile Profile { get; set; }

        [NotMapped]
        public bool WasBanned
        {
            get => _wasBanned;
            set => SetProperty(ref _wasBanned, value);
        }

        //[NotMapped]
        //public Color InterestColor => WasBanned ? Color.FromArgb(0, 0, 0) : Color.FromArgb(1, 1, 1);
    }
}
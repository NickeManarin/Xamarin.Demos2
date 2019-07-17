using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;
using TestApp.Helpers;
using TestApp.View.Dialogs;
using Xamarin.Forms;
using Image = TestApp.Model.Image;

namespace TestApp.ViewModel.RegistrationSteps
{
    internal class PhotosViewModel : BaseViewModel
    {
        private ObservableCollection<Image> _items = new ObservableCollection<Image>();
        private bool _isPersonal;

        public ObservableCollection<Image> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        public bool IsPersonal
        {
            get => _isPersonal;
            set => SetProperty(ref _isPersonal, value);
        }

        public Command LoadPhotosCommand { get; set; }
        public Command SelectPhotoCommand { get; set; }
        public Command RemovePhotoCommand { get; set; }
        public Command ReorderedCommand { get; set; }


        public PhotosViewModel()
        {
            Title = "Select your photos";

            LoadPhotosCommand = new Command<List<Image>>(LoadPhotos_Executed);
            SelectPhotoCommand = new Command<Image>(async a => await SelectPhoto_Executed(a));
            RemovePhotoCommand = new Command(RemovePhoto_Executed);
            ReorderedCommand = new Command(Reordered_Executed);
        }

        private void LoadPhotos_Executed(List<Image> images)
        {
            try
            {
                IsBusy = true;

                if (images != null)
                    foreach (var img in images)
                        Items.Add(img);

                while (Items.Count < 6)
                    Items.Add(new Image { Position = Items.Count + 1 });
            }
            catch (Exception e)
            {
                MessagingCenter.Send<BaseViewModel, string>(this, Constants.ShowError, "It was not possible to load the list.");
            }
            finally
            {
                MessagingCenter.Send<BaseViewModel>(this, Constants.RefreshInterface);
                IsBusy = false;
            }
        }

        private async Task SelectPhoto_Executed(Image param)
        {
            try
            {
                if (IsBusy)
                    return;

                IsBusy = true;

                if (param == null)
                {
                    MessagingCenter.Send<BaseViewModel, string>(this, Constants.ShowError, "Uh oh... There's a problem right now, try again later.");
                    return;
                }

                var mode = await ImageDialog.SelectImageMode();

                if (!mode.HasValue)
                    return;

                await CrossMedia.Current.Initialize();

                var media = mode == true ? await TakePhoto() : await PickPhoto();

                if (media == null)
                    return;

                var bytes = await Task.Factory.StartNew(() =>
                {
                    var stream = media.GetStream();

                    using (var br = new BinaryReader(stream))
                        return br.ReadBytes((int)stream.Length).ToArray();
                });

                if (Items[param.Position - 1].Content.IsNullOrEmpty())
                {
                    //Simply adds an image to the latest position available.
                    var available = Items.FirstOrDefault(f => f.Content.IsNullOrEmpty())?.Position - 1 ?? Items.Count - 1;

                    Items[available].Content = bytes;
                    Items[available].Hash = GenerateHash(bytes);
                }
                else
                {
                    //Replaces an image.
                    Items[param.Position - 1].Content = bytes;
                    Items[param.Position - 1].Hash = GenerateHash(bytes);
                }
            }
            catch (Exception e)
            {
                MessagingCenter.Send<BaseViewModel, string>(this, Constants.ShowError, "Uh oh... There's a problem in the request.");
            }
            finally
            {
                MessagingCenter.Send<BaseViewModel>(this, Constants.Hide);
                MessagingCenter.Send<BaseViewModel>(this, Constants.RefreshInterface);
                IsBusy = false;
            }
        }

        private void RemovePhoto_Executed(object param)
        {
            try
            {
                IsBusy = true;

                if (!(param is Image remove))
                {
                    MessagingCenter.Send<BaseViewModel, string>(this, Constants.ShowError, "Uh oh... There's a problem right now, try again later.");
                    return;
                }

                //Removes the image and adds another one at the end.
                Items.RemoveAt(remove.Position - 1);
                Items.Add(new Image { Position = Items.Count + 1 });

                //Re-index the images.
                for (var i = 0; i < Items.Count; i++)
                    Items[i].Position = i + 1;
            }
            catch (Exception e)
            {
                MessagingCenter.Send<BaseViewModel, string>(this, Constants.ShowError, "Uh oh... There's a problem in removing the image.");
            }
            finally
            {
                MessagingCenter.Send<BaseViewModel>(this, Constants.RefreshInterface);
                IsBusy = false;
            }
        }

        private void Reordered_Executed()
        {
            Items.CollectionChanged += Items_CollectionChanged;
        }

        private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (Items.Count < 6)
                return;

            //Re-index the images.
            for (var i = 0; i < Items.Count; i++)
                Items[i].Position = i + 1;

            Items.CollectionChanged -= Items_CollectionChanged;
        }

        private async Task<MediaFile> TakePhoto()
        {
            if (!CrossMedia.IsSupported || !CrossMedia.Current.IsCameraAvailable)
            {
                MessagingCenter.Send<BaseViewModel, string>(this, Constants.ShowError, "No camera detected... So it's impossible to take a photo right now.");
                return null;
            }

            if (!CrossMedia.Current.IsTakePhotoSupported)
            {
                MessagingCenter.Send<BaseViewModel, string>(this, Constants.ShowError, "There's no support for taking photos on this device... So it's impossible to take a photo right now.");
                return null;
            }

            if (!await PermissionHelper.RequestCamera())
                return null;

            return await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = Constants.AppName,
                SaveToAlbum = false,
                CompressionQuality = 75,
                PhotoSize = PhotoSize.Medium,
                SaveMetaData = true,
                DefaultCamera = CameraDevice.Front,
                AllowCropping = false,
                ModalPresentationStyle = MediaPickerModalPresentationStyle.OverFullScreen,
            });
        }

        private async Task<MediaFile> PickPhoto()
        {
            if (!CrossMedia.Current.IsTakePhotoSupported)
            {
                MessagingCenter.Send<BaseViewModel, string>(this, Constants.ShowError, "There's no support for picking photos on this device... So it's impossible to pick a photo right now.");
                return null;
            }

            if (!await PermissionHelper.RequestMediaLibrary())
                return null;

            return await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
            {
                PhotoSize = PhotoSize.Medium,
            });
        }

        internal bool IsValid()
        {
            return true;
        }

        internal string GenerateHash(byte[] content)
        {
            using (var sha1 = new SHA1CryptoServiceProvider())
                return Convert.ToBase64String(sha1.ComputeHash(content));
        }
    }
}
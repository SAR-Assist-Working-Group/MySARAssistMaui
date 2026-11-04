using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Media;
using MySarAssistModels;
using MySarAssistModels.Clues;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MySARAssist.ViewModels.DebriefViewModels
{
    public partial class ClueItemViewModel : ObservableObject
    {
        public Clue Model { get; }
        public Action? OnChanged { get; set; }

        [ObservableProperty]
        private int _clueNumber;

        [ObservableProperty]
        private string? _description;

        [ObservableProperty]
        private DateTime _dateFound;

        [ObservableProperty]
        private TimeSpan _timeFound;

        [ObservableProperty]
        private string? _foundBy;

        [ObservableProperty]
        private string? _locationFound;

        [ObservableProperty]
        private double? _latitude;

        [ObservableProperty]
        private double? _longitude;

        [ObservableProperty]
        private string _formattedCoordinates = string.Empty;

        [ObservableProperty]
        private string? _imageBytes;

        [ObservableProperty]
        private ImageSource? _photoSource;

        [ObservableProperty]
        private bool _hasPhoto;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanCaptureGps))]
        private bool _isCapturingGps;

        public bool CanCaptureGps => !IsCapturingGps;

        public ClueItemViewModel(Clue clue, Action? onChanged = null)
        {
            Model = clue ?? new Clue();
            OnChanged = onChanged;

            ClueNumber = Model.ClueNumber;
            Description = Model.Description;
            DateFound = Model.DateFound ?? DateTime.Now;
            TimeFound = (Model.DateFound ?? DateTime.Now).TimeOfDay;
            FoundBy = Model.FoundBy;
            LocationFound = Model.LocationFound;
            Latitude = Model.Latitude;
            Longitude = Model.Longitude;
            ImageBytes = Model.ImageBytes;

            UpdateFormattedCoordinates();
            UpdatePhotoSource();
        }

        partial void OnDescriptionChanged(string? value)
        {
            Model.Description = value;
            OnChanged?.Invoke();
        }

        partial void OnClueNumberChanged(int value)
        {
            Model.ClueNumber = value;
            OnChanged?.Invoke();
        }

        partial void OnDateFoundChanged(DateTime value)
        {
            SyncDateTimeFound();
        }

        partial void OnTimeFoundChanged(TimeSpan value)
        {
            SyncDateTimeFound();
        }

        private void SyncDateTimeFound()
        {
            DateTime combined = DateFound.Date + TimeFound;
            Model.DateFound = combined;
            OnChanged?.Invoke();
        }

        partial void OnFoundByChanged(string? value)
        {
            Model.FoundBy = value;
            OnChanged?.Invoke();
        }

        partial void OnLocationFoundChanged(string? value)
        {
            Model.LocationFound = value;
            OnChanged?.Invoke();
        }

        partial void OnLatitudeChanged(double? value)
        {
            Model.Latitude = value;
            UpdateFormattedCoordinates();
            OnChanged?.Invoke();
        }

        partial void OnLongitudeChanged(double? value)
        {
            Model.Longitude = value;
            UpdateFormattedCoordinates();
            OnChanged?.Invoke();
        }

        partial void OnImageBytesChanged(string? value)
        {
            Model.ImageBytes = value;
            UpdatePhotoSource();
            OnChanged?.Invoke();
        }

        private void UpdateFormattedCoordinates()
        {
            if (Latitude.HasValue && Longitude.HasValue)
            {
                var coord = new Coordinate(Latitude.Value, Longitude.Value);
                FormattedCoordinates = coord.CoordinateOutput("UTM");
            }
            else
            {
                FormattedCoordinates = "No GPS recorded";
            }
        }

        private void UpdatePhotoSource()
        {
            HasPhoto = !string.IsNullOrWhiteSpace(ImageBytes);
            if (HasPhoto && ImageBytes != null)
            {
                try
                {
                    byte[] bytes = Convert.FromBase64String(ImageBytes);
                    PhotoSource = ImageSource.FromStream(() => new MemoryStream(bytes));
                }
                catch
                {
                    PhotoSource = null;
                    HasPhoto = false;
                }
            }
            else
            {
                PhotoSource = null;
            }
        }

        [RelayCommand]
        public async Task CaptureGpsAsync()
        {
            try
            {
                IsCapturingGps = true;
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                var location = await Geolocation.Default.GetLocationAsync(request);

                if (location != null)
                {
                    Latitude = location.Latitude;
                    Longitude = location.Longitude;
                    LocationFound = $"GPS: {Latitude:F5}, {Longitude:F5}";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to get GPS location: {ex.Message}");
            }
            finally
            {
                IsCapturingGps = false;
            }
        }

        [RelayCommand]
        public async Task CapturePhotoAsync()
        {
            try
            {
                if (MediaPicker.Default.IsCaptureSupported)
                {
                    FileResult? photo = await MediaPicker.Default.CapturePhotoAsync();
                    if (photo != null)
                    {
                        using var stream = await photo.OpenReadAsync();
                        using var ms = new MemoryStream();
                        await stream.CopyToAsync(ms);
                        byte[] bytes = ms.ToArray();
                        ImageBytes = Convert.ToBase64String(bytes);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to capture photo: {ex.Message}");
            }
        }

        [RelayCommand]
        public async Task PickPhotoAsync()
        {
            try
            {
                FileResult? photo = await MediaPicker.Default.PickPhotoAsync();
                if (photo != null)
                {
                    using var stream = await photo.OpenReadAsync();
                    using var ms = new MemoryStream();
                    await stream.CopyToAsync(ms);
                    byte[] bytes = ms.ToArray();
                    ImageBytes = Convert.ToBase64String(bytes);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to pick photo: {ex.Message}");
            }
        }

        [RelayCommand]
        public void RemovePhoto()
        {
            ImageBytes = null;
        }
    }
}

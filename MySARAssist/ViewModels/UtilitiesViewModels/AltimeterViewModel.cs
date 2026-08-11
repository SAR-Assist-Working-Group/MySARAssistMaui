using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using MySarAssistModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySARAssist.ViewModels.UtilitiesViewModels
{
   public class AltimeterViewModel : ObservableObject
    {
        private Coordinate _coordinate = new Coordinate();
        public Command GetLocationCommand { get; }
        public Coordinate CurrentCoordinate { get => _coordinate; set => _coordinate = value; }

        public string CurrentLocationText
        {
            get => $"Lat: {CurrentCoordinate.Latitude}, Long: {CurrentCoordinate.Longitude}";
        }
        public double? GPSAccuracy { get => CurrentCoordinate.Accuracy;        }
        public double?            CurrentAltitudeByGPS
        {
            get => CurrentCoordinate.Altitude;
        }
        public string CurrentAltitudeByGPSText
        {
            get
            {
                if (CurrentCoordinate.Altitude != null)
                {
                    return $"Altitude: {CurrentCoordinate.Altitude}m";
                }
                else
                {
                    return "Altitude: N/A";
                }
            }
        }

        public AltimeterViewModel()
        {
            GetLocationCommand = new Command(OnGetLocationCommand);

        }

        private async void OnGetLocationCommand(object obj)
        {
            await GetCurrentLocation();
        }

        private async void ShowToastAsync(string message)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;

            var toast = Toast.Make(message, duration, fontSize);

            await toast.Show(cancellationTokenSource.Token);
        }

        #region Location
        public async Task<PermissionStatus> CheckAndRequestLocationPermission()
        {
            PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if (status == PermissionStatus.Granted)
                return status;

            if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
            {
                // Prompt the user to turn on in settings
                // On iOS once a permission has been denied it may not be requested again from the application
                return status;
            }

            if (Permissions.ShouldShowRationale<Permissions.LocationWhenInUse>())
            {
                // Prompt the user with additional information as to why the permission is needed
            }

            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

            return status;
        }
        private async void setLocation()
        {
            try
            {

                await CheckAndRequestLocationPermission();

                var locationStatus = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null && location.Accuracy < 60)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");

                    Coordinate c = new Coordinate();
                    c.Latitude = location.Latitude;
                    c.Longitude = location.Longitude;
                    CurrentCoordinate = c;
                    OnPropertyChanged(nameof(CurrentCoordinate));
                    //DisplayQuestion();

                    ShowToastAsync("Accuracy: " + location.Accuracy + "m");

                }
                else
                {
                    //sendToast("The location was not accurate enough, please try again (" + location.Accuracy + "m)");
                    await GetCurrentLocation();
                }
            }
            catch (FeatureNotSupportedException)
            {
                ShowToastAsync("Sorry, this feature is not supported on this device.");
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException)
            {
                ShowToastAsync("Sorry, geolocation has not been enabled on this device. Check your device settings.");
                // Handle not enabled on device exception
            }
            catch (PermissionException)
            {
                ShowToastAsync("Sorry, you must enable permissions to get your current location.");
                // Handle permission exception
            }
            catch (Exception)
            {
                ShowToastAsync("Sorry, a problem occurred");
                // Unable to get location
            }
        }

        CancellationTokenSource? cts;

        async Task GetCurrentLocation()
        {
            try
            {
                var locationStatus = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);

                if (location != null && location.Accuracy < 100)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");

                    Coordinate c = new Coordinate();
                    c.Latitude = location.Latitude;
                    c.Longitude = location.Longitude;
                    c.Accuracy = location.Accuracy;
                    c.Altitude = location.Altitude;
                    CurrentCoordinate = c;
                    OnPropertyChanged(nameof(CurrentCoordinate));
                    OnPropertyChanged(nameof(CurrentLocationText));
                    OnPropertyChanged(nameof(CurrentAltitudeByGPSText));
                    OnPropertyChanged(nameof(GPSAccuracy));
                    //DisplayQuestion();
                    //ShowToastAsync("Accuracy: " + location.Accuracy + "m");

                }
                else if (location != null)
                {
                    ShowToastAsync("The location was not accurate enough (" + location.Accuracy + "m), please try again.");
                }
            }
            catch (FeatureNotSupportedException)
            {
                ShowToastAsync("Sorry, this feature is not supported on this device.");
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException)
            {
                ShowToastAsync("Sorry, geolocation has not been enabled on this device. Check your device settings.");
                // Handle not enabled on device exception
            }
            catch (PermissionException)
            {
                ShowToastAsync("Sorry, you must enable permissions to get your current location.");
                // Handle permission exception
            }
            catch (Exception)
            {
                ShowToastAsync("Sorry, a problem occurred");
                // Unable to get location
            }
        }

        #endregion
    }
}

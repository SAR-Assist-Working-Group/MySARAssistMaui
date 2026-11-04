using Microsoft.Maui.Controls;
using System;
using System.Linq;
using ZXing.Net.Maui;

namespace MySARAssist.Views.DebriefViews
{
    public partial class ScanAssignmentSheetPage : ContentPage
    {
        public static Action<string>? OnBarcodeScanned { get; set; }
        private bool _hasHandled;

        public ScanAssignmentSheetPage()
        {
            InitializeComponent();
        }

        private void BarcodeReader_BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
        {
            if (_hasHandled) return;

            var firstBarcode = e.Results?.FirstOrDefault();
            if (firstBarcode != null && !string.IsNullOrWhiteSpace(firstBarcode.Value))
            {
                _hasHandled = true;
                string scannedValue = firstBarcode.Value;

                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    OnBarcodeScanned?.Invoke(scannedValue);
                    await Shell.Current.GoToAsync("..");
                });
            }
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}


using Microsoft.Extensions.Logging;
using MySARAssist.Services;
using MySARAssist.ViewModels.CheckInOut;
using CommunityToolkit.Maui;
using MetroLog.MicrosoftExtensions;
using ZXing.Net.Maui.Controls;
using MySARAssist.Interfaces;
using MySARAssist.Views.Calculators;
using MySARAssist.Views;
using MySARAssist.Views.CheckInOut;
using MySARAssist.Views.RADeMS;
using Microsoft.Maui.LifecycleEvents;
using MySARAssist.Models;
using MySARAssist.Views.UrgencyViews;

namespace MySARAssist
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>().ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            }).UseMauiCommunityToolkit();
            // Add this code
            
            
            builder.Services.AddSingleton<PersonnelService>(s => ActivatorUtilities.CreateInstance<PersonnelService>(s));

            builder.Logging.AddInMemoryLogger(options =>
            {
                options.MaxLines = 1024;
                options.MinLevel = Microsoft.Extensions.Logging.LogLevel.Debug;
                options.MaxLevel = Microsoft.Extensions.Logging.LogLevel.Critical;

            });
            builder.Logging.AddStreamingFileLogger(options =>
            {
                options.RetainDays = 2;
                options.FolderPath = Path.Combine(
                    FileSystem.CacheDirectory,
                    "MetroLogs");

            });
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<AboutView>();

            builder.Services.AddTransient<CalculatorsView>();
            builder.Services.AddTransient<CoordinateConverterView>();
            builder.Services.AddTransient<DistanceToPacingPage>();
            builder.Services.AddTransient<GridSearchView>();
            builder.Services.AddTransient<HowToRangeOfDetectionPage>();
            builder.Services.AddTransient<LinearSearchView>();
            builder.Services.AddTransient<PacingToDistancePage>();
            builder.Services.AddTransient<SweepWidthCalculatorView>();
            builder.Services.AddTransient<VisualSearchResourceEstimationView>();

            builder.Services.AddTransient<CheckInOutView>();
            builder.Services.AddTransient<CheckInView>();
            builder.Services.AddTransient<CheckOutView>();
            builder.Services.AddTransient<EditQualificationsPage>();
            builder.Services.AddTransient<PersonnelEditView>();
            builder.Services.AddTransient<PersonnelListView>();

            builder.Services.AddTransient<UrgencyCalculatorView>();

            builder.Services.AddTransient<RADeMSView>();
            builder.Services.AddTransient<RADeMSDetailsPage>();
            builder.Services.AddTransient<RADeMSCardPage>();

            EntryHandler.AddDone();
            builder.UseBarcodeReader();



#if DEBUG
            builder.Logging.AddDebug();
#endif


            return builder.Build();
        }

       
    }



}
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
using MySARAssist.Views.Utilities;
using MySARAssist.ViewModels.UtilitiesViewModels;
using Microsoft.Maui.LifecycleEvents;
using MySARAssist.Models;

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
            }).UseMauiCommunityToolkit()
            .UseSentry(options =>
            {
                // The DSN is the only required setting.
                options.Dsn = "https://f805be8700a9918e12b79c40ccec718d@o4511293765976064.ingest.de.sentry.io/4511560848572496";

                // Use debug mode if you want to see what the SDK is doing.
                // Debug messages are written to stdout with Console.Writeline,
                // and are viewable in your IDE's debug console or with 'adb logcat', etc.
                // This option is not recommended when deploying your application.
#if DEBUG
                options.Debug = true;
#endif

                // Other Sentry options can be set here.
            });


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

            builder.Services.AddTransient<RADeMSView>();
            builder.Services.AddTransient<RADeMSDetailsPage>();
            builder.Services.AddTransient<RADeMSCardPage>();

            builder.Services.AddTransient<UtilitiesListPage>();
            builder.Services.AddTransient<UtilitiesListViewModel>();
            builder.Services.AddTransient<AltimeterPage>();
            builder.Services.AddTransient<AltimeterViewModel>();

            EntryHandler.AddDone();
            builder.UseBarcodeReader();



#if DEBUG
            builder.Logging.AddDebug();
#endif


            return builder.Build();
        }

       
    }



}
using MySARAssist.Views;
using MySARAssist.Views.Calculators;
using MySARAssist.Views.CheckInOut;
using MySARAssist.Views.RADeMS;

using MySARAssist.Views.UrgencyViews;

namespace MySARAssist
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(CalculatorsView), typeof(CalculatorsView));
            Routing.RegisterRoute(nameof(CalculatorsView) + "/" + nameof(CoordinateConverterView), typeof(CoordinateConverterView));
            Routing.RegisterRoute(nameof(CalculatorsView) + "/" + nameof(GridSearchView), typeof(GridSearchView));
            Routing.RegisterRoute(nameof(CalculatorsView) + "/" + nameof(LinearSearchView), typeof(LinearSearchView));
            Routing.RegisterRoute(nameof(CalculatorsView) + "/" + nameof(DistanceToPacingPage), typeof(DistanceToPacingPage));
            Routing.RegisterRoute(nameof(CalculatorsView) + "/" + nameof(PacingToDistancePage), typeof(PacingToDistancePage));
            Routing.RegisterRoute(nameof(CalculatorsView) + "/" + nameof(SweepWidthCalculatorView), typeof(SweepWidthCalculatorView));
            Routing.RegisterRoute(nameof(CalculatorsView) + "/" + nameof(SweepWidthCalculatorView) + "/" + nameof(HowToRangeOfDetectionPage), typeof(HowToRangeOfDetectionPage));
            Routing.RegisterRoute(nameof(CalculatorsView) + "/" + nameof(VisualSearchResourceEstimationView), typeof(VisualSearchResourceEstimationView));


            Routing.RegisterRoute(nameof(CheckInOutView), typeof(CheckInOutView));
            Routing.RegisterRoute(nameof(CheckInOutView) + "/" + nameof(CheckInView), typeof(CheckInView));
            Routing.RegisterRoute(nameof(CheckInOutView) + "/" + nameof(CheckOutView), typeof(CheckOutView));


            Routing.RegisterRoute(nameof(CheckInOutView) + "/" + nameof(PersonnelEditView), typeof(PersonnelEditView));
            Routing.RegisterRoute(nameof(CheckInOutView) + "/" + nameof(PersonnelEditView) + "/" + nameof(EditQualificationsPage), typeof(EditQualificationsPage));
            Routing.RegisterRoute(nameof(CheckInOutView) + "/" + nameof(PersonnelListView), typeof(PersonnelListView));

            Routing.RegisterRoute(nameof(UrgencyCalculatorView), typeof(UrgencyCalculatorView));

            Routing.RegisterRoute(nameof(RADeMSView), typeof(RADeMSView));
            Routing.RegisterRoute(nameof(RADeMSView) + "/" + nameof(RADeMSDetailsPage), typeof(RADeMSDetailsPage));
            Routing.RegisterRoute(nameof(RADeMSView) + "/" + nameof(RADeMSDetailsPage) + "/" + nameof(RADeMSCardPage), typeof(RADeMSCardPage));

            //utilities
            Routing.RegisterRoute(nameof(UtilitiesListPage), typeof(UtilitiesListPage));
            Routing.RegisterRoute(nameof(UtilitiesListPage) + "/" + nameof(AltimeterPage), typeof(AltimeterPage));
        }
    }
}

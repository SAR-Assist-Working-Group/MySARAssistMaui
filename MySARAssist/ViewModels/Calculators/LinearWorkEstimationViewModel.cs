using CommunityToolkit.Mvvm.ComponentModel;
using MySARAssist.Services;
using MySarAssistModels.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySARAssist.ViewModels.Calculators
{
    public class LinearWorkEstimationViewModel : ObservableObject
    {
        public LinearWorkEstimationViewModel()
        {
            CalculateCommand = new Command(() =>
            {
                CalculateEstimate();

            });

            EraseCommand = new Command(() =>
            {
                Length = 0;
                SearcherSpeed = 1.6;
                estimatedDuration = 0;
                NotifyInputs();
            });

            SpeedUpCommand = new Command(() =>
            {
                SearcherSpeed += SpeedStep;
                OnPropertyChanged(nameof(SearcherSpeed));

            });
            SpeedDownCommand = new Command(() =>
            {
                SearcherSpeed = Math.Max(0, SearcherSpeed - SpeedStep);
                OnPropertyChanged(nameof(SearcherSpeed));
            });


            LengthUpCommand = new Command(() =>
            {
                Length += LengthStep;
                OnPropertyChanged(nameof(Length));
            });
            LengthDownCommand = new Command(() =>
            {
                Length = Math.Max(0, Length - LengthStep);
                OnPropertyChanged(nameof(Length));
            });

            ElevationUpCommand = new Command(() =>
            {
                Elevation += ElevationStep;
                OnPropertyChanged(nameof(Elevation));
            });
            ElevationDownCommand = new Command(() =>
            {
                Elevation = Math.Max(0, Elevation - ElevationStep);
                OnPropertyChanged(nameof(Elevation));
            });

        }

        public Command CalculateCommand { get; }
        public Command EraseCommand { get; }
        public Command SpeedUpCommand { get; }
        public Command SpeedDownCommand { get; }
        public Command LengthUpCommand { get; }
        public Command LengthDownCommand { get; }
        public Command ElevationUpCommand { get; }
        public Command ElevationDownCommand { get; }


        private void CalculateEstimate()
        {
            if (Length > 0 && SearcherSpeed > 0)
            {

                estimatedDuration = (Length + (5.3 * (Elevation/1000))) / SearcherSpeed;
                estimatedDurationWithRoundTrip = ((Length + (5.3 * (Elevation / 1000))) / SearcherSpeed) + (Length  / SearcherSpeed);
            }
            else { estimatedDuration = 0; }

            OnPropertyChanged(nameof(EstimatedDuration));
            OnPropertyChanged(nameof(EstimatedDurationWithRoundTrip));
        }

        double estimatedDuration = 0;
        double estimatedDurationWithRoundTrip = 0;
        public string EstimatedDuration
        {
            get => string.Format("{0:#,##0.0}", estimatedDuration);
        }
        public string EstimatedDurationWithRoundTrip
        {
            get => string.Format("{0:#,##0.0}", estimatedDurationWithRoundTrip);
        }


        /// <summary>
        /// Speed in km/h. The view converts to and from the user's units, so every value the
        /// estimate sees is metric. The setter does not raise its own change notification,
        /// which would rewrite the entry while it is being typed in.
        /// </summary>
        double _searcherSpeed = 1.6;
        public double SearcherSpeed { get => _searcherSpeed; set { _searcherSpeed = value; CalculateEstimate(); } }

        /// <summary>Route length in kilometres.</summary>
        double _length = 0;
        public double Length { get => _length; set { _length = value; CalculateEstimate(); } }

        /// <summary>Elevation gain in metres.</summary>
        double _elevation = 0;
        public double Elevation { get => _elevation; set { _elevation = value; CalculateEstimate(); } }

        private static UnitSettings Units => UnitSettings.Current;

        private double SpeedStep => Units.Step(0.1, 0.1, UnitMeasure.Speed);
        private double LengthStep => Units.Step(0.1, 0.1, UnitMeasure.LongDistance);
        private double ElevationStep => Units.Step(100, 250, UnitMeasure.ShortDistance);

        public string SpeedCaption => $"Searcher Speed (in {Units.SpeedUnit})";
        public string SpeedUnit => Units.SpeedUnit;
        public string LengthCaption => $"Length (in {Units.LongDistanceUnit})";
        public string LengthUnit => Units.LongDistanceUnit;
        public string ElevationCaption => $"Elevation Gain (in {Units.ShortDistanceName})";
        public string ElevationUnit => Units.ShortDistanceUnit;

        /// <summary>Re-renders every unit-bearing binding, for when the setting changed while this page was off screen.</summary>
        public void RefreshUnits()
        {
            OnPropertyChanged(nameof(SpeedCaption));
            OnPropertyChanged(nameof(SpeedUnit));
            OnPropertyChanged(nameof(LengthCaption));
            OnPropertyChanged(nameof(LengthUnit));
            OnPropertyChanged(nameof(ElevationCaption));
            OnPropertyChanged(nameof(ElevationUnit));
            NotifyInputs();
        }

        private void NotifyInputs()
        {
            OnPropertyChanged(nameof(SearcherSpeed));
            OnPropertyChanged(nameof(Length));
            OnPropertyChanged(nameof(Elevation));
        }
    }
}

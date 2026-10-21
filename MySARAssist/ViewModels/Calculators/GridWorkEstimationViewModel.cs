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
    public class GridWorkEstimationViewModel : ObservableObject
    {
        public GridWorkEstimationViewModel()
        {
            CalculateCommand = new Command(() =>
            {
                CalculateTimeEstimate();
            });

            EraseCommand = new Command(() =>
            {
                Area = 0;
                SearcherSpeed = 1.6;
                TeamMembers = "2";
                Spacing = 0;
                EstimatedDuration = "0";
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

            MembersUpCommand = new Command(() =>
            {
                teamMembers += 1;

                OnPropertyChanged(nameof(TeamMembers));
            });
            MembersDownCommand = new Command(() =>
            {
                if (teamMembers > 0) { teamMembers -= 1; }

                OnPropertyChanged(nameof(TeamMembers));
            });

            AreaUpCommand = new Command(() =>
            {
                Area += AreaStep;
                OnPropertyChanged(nameof(Area));
            });
            AreaDownCommand = new Command(() =>
            {
                Area = Math.Max(0, Area - AreaStep);
                OnPropertyChanged(nameof(Area));
            });

            SpacingUpCommand = new Command(() =>
            {
                Spacing += SpacingStep;

                OnPropertyChanged(nameof(Spacing));
            });
            SpacingDownCommand = new Command(() =>
            {
                Spacing = Math.Max(0, Spacing - SpacingStep);

                OnPropertyChanged(nameof(Spacing));
            });
        }



        private void CalculateTimeEstimate()
        {
            if (Area > 0 && Spacing > 0 && teamMembers > 0 && SearcherSpeed > 0)
            {
                double tracklengtheffort = (Area * 1000) / Spacing;

                estimatedDuration = tracklengtheffort / SearcherSpeed / teamMembers;
            }
            else { estimatedDuration = 0; }
            OnPropertyChanged(nameof(EstimatedDuration));
        }

        public Command CalculateCommand { get; }
        public Command EraseCommand { get; }
        public Command SpeedUpCommand { get; }
        public Command SpeedDownCommand { get; }
        public Command MembersUpCommand { get; }
        public Command MembersDownCommand { get; }
        public Command AreaUpCommand { get; }
        public Command AreaDownCommand { get; }
        public Command SpacingUpCommand { get; }
        public Command SpacingDownCommand { get; }



        double estimatedDuration = 0;
        public string EstimatedDuration
        {
            get
            {
                if (estimatedDuration > 0) { return string.Format("{0:#,##0.0}", estimatedDuration); }
                return null;
            }
            set
            {
                double.TryParse(value, out estimatedDuration);
                CalculateTimeEstimate();
                OnPropertyChanged(nameof(EstimatedDuration));
            }
        }

        /// <summary>
        /// Speed in km/h. Entries convert to and from the user's units in the view, so the
        /// value stored here and fed to the estimate is always metric. The setter deliberately
        /// does not raise its own change notification: doing so rewrites the entry while it is
        /// being typed in.
        /// </summary>
        double _searcherSpeed = 1.6;
        public double SearcherSpeed { get => _searcherSpeed; set { _searcherSpeed = value; CalculateTimeEstimate(); } }

        int teamMembers = 2;
        public string TeamMembers
        {
            get { if (teamMembers > 0) { return teamMembers.ToString(); } return null; }
            set { int.TryParse(value, out teamMembers); CalculateTimeEstimate(); OnPropertyChanged(nameof(TeamMembers)); }
        }

        /// <summary>Area in square kilometres.</summary>
        double _area = 0.01;
        public double Area { get => _area; set { _area = value; CalculateTimeEstimate(); } }

        /// <summary>Spacing between searchers in metres.</summary>
        double _spacing = 1;
        public double Spacing { get => _spacing; set { _spacing = value; CalculateTimeEstimate(); } }

        private static UnitSettings Units => UnitSettings.Current;

        private double SpeedStep => Units.Step(0.1, 0.1, UnitMeasure.Speed);
        private double AreaStep => Units.Step(0.01, 2, UnitMeasure.Area);
        private double SpacingStep => Units.Step(1, 5, UnitMeasure.ShortDistance);

        public string SpeedCaption => $"Searcher Speed (in {Units.SpeedUnit})";
        public string SpeedUnit => Units.SpeedUnit;
        public string AreaCaption => $"Area Size (in {Units.AreaUnit})";
        public string AreaUnit => Units.AreaUnit;
        public string SpacingCaption => $"Spacing Between Members (in {Units.ShortDistanceUnit})";
        public string SpacingUnit => Units.ShortDistanceUnit;

        /// <summary>Re-renders every unit-bearing binding, for when the setting changed while this page was off screen.</summary>
        public void RefreshUnits()
        {
            OnPropertyChanged(nameof(SpeedCaption));
            OnPropertyChanged(nameof(SpeedUnit));
            OnPropertyChanged(nameof(AreaCaption));
            OnPropertyChanged(nameof(AreaUnit));
            OnPropertyChanged(nameof(SpacingCaption));
            OnPropertyChanged(nameof(SpacingUnit));
            NotifyInputs();
        }

        private void NotifyInputs()
        {
            OnPropertyChanged(nameof(SearcherSpeed));
            OnPropertyChanged(nameof(Area));
            OnPropertyChanged(nameof(Spacing));
        }
    }
}

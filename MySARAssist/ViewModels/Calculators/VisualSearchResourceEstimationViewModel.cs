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
    public class VisualSearchResourceEstimationViewModel : ObservableObject
    {
        public VisualSearchResourceEstimationViewModel()
        {



            SpeedUpCommand = new Command(() =>
            {
                Speed += SpeedStep;
                OnPropertyChanged(nameof(Speed));
            });
            SpeedDownCommand = new Command(() =>
            {
                if (Speed > SpeedStep) { Speed -= SpeedStep; }
                OnPropertyChanged(nameof(Speed));
            });

            CommandStaffUpCommand = new Command(() =>
            {
                CommandStaff += 1;
                OnPropertyChanged(nameof(CommandStaff));
            });
            CommandStaffDownCommand = new Command(() =>
            {
                if (CommandStaff > 0) { CommandStaff -= 1; }
                OnPropertyChanged(nameof(CommandStaff));
            });

            AreaUpCommand = new Command(() =>
            {
                Area += AreaStep;
                OnPropertyChanged(nameof(Area));
            });
            AreaDownCommand = new Command(() =>
            {

                if (Area > AreaStep) { Area -= AreaStep; }
                OnPropertyChanged(nameof(Area));
            });

            SpacingUpCommand = new Command(() =>
            {
                Spacing += SpacingStep;
                OnPropertyChanged(nameof(Spacing));
            });
            SpacingDownCommand = new Command(() =>
            {
                if (Spacing > SpacingStep) { Spacing -= SpacingStep; }
                OnPropertyChanged(nameof(Spacing));
            });

            DurationUpCommand = new Command(() =>
            {
                Duration += 0.5;
                OnPropertyChanged(nameof(Duration));
            });
            DurationDownCommand = new Command(() =>
            {
                if (Duration > 0.5) { Duration -= 0.5; }
                OnPropertyChanged(nameof(Duration));
            });

            ExtraTravelTimeUpCommand = new Command(() =>
            {
                ExtraTravelTime += 0.1;
                OnPropertyChanged(nameof(ExtraTravelTime));
            });
            ExtraTravelTimeDownCommand = new Command(() =>
            {
                if (ExtraTravelTime > 0) { ExtraTravelTime -= 0.1; }
                OnPropertyChanged(nameof(ExtraTravelTime));
            });
        }

        private double _Area = 3.0;
        private double _Spacing = 10;
        private double _Speed = 1.6;
        private double _Duration = 6;
        private double _ExtraTravelTime = 0.25;
        private int _CommandStaff = 3;

        /// <summary>
        /// Area in square kilometres, spacing in metres and speed in km/h. The view converts
        /// these to and from the user's units, so the estimate below only ever sees metric.
        /// Rounding is left to the display layer so an imperial edit is not truncated.
        /// </summary>
        public double Area { get => _Area; set { _Area = value; OnPropertyChanged(nameof(ResourcesNeeded)); } }
        public double Spacing { get => _Spacing; set { _Spacing = value; OnPropertyChanged(nameof(ResourcesNeeded)); } }
        public double Speed { get => _Speed; set { _Speed = value; OnPropertyChanged(nameof(ResourcesNeeded)); } }
        public double Duration { get => Math.Round(_Duration, 1); set { _Duration = value; OnPropertyChanged(nameof(ResourcesNeeded)); } }
        public double ExtraTravelTime { get => Math.Round(_ExtraTravelTime, 2); set { _ExtraTravelTime = value; OnPropertyChanged(nameof(ResourcesNeeded)); } }
        public int CommandStaff { get => _CommandStaff; set { _CommandStaff = value; OnPropertyChanged(nameof(ResourcesNeeded)); } }

        public int ResourcesNeeded
        {
            get { return CalculateResourcesNeeded(); }
        }

        private int CalculateResourcesNeeded()
        {

            double teamSize = 0;
            if (Duration > 0 && Speed > 0 && Spacing > 0)
            {
                double tempDuration = Duration + ExtraTravelTime * 2; //this will take the travel to and from assignments and account for it within the duration
                double tempArea = Area * 1000; //convert KMs to Meters to match the spacing measurment

                teamSize = tempArea / Spacing / Speed / tempDuration;

                //add in the command staff
                teamSize += CommandStaff;
                teamSize = Math.Ceiling(teamSize);
            }
            return (int)teamSize;



        }

        private static UnitSettings Units => UnitSettings.Current;

        private double SpeedStep => Units.Step(0.1, 0.1, UnitMeasure.Speed);
        private double AreaStep => Units.Step(0.1, 25, UnitMeasure.Area);
        private double SpacingStep => Units.Step(1, 5, UnitMeasure.ShortDistance);

        public string SpeedCaption => $"Searcher Speed ({Units.SpeedUnit})";
        public string SpeedUnit => Units.SpeedUnit;
        public string AreaCaption => $"Search Area Size ({Units.AreaUnit})";
        public string AreaUnit => Units.AreaUnit;
        public string SpacingCaption => $"Spacing Between Members ({Units.ShortDistanceName})";
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
            OnPropertyChanged(nameof(Speed));
            OnPropertyChanged(nameof(Area));
            OnPropertyChanged(nameof(Spacing));
        }

        public Command SpeedUpCommand { get; }
        public Command SpeedDownCommand { get; }
        public Command DurationUpCommand { get; }
        public Command DurationDownCommand { get; }
        public Command AreaUpCommand { get; }
        public Command AreaDownCommand { get; }
        public Command SpacingUpCommand { get; }
        public Command SpacingDownCommand { get; }
        public Command CommandStaffUpCommand { get; }
        public Command CommandStaffDownCommand { get; }
        public Command ExtraTravelTimeUpCommand { get; }
        public Command ExtraTravelTimeDownCommand { get; }

    }
}

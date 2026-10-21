using CommunityToolkit.Mvvm.ComponentModel;
using MySARAssist.Models;
using MySARAssist.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySarAssistModels;
using MySarAssistModels.Units;

namespace MySARAssist.ViewModels.Calculators
{
    public class SweepWidthCalculatorViewModel : ObservableObject
    {
        public SweepWidthCalculatorViewModel()
        {
            CalculateCommand = new Command(() =>
            {
                CalculateSpacing();

            });

            EraseCommand = new Command(() =>
            {
                SelectedVisibilityIndex = 0;
                RangeOfDetection = 1;
                TargetPOD = "0.63";
                POD = "0";
                TeamSpacing = 0;
                OnPropertyChanged(nameof(RangeOfDetection));
            });

            RDUpCommand = new Command(() =>
            {
                setRangeOfDetection(rangeOfDetection + RangeOfDetectionStep);
                OnPropertyChanged(nameof(RangeOfDetection));

            });
            RDDownCommand = new Command(() =>
            {
                setRangeOfDetection(rangeOfDetection - RangeOfDetectionStep);
                OnPropertyChanged(nameof(RangeOfDetection));

            });

            TargetPODTo63 = new Command(() =>
            {
                TargetPODAsPercent = 63;
                //setTargetPOD(0.63);

            });
            TargetPODTo83 = new Command(() =>
            {
                TargetPODAsPercent = 83;
                //setTargetPOD(0.83);

            });

        }

        private void setTargetPOD(double newPOD)
        {
            double maxPOD = 0.864665;
            double minPOD = 0.048;
            if (newPOD > maxPOD) { targetPOD = maxPOD; }
            else if (newPOD < minPOD) { targetPOD = minPOD; }
            else { targetPOD = newPOD; }
            OnPropertyChanged(nameof(TargetPODAsPercentText));
            CalculateSpacing();

        }

        private void setRangeOfDetection(double newRD)
        {
            double minRD = 0;
            if (newRD > minRD) { rangeOfDetection = newRD; }
            else { rangeOfDetection = minRD; }
            CalculateSpacing();
        }

        private void CalculateSpacing()
        {
            if (targetPOD <= 0)
            {
                teamSpacing = rangeOfDetection * visibilityModifier;
                pod = StatisticalTools.calculatePOD(rangeOfDetection, visibilityModifier, teamSpacing);
            }
            else
            {
                teamSpacing = StatisticalTools.calculateSpacing(rangeOfDetection, visibilityModifier, targetPOD);
                pod = StatisticalTools.calculatePOD(rangeOfDetection, visibilityModifier, teamSpacing);
            }



            OnPropertyChanged(nameof(TeamSpacing));
            OnPropertyChanged(nameof(POD));
        }

     

        public Command CalculateCommand { get; }
        public Command EraseCommand { get; }
        public Command RDUpCommand { get; }
        public Command RDDownCommand { get; }
        public Command TargetPODTo63 { get; }
        public Command TargetPODTo83 { get; }


        /// <summary>
        /// Range of detection in metres. The view converts to and from the user's units, so the
        /// sweep width maths stays metric. The setter does not raise its own change notification,
        /// which would rewrite the entry while it is being typed in.
        /// </summary>
        double rangeOfDetection;
        public double RangeOfDetection
        {
            get => rangeOfDetection;
            set => setRangeOfDetection(value);
        }


        public bool VisibilityIsLow
        {
            get => selectedVisibilityIndex == 0;
            set
            {
                if (value) { selectedVisibilityIndex = 0; }
                CalculateSpacing();
                OnPropertyChanged(nameof(VisibilityIsLow));
                OnPropertyChanged(nameof(VisibilityIsMid));
                OnPropertyChanged(nameof(VisibilityIsHigh));
            }
        }
        public bool VisibilityIsMid
        {
            get => selectedVisibilityIndex == 1;
            set
            {
                if (value) { selectedVisibilityIndex = 1; }
                CalculateSpacing();
                OnPropertyChanged(nameof(VisibilityIsLow));
                OnPropertyChanged(nameof(VisibilityIsMid));
                OnPropertyChanged(nameof(VisibilityIsHigh));
            }
        }
        public bool VisibilityIsHigh
        {
            get => selectedVisibilityIndex == 2;
            set
            {
                if (value) { selectedVisibilityIndex = 2; }
                CalculateSpacing();
                OnPropertyChanged(nameof(VisibilityIsLow));
                OnPropertyChanged(nameof(VisibilityIsMid));
                OnPropertyChanged(nameof(VisibilityIsHigh));
            }
        }


        int selectedVisibilityIndex = 1;
        public int SelectedVisibilityIndex
        {
            get => selectedVisibilityIndex; set
            {
                selectedVisibilityIndex = value;
                OnPropertyChanged(nameof(SelectedVisibilityIndex));
                CalculateSpacing();
            }
        }
        double visibilityModifier
        {
            get
            {
                switch (selectedVisibilityIndex)
                {
                    case 2:
                        return 1.8;
                    case 1:
                        return 1.6;
                    case 0:
                        return 1.1;
                    default:
                        return 1.8;
                }

                
            }
        }





        double targetPOD = 0.63;
        public string TargetPOD
        {
            get => targetPOD.ToString();
            set
            {
                double.TryParse(value, out targetPOD);
                //if(targetPOD > 1) { targetPOD = targetPOD / 100; }
                OnPropertyChanged(nameof(TargetPOD));

            }
        }


        public string TargetPODAsPercentText
        {
            get { return (targetPOD * 100).ToString("##0"); }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    double newPOD = 0;

                    if (double.TryParse(value, out newPOD)) { newPOD = newPOD / 100; }
                    setTargetPOD(newPOD);
                }
            }
        }
        public double TargetPODAsPercent
        {
            get { return targetPOD * 100; }
            set
            {
                if (value <= 100)
                {

                    setTargetPOD(value / 100);
                }
                OnPropertyChanged(nameof(TargetPODAsPercent));

            }
        }


        /// <summary>Resulting spacing between searchers, in metres.</summary>
        double teamSpacing = 0;
        double pod = 0;
        public double TeamSpacing
        {
            get => teamSpacing;
            set
            {
                teamSpacing = value;
                OnPropertyChanged(nameof(TeamSpacing));

            }
        }
        public string POD
        {
            get => string.Format("{0:P1}", pod);
            set
            {
                double.TryParse(value, out pod);
                OnPropertyChanged(nameof(POD));


            }
        }

        private static UnitSettings Units => UnitSettings.Current;

        private double RangeOfDetectionStep => Units.Step(1, 5, UnitMeasure.ShortDistance);

        public string RangeOfDetectionCaption => $"Range of Detection ({Units.ShortDistanceName})";
        public string TeamSpacingCaption => $"{Units.ShortDistanceName} between searchers";

        /// <summary>Re-renders every unit-bearing binding, for when the setting changed while this page was off screen.</summary>
        public void RefreshUnits()
        {
            OnPropertyChanged(nameof(RangeOfDetectionCaption));
            OnPropertyChanged(nameof(TeamSpacingCaption));
            OnPropertyChanged(nameof(RangeOfDetection));
            OnPropertyChanged(nameof(TeamSpacing));
        }
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using MySarAssistModels.Units;

namespace MySARAssist.Services
{
    /// <summary>
    /// The user's display unit choice, persisted with the platform preference store and shared
    /// by every view. Saved records, sync payloads and all calculator maths stay metric: this
    /// setting only decides what is rendered and how typed values are read back.
    /// </summary>
    public sealed class UnitSettings : ObservableObject
    {
        public const string PreferenceKey = "DisplayUnitSystem";

        public static UnitSettings Current { get; } = new UnitSettings();

        private UnitSystem _system;

        private UnitSettings()
        {
            _system = (UnitSystem)Preferences.Get(PreferenceKey, (int)UnitSystem.Metric);
        }

        public UnitSystem System
        {
            get => _system;
            set
            {
                if (_system == value) { return; }
                _system = value;
                Preferences.Set(PreferenceKey, (int)value);
                NotifySystemChanged();
            }
        }

        /// <summary>Two-way bindable for the settings radio buttons; a false write is ignored so the group's deselect does nothing.</summary>
        public bool IsMetric
        {
            get => _system == UnitSystem.Metric;
            set { if (value) { System = UnitSystem.Metric; } }
        }

        /// <summary>Two-way bindable for the settings radio buttons; a false write is ignored so the group's deselect does nothing.</summary>
        public bool IsImperial
        {
            get => _system == UnitSystem.Imperial;
            set { if (value) { System = UnitSystem.Imperial; } }
        }

        public string SpeedUnit => UnitConversion.Abbreviation(UnitMeasure.Speed, _system);
        public string ShortDistanceUnit => UnitConversion.Abbreviation(UnitMeasure.ShortDistance, _system);
        public string LongDistanceUnit => UnitConversion.Abbreviation(UnitMeasure.LongDistance, _system);
        public string AreaUnit => UnitConversion.Abbreviation(UnitMeasure.Area, _system);

        public string SpeedName => UnitConversion.Name(UnitMeasure.Speed, _system);
        public string ShortDistanceName => UnitConversion.Name(UnitMeasure.ShortDistance, _system);
        public string LongDistanceName => UnitConversion.Name(UnitMeasure.LongDistance, _system);
        public string AreaName => UnitConversion.Name(UnitMeasure.Area, _system);

        /// <summary>Formats a metric value in the current system, for results built as strings.</summary>
        public string Format(double metricValue, UnitMeasure measure)
            => UnitConversion.Format(metricValue, measure, _system);

        /// <summary>
        /// Metric increment for a +/- button, so imperial users step by round feet, miles or
        /// acres while the view model keeps holding metric.
        /// </summary>
        public double Step(double metricStep, double imperialStep, UnitMeasure measure)
            => UnitConversion.Step(metricStep, imperialStep, measure, _system);

        private void NotifySystemChanged()
        {
            OnPropertyChanged(nameof(System));
            OnPropertyChanged(nameof(IsMetric));
            OnPropertyChanged(nameof(IsImperial));
            OnPropertyChanged(nameof(SpeedUnit));
            OnPropertyChanged(nameof(ShortDistanceUnit));
            OnPropertyChanged(nameof(LongDistanceUnit));
            OnPropertyChanged(nameof(AreaUnit));
            OnPropertyChanged(nameof(SpeedName));
            OnPropertyChanged(nameof(ShortDistanceName));
            OnPropertyChanged(nameof(LongDistanceName));
            OnPropertyChanged(nameof(AreaName));
        }
    }
}

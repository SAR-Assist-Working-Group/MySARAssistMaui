using System.Globalization;
using MySARAssist.Services;
using MySarAssistModels.Units;

namespace MySARAssist.Converters
{
    /// <summary>
    /// Renders a metric view model value in the user's chosen unit system and parses edits
    /// straight back to metric. This is the only place a displayed number changes units, which
    /// is what lets the view models and calculators stay metric-only.
    /// </summary>
    public abstract class UnitValueConverter : IValueConverter
    {
        protected abstract UnitMeasure Measure { get; }

        /// <summary>
        /// Render zero as an empty string so an entry shows its placeholder instead of "0".
        /// Set to false where the field is expected to always show a number.
        /// </summary>
        public bool BlankWhenZero { get; set; } = true;

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            double metricValue = ToDouble(value);
            if (BlankWhenZero && metricValue == 0) { return string.Empty; }

            return UnitConversion.Format(metricValue, Measure, UnitSettings.Current.System, culture);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            UnitConversion.TryParseToMetric(value as string, Measure, UnitSettings.Current.System, out double metricValue, culture);
            return metricValue;
        }

        private static double ToDouble(object? value)
        {
            if (value is double doubleValue) { return doubleValue; }
            if (value is int intValue) { return intValue; }
            if (value is float floatValue) { return floatValue; }
            if (value is string text && double.TryParse(text, out double parsed)) { return parsed; }

            return 0;
        }
    }

    /// <summary>Speed held in km/h.</summary>
    public sealed class SpeedConverter : UnitValueConverter
    {
        protected override UnitMeasure Measure => UnitMeasure.Speed;
    }

    /// <summary>Spacing, elevation and detection ranges held in metres.</summary>
    public sealed class ShortDistanceConverter : UnitValueConverter
    {
        protected override UnitMeasure Measure => UnitMeasure.ShortDistance;
    }

    /// <summary>Route and segment lengths held in kilometres.</summary>
    public sealed class LongDistanceConverter : UnitValueConverter
    {
        protected override UnitMeasure Measure => UnitMeasure.LongDistance;
    }

    /// <summary>Search areas held in square kilometres.</summary>
    public sealed class AreaConverter : UnitValueConverter
    {
        protected override UnitMeasure Measure => UnitMeasure.Area;
    }
}

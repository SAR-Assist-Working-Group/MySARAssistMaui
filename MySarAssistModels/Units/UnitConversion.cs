using System.Globalization;

namespace MySarAssistModels.Units
{
    /// <summary>
    /// Translates between the metric values the app stores and calculates with and the values
    /// shown to the user. Only the display boundary calls this: no calculator, model or sync
    /// payload ever holds an imperial number.
    /// </summary>
    public static class UnitConversion
    {
        /// <summary>Exact by international definition.</summary>
        public const double MetresPerFoot = 0.3048;

        /// <summary>Exact by international definition (1760 yards of 0.9144 m).</summary>
        public const double MetresPerMile = 1609.344;

        /// <summary>Exact by international definition (1 chain by 1 furlong).</summary>
        public const double SquareMetresPerAcre = 4046.8564224;

        private const double KilometresPerMile = MetresPerMile / 1000d;
        private const double AcresPerSquareKilometre = 1000000d / SquareMetresPerAcre;

        /// <summary>Converts a stored metric value into the user's chosen unit system.</summary>
        public static double FromMetric(double metricValue, UnitMeasure measure, UnitSystem system)
        {
            if (system == UnitSystem.Metric) { return metricValue; }

            return measure switch
            {
                UnitMeasure.Speed => metricValue / KilometresPerMile,
                UnitMeasure.ShortDistance => metricValue / MetresPerFoot,
                UnitMeasure.LongDistance => metricValue / KilometresPerMile,
                UnitMeasure.Area => metricValue * AcresPerSquareKilometre,
                _ => metricValue
            };
        }

        /// <summary>Converts a value read from or typed into the UI back to metric.</summary>
        public static double ToMetric(double displayValue, UnitMeasure measure, UnitSystem system)
        {
            if (system == UnitSystem.Metric) { return displayValue; }

            return measure switch
            {
                UnitMeasure.Speed => displayValue * KilometresPerMile,
                UnitMeasure.ShortDistance => displayValue * MetresPerFoot,
                UnitMeasure.LongDistance => displayValue * KilometresPerMile,
                UnitMeasure.Area => displayValue / AcresPerSquareKilometre,
                _ => displayValue
            };
        }

        /// <summary>Short unit label for field captions and result rows, e.g. "km/h" or "mph".</summary>
        public static string Abbreviation(UnitMeasure measure, UnitSystem system) => (measure, system) switch
        {
            (UnitMeasure.Speed, UnitSystem.Metric) => "km/h",
            (UnitMeasure.Speed, _) => "mph",
            (UnitMeasure.ShortDistance, UnitSystem.Metric) => "m",
            (UnitMeasure.ShortDistance, _) => "ft",
            (UnitMeasure.LongDistance, UnitSystem.Metric) => "km",
            (UnitMeasure.LongDistance, _) => "mi",
            (UnitMeasure.Area, UnitSystem.Metric) => "km\u00b2",
            (UnitMeasure.Area, _) => "acres",
            _ => string.Empty
        };

        /// <summary>Spelled out unit name for use inside a sentence, e.g. "meters".</summary>
        public static string Name(UnitMeasure measure, UnitSystem system) => (measure, system) switch
        {
            (UnitMeasure.Speed, UnitSystem.Metric) => "kilometers per hour",
            (UnitMeasure.Speed, _) => "miles per hour",
            (UnitMeasure.ShortDistance, UnitSystem.Metric) => "meters",
            (UnitMeasure.ShortDistance, _) => "feet",
            (UnitMeasure.LongDistance, UnitSystem.Metric) => "kilometers",
            (UnitMeasure.LongDistance, _) => "miles",
            (UnitMeasure.Area, UnitSystem.Metric) => "square kilometers",
            (UnitMeasure.Area, _) => "acres",
            _ => string.Empty
        };

        /// <summary>
        /// Decimal places worth showing. Feet and acres are larger numbers than metres and
        /// square kilometres, so they need fewer places to carry the same precision.
        /// </summary>
        public static int Decimals(UnitMeasure measure, UnitSystem system) => measure switch
        {
            UnitMeasure.Speed => 1,
            UnitMeasure.ShortDistance => system == UnitSystem.Metric ? 1 : 0,
            UnitMeasure.LongDistance => 2,
            UnitMeasure.Area => system == UnitSystem.Metric ? 2 : 1,
            _ => 2
        };

        /// <summary>
        /// The increment a +/- button should apply, returned in metric so callers keep storing
        /// metric. <paramref name="imperialStep"/> is stated in display units, so an imperial
        /// user steps by round numbers of feet, miles or acres.
        /// </summary>
        public static double Step(double metricStep, double imperialStep, UnitMeasure measure, UnitSystem system)
            => system == UnitSystem.Metric ? metricStep : ToMetric(imperialStep, measure, system);

        /// <summary>
        /// Formats a metric value for display. Trailing zeros are trimmed, matching how the
        /// calculators have always rendered their inputs.
        /// </summary>
        public static string Format(double metricValue, UnitMeasure measure, UnitSystem system, IFormatProvider? culture = null)
        {
            double displayValue = Math.Round(FromMetric(metricValue, measure, system), Decimals(measure, system));
            return displayValue.ToString(culture ?? CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Parses text typed in display units back to metric. Blank or unparseable text yields
        /// zero and returns false, which is how the entry fields clear themselves.
        /// </summary>
        public static bool TryParseToMetric(string? text, UnitMeasure measure, UnitSystem system, out double metricValue, IFormatProvider? culture = null)
        {
            if (double.TryParse(text, NumberStyles.Float, culture ?? CultureInfo.CurrentCulture, out double displayValue))
            {
                metricValue = ToMetric(displayValue, measure, system);
                return true;
            }

            metricValue = 0;
            return false;
        }
    }
}

using MySarAssistModels.Units;
using System.Globalization;

namespace MySarAssistUnitTests
{
    [TestClass]
    public class UnitConversionTests
    {
        private static readonly CultureInfo Invariant = CultureInfo.InvariantCulture;

        [TestMethod]
        public void MetricSystemLeavesValuesUntouched()
        {
            foreach (UnitMeasure measure in Enum.GetValues<UnitMeasure>())
            {
                Assert.AreEqual(12.34, UnitConversion.FromMetric(12.34, measure, UnitSystem.Metric));
                Assert.AreEqual(12.34, UnitConversion.ToMetric(12.34, measure, UnitSystem.Metric));
            }
        }

        [TestMethod]
        public void ImperialSpeedIsMilesPerHour()
        {
            Assert.AreEqual(1.0, UnitConversion.FromMetric(1.609344, UnitMeasure.Speed, UnitSystem.Imperial), 1e-9);
            Assert.AreEqual(1.609344, UnitConversion.ToMetric(1.0, UnitMeasure.Speed, UnitSystem.Imperial), 1e-9);
        }

        [TestMethod]
        public void ImperialShortDistanceIsFeet()
        {
            Assert.AreEqual(3.280839895, UnitConversion.FromMetric(1.0, UnitMeasure.ShortDistance, UnitSystem.Imperial), 1e-9);
            Assert.AreEqual(0.3048, UnitConversion.ToMetric(1.0, UnitMeasure.ShortDistance, UnitSystem.Imperial), 1e-9);
        }

        [TestMethod]
        public void ImperialLongDistanceIsStatuteMiles()
        {
            Assert.AreEqual(0.621371192, UnitConversion.FromMetric(1.0, UnitMeasure.LongDistance, UnitSystem.Imperial), 1e-9);
            Assert.AreEqual(1.609344, UnitConversion.ToMetric(1.0, UnitMeasure.LongDistance, UnitSystem.Imperial), 1e-9);
        }

        [TestMethod]
        public void ImperialAreaIsAcres()
        {
            Assert.AreEqual(247.105381467, UnitConversion.FromMetric(1.0, UnitMeasure.Area, UnitSystem.Imperial), 1e-8);
            Assert.AreEqual(0.0040468564224, UnitConversion.ToMetric(1.0, UnitMeasure.Area, UnitSystem.Imperial), 1e-12);
        }

        [TestMethod]
        public void RoundTripThroughDisplayUnitsPreservesMetricValue()
        {
            foreach (UnitMeasure measure in Enum.GetValues<UnitMeasure>())
            {
                double displayed = UnitConversion.FromMetric(7.5, measure, UnitSystem.Imperial);
                double back = UnitConversion.ToMetric(displayed, measure, UnitSystem.Imperial);

                Assert.AreEqual(7.5, back, 1e-9, $"round trip failed for {measure}");
            }
        }

        [TestMethod]
        public void AbbreviationsMatchTheSystem()
        {
            Assert.AreEqual("km/h", UnitConversion.Abbreviation(UnitMeasure.Speed, UnitSystem.Metric));
            Assert.AreEqual("mph", UnitConversion.Abbreviation(UnitMeasure.Speed, UnitSystem.Imperial));
            Assert.AreEqual("m", UnitConversion.Abbreviation(UnitMeasure.ShortDistance, UnitSystem.Metric));
            Assert.AreEqual("ft", UnitConversion.Abbreviation(UnitMeasure.ShortDistance, UnitSystem.Imperial));
            Assert.AreEqual("km", UnitConversion.Abbreviation(UnitMeasure.LongDistance, UnitSystem.Metric));
            Assert.AreEqual("mi", UnitConversion.Abbreviation(UnitMeasure.LongDistance, UnitSystem.Imperial));
            Assert.AreEqual("km\u00b2", UnitConversion.Abbreviation(UnitMeasure.Area, UnitSystem.Metric));
            Assert.AreEqual("acres", UnitConversion.Abbreviation(UnitMeasure.Area, UnitSystem.Imperial));
        }

        [TestMethod]
        public void FormatShowsDisplayUnitsAndTrimsTrailingZeros()
        {
            Assert.AreEqual("1.6", UnitConversion.Format(1.6, UnitMeasure.Speed, UnitSystem.Metric, Invariant));
            Assert.AreEqual("1", UnitConversion.Format(1.609344, UnitMeasure.Speed, UnitSystem.Imperial, Invariant));

            Assert.AreEqual("10", UnitConversion.Format(10, UnitMeasure.ShortDistance, UnitSystem.Metric, Invariant));
            Assert.AreEqual("33", UnitConversion.Format(10, UnitMeasure.ShortDistance, UnitSystem.Imperial, Invariant));

            Assert.AreEqual("0.01", UnitConversion.Format(0.01, UnitMeasure.Area, UnitSystem.Metric, Invariant));
            Assert.AreEqual("2.5", UnitConversion.Format(0.01, UnitMeasure.Area, UnitSystem.Imperial, Invariant));
        }

        [TestMethod]
        public void TryParseReadsDisplayUnitsBackToMetric()
        {
            Assert.IsTrue(UnitConversion.TryParseToMetric("33", UnitMeasure.ShortDistance, UnitSystem.Imperial, out double metres, Invariant));
            Assert.AreEqual(10.0584, metres, 1e-9);

            Assert.IsTrue(UnitConversion.TryParseToMetric("2.5", UnitMeasure.Area, UnitSystem.Imperial, out double squareKilometres, Invariant));
            Assert.AreEqual(0.010117141056, squareKilometres, 1e-12);

            Assert.IsTrue(UnitConversion.TryParseToMetric("1.6", UnitMeasure.Speed, UnitSystem.Metric, out double kilometresPerHour, Invariant));
            Assert.AreEqual(1.6, kilometresPerHour, 1e-9);
        }

        [TestMethod]
        public void TryParseRejectsBlankAndUnparseableText()
        {
            Assert.IsFalse(UnitConversion.TryParseToMetric("", UnitMeasure.ShortDistance, UnitSystem.Imperial, out double blank, Invariant));
            Assert.AreEqual(0, blank);

            Assert.IsFalse(UnitConversion.TryParseToMetric(null, UnitMeasure.ShortDistance, UnitSystem.Imperial, out double missing, Invariant));
            Assert.AreEqual(0, missing);

            Assert.IsFalse(UnitConversion.TryParseToMetric("abc", UnitMeasure.Speed, UnitSystem.Metric, out double garbage, Invariant));
            Assert.AreEqual(0, garbage);
        }

        [TestMethod]
        public void StepReturnsMetricEquivalentOfTheDisplayStep()
        {
            Assert.AreEqual(1, UnitConversion.Step(1, 5, UnitMeasure.ShortDistance, UnitSystem.Metric), 1e-9);
            Assert.AreEqual(1.524, UnitConversion.Step(1, 5, UnitMeasure.ShortDistance, UnitSystem.Imperial), 1e-9);

            Assert.AreEqual(0.1, UnitConversion.Step(0.1, 0.1, UnitMeasure.Speed, UnitSystem.Metric), 1e-9);
            Assert.AreEqual(0.1609344, UnitConversion.Step(0.1, 0.1, UnitMeasure.Speed, UnitSystem.Imperial), 1e-9);

            Assert.AreEqual(0.0080937128448, UnitConversion.Step(0.01, 2, UnitMeasure.Area, UnitSystem.Imperial), 1e-12);
        }

        [TestMethod]
        public void SteppingInImperialMovesWholeDisplayUnits()
        {
            // A grid search user in imperial taps "+" on spacing: the metric value the
            // calculator keeps must land exactly five feet higher on screen.
            double metricSpacing = 10;
            double before = UnitConversion.FromMetric(metricSpacing, UnitMeasure.ShortDistance, UnitSystem.Imperial);

            metricSpacing += UnitConversion.Step(1, 5, UnitMeasure.ShortDistance, UnitSystem.Imperial);
            double after = UnitConversion.FromMetric(metricSpacing, UnitMeasure.ShortDistance, UnitSystem.Imperial);

            Assert.AreEqual(5.0, after - before, 1e-9);
        }
    }
}

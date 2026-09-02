namespace MySarAssistModels.Units
{
    /// <summary>
    /// What a number measures, which fixes both the metric unit it is stored in and the
    /// imperial unit it is shown in.
    /// </summary>
    public enum UnitMeasure
    {
        /// <summary>Stored as km/h, shown as mph.</summary>
        Speed = 0,

        /// <summary>Stored as metres, shown as feet. Spacing, elevation, detection ranges.</summary>
        ShortDistance = 1,

        /// <summary>Stored as kilometres, shown as statute miles. Route and segment lengths.</summary>
        LongDistance = 2,

        /// <summary>Stored as square kilometres, shown as acres. Search area sizes.</summary>
        Area = 3
    }
}

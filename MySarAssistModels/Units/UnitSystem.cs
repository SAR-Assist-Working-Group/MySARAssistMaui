namespace MySarAssistModels.Units
{
    /// <summary>
    /// The unit system a value is displayed in. Stored records and every calculation stay
    /// metric regardless of this setting - it only affects what the user sees and types.
    /// </summary>
    public enum UnitSystem
    {
        Metric = 0,
        Imperial = 1
    }
}

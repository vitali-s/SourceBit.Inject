namespace SourceBit.Inject
{
    /// <summary>
    /// Specifies when the class is created and how long it exists in IoC container.
    /// </summary>
    public enum LifeTypes
    {
        /// <summary>
        /// The class is created only once and exists forever.
        /// </summary>
        Single = 1,

        /// <summary>
        /// The class is created every time when the Resolve method is called.
        /// </summary>
        PerDependency = 2,
    }
}

namespace Trip.Domain
{
    /// <summary>
    /// The trip status.
    /// </summary>
    public enum TripStatus
    {
        /// <summary>
        /// Waiting for the start date of the trip.
        /// </summary>
        Waiting = 0,

        /// <summary>
        /// In trip way.
        /// </summary>
        InProgress = 1,

        /// <summary>
        /// The trip was canseled by the driver.
        /// </summary>
        Canceled = 2,

        /// <summary>
        /// The trip was canseled by the driver.
        /// </summary>
        Completed = 3,
    }
}

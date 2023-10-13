namespace Trip.Domain
{
    /// <summary>
    /// The booking status.
    /// </summary>
    public enum BookingStatus
    {
        /// <summary>
        /// The order is awaiting confirmation from the trip driver.
        /// </summary>
        Waiting = 0,

        /// <summary>
        /// The order was canceled by the trip driver.
        /// </summary>
        CanceledByDriver = 1,

        /// <summary>
        /// The order was canceled by the user who created the order.
        /// </summary>
        CanceledByPassenger = 2,

        /// <summary>
        /// The order was confirmed by the trip driver.
        /// </summary>
        Confirmed = 3,

        /// <summary>
        /// The order was successfully completed.
        /// </summary>
        Completed = 4,
    }
}

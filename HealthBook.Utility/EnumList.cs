namespace HealthBook.Utility
{
    public class EnumList
    {
        /// <summary>
        /// This enum is use for get response from api execution
        /// </summary>
        public enum ResponseType
        {
            Error = 0,
            Success = 1,
            Exception = 2,
            NotFound = 404
        }

        public enum BookingStatus
        {
            Booked = 1,
            Completed = 2,
            Cancelled = 3,
        }
    }
}

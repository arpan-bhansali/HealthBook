using static HealthBook.Utility.EnumList;

namespace HealthBook.Model
{
    public class Appointment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int HealthProfessionalId { get; set; }
        public DateTime AppointmentStartTime { get; set; }
        public DateTime AppointmentEndTime { get; set; }
        public BookingStatus Status { get; set; }
    }
}

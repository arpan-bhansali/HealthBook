using HealthBook.Data;
using HealthBook.Model;
using HealthBook.Utility;
using Microsoft.EntityFrameworkCore;

namespace HealthBook.Repository.AppointmentRepo
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly HealthBookContext _context;

        public AppointmentRepository(HealthBookContext context)
        {
            _context = context;
        }

        public async Task<BaseResponseModel> BookAppointmentsAsync(Appointment appointment)
        {
            if (appointment.AppointmentStartTime < DateTime.Now)
                return StatusBuilder.ResponseFailStatus("Appointment time must be in the future.");

            // Check for healthcare professional availability
            bool isProfessionalAvailable = !await _context.Appointments
                .AnyAsync(a => a.HealthProfessionalId == appointment.HealthProfessionalId &&
                               a.AppointmentStartTime < appointment.AppointmentEndTime &&
                               a.AppointmentEndTime > appointment.AppointmentStartTime &&
                               a.Status != EnumList.BookingStatus.Cancelled);

            if (!isProfessionalAvailable)
                return StatusBuilder.ResponseFailStatus("The health professional is not available at the requested time.");

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            return StatusBuilder.ResponseSuccessStatus(appointment);
        }

        public async Task<BaseResponseModel> CancelAppointmentsAsync(int appointmentId)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment == null)
                return StatusBuilder.ResponseFailStatus("Appointments " + StringConstant.NotFound);

            if (appointment.AppointmentStartTime <= DateTime.Now.AddHours(24))
                return StatusBuilder.ResponseFailStatus("Cannot cancel appointment within 24 hours.");

            if (appointment.Status == EnumList.BookingStatus.Completed)
                return StatusBuilder.ResponseFailStatus("Cannot cancel already completed appointment.");

            appointment.Status = EnumList.BookingStatus.Cancelled;
            await _context.SaveChangesAsync();
            return StatusBuilder.ResponseSuccessStatus("Appointment cancelled successfully.");
        }

        public async Task<BaseResponseModel> CompleteAppointmentAsync(int appointmentId)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment == null)
                return StatusBuilder.ResponseFailStatus("Appointments " + StringConstant.NotFound);

            if (appointment.AppointmentStartTime >= DateTime.Now)
                return StatusBuilder.ResponseFailStatus("Cannot complete appointment prior start time.");

            if (appointment.Status == EnumList.BookingStatus.Cancelled)
                return StatusBuilder.ResponseFailStatus("Cannot complete already cancelled appointment.");

            appointment.Status = EnumList.BookingStatus.Completed;
            await _context.SaveChangesAsync();
            return StatusBuilder.ResponseSuccessStatus("Appointment completed successfully.");
        }

        public async Task<BaseResponseModel> GetAppointmentsAsync(int userId)
        {
            var appointments = await _context.Appointments.Where(a=>a.UserId==userId).ToListAsync();

            if (appointments != null && appointments.Any())
            {
                return StatusBuilder.ResponseSuccessStatus(appointments);
            }

            return StatusBuilder.ResponseFailStatus("Appointments " + StringConstant.NotFound);
        }
    }
}

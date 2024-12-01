using HealthBook.Model;
using HealthBook.Utility;

namespace HealthBook.Repository.AppointmentRepo
{
    public interface IAppointmentRepository
    {
        Task<BaseResponseModel> GetAppointmentsAsync(int userId);

        Task<BaseResponseModel> BookAppointmentsAsync(Appointment appointment);

        Task<BaseResponseModel> CancelAppointmentsAsync(int appointmentId);
            
        Task<BaseResponseModel> CompleteAppointmentAsync(int  appointmentId);
    }
}

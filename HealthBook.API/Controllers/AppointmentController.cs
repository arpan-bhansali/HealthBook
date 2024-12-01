using HealthBook.Model;
using HealthBook.Repository.AppointmentRepo;
using HealthBook.Repository.UserRepo;
using HealthBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthBook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IConfiguration _configuration;

        public AppointmentController(IAppointmentRepository appointmentRepository, IConfiguration configuration)
        {
            _appointmentRepository = appointmentRepository;
            _configuration = configuration;
        }

        [HttpPost("book")]
        public async Task<BaseResponseModel> BookAppointment([FromBody] Appointment appointment)
        {
            try
            {
                return await _appointmentRepository.BookAppointmentsAsync(appointment);
            }
            catch (Exception ex)
            {
                return StatusBuilder.ResponseExceptionStatus(ex);
            }
        }

        [HttpPut("cancel/{id}")]
        public async Task<BaseResponseModel> CancelAppointment(int id)
        {
            try
            {
                return await _appointmentRepository.CancelAppointmentsAsync(id);
            }
            catch (Exception ex)
            {
                return StatusBuilder.ResponseExceptionStatus(ex);
            }
        }

        [HttpPut("complete/{id}")]
        public async Task<BaseResponseModel> CompleteAppointment(int id)
        {
            try
            {
                return await _appointmentRepository.CompleteAppointmentAsync(id);
            }
            catch (Exception ex)
            {
                return StatusBuilder.ResponseExceptionStatus(ex);
            }
        }

        [HttpGet("appointments/{id}")]
        public async Task<BaseResponseModel> GetAppointments(int id)
        {
            try
            {
                return await _appointmentRepository.GetAppointmentsAsync(id);
            }
            catch (Exception ex)
            {
                return StatusBuilder.ResponseExceptionStatus(ex);
            }
        }
    }
}

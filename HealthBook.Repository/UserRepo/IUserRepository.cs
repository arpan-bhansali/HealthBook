using HealthBook.Model;
using HealthBook.Utility;

namespace HealthBook.Repository.UserRepo
{
    public interface IUserRepository
    {
        Task<User?> LoginAsync(Login loginDetail);

        Task<BaseResponseModel> RegisterAsync(User userDetail);

        Task<BaseResponseModel> GetProfessionalAsync();
    }
}

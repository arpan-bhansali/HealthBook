using HealthBook.Data;
using HealthBook.Model;
using HealthBook.Utility;
using Microsoft.EntityFrameworkCore;

namespace HealthBook.Repository.UserRepo
{
    public class UserRepository : IUserRepository
    {
        private readonly HealthBookContext _context;

        public UserRepository(HealthBookContext context)
        {
            _context = context;
        }

        public async Task<User?> LoginAsync(Login loginDetail)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDetail.Email);
            if (user != null && PasswordHashUtility.VerifyHashedPassword(user.Password, loginDetail.Password)) {
                return user;
            }

            return null;
        }

        public async Task<BaseResponseModel> RegisterAsync(User userDetail)
        {
            userDetail.Password=PasswordHashUtility.HashPassword(userDetail.Password);
            _context.Users.Add(userDetail);
            await _context.SaveChangesAsync();
            return StatusBuilder.ResponseSuccessStatus(userDetail);
        }

        public async Task<BaseResponseModel> GetProfessionalAsync()
        {
            var professionals = await _context.HealthProfessionals.ToListAsync();

            if(professionals != null && professionals.Any())
            {
                return StatusBuilder.ResponseSuccessStatus(professionals);
            }

            return StatusBuilder.ResponseFailStatus("Professionals " + StringConstant.NotFound);
        }
    }
}

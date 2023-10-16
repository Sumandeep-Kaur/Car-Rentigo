using CarRental.Data.Entities;
using System.Threading.Tasks;

namespace CarRental.Business.IServices
{
    public interface IUserService
    {
        Task AddUser(User user);
        Task<bool> UserExists(string email);
        Task<User> Authenticate(string email, string password);
    }
}

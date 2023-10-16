using CarRental.Data.Entities;
using CarRental.Data.UnitOfWork;
using System.Threading.Tasks;

namespace CarRental.Data.DBContext
{
    public class DbSeeder
    {

        private readonly IUnitOfWork _unitOfWork;

        public DbSeeder(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task SeedAdminUser()
        {
            var admin = new User
            {
                Name = "Admin",
                Email = "admin@gmail.com",
                Password = "#Admin@123",
                Role = "Admin"
            };

            await _unitOfWork.UserRepository.Insert(admin);
            await _unitOfWork.SaveAsyc();
        }
    }
}

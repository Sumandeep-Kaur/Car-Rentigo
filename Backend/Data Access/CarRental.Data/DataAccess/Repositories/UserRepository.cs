using CarRental.Data.DataAccess.IRepositories;
using CarRental.Data.DBContext;
using CarRental.Data.Entities;
using CarRental.Data.GenericRepository;

namespace CarRental.Data.DataAccess.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(CarRentalDbContext dbContext) : base(dbContext)
        {

        }
    }
}

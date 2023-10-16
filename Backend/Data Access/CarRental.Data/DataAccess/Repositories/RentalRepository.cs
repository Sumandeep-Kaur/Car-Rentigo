using CarRental.Data.DataAccess.IRepositories;
using CarRental.Data.DBContext;
using CarRental.Data.Entities;
using CarRental.Data.GenericRepository;

namespace CarRental.Data.DataAccess.Repositories
{
    public class RentalRepository : Repository<Rental>, IRentalRepository
    {
        public RentalRepository(CarRentalDbContext dbContext) : base(dbContext)
        {

        }
    }
}

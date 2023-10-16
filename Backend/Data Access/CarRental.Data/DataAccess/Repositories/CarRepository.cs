using CarRental.Data.DataAccess.IRepositories;
using CarRental.Data.DBContext;
using CarRental.Data.Entities;
using CarRental.Data.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Data.DataAccess.Repositories
{
    public class CarRepository : Repository<Car>, ICarRepository
    {
        public CarRepository(CarRentalDbContext dbContext) : base(dbContext)
        {

        }
    }
}

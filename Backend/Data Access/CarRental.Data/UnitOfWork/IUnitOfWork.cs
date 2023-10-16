using CarRental.Data.DataAccess.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        ICarRepository CarRepository { get; }
        IUserRepository UserRepository { get; }
        IRentalRepository RentalRepository { get; }

        Task SaveAsyc();
        Task CreateTransaction();
        Task Commit();
        Task RollBack();
    }
}

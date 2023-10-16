using CarRental.Data.DataAccess.IRepositories;
using CarRental.Data.DBContext;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CarRentalDbContext _dbContext;
        private readonly ICarRepository _carRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRentalRepository _rentalRepository;
        private IDbContextTransaction transaction;

        public UnitOfWork(CarRentalDbContext dbContext, ICarRepository carRepository, 
            IUserRepository userRepository, IRentalRepository rentalRepository)
        {
            _dbContext = dbContext;
            _carRepository = carRepository;
            _userRepository = userRepository;
            _rentalRepository = rentalRepository;
        }

        private bool _disposedValue;

        public ICarRepository CarRepository => _carRepository;

        public IUserRepository UserRepository => _userRepository;

        public IRentalRepository RentalRepository => _rentalRepository;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    if (_dbContext != null)
                    {
                        _dbContext.Dispose();
                    }
                }
            }
            _disposedValue = true;
        }

        public async Task SaveAsyc()
        {
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task CreateTransaction()
        {
            this.transaction = await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task Commit()
        {
            await this.transaction.CommitAsync();
        }

        public async Task RollBack()
        {
            await this.transaction.RollbackAsync();
            await this.transaction.DisposeAsync();
        }
    }
}

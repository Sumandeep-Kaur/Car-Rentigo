using CarRental.Business.IServices;
using CarRental.Data.Entities;
using CarRental.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Business.Services
{
    public class RentalService : IRentalService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RentalService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Add(Rental rental)
        {
            try
            {
                await _unitOfWork.CreateTransaction();
                Car car = await _unitOfWork.CarRepository.GetById(rental.CarID);
                await _unitOfWork.RentalRepository.Insert(rental);
                car.IsAvailable = false;
                await _unitOfWork.SaveAsyc();
                await _unitOfWork.Commit();
            }
            catch (Exception)
            {
                await _unitOfWork.RollBack();
                throw;
            }
        }

        public async Task Update(Rental rental, int id)
        {
            var entity = await GetRentalById(id);
            entity.RentalStartDate = rental.RentalStartDate;
            entity.RentDurationDays = rental.RentDurationDays;
            entity.RentalEndDate = rental.RentalEndDate;
            entity.TotalCost = rental.TotalCost;

            try
            {
                await _unitOfWork.CreateTransaction();
                await _unitOfWork.RentalRepository.Update(entity);
                await _unitOfWork.SaveAsyc();
                await _unitOfWork.Commit();
            }
            catch (Exception)
            {
                await _unitOfWork.RollBack();
                throw;
            }
        }

        public async Task Delete(int rentalId)
        {
            var rental = await GetRentalById(rentalId);
            await Delete(rental);
        }

        public async Task Delete(Rental rental)
        {
            try
            {
                Car car = await _unitOfWork.CarRepository.GetById(rental.CarID);
                await _unitOfWork.CreateTransaction();
                await _unitOfWork.RentalRepository.Delete(rental);
                car.IsAvailable = true;
                await _unitOfWork.SaveAsyc();
                await _unitOfWork.Commit();
            }
            catch (Exception)
            {
                await _unitOfWork.RollBack();
                throw;
            }
        }

        public async Task RequestForRentalReturn(int rentalId)
        {
            try
            {
                await _unitOfWork.CreateTransaction();
                Rental rental = await _unitOfWork.RentalRepository.GetById(rentalId);
                rental.RequestForReturn = true;
                await _unitOfWork.SaveAsyc();
                await _unitOfWork.Commit();
            }
            catch (Exception)
            {
                await _unitOfWork.RollBack();
                throw;
            }
        }

        public async Task<Rental> GetRentalById(int rentalId)
        {
            return await _unitOfWork.RentalRepository.GetById(rentalId);
        }

        public async Task<List<Rental>> GetAllRentals()
        {
            Expression<Func<Rental, object>>[] include = new Expression<Func<Rental, object>>[]
            {
                p => p.Car,
                p => p.User
            };
            return (List<Rental>)await _unitOfWork.RentalRepository.GetAll(include);
        }

        public async Task<List<Rental>> GetUserRentals(int userId)
        {
            Expression<Func<Rental, bool>> filter = rental => rental.UserID == userId;
            Expression<Func<Rental, object>>[] include = new Expression<Func<Rental, object>>[]
            {
                p => p.Car
            };
            return (List<Rental>)await _unitOfWork.RentalRepository.Filter(filter, include);
        }

        public async Task<List<Rental>> GetReturnRequestedRentals()
        {
            Expression<Func<Rental, bool>> filter = rental => rental.RequestForReturn == true;
            Expression<Func<Rental, object>>[] include = new Expression<Func<Rental, object>>[]
            {
                p => p.Car,
                p => p.User
            };
            return (List<Rental>)await _unitOfWork.RentalRepository.Filter(filter, include);
        }
    }
}

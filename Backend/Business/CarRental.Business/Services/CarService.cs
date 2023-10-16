using CarRental.Business.IServices;
using CarRental.Data.Entities;
using CarRental.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CarRental.Business.Services
{
    public class CarService : ICarService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CarService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Add(Car car)
        {
            try
            {
                await _unitOfWork.CreateTransaction();
                await _unitOfWork.CarRepository.Insert(car);
                await _unitOfWork.SaveAsyc();
                await _unitOfWork.Commit();
            }
            catch (Exception)
            {
                await _unitOfWork.RollBack();
                throw;
            }
        }

        public async Task Update(Car car, int id)
        {
            var entity = await GetCarById(id);
            entity.Name = car.Name;
            entity.Brand = car.Brand;
            entity.Model = car.Model;
            entity.Color = car.Color;
            entity.Capacity = car.Capacity;
            entity.FuelType = car.FuelType;
            entity.IsAvailable = car.IsAvailable;
            entity.Price = car.Price;
            entity.ImageUrl = car.ImageUrl;

            try
            {
                await _unitOfWork.CreateTransaction();
                await _unitOfWork.CarRepository.Update(entity);
                await _unitOfWork.SaveAsyc();
                await _unitOfWork.Commit();
            }
            catch (Exception)
            {
                await _unitOfWork.RollBack();
                throw;
            }
        }

        public async Task Delete(int carId)
        {
            try
            {
                await _unitOfWork.CreateTransaction();
                await _unitOfWork.CarRepository.Delete(carId);
                await _unitOfWork.SaveAsyc();
                await _unitOfWork.Commit();
            }
            catch (Exception)
            {
                await _unitOfWork.RollBack();
                throw;
            }
        }

        public async Task Delete(Car car)
        {
            try
            {
                await _unitOfWork.CreateTransaction();
                await _unitOfWork.CarRepository.Delete(car);
                await _unitOfWork.SaveAsyc();
                await _unitOfWork.Commit();
            }
            catch (Exception)
            {
                await _unitOfWork.RollBack();
                throw;
            }
        }

        public async Task<List<Car>> GetAllCars()
        {
            return (List<Car>)await _unitOfWork.CarRepository.GetAll();
        }

        public async Task<Car> GetCarById(int carId)
        {
            return await _unitOfWork.CarRepository.GetById(carId);
        }

        public async Task<List<Car>> SearchCars(string searchBy, string searchValue)
        {
            if(searchBy == "Brand")
            {
                return await GetCarsByBrand(searchValue);
            }
            else if (searchBy == "Model")
            {
                return await GetCarsByModel(searchValue);
            }
            else if (searchBy == "Price")
            {
                decimal price = decimal.Parse(searchValue);
                return await GetCarsByPrice(price);
            }
            else
            {
                return await GetCarsByColor(searchValue);
            }
        }

        public async Task<List<Car>> GetCarsByBrand(string brand)
        {
            Expression<Func<Car, bool>> brandFilter = car => car.Brand == brand;
            return (List<Car>)await _unitOfWork.CarRepository.Filter(brandFilter);
        }

        public async Task<List<Car>> GetCarsByModel(string model)
        {
            Expression<Func<Car, bool>> brandFilter = car => car.Model == model;
            return (List<Car>)await _unitOfWork.CarRepository.Filter(brandFilter);
        }

        public async Task<List<Car>> GetCarsByPrice(decimal price)
        {
            Expression<Func<Car, bool>> brandFilter = car => car.Price == price;
            return (List<Car>)await _unitOfWork.CarRepository.Filter(brandFilter);
        }

        public async Task<List<Car>> GetCarsByColor(string color)
        {
            Expression<Func<Car, bool>> brandFilter = car => car.Color == color;
            return (List<Car>)await _unitOfWork.CarRepository.Filter(brandFilter);
        }
    }
}

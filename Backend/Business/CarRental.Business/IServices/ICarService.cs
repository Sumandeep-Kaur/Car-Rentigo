using CarRental.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRental.Business.IServices
{
    public interface ICarService
    {
        Task Add(Car car);
        Task Update(Car car, int id);
        Task Delete(int carId);
        Task Delete(Car car);
        Task<Car> GetCarById(int carId);
        Task<List<Car>> GetAllCars();
        Task<List<Car>> GetCarsByBrand(string brand);
        Task<List<Car>> GetCarsByModel(string model);
        Task<List<Car>> GetCarsByPrice(decimal price);
        Task<List<Car>> GetCarsByColor(string color);
        Task<List<Car>> SearchCars(string searchBy, string searchValue);
    }
}

using CarRental.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Business.IServices
{
    public interface IRentalService
    {
        Task Add(Rental rental);
        Task Update(Rental rental, int rentalId);
        Task Delete(int rentalId);
        Task Delete(Rental rental);
        Task RequestForRentalReturn(int rentalId);
        Task<Rental> GetRentalById(int rentalId);
        Task<List<Rental>> GetAllRentals();
        Task<List<Rental>> GetUserRentals(int userId);
        Task<List<Rental>> GetReturnRequestedRentals();
    }
}

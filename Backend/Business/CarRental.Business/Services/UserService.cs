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
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddUser(User user)
        {
            try
            {
                user.Role = "User";
                await _unitOfWork.CreateTransaction();
                await _unitOfWork.UserRepository.Insert(user);
                await _unitOfWork.SaveAsyc();
                await _unitOfWork.Commit();
            }
            catch (Exception)
            {
                await _unitOfWork.RollBack();
                throw;
            }
        }

        public async Task<User> Authenticate(string email, string password)
        {
            Expression<Func<User, bool>> userFilter = user => user.Email == email && user.Password == password;
            return await _unitOfWork.UserRepository.Get(userFilter);
        }

        public async Task<bool> UserExists(string email)
        {
            Expression<Func<User, bool>> userFilter = user => user.Email == email;
            return await _unitOfWork.UserRepository.Contains(userFilter);
        }
    }
}

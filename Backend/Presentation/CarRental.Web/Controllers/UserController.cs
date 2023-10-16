using CarRental.Business.IServices;
using CarRental.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        [Consumes("application/json")]
        public async Task<IActionResult> Register(User userModel)
        {
            var userExists = await _userService.UserExists(userModel.Email);
            if (userExists)
            {
                return BadRequest(new
                {
                    Status = "Unsuccess",
                    Message = "User Already Exists in the database."
                });
            }

            await _userService.AddUser(userModel);
            return Ok(new
            {
                Status = "Success",
                Message = "User Registered Successfully!"
            });
        }

        [HttpPost("login")]
        [Consumes("application/json")]
        public async Task<IActionResult> Login(User userModel)
        {
            var user = await _userService.Authenticate(userModel.Email, userModel.Password);

            if (user != null)
            {
                return Ok(new
                {
                    Status = "Success",
                    Message = "Login Successful!",
                    User = new
                    {
                        userId = user.ID,
                        name = user.Name,
                        role = user.Role
                    }
                });
            }
            return Unauthorized(new
            {
                Status = "Unsuccess",
                Message = "Login Unsuccesful, Invalid Credentials!"
            });
        }
    }
}

using CarRental.Business.IServices;
using CarRental.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RentalController : ControllerBase
    {
        private readonly IRentalService _rentalService;

        public RentalController(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }

        [HttpPost("add")]
        [Consumes("application/json")]
        public async Task<IActionResult> AddCar(Rental rental)
        {
            try
            {
                await _rentalService.Add(rental);
                return Ok(new
                {
                    Status = "Success",
                    Message = "Rental Added Successfully!"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    ex.Message,
                    Status = "Failed",
                    Messaage = "Cannot add car to rentals."
                }); ;
            }
        }

        [HttpGet("userId")]
        public async Task<IActionResult> GetUserRentals(int userId)
        {
            try
            {
                var rentals = await _rentalService.GetUserRentals(userId);
                return Ok(rentals);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    ex.Message,
                    Error = "Key not found in database.",
                });
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllRentals()
        {
            try
            {
                var rentals = await _rentalService.GetAllRentals();
                return Ok(rentals);
            }
            catch (DataException ex)
            {
                return NotFound(new
                {
                    ex.Message,
                    Error = "Cannot get data."
                });
            }
        }

        [HttpGet("returns")]
        public async Task<IActionResult> GetReturnRequestRentals()
        {
            try
            {
                var rentals = await _rentalService.GetReturnRequestedRentals();
                return Ok(rentals);
            }
            catch (DataException ex)
            {
                return NotFound(new
                {
                    ex.Message,
                    Error = "Cannot get data."
                });
            }
        }

        [HttpPut("edit/id")]
        public async Task<IActionResult> UpdateRental(int id, [FromBody] Rental rental)
        {
            try
            {
                await _rentalService.Update(rental, id);
                return Ok(new
                {
                    Status = "Success",
                    Message = "Car Updated Successfully!"
                });
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new
                {
                    ex.Message,
                    Status = "Failed",
                    Messaage = "Cannot update car.",
                }); ;
            }
        }

        [HttpDelete("delete/id")]
        public async Task<IActionResult> DeleteRental([FromQuery] int id)
        {
            try
            {
                await _rentalService.Delete(id);
                return Ok(new
                {
                    Status = "Success",
                    Message = "Car Deleted Successfully!"
                });
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new
                {
                    ex.Message,
                    Status = "Failed",
                    Messaage = "Cannot delete car."
                }); ;
            }
        }

        [HttpPut("return/id")]
        public async Task<IActionResult> RequestForReturn(int id)
        {
            try
            {
                await _rentalService.RequestForRentalReturn(id);
                return Ok(new
                {
                    Status = "Success",
                    Message = "Return Request Added Successfully!"
                });
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new
                {
                    ex.Message,
                    Status = "Failed",
                    Messaage = "Cannot Put Request For Return Car.",
                }); ;
            }
        }
    }
}

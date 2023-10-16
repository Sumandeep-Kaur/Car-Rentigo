using CarRental.Business.IServices;
using CarRental.Data.Entities;
using Microsoft.AspNetCore.Authorization;
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
    public class CarController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllCars()
        {
            try
            {
                var cars = await _carService.GetAllCars();
                return Ok(cars);
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

        [HttpGet("id")]
        public async Task<IActionResult> GetCar(int id)
        {
            try
            {
                var car = await _carService.GetCarById(id);
                return Ok(car);
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

        [HttpGet("search")]
        public async Task<IActionResult> SearchCars(string searchBy, string searchValue)
        {
            try
            {
                var car = await _carService.SearchCars(searchBy, searchValue);
                return Ok(car);
            }
            catch (Exception ex)
            {
                return NotFound(new
                {
                    ex.Message,
                    Error = "Cannot Fetch data from database",
                });
            }
        }

        [HttpPost("add")]
        [Consumes("application/json")]
        public async Task<IActionResult> AddCar(Car car)
        {
            try
            {
                await _carService.Add(car);
                return Ok(new
                {
                    Status = "Success",
                    Message = "Car Added Successfully!"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    ex.Message,
                    Status = "Failed",
                    Messaage = "Cannot add car to database."
                }); ;
            }
        }

        [HttpPut("edit/id")]
        public async Task<IActionResult> UpdateCar(int id, [FromBody] Car car)
        {
            try
            {
                await _carService.Update(car, id);
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
        public async Task<IActionResult> DeleteCar([FromQuery] int id)
        {
            try
            {
                await _carService.Delete(id);
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
    }
}

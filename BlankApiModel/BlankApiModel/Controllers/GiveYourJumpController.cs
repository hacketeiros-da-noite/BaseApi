using BlankApiModel.Dao;
using BlankApiModel.Implementations;
using BlankApiModel.Model.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BlankApiModel.Controllers
{
    /// <summary>
    /// Example of a controller used to demonstrate the use of the template.
    /// </summary>
    public class GiveYourJumpController : Controller
    {
        private readonly GiveYourJumpsService _giveYourJumpsService;
        public GiveYourJumpController(GiveYourJumpsService giveYourJumpsService)
        {
            _giveYourJumpsService = giveYourJumpsService;
        }

        /// <summary>
        /// Simple GET request example
        /// </summary>
        /// <returns></returns>
        [HttpGet("IsAlive")]
        public IActionResult IsAlive() => Ok("Nobody yes door");

        /// <summary>
        /// Example of selecting all database data through a GET request.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _giveYourJumpsService.GetAllAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Example of insertion using a POST request.
        /// </summary> 
        [HttpPost("InsertGiveYourJumps")]
        public async Task<IActionResult> Post(GiveYourJumpsModel giveYourJumps)
        {
            try
            {
                return Ok(await _giveYourJumpsService.InsertAsync(giveYourJumps));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
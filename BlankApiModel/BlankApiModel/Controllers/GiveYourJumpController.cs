using BlankApiModel.Dao;
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
        private IBaseDao _dbConnection;
        public GiveYourJumpController(IBaseDao dbConnection)
        {
            _dbConnection = dbConnection;
        }

        [HttpGet("IsAlive")]
        public IActionResult IsAlive()
        {
            return Ok("Nobody yes door");
        }

        /// <summary>
        /// Example of selecting all database data through a GET request.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _dbConnection.GetAll<GiveYourJumpsModel>());
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
                return Ok(await _dbConnection.Insert(giveYourJumps));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
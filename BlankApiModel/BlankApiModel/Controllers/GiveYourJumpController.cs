using BlankApiModel.Dao;
using BlankApiModel.Model.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BlankApiModel.Controllers
{
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
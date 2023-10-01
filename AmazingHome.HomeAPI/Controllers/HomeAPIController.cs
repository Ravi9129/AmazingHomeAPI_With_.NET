using AmazingHome.HomeAPI.Data;
using AmazingHome.HomeAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace AmazingHome.HomeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeAPIController : ControllerBase
    {
        private readonly ILogger<HomeAPIController> _logger;
       

        public HomeAPIController(ILogger<HomeAPIController>logger)
        {
            _logger = logger;  
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Home>> GetHomes()
        {
            _logger.LogInformation("Gettng all homes");
            return Ok(HomeStore.homeList);

        }
        [HttpGet("{id:int}", Name = "GetHome")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        /*   [ProducesResponseType(200, Type = typeof(Home))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
     */
        public ActionResult<Home> GetHome(int id)
        {


            if (id == 0)
            {
                _logger.LogError("Get Home Error with Id");
                return BadRequest();
            }

            var home = HomeStore.homeList.FirstOrDefault(x => x.Id == id);
            if (home == null)
            {
                return NotFound();
            }
            return Ok(home);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult<Home> CreateHome([FromBody] Home home)
        {
            /*
         if (!ModelState.IsValid)
         {
             return BadRequest(ModelState);
         }
         */
            if (HomeStore.homeList.FirstOrDefault(x => x.Name.ToLower() == home.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Home Already Exixts!");
                return BadRequest(ModelState);
            }

            if (home == null)
            {
                return BadRequest(home);
            }
            if (home.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            home.Id = HomeStore.homeList.OrderByDescending(x => x.Id).FirstOrDefault().Id + 1;

            HomeStore.homeList.Add(home);
            return CreatedAtRoute("GetHome", new { id = home.Id }, home);

        }

        [HttpDelete("{id:int}", Name = "DeleteHome")]
        public IActionResult DeleteHome(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var home = HomeStore.homeList.FirstOrDefault(x => x.Id == id);
            if (home == null)
            {
                return NotFound();
            }
            HomeStore.homeList.Remove(home);
            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateHome")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult UpdateHome(int id, [FromBody] Home home)
        {
            if (home == null || id != home.Id)
            {
                return BadRequest();
            }
            var homeexist = HomeStore.homeList.FirstOrDefault(x => x.Id == id);
            homeexist.Name = home.Name;
            homeexist.Sqft = home.Sqft;
            homeexist.Occupancy = home.Occupancy;
            return NoContent();
        }
    }
    }

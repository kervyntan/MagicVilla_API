using MagicVilla_VillaAPI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(_context.Users.ToList());
        }

        [HttpGet("{id:int}")]
        public IActionResult GetUserById(int id)
        {
            if(id > _context.Users.ToList().Count)
            {
                return BadRequest();
            }

            var response = _context.Users.FirstOrDefault(x => x.Id == id);

            if (response == null)
            {
                return NotFound("User was not found.");
            }

            return Ok(response);
        }

        [HttpGet("{name}")]
        public IActionResult GetUserByName(string name)
        {
            if (name == "")
            {
                return BadRequest();
            }

            var response = _context.Users.FirstOrDefault(x => x.Name == name);

            if (response == null)
            {
                return NotFound("User was not found.");
            }

            return Ok(response);
        }
    }
}

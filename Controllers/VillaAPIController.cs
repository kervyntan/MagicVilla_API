using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dtos;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Controllers
{
    // Attribute
    // Route Attribute -> necessary for controllers
    [Route("api/[controller]")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        // Initialize constructor to use the DbContext from Data
        public VillaAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {
            return Ok(await _context.Villas.ToListAsync());
            //return new List<VillaDTO>
            //{
            //    new VillaDTO { Id = 1, Name="Pool Van"},
            //};
        }

        // Getting Villa by Id
        // appending the placeholder for id to api/villa
        // makes sure id passed in is required, and also of type int 
        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDTO>> GetVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            if (id == -1 || id > _context.Villas.ToList().Count)
            {
                return BadRequest("Id provided is invalid.");
            }

            return Ok(await _context.Villas.FirstOrDefaultAsync
            (
                u => u.Id == id
            ));
        }

        // PUT Method to update name of villa
        [HttpPut("id")]
        public async Task<ActionResult<VillaDTO>> UpdateVillaName(int id, string updatedName)
        {
            if (id == -1 || id > _context.Villas.ToList().Count)
            {
                return BadRequest("Id provided is invalid.");
            }

            // FirstOrDefault -> When you know that you will need to check whether
            // there was an element or not
            Villa entity = await _context.Villas.FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            // if somehow passes Id check
            {
                return NotFound("Villa is not found.");
            }

            entity.Name = updatedName;

            return Ok(entity);
        }

        [HttpPost("create")]
        public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody] VillaCreateDTO villaDTO)
        {
            // Used to check the Model validation (eg. MaxLength) IF you don't use [ApiController] above
            //if(!ModelState.IsValid) // Model here refers to the villaDTO argument
            //{
            //    return BadRequest("Length of name is too long.");
            //}
            // If the new villa passed in is invalid, then cannot
            if (villaDTO == null || villaDTO.Name == "")
            {
                return BadRequest(villaDTO);
            }

            //if (villaDTO.Id > 0)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError);
            //}

            // if villa's name is NOT unique, then add 
            if (await _context.Villas.FirstOrDefaultAsync(x => x.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                // First parameter is the key i.e."CustomError" - can be left empty
                ModelState.AddModelError("CustomError", "Villa already Exists!");

                // Throws error 400, with response body
                return BadRequest(ModelState);
            }

            // EF Core automatically populates the model with an Id
            Villa model = new()
            {
                Amenity = villaDTO.Amenity,
                Details = villaDTO.Details,
                Name = villaDTO.Name,
                ImageUrl = villaDTO.ImageUrl,
                Occupancy = villaDTO.Occupancy,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft
            };
            await _context.Villas.AddAsync(model);

            // Get all changes done and save it to DB
            await _context.SaveChangesAsync();

            //return Ok(villaDTO);

            // In Response Headers -> location will now have the available route
            // For us to use the GetVilla Endpoint to retrieve this specific added Villa
            return CreatedAtRoute("GetVilla", new { id = model.Id }, model);
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        // Use IActionResult since we don't need to specify the return type
        public async Task<IActionResult> DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest("Villa with this Id does not exist.");
            }

            var villa = await _context.Villas.FirstOrDefaultAsync(x => x.Id == id);

            if (villa == null)
            {
                return NotFound("Villa specified is not found.");
            }

            _context.Villas.Remove(villa);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDTO villaDTO)
        {
            if (villaDTO == null || id != villaDTO.Id)
            {
                return BadRequest();
            }

            // Convert DTO to Villa to pass to Db
            Villa model = new()
            {
                Amenity = villaDTO.Amenity,
                Details = villaDTO.Details,
                Id = villaDTO.Id,
                Name = villaDTO.Name,
                ImageUrl = villaDTO.ImageUrl,
                Occupancy = villaDTO.Occupancy,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft
            };

            _context.Villas.Update(model);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var villa = await _context.Villas.FirstOrDefaultAsync(x => x.Id == id);

            if (villa == null)
            {
                return BadRequest();
            }

            // Convert Villa object into VillaDTO
            VillaUpdateDTO villaUpdateDTO = new()
            {
                Amenity = villa.Amenity,
                Details = villa.Details,
                Id = villa.Id,
                Name = villa.Name,
                ImageUrl = villa.ImageUrl,
                Occupancy = villa.Occupancy,
                Rate = villa.Rate,
                Sqft = villa.Sqft
            };

            patchDTO.ApplyTo(villaUpdateDTO, ModelState);
            Villa model = new Villa()
            {
                Amenity = villaUpdateDTO.Amenity,
                Details = villaUpdateDTO.Details,
                Id = villaUpdateDTO.Id,
                Name = villaUpdateDTO.Name,
                ImageUrl = villaUpdateDTO.ImageUrl,
                Occupancy = villaUpdateDTO.Occupancy,
                Rate = villaUpdateDTO.Rate,
                Sqft = villaUpdateDTO.Sqft
            };

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return NoContent();
        }

    }
}

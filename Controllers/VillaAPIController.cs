using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;

namespace MagicVilla_VillaAPI.Controllers
{
    // Attribute
    // Route Attribute -> necessary for controllers
    [Route("api/[controller]")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        private readonly ILogging _logger;

        public VillaAPIController(ILogging logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            _logger.Log("All Villas retrieved.","");
            return Ok(VillaStore.villaList);
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
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id == 0)
            {
                _logger.Log("Id of " + id + "does not exist.", "error");
                return BadRequest();
            }

            if (id == -1 || id > VillaStore.villaList.Count)
            {
                return BadRequest("Id provided is invalid.");
            }

            return Ok(VillaStore.villaList.FirstOrDefault
            (
                u => u.Id == id
            ));
        }

        // PUT Method to update name of villa
        [HttpPut("id")]
        public ActionResult<VillaDTO> UpdateVillaName(int id, string updatedName)
        {
            if (id == -1 || id > VillaStore.villaList.Count)
            {
                return BadRequest("Id provided is invalid.");
            }

            // FirstOrDefault -> When you know that you will need to check whether
            // there was an element or not
            VillaDTO entity = VillaStore.villaList.FirstOrDefault(x => x.Id == id);

            if (entity == null)
                // if somehow passes Id check
            {
                return NotFound("Villa is not found.");
            }

            entity.Name = updatedName;

            return Ok(entity);
        }

        [HttpPost("create")]
        public ActionResult<VillaDTO> CreateVilla([FromBody]VillaDTO villaDTO)
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

            if (villaDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            // if villa's name is NOT unique, then add 
            if (VillaStore.villaList.FirstOrDefault(x => x.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                // First parameter is the key i.e."CustomError" - can be left empty
                ModelState.AddModelError("CustomError", "Villa already Exists!");
                
                // Throws error 400, with response body
                return BadRequest(ModelState);
            }

            // Order the list by descending order,
            // Get the latest (i.e. First item in the list) based on predicate passed
            // Increment that Id
            villaDTO.Id = VillaStore.villaList.OrderByDescending(x => x.Id).FirstOrDefault().Id + 1;
            VillaStore.villaList.Add(villaDTO);

            //return Ok(villaDTO);

            // In Response Headers -> location will now have the available route
            // For us to use the GetVilla Endpoint to retrieve this specific added Villa
            return CreatedAtRoute("GetVilla", new { id = villaDTO.Id }, villaDTO);
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        // Use IActionResult since we don't need to specify the return type
        public IActionResult DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest("Villa with this Id does not exist.");
            }

            var villa = VillaStore.villaList.FirstOrDefault(x => x.Id == id);

            if (villa == null)
            {
                return NotFound("Villa specified is not found.");
            }

            VillaStore.villaList.Remove(villa);
            return NoContent();
        }

    }
}

using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{
    // Attribute
    // Route Attribute -> necessary for controllers
    [Route("api/[controller]")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            return Ok(VillaStore.villaList);
            //return new List<VillaDTO>
            //{
            //    new VillaDTO { Id = 1, Name="Pool Van"},
            //};
        }

        // Getting Villa by Id
        // appending the placeholder for id to api/villa
        // makes sure id passed in is required, and also of type int 
        [HttpGet("{id:int}")]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            return Ok(VillaStore.villaList.FirstOrDefault
            (
                u => u.Id == id
            ));
        }

        // POST Method to update name of villa
        [HttpPost("id")]
        public ActionResult<VillaDTO> UpdateVillaName(int id, string updatedName)
        {
            VillaDTO entity = VillaStore.villaList.Find(x => x.Id == id);

            entity.Name = updatedName;

            return Ok(entity);
        }
    }
}

using MagicVilla_VillaAPI.Models.Dtos;

namespace MagicVilla_VillaAPI.Data
{
    public static class VillaStore
    {
        public static List<VillaDTO> villaList = new List<VillaDTO>()
        {
            new VillaDTO { Id = 1, Name = "Pool Van" },
            new VillaDTO { Id = 2, Name = "Villa House" },
            new VillaDTO { Id = 3, Name = "Party House" },
            new VillaDTO { Id = 7, Name= "Eighth" }
        };
    }
}

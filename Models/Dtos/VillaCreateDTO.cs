using System.ComponentModel.DataAnnotations;

namespace MagicVilla_VillaAPI.Models.Dtos
{
    public class VillaCreateDTO
    {
        //Not required to provide Id when creating
        //public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        public string Details { get; set; }

        [Required]
        public double Rate { get; set; }

        public int Sqft { get; set; }

        public int Occupancy { get; set; }

        public string ImageUrl { get; set; }

        public string Amenity { get; set; }

    }
}

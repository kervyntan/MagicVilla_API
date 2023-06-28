using MagicVilla_VillaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        // Create individual dbset for each table

        // Name is the name of the table in the database
        public DbSet<Villa> Villas { get; set; }
    }
}

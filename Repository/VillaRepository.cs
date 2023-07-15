using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository
{
    public class VillaRepository : Repository<Villa>, IVillaRepository
    {
        private readonly ApplicationDbContext _db;

        // pass the db context to the base class
        // in this case -> Repository<Villa>
        public VillaRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;   
        }

        public new async Task<Villa> UpdateAsync(Villa entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _db.Villas.Update(entity); 
            await _db.SaveChangesAsync();

            return entity;
        }

    }
}

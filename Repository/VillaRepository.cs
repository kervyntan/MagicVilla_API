using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository
{
    public class VillaRepository : IVillaRepository
    {
        private readonly ApplicationDbContext _context;

        // Using Dependency Injection
        public VillaRepository(ApplicationDbContext context) 
        {
            _context = context;
        }
        public async Task Create(Villa entity)
        {
            await _context.Villas.AddAsync(entity);
            await SaveChanges();
        }

        public async Task<Villa> Get(Expression<Func<Villa, bool>> filter = null, bool tracked = true)
        {
            // IQueryable does not get executed right away, so can do methods on it to manipulate
            IQueryable<Villa> query = _context.Villas;

            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            if (filter != null)
            {
                // if filter not null, apply it on the query from DB
                query = query.Where(filter);
            }

            // Only 1 Villa getting back
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<Villa>> GetAll(Expression<Func<Villa, bool>> filter = null)
        {
            // IQueryable does not get executed right away, so can do methods on it to manipulate
            IQueryable<Villa> query = _context.Villas;

            if (filter != null)
            {
                // if filter not null, apply it on the query from DB
                query = query.Where(filter);
            }

            return await query.ToListAsync();
        }

        public Task Remove(Villa entity)
        {
            throw new NotImplementedException();
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}

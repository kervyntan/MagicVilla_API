using MagicVilla_VillaAPI.Models;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
    public interface IVillaRepository
    {
        // Filter to get the villa items that we want
        // Expression<Func<Villa>> is a filter/condition/linq statement
        Task<List<Villa>> GetAllAsync(Expression<Func<Villa, bool>> filter = null);

        // For Patch -> AsNoTracking
        Task<Villa> GetAsync(Expression<Func<Villa, bool>> filter = null, bool tracked=true);

        // Creating a Villa
        Task CreateAsync(Villa entity);

        // Update Villa
        Task UpdateAsync(Villa entity);

        // Remove Villa
        Task RemoveAsync(Villa entity);

        // Save changes to database
        Task SaveChanges();
    }
}

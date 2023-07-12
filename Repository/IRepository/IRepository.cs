using MagicVilla_VillaAPI.Models;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
    // Generic
    public interface IRepository<T> where T : class
    {
        // Filter to get the villa items that we want
        // Expression<Func<Villa>> is a filter/condition/linq statement
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null);

        // For Patch -> AsNoTracking
        Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true);

        // Creating a Villa
        Task CreateAsync(T entity);
        // Remove Villa
        Task RemoveAsync(T entity);

        // Save changes to database
        Task SaveChanges();
    }
}

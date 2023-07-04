using MagicVilla_VillaAPI.Models;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
    public interface IVillaRepository
    {
        // Filter to get the villa items that we want
        // Expression<Func<Villa>> is a filter/condition/linq statement

        // Expression<Func<Type, ReturnType>>
        Task<List<Villa>> GetAll(Expression<Func<Villa, bool>> filter = null);

        // For Patch -> AsNoTracking
        Task<Villa> Get(Expression<Func<Villa, bool>> filter = null, bool tracked=true);

        // Creating a Villa
        Task Create(Villa entity);

        // Remove Villa
        Task Remove(Villa entity);

        // Save changes to database
        Task SaveChanges();
    }
}

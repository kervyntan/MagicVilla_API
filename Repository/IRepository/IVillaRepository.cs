using MagicVilla_VillaAPI.Models;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
    public interface IVillaRepository : IRepository<Villa>
    {
        // Update Villa
        // not inherited fromm IRepository because Update tends to be
        // specific to that entity
        Task<Villa> UpdateAsync(Villa entity);

    }
}

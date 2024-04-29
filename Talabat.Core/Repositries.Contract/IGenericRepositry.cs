using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repositries.Contract
{
    public interface IGenericRepositry<T>where T:BaseEntity 
    {
        Task<T?> GetAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();

        Task<T?> GetAsyncWithSpec(ISpecifications<T>spec);
        Task<IReadOnlyList <T>> GetAllAsyncWithSpec(ISpecifications<T> spec);
    }
}

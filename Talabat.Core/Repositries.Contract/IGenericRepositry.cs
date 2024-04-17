using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Repositries.Contract
{
    public interface IGenericRepositry<T>where T:BaseEntity 
    {
        Task<T?> GetAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
    }
}

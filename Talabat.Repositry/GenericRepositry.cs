using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositries.Contract;
using Talabat.Core.Specifications;
using Talabat.Repositry.Data;

namespace Talabat.Repositry
{
    public class GenericRepositry<T> : IGenericRepositry<T> where T : BaseEntity
    {
        private readonly StoreContext _dbContext;

        public GenericRepositry(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<IReadOnlyList <T>> GetAllAsync()
        {
            //if (typeof(T) == typeof(Product))
            //    return (IEnumerable < T >) await _dbContext.Set<Product>().Include(p => p.Brand).Include(p => p.Category).ToListAsync();
            return await _dbContext.Set<T>().ToListAsync();

        }


        public async Task<T?> GetAsync(int id)
        {
            //if (typeof(T) == typeof(Product))

            //    return await _dbContext.Set<Product>().Where(p => p.Id == id).Include(p => p.Brand).Include(p => p.Category).FirstOrDefaultAsync() as T;
            return await _dbContext.Set<T>().FindAsync(id);
        }
       
       
   

        public async Task<T?> GetAsyncWithSpec(ISpecifications<T> spec)
        {
            return await ApplaySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList <T>> GetAllAsyncWithSpec(ISpecifications<T> spec)
        {
            return await ApplaySpecification(spec).ToListAsync();
        }
        private IQueryable<T> ApplaySpecification(ISpecifications<T> Spec)
        {
            return SpecificationEvaluatotr<T>.GetQuery(_dbContext.Set<T>(), Spec);

        }

		public async Task<int> GetCountAsync(ISpecifications<T> spec)
		{
			return await ApplaySpecification (spec).CountAsync ();
		}
	}
}

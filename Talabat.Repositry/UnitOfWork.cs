using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositries.Contract;
using Talabat.Repositry.Data;

namespace Talabat.Repositry
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly StoreContext _dbcontext;
		private Hashtable _repositries;

		//public IGenericRepositry<Product> ProductRepo {get;set; }
		//public IGenericRepositry<ProductBrand> BrandRepo {get;set; }
		//public IGenericRepositry<ProductCategory> CategoryRepo {get;set; }
		//public IGenericRepositry<DeliveryMethod> DeliveryMethodRepo {get;set; }
		//public IGenericRepositry<OrderItem> OrderItemsRepo {get;set; }
		//public IGenericRepositry<Order> OrdersRepo {get;set; }

        public UnitOfWork(StoreContext dbcontext )
        {
			_dbcontext = dbcontext;
			_repositries = new Hashtable ();


		}
        public async Task<int> CompleteAsync()
		=>await _dbcontext.SaveChangesAsync();

		public async ValueTask DisposeAsync()
		=> await _dbcontext.DisposeAsync();

		public IGenericRepositry<TEntity> Repositry<TEntity>() where TEntity : BaseEntity
		{
			var key = typeof(TEntity).Name;
			if(!_repositries.ContainsKey(key)) {
				var repositry = new GenericRepositry<TEntity>(_dbcontext);
				_repositries.Add(key, repositry);
			}
			return _repositries[key] as IGenericRepositry<TEntity>;

		}
	}
}

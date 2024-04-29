using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repositry
{
  internal static class SpecificationEvaluatotr<TEntity>where TEntity : BaseEntity 
    {
        public static IQueryable <TEntity >GetQuery(IQueryable<TEntity> Inputquery,ISpecifications<TEntity>spec)
        {
            var query = Inputquery;

            if (spec.Criteria != null)
                query = query.Where(spec.Criteria);
			if (spec.OrderBy != null)
				query = query.OrderBy(spec.OrderBy);
			else if (spec.OrderByDesc != null)
				query = query.OrderByDescending(spec.OrderByDesc);
            if (spec.IsPaginationEnable )
                query = query.Skip(spec.Skip).Take(spec.Take);
            query = spec.Includes.Aggregate(query, (CurrentQuery, IncludeExperssion) =>
                CurrentQuery.Include(IncludeExperssion));


            query = spec.Includes.Aggregate(query, (CurrentQuery, IncludeExperssion) =>
                CurrentQuery.Include(IncludeExperssion));
            
            return query;
        }

    }
}

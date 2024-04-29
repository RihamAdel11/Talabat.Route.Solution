using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Specifications.ProdectSpec
{
	public class ProductSpecParams
	{
		private const int MaxCountSize = 10;
        private int PageSize=5;
		private string search;

		public string? Search
		{
			get { return search; }
			set { search = value?.ToLower(); }
		}

		public int pagesize
		{
			get { return PageSize; }
			set { PageSize = value> MaxCountSize ? MaxCountSize : value; }
		}
		public int PageIndex { get; set; } = 1;
        public string? Sort { get; set; }
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }
    }
}

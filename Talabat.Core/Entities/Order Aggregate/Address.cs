using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregate
{
	public class Address
	{
        public Address()
        {
            
        }

		public Address(string firstName, string lasttName, string city, string street, string country)
		{
			FirstName = firstName;
			LasttName = lasttName;
			City = city;
			Street = street;
			Country = country;
		}

		public required  string FirstName { get; set; }
		public string LasttName { get; set; }
		public string City{ get; set; }
		public string Street { get; set; }
		public string Country { get; set; }
	}
}

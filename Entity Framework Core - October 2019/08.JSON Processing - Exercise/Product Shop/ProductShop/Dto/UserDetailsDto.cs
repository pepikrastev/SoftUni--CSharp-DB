using System;
using System.Collections.Generic;
using System.Text;

namespace ProductShop.Dto
{
        // for problem 8
    public class UserDetailsDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int? Age { get; set; }

        public SoldProductsDto SoldProducts { get; set; }
    }
}

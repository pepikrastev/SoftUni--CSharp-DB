using System;
using System.Collections.Generic;
using System.Text;

namespace ProductShop.Dto
{
    // for problem 8
    public class SoldProductsDto
    {
        public int Count { get; set; }

        public ProductDetailsDto[] Products { get; set; }
    }
}

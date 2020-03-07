﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace BookShop.DataProcessor.ImportDto
{
    [XmlType("Book")]
    public class ImportBookXmlDto
    {
        [MinLength(3), MaxLength(30), Required]
        public string Name { get; set; }

        [Required]
        public string Genre { get; set; }

        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        public decimal Price { get; set; }

        [Range(typeof(int), "50", "5000")]
        public int Pages { get; set; }

        [Required]
        public string PublishedOn { get; set; }
    
    }
}

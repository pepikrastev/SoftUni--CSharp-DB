using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookShop.DataProcessor.ImportDto
{
     public class ImportAuthorsJsonDto
    {
        [MinLength(3), MaxLength(30), Required]
        public string FirstName { get; set; }

        [MinLength(3), MaxLength(30), Required]
        public string LastName { get; set; }

        [RegularExpression(@"^[0-9]{3}-[0-9]{3}-[0-9]{4}$")]
        public string Phone { get; set; }

        [EmailAddress, Required]
        public string Email { get; set; }

        public ImportBookJsonDto[] Books { get; set; }
    }

    public class ImportBookJsonDto
    {
        [Required]
        public int? Id { get; set; }
    }
}

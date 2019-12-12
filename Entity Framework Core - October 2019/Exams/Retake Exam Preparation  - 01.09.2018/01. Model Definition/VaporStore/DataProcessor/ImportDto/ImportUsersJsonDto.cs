using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VaporStore.DataProcessor.ImportDto
{
    public class ImportUsersJsonDto
    {
        [MinLength(3), MaxLength(20), Required]
        public string Username { get; set; }

        [RegularExpression("[A-Z][a-z]+ [A-Z][a-z]+"), Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        [Range(3, 103), Required]
        public int Age { get; set; }

        public ImportCardsJsonDto[] Cards { get; set; } 
    }
}

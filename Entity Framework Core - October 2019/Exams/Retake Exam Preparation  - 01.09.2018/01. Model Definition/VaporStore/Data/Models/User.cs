using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VaporStore.Data.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [MinLength(3), MaxLength(20), Required]
        public string Username { get; set; }

        [RegularExpression("[A-Z][a-z]+ [A-Z][a-z]+"), Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        [Range(3, 103), Required]
        public int Age { get; set; }

        public ICollection<Card> Cards { get; set; } = new HashSet<Card>();
    }
}

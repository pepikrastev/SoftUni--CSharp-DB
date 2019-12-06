namespace Cinema.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Customer
    {
        public Customer()
        {
            this.Tickets = new HashSet<Ticket>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string FirstName  { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string LastName  { get; set; }

        [Required]
        [Range(12, 110)]
        public int Age { get; set; }

        [Required]
        [Range(typeof(decimal), "0.01", "10000000000000000" /*"decimal.MaxValue"*/)]
        //[Range(0.01, double.MaxValue)]
        public decimal Balance  { get; set; }

        public ICollection<Ticket> Tickets { get; set; }
    }
}

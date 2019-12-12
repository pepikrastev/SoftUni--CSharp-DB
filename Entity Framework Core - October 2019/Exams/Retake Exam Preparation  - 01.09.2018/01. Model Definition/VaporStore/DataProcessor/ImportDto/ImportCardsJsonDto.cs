using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VaporStore.DataProcessor.ImportDto
{
    public class ImportCardsJsonDto
    {
        [RegularExpression("[0-9]{4} [0-9]{4} [0-9]{4} [0-9]{4}"), Required]
        public string Number { get; set; }

        [RegularExpression("[0-9]{3}"), Required]
        public string Cvc { get; set; }

        [Required]
        public string Type { get; set; }
    }
}

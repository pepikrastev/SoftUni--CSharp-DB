using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema.DataProcessor.ExportDto
{
    public class ExportMovieDto
    {
        //[JsonProperty("MovieName")]
        public string MovieName { get; set; }

        //[JsonProperty("Rating")]
        public string Rating { get; set; }

        //[JsonProperty("TotalIncomes")]
        public string TotalIncomes { get; set; }

        //[JsonProperty("Customers")]
        public ExportMovieCustomersDto[] Customers { get; set; }
    }
}

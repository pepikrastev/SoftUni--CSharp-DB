namespace Cinema.DataProcessor.ExportDto
{
    using System;
    using System.Collections.Generic;

    public class MovieExportDto
    {
        public string MovieName { get; set; }

        public string Rating { get; set; }

        public string TotalIncomes { get; set; }

        public ICollection<CustomersMovieExportDto> Customers { get; set; }
    }
}

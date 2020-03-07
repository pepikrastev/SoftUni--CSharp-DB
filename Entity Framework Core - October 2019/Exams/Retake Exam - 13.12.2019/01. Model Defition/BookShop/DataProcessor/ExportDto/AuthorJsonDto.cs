using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.DataProcessor.ExportDto
{
    public class AuthorJsonDto
    {
        public string AuthorName { get; set; }

        public BookJsonDto[] Books { get; set; }
    }

    public class BookJsonDto
    {
        public string BookName { get; set; }
        public string BookPrice { get; set; }
    }
}

namespace BookShop.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using BookShop.DataProcessor.ExportDto;
    using Data;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportMostCraziestAuthors(BookShopContext context)
        {
            var author = context.Authors
                .Select(a => new AuthorJsonDto
                {
                    AuthorName = a.FirstName + " " + a.LastName,
                    Books = a.AuthorsBooks
                            .Select(ab => new BookJsonDto
                            {
                                BookName = ab.Book.Name,
                                BookPrice = $"{ab.Book.Price:f2}"
                            })
                            .OrderByDescending(b => b.BookPrice)
                            .ToArray()

                })
                .ToArray()
                .OrderByDescending(a => a.Books.Length)
                .ThenBy(a => a.AuthorName)
                .ToArray();

            var json = JsonConvert.SerializeObject(author, Formatting.Indented);

            return json;
        }

        public static string ExportOldestBooks(BookShopContext context, DateTime date)
        {
            var books = context.Books
                .Where(b => b.PublishedOn < date && b.Genre.ToString() == "Science")
                .OrderByDescending(b => b.PublishedOn)
                .OrderByDescending(b => b.Pages)
               .Select(b => new BookXmlDto
               {
                   Pages = b.Pages.ToString(),
                   Name = b.Name,
                   Date = b.PublishedOn.ToString("d", CultureInfo.InvariantCulture)
               })
               .ToArray()
               .Take(10)
               .ToArray();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(BookXmlDto[]), new XmlRootAttribute("Books"));

            var sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            xmlSerializer.Serialize(new StringWriter(sb), books, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}
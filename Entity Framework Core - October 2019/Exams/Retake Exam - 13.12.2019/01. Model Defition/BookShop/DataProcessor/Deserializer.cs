namespace BookShop.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using BookShop.Data.Models;
    using BookShop.Data.Models.Enums;
    using BookShop.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedBook
            = "Successfully imported book {0} for {1:F2}.";

        private const string SuccessfullyImportedAuthor
            = "Successfully imported author - {0} with {1} books.";

        public static string ImportBooks(BookShopContext context, string xmlString)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportBookXmlDto[]), new XmlRootAttribute("Books"));

            var bookDtos = (ImportBookXmlDto[])xmlSerializer.Deserialize(new StringReader(xmlString));

            HashSet<Book> books = new HashSet<Book>();

            StringBuilder sb = new StringBuilder();

            foreach (var dto in bookDtos)
            {
                bool isValidGenre = false; 
                //= Enum.TryParse<Genre>(bookDto.Genre, out Genre genre);
                //=Enum.IsDefined(typeof(Genre), bookDto.Genre);

                if (dto.Genre == "1" || dto.Genre == "2" || dto.Genre == "3" ||
                    dto.Genre == "Biography" || dto.Genre == "Business" || dto.Genre == "Science")
                {
                    isValidGenre = true;
                }
;               // var ganer = Enum.TryParse(dto.Genre, out Genre genre);
                //  var validGenre = context.Books.FirstOrDefault(b => b.Genre == genre);
                
                if (!IsValid(dto) || !isValidGenre)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var book = new Book
                {
                    Name = dto.Name,
                    Price = dto.Price,
                    Pages = dto.Pages,
                    Genre = Enum.Parse<Genre>(dto.Genre),
                    PublishedOn = DateTime.ParseExact(dto.PublishedOn, "MM/dd/yyyy", CultureInfo.InvariantCulture)
                };

                books.Add(book);
                sb.AppendLine(string.Format(SuccessfullyImportedBook, book.Name, book.Price));
            }

            context.Books.AddRange(books);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportAuthors(BookShopContext context, string jsonString)
        {
            var authorDtos = JsonConvert.DeserializeObject<ImportAuthorsJsonDto[]>(jsonString);

            HashSet<Author> authors = new HashSet<Author>();
            List<string> emails = new List<string>();
            var sb = new StringBuilder();

            foreach (var authorDto in authorDtos)
            {
                //var emailExist = context.Authors.Any(a => a.Email == dto.Email);

                if (!IsValid(authorDto) || emails.Contains(authorDto.Email))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                emails.Add(authorDto.Email);

                var author = new Author
                {
                    FirstName = authorDto.FirstName,
                    LastName = authorDto.LastName,
                    Phone = authorDto.Phone,
                    Email = authorDto.Email
                };

                //var booksDto = dto.Books.Distinct();
                //if(booksDto.Count() < 1)
                //{
                //    sb.AppendLine(ErrorMessage);
                //    continue;
                //}

                foreach (var bookDto in authorDto.Books)
                {
                    var book = context.Books.FirstOrDefault(x => x.Id == bookDto.Id);

                    if (book == null)
                    {
                        continue;
                    }

                    var authorBook = new AuthorBook
                    {
                        Book = book,
                        Author = author
                    };

                    author.AuthorsBooks.Add(authorBook);
                }

                if (author.AuthorsBooks.Count <= 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                authors.Add(author);
                sb.AppendLine(string.Format(SuccessfullyImportedAuthor, author.FirstName + " " + author.LastName, author.AuthorsBooks.Count));
            }

            context.Authors.AddRange(authors);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
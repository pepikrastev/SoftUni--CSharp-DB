namespace BookShop
{
    using Data;
    using System;
    using Initializer;
    using System.Text;
    using System.Linq;
    using System.Collections.Generic;

    using BookShop.Models;
    using BookShop.Models.Enums;
    using System.Globalization;

    public class StartUp
    {
        public static void Main()
        {
            using (var db = new BookShopContext())
            {
                // DbInitializer.ResetDatabase(db);
                // string input = Console.ReadLine();
                // int input = int.Parse(Console.ReadLine());

                int result = RemoveBooks(db);
                Console.WriteLine(result);
            }
        }

        //StringBuilder sb = new StringBuilder();
        //return sb.ToString().TrimEnd();

        // 1. Age Restriction
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var commandText = command.ToLower();

            StringBuilder sb = new StringBuilder();

            var books = context
                .Books
                .Where(b => b.AgeRestriction.ToString().ToLower() == command.ToLower())
                .Select(b => new { b.Title })
                .OrderBy(b => b.Title)
                .ToList();

            foreach (var book in books)
            {
                sb.AppendLine(book.Title);
            }

            return sb.ToString().TrimEnd();
        }

        // 2. Golden Books
        public static string GetGoldenBooks(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var books = context.Books
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000
)
                .OrderBy(b => b.BookId)
                .Select(b => new
                {
                    b.Title
                });

            foreach (var b in books)
            {
                sb.AppendLine(b.Title);
            }

            return sb.ToString().TrimEnd();
        }

        //3. Books by Price
        public static string GetBooksByPrice(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var books = context.Books
                .Where(b => b.Price > 40)
                .Select(b => new
                {
                    b.Title,
                    b.Price
                })
                .OrderByDescending(b => b.Price);

            foreach (var b in books)
            {
                sb.AppendLine($"{b.Title} - ${b.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        //4. Not Released In
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            StringBuilder sb = new StringBuilder();

            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title);

            foreach (var b in books)
            {
                sb.AppendLine(b);
            }

            return sb.ToString().TrimEnd();
        }

        // 5. Book Titles by Category
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            List<Book> books = new List<Book>();

            string[] categories = input.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(c => c.ToLower())
                    .ToArray();

            foreach (var c in categories)
            {
                var booksToCategory = context
                    .Books
                    .Where(b => b.BookCategories
                         .Select(bc => new { bc.Category.Name })
                         .Any(ca => ca.Name.ToLower() == c))
                    .ToList();

                books.AddRange(booksToCategory);
            }

            var orderedBooks = books
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToList();

            return String.Join(Environment.NewLine, orderedBooks);
        }

        //Second 5. Book Titles by Category
        public static string SecondGetBooksByCategory(BookShopContext context, string input)
        {
            string[] categories = input.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(c => c.ToLower())
                    .ToArray();

            var books = context
                 .Books
                 .Where(b => b.BookCategories
                         .Any(bc => categories.Contains(bc.Category.Name.ToLower())))
                 .Select(b => b.Title)
                 .OrderBy(b => b)
                 .ToList();
            
            return String.Join(Environment.NewLine, books);
        }

        //6. Released Before Date
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            StringBuilder sb = new StringBuilder();

            var format = "dd-MM-yyyy";
            var provider = CultureInfo.InvariantCulture;
            var parsedDate = DateTime.ParseExact(date, format, provider);

            var books = context.Books
                .Where(b => b.ReleaseDate < parsedDate)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => new
                {
                    b.Title,
                    b.EditionType,
                    b.Price
                });

            foreach (var b in books)
            {
                sb.AppendLine($"{b.Title} - {b.EditionType} - ${b.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        //7. Author Search
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authours = context.Authors
                .Where(a => a.FirstName.ToLower().EndsWith(input.ToLower()))
                .OrderBy(a => a.FirstName)
                .ThenBy(a => a.LastName)
                .Select(a => a.FirstName + " " + a.LastName);

            var result = string.Join(Environment.NewLine, authours);
            return result;
        }

        //8. Book Search
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .OrderBy(b => b.Title)
                .Select(b => b.Title);

            var result = string.Join(Environment.NewLine, books);
            return result;
        }

        //9. Book Search by Author
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            var booksAndAuthors = context
                .Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(b => b.BookId)
                .Select(b => new
                {
                    b.Title,
                    Author = b.Author.FirstName + " " + b.Author.LastName,
                })
                .ToList();

            foreach (var ba in booksAndAuthors)
            {
                sb.AppendLine($"{ba.Title} ({ba.Author})");
            }

            return sb.ToString().TrimEnd();
        }

        //10. Count Books
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var books = context.Books
                .Where(b => b.Title.Length > lengthCheck);

            return books.Count();
        }

        //11. Total Book Copies
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var author = context
                .Authors
                .Select(a => new
                {
                    Name = a.FirstName + " " + a.LastName,
                    BookCopies = a.Books.Sum(b => b.Copies)
                })
                .OrderByDescending(a => a.BookCopies);

            foreach (var a in author)
            {
                sb.AppendLine($"{a.Name} - {a.BookCopies}");
            }

            return sb.ToString().TrimEnd();
        }

        //12. Profit by Category
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var categories = context.Categories
                .Select(c => new
                {
                    c.Name,
                    TotalProfit = c.CategoryBooks
                                        .Select(cb => cb.Book.Price * cb.Book.Copies).Sum()

                })
                .OrderByDescending(c => c.TotalProfit)
                .ThenBy(c => c.Name);

            StringBuilder sb = new StringBuilder();
            foreach (var b in categories)
            {
                sb.AppendLine($"{b.Name} ${b.TotalProfit:f2}");
            }
            return sb.ToString().TrimEnd();
        }

        //13. Most Recent Books
        public static string GetMostRecentBooks(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var categoriesAndMostRecentBooks = context
                .Categories
                .Select(c => new
                {
                    c.Name,
                    RecentBooks = c.CategoryBooks
                                    .OrderByDescending(cb => cb.Book.ReleaseDate)
                                    .Take(3)
                                    .Select(cb => new
                                    {
                                        BookTitle = cb.Book.Title,
                                        BookRelease = cb.Book.ReleaseDate.Value.Year
                                    })
                                    .ToList()
                })
                .OrderBy(c => c.Name)
                .ToList();

            foreach (var c in categoriesAndMostRecentBooks)
            {
                sb.AppendLine($"--{c.Name}");
                foreach (var b in c.RecentBooks)
                {
                    sb.AppendLine($"{b.BookTitle} ({b.BookRelease})");
                }
            }

            return sb.ToString().TrimEnd();
        }

        //14. Increase Prices
        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year < 2010);

            foreach (var book in books)
            {
                book.Price += 5;
            }
            context.SaveChanges();
        }

        //15. Remove Books
        public static int RemoveBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.Copies < 4200)
                .ToList();

            context.Books.RemoveRange(books);
            context.SaveChanges();

            return books.Count;
        }
    }
}

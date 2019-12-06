namespace Cinema.DataProcessor
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using AutoMapper;
    using Cinema.DataProcessor.ExportDto;
    using Data;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportTopMovies(CinemaContext context, int rating)
        {
            var movies = context.Movies
                .Where(m => m.Rating >= rating && m.Projections.Any(p => p.Tickets.Count >= 1))
                .OrderByDescending(m => m.Rating)
                .ThenByDescending(m => m.Projections.Sum(p => p.Tickets.Sum(t => t.Price)))
                .Select(m => new ExportMovieDto
                {
                    MovieName = m.Title,
                    Rating = m.Rating.ToString("F2"),
                    TotalIncomes = m.Projections.Sum(p => p.Tickets.Sum(t => t.Price)).ToString("F2"),
                    Customers = m.Projections
                    .SelectMany(p => p.Tickets)
                    .Select(c => new ExportMovieCustomersDto
                    {
                        FirstName = c.Customer.FirstName,
                        LastName = c.Customer.LastName,
                        Balance = c.Customer.Balance.ToString("F2")
                    })
                    .OrderByDescending(c => c.Balance)
                    .ThenBy(c => c.FirstName)
                    .ThenBy(c => c.LastName)
                    .ToArray()
                })
                .Take(10)
                .ToArray();

            var json = JsonConvert.SerializeObject(movies, Formatting.Indented);

            return json;

        }

        public static string ExportTopCustomers(CinemaContext context, int age)
        {
            //var customers = context.Customers
            //     .Where(c => c.Age >= age)
            //     .OrderByDescending(c => c.Tickets.Sum(t => t.Price))
            //     .Select(c => new ExportTopCustomersDto
            //     {
            //         FirstName = c.FirstName,
            //         LastName = c.LastName,
            //         SpentMoney = c.Tickets.Sum(t => t.Price).ToString("F2"),
            //         SpentTime = TimeSpan.FromMilliseconds(c.Tickets.Sum(t => t.Projection.Movie.Duration.TotalMilliseconds))
            //            .ToString(@"hh\:mm\:ss")
            //     })
            //    .Take(10)
            //    .ToArray();

            //XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportTopCustomersDto[]), new XmlRootAttribute("Customers"));

            //var sb = new StringBuilder();
            //var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            //xmlSerializer.Serialize(new StringWriter(sb), customers, namespaces);

            //return sb.ToString().TrimEnd();

            //or
            var customers = context
                .Customers
                .Where(c => c.Age >= age)
                .OrderByDescending(c => c.Tickets.Sum(t => t.Price))
                .Take(10)
                .ToArray();

            var customerDtos = Mapper.Map<ExportTopCustomersDto[]>(customers);

            var xmlSerializer = new XmlSerializer(typeof(ExportTopCustomersDto[]),
                           new XmlRootAttribute("Customers"));

            var stringBuilder = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            xmlSerializer.Serialize(new StringWriter(stringBuilder), customerDtos, namespaces);

            return stringBuilder.ToString().TrimEnd();
        }
    }
}
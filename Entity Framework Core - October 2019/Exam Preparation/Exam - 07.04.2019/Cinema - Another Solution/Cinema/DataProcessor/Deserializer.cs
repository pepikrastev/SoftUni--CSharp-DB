namespace Cinema.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using AutoMapper;
    using Cinema.Data.Models;
    using Cinema.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";
        private const string SuccessfulImportMovie 
            = "Successfully imported {0} with genre {1} and rating {2}!";
        private const string SuccessfulImportHallSeat 
            = "Successfully imported {0}({1}) with {2} seats!";
        private const string SuccessfulImportProjection 
            = "Successfully imported projection {0} on {1}!";
        private const string SuccessfulImportCustomerTicket 
            = "Successfully imported customer {0} {1} with bought tickets: {2}!";

        public static string ImportMovies(CinemaContext context, string jsonString)
        {
            var movieDtos = JsonConvert.DeserializeObject<ImportMovieDto[]>(jsonString);

            List<Movie> movies = new List<Movie>();
            StringBuilder sb = new StringBuilder();

            foreach (var movieDto in movieDtos)
            {
                if (!IsValid(movieDto) || movies.Any(m => m.Title == movieDto.Title))
                {
                    sb.AppendLine(ErrorMessage);
                }
                else
                {
                    var movie = Mapper.Map<Movie>(movieDto);
                    movies.Add(movie);

                    sb.AppendLine(string.Format(SuccessfulImportMovie, movie.Title, movie.Genre, $"{movie.Rating:f2}"));
                }
            }

            context.Movies.AddRange(movies);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportHallSeats(CinemaContext context, string jsonString)
        {
            var hallDtos = JsonConvert.DeserializeObject<ImportHallWithSeatDto[]>(jsonString);

            List<Hall> halls = new List<Hall>();
            var sb = new StringBuilder();
            string projectionType = "";

            foreach (var hallDto in hallDtos)
            {
                if (!IsValid(hallDto))
                {
                    sb.AppendLine(ErrorMessage);
                }
                else
                {
                    //var hall = Mapper.Map<Hall>(hallDto);
                    //or
                    var hall = new Hall
                    {
                        Name = hallDto.Name,
                        Is3D = hallDto.Is3D,
                        Is4Dx = hallDto.Is4Dx
                    };
                    //
                    for (int i = 0; i < hallDto.SeatsCount; i++)
                    {
                        hall.Seats.Add(new Seat());
                    }

                    halls.Add(hall);

                    if (hall.Is4Dx && hall.Is3D)
                    {
                        projectionType = "4Dx/3D";
                    }
                    else if (hall.Is3D)
                    {
                        projectionType = "3D";
                    }
                    else if (hall.Is4Dx)
                    {
                        projectionType = "4Dx";
                    }
                    else
                    {
                        projectionType = "Normal";
                    }

                    sb.AppendLine(string.Format(SuccessfulImportHallSeat, hall.Name, projectionType, hall.Seats.Count));
                }
            }

            context.Halls.AddRange(halls);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportProjections(CinemaContext context, string xmlString)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportProjectionDto[]), new XmlRootAttribute("Projections"));

            var projectionDtos = (ImportProjectionDto[])xmlSerializer.Deserialize(new StringReader(xmlString));

            List<Projection> projections = new List<Projection>();

            var sb = new StringBuilder();

            foreach (var projectionDto in projectionDtos)
            {
                var movie = context.Movies.FirstOrDefault(m => m.Id == projectionDto.MovieId);
                var hall = context.Halls.FirstOrDefault(h => h.Id == projectionDto.HallId);

                if (!IsValid(projectionDto) || movie == null || hall == null)
                {
                    sb.AppendLine(ErrorMessage);
                }
                else
                {
                    //var projection = Mapper.Map<Projection>(projectionDto);
                    //or
                    var projection = new Projection
                    {
                        MovieId = projectionDto.MovieId,
                        HallId = projectionDto.HallId,
                        DateTime = DateTime.Parse(projectionDto.DateTime)
                    };
                    //
                    projection.Movie = movie;
                    projection.Hall = hall;
                    projections.Add(projection);

                    sb.AppendLine(string.Format(SuccessfulImportProjection, 
                                                projection.Movie.Title, 
                                                projection.DateTime.ToString(@"MM/dd/yyyy")));
                }
            }

            context.Projections.AddRange(projections);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportCustomerTickets(CinemaContext context, string xmlString)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCustomerDto[]), new XmlRootAttribute("Customers"));

            var customerDtos = (ImportCustomerDto[])xmlSerializer.Deserialize(new StringReader(xmlString));

            List<Customer> customers = new List<Customer>();

            StringBuilder sb = new StringBuilder();

            foreach (var customerDto in customerDtos)
            {
                if (!IsValid(customerDto))
                {
                    sb.AppendLine(ErrorMessage);
                }
                else
                {
                    var customer = Mapper.Map<Customer>(customerDto);
                    customers.Add(customer);

                    sb.AppendLine(string.Format(SuccessfulImportCustomerTicket, customer.FirstName, customer.LastName, customer.Tickets.Count));
                }
            }
            context.Customers.AddRange(customers);
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
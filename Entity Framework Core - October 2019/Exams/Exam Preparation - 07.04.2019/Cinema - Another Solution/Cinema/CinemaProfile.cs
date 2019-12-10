using AutoMapper;
using Cinema.Data.Models;
using Cinema.Data.Models.Enums;
using Cinema.DataProcessor.ExportDto;
using Cinema.DataProcessor.ImportDto;
using System;
using System.Globalization;
using System.Linq;

namespace Cinema
{
    public class CinemaProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public CinemaProfile()
        {
            this.CreateMap<ImportMovieDto, Movie>()
                .ForMember(x => x.Genre, y => y.MapFrom(x => Enum.Parse(typeof(Genre), x.Genre)))
                .ForMember(x => x.Duration, y => y.MapFrom(x => /* TimeSpan.Parse(src.Duration) or */ TimeSpan.ParseExact(x.Duration, @"hh\:mm\:ss", CultureInfo.InvariantCulture)));

            this.CreateMap<ImportHallWithSeatDto, Hall>();

            this.CreateMap<ImportProjectionDto, Projection>()
                .ForMember(x => x.DateTime, y => y.MapFrom(x => DateTime.ParseExact(x.DateTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));

            this.CreateMap<ImportTicketDto, Ticket>();
            this.CreateMap<ImportCustomerDto, Customer>();

            this.CreateMap<Customer, ExportTopCustomersDto>()
                .ForMember(x => x.SpentMoney,
                y => y.MapFrom(x => x.Tickets.Sum(t => t.Price).ToString("f2")))
                .ForMember(x => x.SpentTime,
                y => y.MapFrom(x => TimeSpan.FromMilliseconds(
                    x.Tickets.Sum(t => t.Projection.Movie.Duration.TotalMilliseconds)).ToString(@"hh\:mm\:ss")));
        }
    }
}

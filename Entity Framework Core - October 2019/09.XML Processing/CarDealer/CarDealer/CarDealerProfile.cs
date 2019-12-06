using AutoMapper;
using CarDealer.Dtos.Export;
using CarDealer.Dtos.Import;
using CarDealer.Models;
using System.Linq;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            this.CreateMap<ImportSupplierDto, Supplier>();
            this.CreateMap<ImportPartDto, Part>();
            this.CreateMap<CustomerDto, Customer>();
            this.CreateMap<SaleDto, Sale>();

            this.CreateMap<Supplier, ExportLocalSuppliersDto>();
            this.CreateMap<Part, ExportCarPartDto>();
            this.CreateMap<Car, ExportCarDto>()
                .ForMember(x => x.Parts, y => y.MapFrom(x => x.PartCars.Select(pc => pc.Part)));

            this.CreateMap<Customer, CustomersTotalSalesDto>()
                .ForMember(x => x.FullName, y => y.MapFrom(x => x.Name))
                .ForMember(x => x.BoughtCars, y => y.MapFrom(x => x.Sales.Count))
                .ForMember(x => x.SpentMoney, y => y.MapFrom(x => x.Sales.Sum(s => s.Car.PartCars.Sum(pc => pc.Part.Price))));

            this.CreateMap<Sale, SalesWithDiscountDto>()
                .ForMember(x => x.CustomerName, y => y.MapFrom(x => x.Customer.Name))
                .ForMember(x => x.Price, y => y.MapFrom(x => x.Car.PartCars.Sum(p => p.Part.Price)))
                .ForMember(x => x.PriceWithDiscount, 
                            y => y.MapFrom(x => (x.Car.PartCars.Sum(p => p.Part.Price)) -
                            (x.Car.PartCars.Sum(p => p.Part.Price) * x.Discount / 100)));
           

            

        }
    }
}

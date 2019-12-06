using System;
using System.IO;
using System.Linq;  
using System.Xml.Serialization;
using System.Collections.Generic;

using CarDealer.Data;
using CarDealer.Models;
using CarDealer.Dtos.Import;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarDealer.Dtos.Export;
using System.Text;
using System.Xml;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            Mapper.Initialize(cfg => cfg.AddProfile<CarDealerProfile>());

            using (var db = new CarDealerContext())
            {
                //db.Database.EnsureDeleted();
                //db.Database.EnsureCreated();

                //var inputXml = File.ReadAllText("./../../../Datasets/suppliers.xml");
                //var inputXml = File.ReadAllText("./../../../Datasets/parts.xml");
                // var inputXml = File.ReadAllText("./../../../Datasets/cars.xml");
                //var inputXml = File.ReadAllText("./../../../Datasets/customers.xml");
               // var inputXml = File.ReadAllText("./../../../Datasets/sales.xml");

                //var result = ImportSales(db, inputXml);
                var result = GetSalesWithAppliedDiscount(db);

                Console.WriteLine(result);
            }
        }

        //Query 9. Import Suppliers
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportSupplierDto[]),
                new XmlRootAttribute("Suppliers"));

            ImportSupplierDto[] supplierDtos;

            using (var reader = new StringReader(inputXml))
            {
                supplierDtos = (ImportSupplierDto[])xmlSerializer.Deserialize(reader);
            }

            var suppliers = Mapper.Map<Supplier[]>(supplierDtos);

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Length}";
        }

        //Query 10. Import Parts
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(ImportPartDto[]), new XmlRootAttribute("Parts"));

            ImportPartDto[] partDtos;

            using (var reader = new StringReader(inputXml))
            {
                partDtos = ((ImportPartDto[])xmlSerializer
                      .Deserialize(reader))
                      .Where(p => context.Suppliers.Any(s => s.Id == p.SupplierId))
                      .ToArray();
            }

            var parts = Mapper.Map<Part[]>(partDtos);

            context.Parts.AddRange(parts);
            context.SaveChanges();  

            return $"Successfully imported {parts.Length}";
        }

        //Query 11. Import Cars
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCarDto[]), new XmlRootAttribute("Cars"));

            ImportCarDto[] carDtos;

            using (var reader = new StringReader(inputXml))
            {
                carDtos = (ImportCarDto[])xmlSerializer.Deserialize(reader);
            }

            List<Car> cars = new List<Car>();
            List<PartCar> partCars = new List<PartCar>();

            foreach (var carDto in carDtos)
            {
                var car = new Car()
                {
                    Make = carDto.Make,
                    Model = carDto.Model,
                    TravelledDistance = carDto.TravelledDistance
                };

                var parts = carDto
                    .Parts
                    .Where(pdto => context.Parts.Any(p => p.Id == pdto.Id))
                    .Select(p => p.Id)
                    .Distinct();

                foreach (var partId in parts)
                {
                    var partCar = new PartCar()
                    {
                        PartId = partId,
                        Car = car
                    };

                    partCars.Add(partCar);
                }

                cars.Add(car);
            }

            context.Cars.AddRange(cars);
            context.PartCars.AddRange(partCars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }

        //Query 12. Import Customers
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CustomerDto[]), new XmlRootAttribute("Customers"));

            var customerDtos = (CustomerDto[])xmlSerializer.Deserialize(new StringReader(inputXml));

            var customers = Mapper.Map<Customer[]>(customerDtos);

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Length}";
        }

        //Query 13. Import Sales
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SaleDto[]), new XmlRootAttribute("Sales"));

            SaleDto[] saleDtos;

            using (var reader = new StringReader(inputXml))
            {
                saleDtos = ((SaleDto[])xmlSerializer.Deserialize(reader))
                    .ToArray();
            }

            var carsId = context.Cars.Select(c => c.Id);

            var validSales = new List<SaleDto>();

            foreach (var sale in saleDtos)
            {
                if (carsId.Contains(sale.CarId))
                {
                    validSales.Add(sale);
                }
            }

            var sales = Mapper.Map<Sale[]>(validSales);

            context.Sales.AddRange(sales);
            context.SaveChanges();

              return $"Successfully imported {sales.Length}";
        }

        //Query 14. Cars With Distance
        public static string GetCarsWithDistance(CarDealerContext context)
        {
            StringBuilder sb = new StringBuilder();

            var cars = context.Cars
                .Where(c => c.TravelledDistance > 2000000)
                .Take(10)
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                //.ProjectTo<CarsWithDistanceDto>()
                .Select(c => new CarsWithDistanceDto
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                })
                .ToArray();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CarsWithDistanceDto[]), new XmlRootAttribute("cars"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            using (var writher = new StringWriter(sb))
            {
                xmlSerializer.Serialize(writher, cars, namespaces);
            }

            return sb.ToString().TrimEnd();
        }

        //Query 15. Cars from make BMW
        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            StringBuilder sb = new StringBuilder();

            var cars = context.Cars
                .Where(c => c.Make == "BMW")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .ProjectTo<CarsBMWDto>()
                .ToArray();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CarsBMWDto[]), new XmlRootAttribute("cars"));
            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            xmlSerializer.Serialize(new StringWriter(sb), cars, namespaces);

            return sb.ToString().TrimEnd();
        }

        //Query 16. Local Suppliers
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            StringBuilder sb = new StringBuilder();

            var suppliers = context.Suppliers
                .Where(s => !s.IsImporter)
                .ProjectTo<ExportLocalSuppliersDto>()
                .ToArray();

            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(ExportLocalSuppliersDto[]), new XmlRootAttribute("suppliers"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            using (var writer = new StringWriter(sb))
            {
                xmlSerializer.Serialize(writer, suppliers, namespaces);

            }

            return sb.ToString().TrimEnd();
        }

        //Query 17. Cars with Their List of Parts
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            StringBuilder sb = new StringBuilder();

            var cars = context.Cars
                .OrderByDescending(c => c.TravelledDistance)
                .ThenBy(c => c.Model)
                .Take(5)
                .ProjectTo<ExportCarDto>()
                .ToArray();

            foreach (var car in cars)
            {
                car.Parts = car.Parts
                    .OrderByDescending(c => c.Price)
                    .ToArray();
            }

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportCarDto[]), new XmlRootAttribute("cars"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            using (var writer = new StringWriter(sb))
            {
                xmlSerializer.Serialize(writer, cars, namespaces);
            }

            return sb.ToString().TrimEnd();
        }

        //Query 18. Total Sales by Customer
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .Where(c => c.Sales.Any())
                .ProjectTo<CustomersTotalSalesDto>()
                .OrderByDescending(ctsd => ctsd.SpentMoney)
                .ToArray();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CustomersTotalSalesDto[]),
               new XmlRootAttribute("customers"));

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            StringBuilder sb = new StringBuilder();

            xmlSerializer.Serialize(new StringWriter(sb), customers, namespaces);

            return sb.ToString().TrimEnd();
        }

        //Query 19. Sales with Applied Discount
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            //  first way
            //  var sales = context.Sales
            //    .ProjectTo<SalesWithDiscountDto>()
            //    .ToArray();

            //second way
            var sales = context.Sales
                .Select(s => new SalesWithDiscountDto
                {
                    Car = new CarsWithDistanceDto
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TravelledDistance = s.Car.TravelledDistance
                    },
                    Discount = s.Discount,
                    CustomerName = s.Customer.Name,
                    Price = s.Car.PartCars.Sum(p => p.Part.Price),
                    PriceWithDiscount = s.Car.PartCars.Sum(p => p.Part.Price) - 
                                (s.Car.PartCars.Sum(p => p.Part.Price)) * s.Discount / 100
                    
                })
                .ToArray();
            

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SalesWithDiscountDto[]), new XmlRootAttribute("sales"));

            StringBuilder sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            xmlSerializer.Serialize(new StringWriter(sb), sales, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}
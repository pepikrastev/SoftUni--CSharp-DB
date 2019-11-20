using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using AutoMapper;
using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var db = new CarDealerContext())
            {
                //db.Database.EnsureDeleted();
                //db.Database.EnsureCreated();

                //string inputJson = File.ReadAllText("./../../../Datasets/cars.json");
                // string inputJson = File.ReadAllText("./../../../Datasets/customers.json");
                //  string inputJson = File.ReadAllText("./../../../Datasets/sales.json");

                Mapper.Initialize(cfg => cfg.AddProfile<CarDealerProfile>());

                Console.WriteLine(GetSalesWithAppliedDiscount(db/*, inputJson*/));
            }
        }

        //Query 9. Import Suppliers
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var suppliers = JsonConvert.DeserializeObject<Supplier[]>(inputJson);

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Length}.";
        }

        //Query 10. Import Parts
        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var parts = JsonConvert.DeserializeObject<Part[]>(inputJson)
                .Where(p => context.Suppliers.Any(s => s.Id == p.SupplierId))
                .ToList();

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count}.";
        }

        //Query 11. Import Cars
        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var carsDto = JsonConvert.DeserializeObject<ImportCarDto[]>(inputJson)
               .ToList();

            var cars = new List<Car>();
            var carParts = new List<PartCar>();

            foreach (var carDto in carsDto)
            {
                
                var car = new Car()
                {
                    Make = carDto.Make,
                    Model = carDto.Model,
                    TravelledDistance = carDto.TravelledDistance
                };

                foreach (var part in carDto.PartsId.Distinct())
                {
                    var carPart = new PartCar()
                    {
                        PartId = part,
                        Car = car
                    };

                    carParts.Add(carPart);
                }

                cars.Add(car);
            }

            context.Cars.AddRange(cars);
            context.PartCars.AddRange(carParts);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}.";
        }

        //Query 12. Import Customers
        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            var customers = JsonConvert.DeserializeObject<Customer[]>(inputJson);

            context.Customers.AddRange(customers);

            var result = context.SaveChanges();

            return $"Successfully imported {result}.";
        }

        //Query 13. Import Sales
        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            var sales = JsonConvert.DeserializeObject<Sale[]>(inputJson);

            context.Sales.AddRange(sales);
            int result = context.SaveChanges();

            return $"Successfully imported {result}.";
        }

        //Query 14. Export Ordered Customers
        public static string GetOrderedCustomers(CarDealerContext context)
        {
            //wihtout DTO

            //var customers = context.Customers
            //    .OrderBy(c => c.BirthDate)
            //    .ThenBy(c => c.IsYoungDriver)
            //    .Select(c => new
            //    {
            //        c.Name,
            //        BirthDate = c.BirthDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
            //        c.IsYoungDriver
            //    });

            var customers = context
                .Customers
                .OrderBy(c => c.BirthDate)
                .ThenBy(c => c.IsYoungDriver)
                .ToList();

            var customerDtos = Mapper.Map<IEnumerable<Customer>, IEnumerable<CustomerDto>>(customers);

            var jsonCustomers = JsonConvert.SerializeObject(customerDtos, Formatting.Indented);

            return jsonCustomers;
        }

        //Query 15. Export Cars from make Toyota
        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            //wihtout DTO

            //var cars = context.Cars
            //      .Where(c => c.Make == "Toyota")
            //      .OrderBy(c => c.Model)
            //      .ThenByDescending(c => c.TravelledDistance)
            //      .Select(c => new
            //      {
            //          c.Id,
            //          c.Make,
            //          c.Model,
            //          c.TravelledDistance
            //      });

            var cars = context
                .Cars
                .Where(c => c.Make == "Toyota")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .ToList();

            var carDtos = Mapper.Map<IEnumerable<Car>, IEnumerable<CarExportDto>>(cars);

            var jsonCars = JsonConvert.SerializeObject(carDtos
, Formatting.Indented);

            return jsonCars;
        }

        //Query 16. Export Local Suppliers
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            //wihtout DTO

            //var suppliers = context.Suppliers
            //    .Where(s => !s.IsImporter )
            //    .Select(s => new
            //    {
            //        s.Id,
            //        s.Name,
            //       PartsCount = s.Parts.Count
            //    });

            var suppliers = context.Suppliers
                .Where(s => !s.IsImporter);

            var suppliersDtos = Mapper.Map<IEnumerable<Supplier>, IEnumerable<SupplierDto>>(suppliers);

            var jsonSuppliers = JsonConvert.SerializeObject(suppliersDtos, Formatting.Indented);

            return jsonSuppliers;
        }

        //Query 17. Export Cars with Their List of Parts
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
                .Select(c => new
                {
                    car = new
                    {
                        c.Make,
                        c.Model,
                        c.TravelledDistance
                    },
                    parts = c.PartCars
                    .Select(pc => new
                    {
                        pc.Part.Name,
                        Price = $"{pc.Part.Price:f2}"
                    })
                });

            var jsonCars = JsonConvert.SerializeObject(cars, Formatting.Indented);
            return jsonCars;
        }

        //Query 18. Export Total Sales by Customer
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            //wihtout DTO

            //var customers = context.Customers
            //    .Where(c => c.Sales.Count > 0)
            //    .Select(c => new
            //    {
            //        fullName = c.Name,
            //        boughtCars = c.Sales.Count,
            //        spentMoney = c.Sales.Sum(s => s.Car.PartCars.Sum(pc => pc.Part.Price))
            //    });

            var customers = context.Customers
                .Where(c => c.Sales.Count > 0)
                .Select(c => new CustomerWithSalesDto()
                {
                    FullName = c.Name,
                    BoughtCars = c.Sales.Count,
                    SpentMoney = c.Sales.Sum(s => s.Car.PartCars.Sum(pc => pc.Part.Price))
                });

            DefaultContractResolver contractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var jsonCustomer = JsonConvert.SerializeObject(customers, new JsonSerializerSettings()
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            });

            return jsonCustomer;
        }

        //Query 19. Export Sales with Applied Discount
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Select(s => new
                {
                    car = new
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        s.Car.TravelledDistance
                    },
                    customerName = s.Customer.Name,
                    Discount = $"{s.Discount:f2}",
                    price = $"{s.Car.PartCars.Sum(pc => pc.Part.Price):f2}",
                    priceWithDiscount = $"{s.Car.PartCars.Sum(pc => pc.Part.Price) - s.Car.PartCars.Sum(pc => pc.Part.Price) * (s.Discount / 100m):f2}"
                })
                .Take(10);

            var jsonSales = JsonConvert.SerializeObject(sales, Formatting.Indented);
            return jsonSales;
        }
    }
}
using System;
using System.Collections.Generic;
//using System.Globalization;
using System.IO;
using System.Linq;
using AutoMapper;
//using System.Threading;
using AutoMapper.QueryableExtensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.Dto;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            // Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            Mapper.Initialize(cfg => cfg.AddProfile<ProductShopProfile>());

            using (var db = new ProductShopContext())
            {
                //db.Database.EnsureDeleted();
                //db.Database.EnsureCreated();

                // var inputJson = File.ReadAllText("./../../../Datasets/users.json");
                // var inputJson = File.ReadAllText("./../../../Datasets/products.json");
                // var inputJson = File.ReadAllText("./../../../Datasets/categories.json");
                // var inputJson = File.ReadAllText("./../../../Datasets/categories-products.json");

                // var result = ImportProducts(db, inputJson);
                var result = GetCategoriesByProductsCount(db);

                Console.WriteLine(result);
            }
        }

        //Query 1. Import Users
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
           var users = JsonConvert.DeserializeObject<User[]>(inputJson);

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Length}";
        }

        //Query 2. Import Products
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var products = JsonConvert.DeserializeObject<Product[]>(inputJson);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Length}";
        }

        //Query 3. Import Categories
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var categories = JsonConvert.DeserializeObject<Category[]>(inputJson);

            var validCategoties = categories
                .Where(c => c.Name != null)
                .ToArray();

            context.Categories.AddRange(validCategoties);
            context.SaveChanges();

            return $"Successfully imported {validCategoties.Length}";
        }

        //Query 4. Import Categories and Products
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var categoriyProducts = JsonConvert.DeserializeObject<CategoryProduct[]>(inputJson);

            context.CategoryProducts.AddRange(categoriyProducts);
            context.SaveChanges();

            return $"Successfully imported {categoriyProducts.Length}";
        }

        //Query 5. Export Products In Range
        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Select(p => new
                {
                   name = p.Name,
                   price = p.Price,
                   seller = p.Seller.FirstName + " " + p.Seller.LastName
                })
                .ToList();

            var json = JsonConvert.SerializeObject(products, Formatting.Indented);

            return json;
        }

        //Query 6. Export Successfully Sold Products
        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(ps => ps.Buyer != null))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Select(u => new
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    SoldProducts = u.ProductsSold
                             .Where(ps => ps.Buyer != null)
                             .Select(p => new
                             {
                                 Name = p.Name,
                                 Price = p.Price,
                                 BuyerFirstName = p.Buyer.FirstName,
                                 BuyerLastName = p.Buyer.LastName
                             })
                              .ToArray()
                })
                .ToArray();

            DefaultContractResolver contractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var jsonResult = JsonConvert.SerializeObject(
                users, 
                new JsonSerializerSettings
                {
                    ContractResolver  = contractResolver,
                    Formatting = Formatting.Indented
                });

            return jsonResult;
        }

        //Query 7. Export Categories By Products Count
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .OrderByDescending(c => c.CategoryProducts.Count())
                .Select(c => new
                {
                    category = c.Name,
                    productsCount = c.CategoryProducts.Count(),
                    averagePrice = $"{c.CategoryProducts.Average(cp => cp.Product.Price):f2}",
                    totalRevenue = $"{c.CategoryProducts.Sum(cp => cp.Product.Price):f2}"
                });

            var json = JsonConvert.SerializeObject(categories, Formatting.Indented);
            return json;
        }

        //Query 8. Export Users and Products
        public static string GetUsersWithProductss(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
               // .OrderByDescending(u => u.ProductsSold.Count)
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    age = u.Age,
                    soldProducts = new
                    {
                        count = u.ProductsSold
                                        .Where(p => p.Buyer != null)
                                        .Count(),
                        products = u.ProductsSold
                                    .Where(p => p.Buyer != null)
                                    .Select(ps => new
                                    {
                                        name = ps.Name,
                                        price = ps.Price
                                    })
                                    .ToList()
                    }
                })
                .OrderByDescending(u => u.soldProducts.count)
                .ToList();

            var userOutput = new
            {
                usersCount = users.Count,
                users = users
            };

            // this - new JsonSerializerSettings - is for remove the null values in usersOutput.
            var json = JsonConvert.SerializeObject(userOutput, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            });

            return json;
        }

        //Query 8. Export Users and Products with Auto Mapper(DTO)
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(ps => ps.Buyer != null))
                .ProjectTo<UserDetailsDto>()
                .OrderByDescending(u => u.SoldProducts.Count)
                .ToArray();

            var userOutput = Mapper.Map<UserInfoDto>(users);

            var defaultResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            // this - new JsonSerializerSettings - is for remove the null values in usersOutput.
            var json = JsonConvert.SerializeObject(userOutput, new JsonSerializerSettings()
            {
                ContractResolver = defaultResolver,
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            });

            return json;
        }
    }
}
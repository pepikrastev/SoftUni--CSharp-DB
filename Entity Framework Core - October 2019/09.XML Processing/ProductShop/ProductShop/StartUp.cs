using AutoMapper;
using AutoMapper.QueryableExtensions;
using ProductShop.Data;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;


namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
             //string inputXml = File.ReadAllText("./../../../Datasets/users.xml");
            //string inputXml = File.ReadAllText("./../../../Datasets/products.xml");
            // string inputXml = File.ReadAllText("./../../../Datasets/categories.xml");
            string inputXml = File.ReadAllText("./../../../Datasets/categories-products.xml");

            Mapper.Initialize(cfg => cfg.AddProfile<ProductShopProfile>());

            using (var context = new ProductShopContext())
            {
                //context.Database.EnsureDeleted();
                //context.Database.EnsureCreated();

                // var result = ImportCategoryProducts(context, inputXml);
                var result = GetUsersWithProducts(context);

                Console.WriteLine(result);
            }
        }

        //Query 1. Import Users
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(ImportUserDto[]),
                new XmlRootAttribute("Users"));

            var userDtos = (ImportUserDto[])(xmlSerializer.Deserialize(new StringReader(inputXml)));
            var users = Mapper.Map<IEnumerable<ImportUserDto>, IEnumerable<User>>(userDtos);

            context.Users.AddRange(users);
            int count = context.SaveChanges();

            return $"Successfully imported {count}";
        }

        //Query 2. Import Products
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportProductDto[]), new XmlRootAttribute("Products"));

            var productDtos = (ImportProductDto[])xmlSerializer.Deserialize(new StringReader(inputXml));
            var products = Mapper.Map<IEnumerable<ImportProductDto>, IEnumerable<Product>>(productDtos);

            context.Products.AddRange(products);
            int count = context.SaveChanges();
            return $"Successfully imported {count}";
        }

        //Query 3. Import Categories
        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCategoryDto[]), new XmlRootAttribute("Categories"));

            var categoryDtos = (ImportCategoryDto[])xmlSerializer.Deserialize(new StringReader(inputXml));
            
            var categories = Mapper.Map<IEnumerable<ImportCategoryDto>, IEnumerable<Category>>(categoryDtos);

            context.Categories.AddRange(categories);
            int count = context.SaveChanges();
            return $"Successfully imported {count}";
        }

        //Query 4. Import Categories and Products
         public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCategoryProductsDto[]), new XmlRootAttribute("CategoryProducts"));

            var categoryProductDtos = (ImportCategoryProductsDto[])xmlSerializer.Deserialize(new StringReader(inputXml));

            var categoryProducts = Mapper.Map<IEnumerable<ImportCategoryProductsDto>, IEnumerable<CategoryProduct>>(categoryProductDtos);

            var categories = context.Categories.Select(c => c.Id);
            var products = context.Products.Select(p => p.Id);

            var validCategoryProducts = categoryProducts
                .Where(cp => categories.Contains(cp.CategoryId)
                 && products.Contains(cp.ProductId));


            context.CategoryProducts.AddRange(validCategoryProducts);
            int count = context.SaveChanges();
            return $"Successfully imported {count}";
        }

        //Query 5. Products In Range
        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(p => p.Price > 500 && p.Price < 1000)
                .OrderBy(p => p.Price)
                .Take(10)
                //
                .Select(p => new ProductInRangeDto
                {
                    Name = p.Name,
                    Price = p.Price,
                    Buyer = p.Buyer.FirstName + " " + p.Buyer.LastName
                })
                //.ProjectTo<ProductInRangeDto>() - todo - implement in ProductShopProfile
                .ToArray();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ProductInRangeDto[]), new XmlRootAttribute("Products"));

            StringBuilder sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            xmlSerializer.Serialize(new StringWriter(sb), products, namespaces);

            return sb.ToString().TrimEnd();
        }

        //Query 6. Sold Products
        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Count > 0)
                .Select(u => new GetSoldProductsDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    ProductsSold = u.ProductsSold.Select(p => new SoldProductDto
                    {
                        Name = p.Name,
                        Price = p.Price
                    })
                    .ToArray()
                })
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Take(5)
                .ToArray();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(GetSoldProductsDto[]), new XmlRootAttribute("Users"));

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            xmlSerializer.Serialize(new StringWriter(sb), users, namespaces);

            return sb.ToString().TrimEnd();
        }

        //Query 7. Categories By Products Count
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .Select(c => new CategoriesWithProductsDto
                {
                    Name = c.Name,
                    Count = c.CategoryProducts.Count(),
                    AveragePrice = c.CategoryProducts.Average(cp => cp.Product.Price),
                    TotalRevenue = c.CategoryProducts.Sum(cp => cp.Product.Price)
                })
                .OrderByDescending(c => c.Count)
                .ThenBy(c => c.TotalRevenue)
                .ToArray();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CategoriesWithProductsDto[]), new XmlRootAttribute("Categories"));

            var sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            xmlSerializer.Serialize(new StringWriter(sb), categories, namespaces);

            return sb.ToString().TrimEnd();
        }

        //Query 8. Users and Products
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var userDtos = context
                    .Users
                    .Where(u => u.ProductsSold.Any())
                    .OrderByDescending(u => u.ProductsSold.Count)
                    .Select(u => new UsersWithProductsDto
                    {
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Age = u.Age,
                        ProductsSold = new SoldProductsWithCountDto
                        {
                            Count = u.ProductsSold.Count,
                            Products = u.ProductsSold
                            .OrderByDescending(ps => ps.Price)
                            .Select(ps => new SoldProductDto
                            {
                                Name = ps.Name,
                                Price = ps.Price
                            })
                            .ToArray()
                        }
                    })
                    .Take(10)
                    .ToArray();

            var result = new GetCountAndUsersWithProductsDto
            {
                Count = context.Users.Count(u => u.ProductsSold.Any()),
                Users = userDtos
            };

            var xmlSerializer = new XmlSerializer(typeof(GetCountAndUsersWithProductsDto),
                new XmlRootAttribute("Users"));

            var stringBuilder = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            xmlSerializer.Serialize(new StringWriter(stringBuilder), result, namespaces);

            return stringBuilder.ToString().TrimEnd();
        }
    }
}
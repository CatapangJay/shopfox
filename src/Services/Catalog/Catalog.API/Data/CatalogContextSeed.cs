using Bogus;
using Catalog.API.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Data
{
    public static class CatalogContextSeed
    {
        public static void SeedData(IMongoCollection<Product> productCollection)
        {
            bool productExists = productCollection.Find(p => true).Any();

            if (!productExists)
            {
                productCollection.InsertManyAsync(GetProducts());
            }
        }

        private static IEnumerable<Product> GetProducts()
        {
            var productFaker = new Faker<Product>()
                .RuleFor(o => o.Id, f => Guid.NewGuid().ToString())
                .RuleFor(o => o.Name, f => f.Commerce.Product())
                .RuleFor(o => o.Category, f => f.Commerce.Categories(new Random().Next())[new Random().Next()])
                .RuleFor(o => o.Summary, f => f.Commerce.ProductAdjective())
                .RuleFor(o => o.Description, f => f.Commerce.ProductDescription())
                .RuleFor(o => o.Description, f => f.Image.LoremPixelUrl("business"))
                .RuleFor(o => o.Price, f => Decimal.Parse(f.Commerce.Price()));

            return productFaker.Generate(20);
        }
    }
}

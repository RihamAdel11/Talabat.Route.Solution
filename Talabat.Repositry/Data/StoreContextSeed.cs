using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Repositry.Data
{
    public class StoreContextSeed
    {
        public async static Task SeedAsync(StoreContext _dbContext)
        {
            if (_dbContext.ProductBrands.Count() == 0)
            {
                var brandData = File.ReadAllText("../Talabat.Repositry/Data/DataSeed/brands (1).json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);
                if (brands?.Count() > 0)
                {
                    foreach (var brand in brands)
                    {
                        _dbContext.Set<ProductBrand>().Add(brand);

                    }
                    await _dbContext.SaveChangesAsync();

                }
            }
            if (_dbContext.ProductCategory.Count() == 0)
            {
                var CategoryData = File.ReadAllText("../Talabat.Repositry/Data/DataSeed/categories (1).json");
                var Categories = JsonSerializer.Deserialize<List<ProductCategory>>(CategoryData);
                if (Categories?.Count() > 0)
                {
                    foreach (var category in Categories)
                    {
                        _dbContext.Set<ProductCategory>().Add(category);

                    }
                    await _dbContext.SaveChangesAsync();

                }
            }

            if (_dbContext.Products.Count() == 0)
            {
                var ProductData = File.ReadAllText("../Talabat.Repositry/Data/DataSeed/products.json");
                var Products = JsonSerializer.Deserialize<List<Product>>(ProductData);
                if (Products?.Count() > 0)
                {
                    foreach (var product in Products)
                    {
                        _dbContext.Set<Product>().Add(product);

                    }
                    await _dbContext.SaveChangesAsync();

                }
            }

        }
    }
}

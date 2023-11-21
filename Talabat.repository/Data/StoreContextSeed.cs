using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.core.Entities;
using Talabat.core.Entities.OrderAggrigation;

namespace Talabat.repository.Data
{
    public class StoreContextSeed
    {
        //Seeding
        public static async Task SeedAsync(StoreContext dbContext)
        {
            if(!dbContext.ProductBrands.Any())
            {

            var BrandsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
            var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandsData);
            if(Brands?.Count > 0) {
                foreach (var Brand in Brands)
                {
                    await dbContext.Set<ProductBrand>().AddAsync(Brand);
                }
                await dbContext.SaveChangesAsync();
            }

            }
            //Seeding Types
            if (!dbContext.ProductTypes.Any())
            {
                var TypesData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/types.json");
                var Types = JsonSerializer.Deserialize<List<ProductType>>(TypesData);
                if (Types?.Count > 0)
                {
                    foreach (var type in Types)
                    {
                        await dbContext.Set<ProductType>().AddAsync(type);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }
            //Seeding Products
            if (!dbContext.Products.Any())
            {
                var ProductsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
                var Products = JsonSerializer.Deserialize<List<Products>>(ProductsData);
                if (Products?.Count > 0)
                {
                    foreach (var product in Products)
                    {
                        await dbContext.Set<Products>().AddAsync(product);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }

            //Seeding DeliveryMethod
            if (!dbContext.DeliveryMethods.Any())
            {
                var DeliveryMethodsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/delivery.json");
                var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodsData);
                if (DeliveryMethods?.Count > 0)
                {
                    foreach (var DeliveryMethod in DeliveryMethods)
                    {
                        await dbContext.Set<DeliveryMethod>().AddAsync(DeliveryMethod);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}

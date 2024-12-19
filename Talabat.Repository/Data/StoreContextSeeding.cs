using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Orders;

namespace Talabat.Repository.Data
{
    public class StoreContextSeeding
    {

        public async static Task SeedAsync( StoreContext _dbContext)
        {

            if (_dbContext.ProductBrands.Count()==0)
            {
                var brandsData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                if (brands?.Count() > 0)
                {

                    //brands = brands.Select(B => new ProductBrand() // ex to identity prpimary key
                    //{
                    //    Name = B.Name,
                    //}).ToList();

                    foreach (var brand in brands)
                    {

                        _dbContext.Set<ProductBrand>().Add(brand);

                    }

                    await _dbContext.SaveChangesAsync();
                } 
            }


            if (_dbContext.ProductCategories.Count() == 0)
            {
                var categoryData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/categories.json");
                var categories = JsonSerializer.Deserialize<List<ProductCategory>>(categoryData);

                if (categories?.Count() > 0)
                {

                    foreach (var category in categories)
                    {

                        _dbContext.Set<ProductCategory>().Add(category);

                    }

                    await _dbContext.SaveChangesAsync();
                }
            }


            if (_dbContext.Products.Count() == 0)
            {
                var ProductsData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/products.json");
                var Products = JsonSerializer.Deserialize<List<Product>>(ProductsData);

                if (Products?.Count() > 0)
                {

                    foreach (var Product in Products)
                    {

                        _dbContext.Set<Product>().Add(Product);

                    }

                    await _dbContext.SaveChangesAsync();
                }
            }


            if (_dbContext.DeliveryMethod.Count() == 0)
            {
                var DeliveryMethodData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/delivery.json");
                var DeliveryMethod = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodData);

                if (DeliveryMethod?.Count() > 0)
                {

                    foreach (var deliveryMethod in DeliveryMethod)
                    {

                        _dbContext.Set<DeliveryMethod>().Add(deliveryMethod);

                    }

                    await _dbContext.SaveChangesAsync();
                }
            }

        }

    }
}

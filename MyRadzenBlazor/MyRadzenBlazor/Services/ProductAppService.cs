using MyRadzenBlazor.Entities;

namespace MyRadzenBlazor.Services
{
    public class ProductAppService
    {
        public List<Product> Products { get; private set; }

        public ProductAppService()
        {
            // Mock data
            Products = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", Category = "Electronics", Price = 999.99M },
                new Product { Id = 2, Name = "Smartphone", Category = "Electronics", Price = 699.99M },
                new Product { Id = 3, Name = "Tablet", Category = "Electronics", Price = 399.99M },
                new Product { Id = 4, Name = "Headphones", Category = "Accessories", Price = 199.99M },
                new Product { Id = 5, Name = "Monitor", Category = "Electronics", Price = 299.99M },
                new Product { Id = 6, Name = "Mouse", Category = "Accessories", Price = 49.99M },
            };
        }

        public List<Product> GetProducts()
        {
            return Products.OrderByDescending(a=> a.Id).ToList();
        }
    }
}

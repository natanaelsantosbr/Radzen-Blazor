using Microsoft.AspNetCore.Components;
using MyRadzenBlazor.Client.Models;
using MyRadzenBlazor.Client.Pages.Products;
using Radzen;

namespace MyRadzenBlazor.Client.Services
{
    public class ProductService
    {
        private readonly NotificationService _notificationService;
        private readonly DialogService _dialogService;
        private readonly Action _stateHasChanged;

        public List<ProductRequest> Products { get; private set; }
        public List<ProductRequest> FilteredProducts { get; private set; }

        public ProductService(NotificationService notificationService, DialogService dialogService, Action stateHasChanged)
        {
            _notificationService = notificationService;
            _dialogService = dialogService;

            // Mock data
            Products = new List<ProductRequest>
        {
            new ProductRequest { Id = 1, Name = "Laptop", Category = "Electronics", Price = 999.99M },
            new ProductRequest { Id = 2, Name = "Smartphone", Category = "Electronics", Price = 699.99M },
            new ProductRequest { Id = 3, Name = "Tablet", Category = "Electronics", Price = 399.99M },
            new ProductRequest { Id = 4, Name = "Headphones", Category = "Accessories", Price = 199.99M },
            new ProductRequest { Id = 5, Name = "Monitor", Category = "Electronics", Price = 299.99M },
            new ProductRequest { Id = 6, Name = "Mouse", Category = "Accessories", Price = 49.99M },
        };

            FilteredProducts = Products;
            _stateHasChanged = stateHasChanged;
        }

        public void Search(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                FilteredProducts = Products;
            }
            else
            {
                FilteredProducts = Products.Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                                       p.Category.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            _stateHasChanged();
        }

        public void EditProduct(ProductRequest product)
        {
            var selectedProduct = new ProductRequest
            {
                Id = product.Id,
                Name = product.Name,
                Category = product.Category,
                Price = product.Price
            };

            _dialogService.Open<EditProductDialog>("Edit Product", new Dictionary<string, object>
        {
            { "Product", selectedProduct },
            { "OnSave", EventCallback.Factory.Create<ProductRequest>(this, SaveProduct) }
        }, new DialogOptions() { Width = "500px", Height = "400px" });
        }

        public void CreateProduct()
        {
            var selectedProduct = new ProductRequest();

            _dialogService.Open<EditProductDialog>("Create Product", new Dictionary<string, object>
        {
            { "Product", selectedProduct },
            { "OnSave", EventCallback.Factory.Create<ProductRequest>(this, AddProduct) }
        }, new DialogOptions() { Width = "500px", Height = "400px" });
        }

        public void SaveProduct(ProductRequest updatedProduct)
        {
            var product = Products.FirstOrDefault(p => p.Id == updatedProduct.Id);
            if (product != null)
            {
                product.Name = updatedProduct.Name;
                product.Category = updatedProduct.Category;
                product.Price = updatedProduct.Price;
            }
            _notificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "Product Saved", Detail = $"Product {updatedProduct.Name} has been updated." });
            UpdateFilteredProducts();
        }

        public void AddProduct(ProductRequest newProduct)
        {
            newProduct.Id = Products.Any() ? Products.Max(p => p.Id) + 1 : 1;
            Products.Add(newProduct);
            UpdateFilteredProducts();
            _notificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "Product Created", Detail = $"Product {newProduct.Name} has been created." });
        }

        public async Task ConfirmDelete(ProductRequest product)
        {
            string message = "Are you sure you want to delete the product " + product.Name + "?";
            string title = "Delete Product";
            var confirmed = await _dialogService.Confirm(message, title);
            if (confirmed == true)
            {
                DeleteProduct(product);
            }
        }

        public void DeleteProduct(ProductRequest product)
        {
            Products.Remove(product);
            UpdateFilteredProducts();
            _notificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Warning, Summary = "Product Deleted", Detail = $"Product {product.Name} has been deleted." });
        }

        private void UpdateFilteredProducts()
        {
            FilteredProducts = new List<ProductRequest>(Products);
            _stateHasChanged();
        }
    }
}

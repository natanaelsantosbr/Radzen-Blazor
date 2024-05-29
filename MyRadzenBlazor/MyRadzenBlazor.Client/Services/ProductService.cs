using Microsoft.AspNetCore.Components;
using MyRadzenBlazor.Client.Models;
using MyRadzenBlazor.Client.Pages.Products;
using Radzen;
using System.Net.Http.Json;

namespace MyRadzenBlazor.Client.Services
{
    public class ProductService
    {
        private readonly NotificationService _notificationService;
        private readonly DialogService _dialogService;
        private readonly HttpClient _httpClient;

        public List<ProductRequest> Products { get; private set; }
        public List<ProductRequest> FilteredProducts { get; private set; }

        public event Action ProductsChanged;

        public ProductService(NotificationService notificationService, DialogService dialogService, HttpClient httpClient)
        {
            _notificationService = notificationService;
            _dialogService = dialogService;
            _httpClient = httpClient;
        }

        public async Task<List<ProductRequest>> LoadProductsAsync()
        {
            var response = await this._httpClient.GetFromJsonAsync<List<ProductRequest>>("api/products");
            Products = response;
            FilteredProducts = response;
            NotifyProductsChanged();

            return response;
        }

        public List<ProductRequest> Search(string searchTerm)
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
            NotifyProductsChanged();

            return FilteredProducts;
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
            FilteredProducts = new List<ProductRequest>(Products.OrderByDescending(a=> a.Id).ToList());
            NotifyProductsChanged();
        }

        private void NotifyProductsChanged() => ProductsChanged?.Invoke();
    }
}

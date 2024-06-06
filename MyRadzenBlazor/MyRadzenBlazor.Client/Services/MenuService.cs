using System.Globalization;
using System.Text;

namespace MyRadzenBlazor.Client.Services
{
    public class MenuService
    {
        private List<MenuItem> menuItems;

        public MenuService()
        {
            menuItems = new List<MenuItem>
            {
                new MenuItem { Text = "Home", Icon = "home", Path = "dashboard", IsNew = true },
                new MenuItem { Text = "Clientes", Icon = "groups", Path = "clients", IsUpdated = true },
                new MenuItem { Text = "Products", Icon = "category", Path = "product-search" },
                new MenuItem { Text = "TemplateForm", Icon = "contact_mail", Path = "TemplateForm" },
                new MenuItem { Text = "Login", Icon = "login", Path = "login" },
                new MenuItem
                {
                    Text = "Layout", Icon = "dns", Children = new List<MenuItem>
                    {
                        new MenuItem { Text = "Stack", Icon = "list", Path = "stack" },
                        new MenuItem { Text = "Row", Icon = "line_weight", Path = "row" },
                        new MenuItem { Text = "Column", Icon = "line_style", Path = "column" }
                    }
                },               
                new MenuItem { Text = "Counter", Icon = "alarm", Path = "counter" },
                new MenuItem { Text = "Weather", Icon = "build", Path = "weather" }
            };
        }

        public IEnumerable<MenuItem> GetFilteredMenuItems(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return menuItems;
            }

            searchText = RemoveDiacritics(searchText);
            foreach (var item in menuItems)
            {
                item.Expanded = IsMatch(item, searchText);
                if (item.Children != null)
                {
                    foreach (var child in item.Children)
                    {
                        if (IsMatch(child, searchText))
                        {
                            item.Expanded = true;
                            break;
                        }
                    }
                }
            }

            return menuItems.Where(item => IsMatch(item, searchText) || item.Expanded);
        }

        public bool IsMatch(MenuItem item, string searchText)
        {
            return RemoveDiacritics(item.Text).Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                   (item.Children != null && item.Children.Any(child => RemoveDiacritics(child.Text).Contains(searchText, StringComparison.OrdinalIgnoreCase)));
        }

        private string RemoveDiacritics(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            var normalizedText = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }

    public class MenuItem
    {
        public string Text { get; set; }
        public string Icon { get; set; }
        public string Path { get; set; }
        public bool IsNew { get; set; }
        public bool IsUpdated { get; set; }
        public List<MenuItem> Children { get; set; }
        public bool Expanded { get; set; } // New property to control expansion
    }
}

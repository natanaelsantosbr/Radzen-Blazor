using static MyRadzenBlazor.Client.Layout.NavMenu;
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
            new MenuItem { Text = "Home", Icon = "home", Path = "", IsNew = true },
            new MenuItem { Text = "Dashboard", Icon = "dashboard", Path = "dashboard", IsUpdated = true },
            new MenuItem { Text = "Counter", Icon = "alarm", Path = "counter" },
            new MenuItem { Text = "Weather", Icon = "build", Path = "weather" },
            new MenuItem
            {
                Text = "Layout", Icon = "dns", Children = new List<MenuItem>
                {
                    new MenuItem { Text = "Stack", Icon = "list", Path = "stack" },
                    new MenuItem { Text = "Row", Icon = "line_weight", Path = "row" },
                    new MenuItem { Text = "Column", Icon = "line_style", Path = "column" }
                }
            },
            new MenuItem
            {
                Text = "Navigation", Icon = "navigation", Children = new List<MenuItem>
                {
                    new MenuItem { Text = "Login", Icon = "login", Path = "login" }
                }
            },
            new MenuItem
            {
                Text = "Forms", Icon = "form", Children = new List<MenuItem>
                {
                    new MenuItem { Text = "Clientes", Icon = "groups", Path = "clients" },
                    new MenuItem { Text = "Products", Icon = "category", Path = "product-search" },
                    new MenuItem { Text = "TemplateForm", Icon = "template", Path = "TemplateForm" }
                }
            },
            new MenuItem { Text = "Usuários", Icon = "person", Path = "users" },
            new MenuItem { Text = "Relatórios", Icon = "bar_chart", Path = "reports" },
            new MenuItem { Text = "Configurações", Icon = "settings", Path = "settings" },
            new MenuItem { Text = "Notificações", Icon = "notifications", Path = "notifications" },
            new MenuItem { Text = "Pedidos", Icon = "shopping_cart", Path = "orders" },
            new MenuItem { Text = "Inventário", Icon = "inventory", Path = "inventory" },
            new MenuItem { Text = "Financeiro", Icon = "account_balance", Path = "finance" },
            new MenuItem { Text = "Suporte", Icon = "support", Path = "support" },
            new MenuItem { Text = "Perfil do Usuário", Icon = "account_circle", Path = "profile" }
        };
        }

        public IEnumerable<MenuItem> GetFilteredMenuItems(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return menuItems;
            }

            searchText = RemoveDiacritics(searchText);
            return menuItems.Where(item => IsMatch(item, searchText) ||
                                            (item.Children != null && item.Children.Any(child => IsMatch(child, searchText))));
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
    }
}
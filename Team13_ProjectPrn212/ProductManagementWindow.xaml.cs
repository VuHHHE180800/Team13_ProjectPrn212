using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Team13_ProjectPrn212
{
    public partial class ProductManagementWindow : Window
    {
        private Models.Team13QlbhContext _dbContext;

        public ProductManagementWindow()
        {
            InitializeComponent();
            _dbContext = new Models.Team13QlbhContext();
            LoadCategories();
            LoadProducts();
        }

        private void LoadProducts()
        {
            try
            {
                var products = from p in _dbContext.Products
                               join c in _dbContext.Categories on p.CategoryId equals c.CategoryId
                               join b in _dbContext.Brands on p.BrandId equals b.BrandId
                               select new
                               {
                                   p.ProductId,
                                   p.ProductName,
                                   CategoryName = c.CategoryName,
                                   BrandName = b.BrandName,
                                   p.Price,
                                   p.Quantity
                               };

                ProductGrid.ItemsSource = products.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading products: {ex.Message}");
            }
        }

        private void LoadCategories()
        {
            try
            {
                var categories = _dbContext.Categories.Select(c => c.CategoryName).ToList();
                CategoryFilter.Items.Add("All Categories");
                foreach (var category in categories)
                {
                    CategoryFilter.Items.Add(category);
                }
                CategoryFilter.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading categories: {ex.Message}");
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string searchQuery = SearchBox.Text.Trim();
                string selectedCategory = CategoryFilter.SelectedItem?.ToString();

                var filteredProducts = from p in _dbContext.Products
                                       join c in _dbContext.Categories on p.CategoryId equals c.CategoryId
                                       join b in _dbContext.Brands on p.BrandId equals b.BrandId
                                       where (string.IsNullOrEmpty(searchQuery) ||
                                              p.ProductName.Contains(searchQuery) ||
                                              p.ProductId.ToString().Contains(searchQuery)) &&
                                             (selectedCategory == "All Categories" ||
                                              c.CategoryName == selectedCategory)
                                       select new
                                       {
                                           p.ProductId,
                                           p.ProductName,
                                           CategoryName = c.CategoryName,
                                           BrandName = b.BrandName,
                                           p.Price,
                                           p.Quantity
                                       };

                ProductGrid.ItemsSource = filteredProducts.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching products: {ex.Message}");
            }
        }

        private void CategoryFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchButton_Click(sender, e);
        }

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            PlaceholderText.Visibility = Visibility.Collapsed;
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SearchBox.Text))
            {
                PlaceholderText.Visibility = Visibility.Visible;
            }
        }
    }
}

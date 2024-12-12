using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Team13_ProjectPrn212.Models;

namespace Team13_ProjectPrn212
{
    /// <summary>
    /// Interaction logic for ProductManagementWindow.xaml
    /// </summary>
    /// private Models.Team13QlbhContext _dbContext;
    public partial class ProductManagementWindow : Window
    {
        private Models.Team13QlbhContext _dbContext;
        public ProductManagementWindow()
        {
            InitializeComponent();
            _dbContext = new Models.Team13QlbhContext();
            LoadCategories();
            LoadProducts();
            Category_Load();
            Brand_Load();
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

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            
            var product= new Product();
            product.ProductName = DetailProductName.Text;
            product.BrandId = int.Parse(DetailBrand.SelectedValue.ToString());
            product.CategoryId = int.Parse(DetailCategory.SelectedValue.ToString());
            product.Price = int.Parse(DetailPrice.Text);
            product.Quantity = int.Parse(DetailQuantity.Text);
            product.Description = DetailDescription.Text;

            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();
            LoadProducts();
            MessageBox.Show("Product added successfully.");
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            
                if (ProductGrid.SelectedItem == null)
                {
                    MessageBox.Show("Please select a product to edit.");
                    return;
                }

                dynamic selectedProduct = ProductGrid.SelectedItem;
                int productId = selectedProduct.ProductId;

                var productToEdit = _dbContext.Products.FirstOrDefault(p => p.ProductId == productId);
                if (productToEdit != null)
                {
                    productToEdit.ProductName = DetailProductName.Text;
                    productToEdit.BrandId = int.Parse(DetailBrand.SelectedValue.ToString());
                    productToEdit.CategoryId = int.Parse(DetailCategory.SelectedValue.ToString());
                    productToEdit.Price = int.Parse(DetailPrice.Text);
                    productToEdit.Quantity = int.Parse(DetailQuantity.Text);
                    productToEdit.Description = DetailDescription.Text;
                    _dbContext.SaveChanges();
                    LoadProducts();
                    MessageBox.Show("Product edited successfully.");
                }
            
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ProductGrid.SelectedItem == null)
                {
                    MessageBox.Show("Please select a product to delete.");
                    return;
                }

                dynamic selectedProduct = ProductGrid.SelectedItem;
                int productId = selectedProduct.ProductId;

                var productToDelete = _dbContext.Products.FirstOrDefault(p => p.ProductId == productId);
                if (productToDelete != null)
                {
                    var result = MessageBox.Show("Are you sure you want to delete this product?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        _dbContext.Products.Remove(productToDelete);
                        _dbContext.SaveChanges();
                        LoadProducts();
                        MessageBox.Show("Product deleted successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting product: {ex.Message}");
            }
        }
        private void Category_Load()
        {
            var categories = _dbContext.Categories.ToList();
            DetailCategory.ItemsSource = categories;
            DetailCategory.DisplayMemberPath = "CategoryName";
            DetailCategory.SelectedValuePath = "CategoryId";
        }
        private void Brand_Load()
        {
            var brands = _dbContext.Brands.ToList();
            DetailBrand.ItemsSource = brands;
            DetailBrand.DisplayMemberPath = "BrandName";
            DetailBrand.SelectedValuePath = "BrandId";
        }

        private void ProductGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic productGrid = ProductGrid.SelectedItem;
            int productId = productGrid.ProductId;
            var product = _dbContext.Products.FirstOrDefault(p => p.ProductId == productId);
            if (product != null)
            {
                DetailCategory.SelectedItem = _dbContext.Categories.AsEnumerable().FirstOrDefault(c => c.CategoryId == product.CategoryId);
                DetailBrand.SelectedItem = _dbContext.Brands.AsEnumerable().FirstOrDefault(b => b.BrandId == product.BrandId);
                DetailProductId.Text = product.ProductId.ToString();
                DetailProductName.Text = product.ProductName;
                DetailPrice.Text = product.Price.ToString();
                DetailQuantity.Text = product.Quantity.ToString();
                DetailDescription.Text = product.Description;
                //DetailPhoto.Source = new BitmapImage(new Uri(product.Photo));
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để chỉnh sửa.");
            }
        }

        // Helper method to clear details when no product is selected
        private void ClearDetails()
        {
            DetailCategory.SelectedItem = null;
            DetailBrand.SelectedItem = null;
            DetailProductId.Clear();
            DetailProductName.Clear();
            DetailPrice.Clear();
            DetailQuantity.Clear();
            DetailDescription.Clear();
            DetailPhoto.Source = null;
        }

    }
}

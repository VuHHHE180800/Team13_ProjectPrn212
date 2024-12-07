using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for CreatOrderWindow.xaml
    /// </summary>
    public partial class CreatOrderWindow : Window
    {
        private ObservableCollection<CartItem> cart = new ObservableCollection<CartItem>();
        private Team13QlbhContext db = new Team13QlbhContext();
        public CreatOrderWindow()
        {
            InitializeComponent();
            loadBrand();
            loadCategory();
            loadForm();
        }

        private void ProductDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            ProductDataGrid.ItemsSource = db.Products.Include(p=>p.Brand).Include(p=>p.Category).Where(p=>p.Quantity>0).ToList();
        }

        private void btnShowAll_Click(object sender, RoutedEventArgs e)
        {
            filterProduct();
        }

        private void btnTaoMoi_Click(object sender, RoutedEventArgs e)
        {
            clearForm();

        }
        private void clearForm()
        {
            txtTenKhachHang.Text = string.Empty;
            txtDiaChi.Text = string.Empty;
            txtSoDienThoai.Text = string.Empty;
            txtThanhTien.Text = string.Empty;
            txtMaSanPham.Text = string.Empty;
            txtTenSanPham.Text = string.Empty;
            txtDonGia.Text = string.Empty;
            txtSoLuongMua.Text = "1";
            cart.Clear();
        }
        private void btnTinhTien_Click(object sender, RoutedEventArgs e)
        {
var selectedProduct = ProductDataGrid.SelectedItem as Product;

    if (selectedProduct != null)
    {
         // Kiểm tra số lượng nhập từ TextBox
        int soluong;
                try
                {
                    soluong = int.Parse(txtSoLuongMua.Text);
                }
                catch (FormatException)
                {
                    MessageBox.Show("Vui lòng chỉ nhập số.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
        if (soluong<=selectedProduct.Quantity )
        {
            // Kiểm tra xem sản phẩm đã có trong giỏ hàng chưa
            var existingItem = cart.FirstOrDefault(item => item.ProductId == selectedProduct.ProductId);

            if (existingItem != null)
            {
                // Nếu có, cập nhật số lượng
                existingItem.Quantity += soluong;
            }
            else
            {
                // Nếu chưa, thêm sản phẩm mới vào giỏ
                cart.Add(new CartItem
                {
                    ProductId = selectedProduct.ProductId,
                    ProductName = selectedProduct.ProductName,
                    Quantity = soluong,
                    Price = selectedProduct.Price
                });
                txtThanhTien.Text = cart.Sum(item =>  item.Price).ToString(); // Câp nhật TextBox txtThanhTien
            }

            // Làm mới DataGrid hiển thị giỏ hàng
            CartDataGrid.Items.Refresh();
        }
        else
        {
            MessageBox.Show("Số lượng không hợp lệ!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
    else
    {
        MessageBox.Show("Vui lòng chọn sản phẩm để thêm vào giỏ hàng.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
    }
        }
        private void CartDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            CartDataGrid.ItemsSource = cart;
        }

        private void ProductDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var product = ProductDataGrid.SelectedItem as Product;
            if(product != null)
            {
                txtMaSanPham.Text = product.ProductId.ToString();
                txtTenSanPham.Text = product.ProductName;
                txtDonGia.Text = product.Price.ToString();
                txtSoLuongConLai.Text = "Số lượng còn lại: "+product.Quantity.ToString();
            }
        }
        private void loadBrand()
        {
            var brands = db.Brands.ToList();
            cbbNSXTimKiem.ItemsSource = brands;
            cbbNSXTimKiem.DisplayMemberPath = "BrandName";
            cbbNSXTimKiem.SelectedValuePath = "BrandId";
        }
        private void loadCategory()
        {
            var categories = db.Categories.ToList();
            cbbLoaiTimKiem.ItemsSource = categories;
            cbbLoaiTimKiem.DisplayMemberPath = "CategoryName";
            cbbLoaiTimKiem.SelectedValuePath = "CategoryId"; 
        }
        private void filterProduct()
        {
            List<Product>plist= db.Products.ToList();
            string productName = txtTenSPTimKiem.Text;
            if(productName.Length > 0 )
            {
                plist = plist.Where(p => p.ProductName.ToLower().Contains(productName)).ToList();
            }

            int brandId = -1;
            int categoryId = -1;

            if(cbbLoaiTimKiem.SelectedIndex > -1)
            {
                categoryId = (int)cbbLoaiTimKiem.SelectedValue;
                plist = plist.Where(p => p.CategoryId == categoryId).ToList();
            }
            if(cbbNSXTimKiem.SelectedIndex > -1)
            {
                brandId = (int)cbbNSXTimKiem.SelectedValue;
                plist = plist.Where(p => p.BrandId == brandId).ToList();
            }
            ProductDataGrid.ItemsSource = plist;
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            txtTenSPTimKiem.Text = string.Empty;
            cbbLoaiTimKiem.SelectedIndex = -1;
            cbbNSXTimKiem.SelectedIndex = -1;
            ProductDataGrid.ItemsSource = db.Products.Include(p => p.Brand).Include(p => p.Category).Where(p => p.Quantity > 0).ToList();
        }
        private void loadForm()
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            dpNgayLap.Text = today.ToString();
            Employee employee = Application.Current.Properties["employee"] as Employee;
            txtNVLapHoaDon.Text = employee.EmployeeName;
        }
        private bool checkOrder()
        {
            bool kt = true;
            if (txtTenKhachHang.Text == "" || txtDiaChi.Text == "" || txtSoDienThoai.Text == "")
            {
                MessageBox.Show("Thông tin khách hàng chưa đủ, vui lòng kiểm tra lại !", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                kt = false;
            }
            if (!cart.Any())
            {
                MessageBox.Show("Bạn chưa có sản phẩm nào trong giỏ hàng!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                kt = false;
            }
            string phone= txtSoDienThoai.Text;
            //if(Regex.IsMatch(phone, @"^\d{10,11}$"))
            //{
            //    MessageBox.Show("Số điện thoại khách hàng đang không hợp lệ ! Vui lòng điền số điện thoại gồm 10 đến 11 chữ số!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //    kt = false;
            //}
            return kt;
        }
        private void btnLapHoaDon_Click(object sender, RoutedEventArgs e)
        {
            if (checkOrder())
            {
                try
                {
                    Employee employee = Application.Current.Properties["employee"] as Employee;

                    Order order = new Order
                    {
                        CustomerName = txtTenKhachHang.Text,
                        CustomerPhonenumber = txtSoDienThoai.Text,
                        CustomerAddress = txtDiaChi.Text,
                        OrderDate = DateOnly.FromDateTime(DateTime.Now),
                        StatusId = 1,
                        EmployeeId = employee.EmployeeId
                    };

                    db.Orders.Add(order);

                    foreach (var item in cart)
                    {
                        OrderDetail od = new OrderDetail
                        {
                            OrderId = order.OrderId,
                            ProductId = item.ProductId,
                            TotalQuantity = item.Quantity,
                            TotalPrice = item.TotalPrice
                        };
                        db.OrderDetails.Add(od);
                    }

                    db.SaveChanges();
                    MessageBox.Show("Lập hóa đơn thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    clearForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lập hóa đơn thất bại!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            LoginForm loginForm = new LoginForm();
            loginForm.ShowDialog();
        }
    }
}

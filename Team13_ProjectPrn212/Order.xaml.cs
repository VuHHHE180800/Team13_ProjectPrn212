using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using BusinessModel;
using DataAccessLayer;
using Team13_ProjectPrn212.Models;
using Team13_ProjectPrn212;

namespace QuanLyBanHang
{
    public partial class DonHang : Window
    {
        private readonly DataAccessLayer.Team13QlbhContext _context;
        private readonly OrderDAO _orderDAO;
        private readonly OrderDetailDAO _orderDetailDAO;
        private readonly EmployeeDAO _employeeDAO;
        private readonly OrderStatusDAO _orderStatusDAO;

        private int _currentPage = 1;
        private const int _pageSize = 10;

        public DonHang()
        {
            InitializeComponent();

            // Initialize your DbContext and DAOs here
            // Replace YourDbContext with your actual DbContext class
            _context = new DataAccessLayer.Team13QlbhContext();
            _orderDAO = new OrderDAO(_context);
            _orderDetailDAO = new OrderDetailDAO(_context);
            _employeeDAO = new EmployeeDAO(_context); // Assuming you have this DAO
            _orderStatusDAO = new OrderStatusDAO(_context);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadOrderList();
        }

        private void BtnTaiDanhSach_Click(object sender, RoutedEventArgs e)
        {
            LoadOrderList();
        }

        private void LoadOrderList()
        {
            try
            {
                // Get all orders with pagination
                var orders = _orderDAO.GetAllOrders()
                    .Skip((_currentPage - 1) * _pageSize)
                    .Take(_pageSize)
                    .Select(o => new OrderViewModel
                    {
                        MaHD = o.OrderId,
                        TenKhachHang = o.CustomerName,
                        TongTien = _orderDetailDAO.GetOrderDetailsByOrderId(o.OrderId)
                            .Sum(od => od.TotalPrice),
                        NgayLap = o.OrderDate.ToString("dd/MM/yyyy"),
                        TenNhanVien = _employeeDAO.GetEmployeeById(o.EmployeeId)?.EmployeeName ?? "Không xác định",
                        TrangThai = _orderStatusDAO.GetOrderStatusById(o.StatusId)?.StatusName ?? "Không xác định"
                    })
                    .ToList();

                // Bind to ListView
                listView1.ItemsSource = orders;
                listView2.ItemsSource = orders; // For delete tab
                listView3.ItemsSource = orders; // For edit status tab
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách đơn hàng: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnTrangTruoc_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                LoadOrderList();
            }
        }

        private void BtnTrangSau_Click(object sender, RoutedEventArgs e)
        {
            var totalOrders = _orderDAO.GetAllOrders().Count;
            if (_currentPage * _pageSize < totalOrders)
            {
                _currentPage++;
                LoadOrderList();
            }
        }

        private void BtnTaoDonHang_Click(object sender, RoutedEventArgs e)
        {
            CreatOrderWindow banHang = new CreatOrderWindow();
            banHang.Show();
        }

        private void BtnTaiDanhSachXoa_Click(object sender, RoutedEventArgs e)
        {
            LoadOrderList();
        }

        private void BtnXoaDH_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrder = listView2.SelectedItem as OrderViewModel;
            if (selectedOrder == null)
            {
                MessageBox.Show("Vui lòng chọn đơn hàng để xóa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa đơn hàng {selectedOrder.MaHD}?",
                "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _orderDAO.DeleteOrder(selectedOrder.MaHD);
                    MessageBox.Show("Xóa đơn hàng thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadOrderList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa đơn hàng: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnTaiDanhSachSua_Click(object sender, RoutedEventArgs e)
        {
            LoadOrderList();
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Highlight selected item for status update
            var listViewItem = sender as ListViewItem;
            if (listViewItem != null)
            {
                listViewItem.IsSelected = true;
            }
        }

        private void BtnCapNhat_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrder = listView3.SelectedItem as OrderViewModel;
            var selectedStatus = cbbTrangThaiDonHang.SelectedItem as ComboBoxItem;

            if (selectedOrder == null)
            {
                MessageBox.Show("Vui lòng chọn đơn hàng để cập nhật.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (selectedStatus == null)
            {
                MessageBox.Show("Vui lòng chọn trạng thái.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Find the corresponding status ID based on the selected status name
                var status = _orderStatusDAO.GetOrderStatusByName(selectedStatus.Content.ToString());
                if (status == null)
                {
                    MessageBox.Show("Không tìm thấy trạng thái.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Fetch the actual order and update its status
                var order = _orderDAO.GetOrderById(selectedOrder.MaHD);
                order.StatusId = status.StatusId;
                _orderDAO.UpdateOrder(order);

                MessageBox.Show("Cập nhật trạng thái thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadOrderList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật trạng thái: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // View Model for displaying order information
        public class OrderViewModel
        {
            public int MaHD { get; set; }
            public string TenKhachHang { get; set; }
            public decimal TongTien { get; set; }
            public string NgayLap { get; set; }
            public string TenNhanVien { get; set; }
            public string TrangThai { get; set; }
        }
    }
}
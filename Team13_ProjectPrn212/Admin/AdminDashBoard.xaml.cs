using BusinessObjects;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
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

namespace Team13_ProjectPrn212.Admin
{
    /// <summary>
    /// Interaction logic for AdminDashBoard.xaml
    /// </summary>
    public partial class AdminDashBoard : Window
    {
        private readonly Team13QlbhContext _context;
        public AdminDashBoard()
        {
            InitializeComponent();
            _context = new Team13QlbhContext();
            load();
        }
        public void load()
        {
            var employees = _context.Employees.Where(e => e.RoleId == 1).ToList();
            lvUsers.ItemsSource = employees;
        }

        
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Employee employee = (Employee)lvUsers.SelectedItem;
            if (employee != null)
            {
                var employeeToDelete = _context.Employees.Find(employee.EmployeeId);
                if (employeeToDelete != null)
                {
                    _context.Employees.Remove(employeeToDelete);
                    _context.SaveChanges();
                    MessageBox.Show("Employee deleted successfully!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    load();
                }
            }
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            HomeAdmin homeAdmin = new HomeAdmin();
            homeAdmin.Show();
            this.Close();
        }
    }
}

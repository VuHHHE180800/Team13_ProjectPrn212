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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Team13_ProjectPrn212.Models;

namespace Team13_ProjectPrn212
{
    public partial class ManagerEmployee : Page
    {
        private Team13QlbhContext _context;

        public ManagerEmployee()
        {
            InitializeComponent();
            _context = new Team13QlbhContext();
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            var employees = _context.Employees.ToList();
            dgProducts.ItemsSource = employees;
        }

        private void dgProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedEmployee = dgProducts.SelectedItem as Employee;
            if (selectedEmployee != null)
            {
                txtEmployeeID.Text = selectedEmployee.EmployeeId.ToString();
                txtEmployeeName.Text = selectedEmployee.EmployeeName;
                txtAddress.Text = selectedEmployee.Address;
                txtEmail.Text = selectedEmployee.Email;
                txtPhone.Text = selectedEmployee.Phonenumber;
                dpEndDate.SelectedDate = selectedEmployee.Dob.ToDateTime(new TimeOnly(0, 0));
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var newEmployee = new Employee
            {
                Username = "newuser", // Replace with actual input
                Password = "password", // Replace with actual input
                EmployeeName = txtEmployeeName.Text,
                Address = txtAddress.Text,
                Dob = DateOnly.FromDateTime(dpEndDate.SelectedDate.Value),
                Email = txtEmail.Text,
                Phonenumber = txtPhone.Text,
                RoleId = 1 // Replace with actual role ID
            };

            _context.Employees.Add(newEmployee);
            _context.SaveChanges();
            LoadEmployees();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var selectedEmployee = dgProducts.SelectedItem as Employee;
            if (selectedEmployee != null)
            {
                selectedEmployee.EmployeeName = txtEmployeeName.Text;
                selectedEmployee.Address = txtAddress.Text;
                selectedEmployee.Dob = DateOnly.FromDateTime(dpEndDate.SelectedDate.Value);
                selectedEmployee.Email = txtEmail.Text;
                selectedEmployee.Phonenumber = txtPhone.Text;

                _context.Entry(selectedEmployee).State = EntityState.Modified;
                _context.SaveChanges();
                LoadEmployees();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedEmployee = dgProducts.SelectedItem as Employee;
            if (selectedEmployee != null)
            {
                _context.Employees.Remove(selectedEmployee);
                _context.SaveChanges();
                LoadEmployees();
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtEmployeeID.Clear();
            txtEmployeeName.Clear();
            txtAddress.Clear();
            txtEmail.Clear();
            txtPhone.Clear();
            dpEndDate.SelectedDate = null;
        }
    }
}

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
    /// Interaction logic for HomeAdmin.xaml
    /// </summary>
    public partial class HomeAdmin : Window
    {
        public HomeAdmin()
        {
            InitializeComponent();
        }
        private void btnUserManagement_Click(object sender, RoutedEventArgs e)
        {
            AdminDashBoard adminDashBoard = new AdminDashBoard();
            adminDashBoard.Show();
            this.Close();
        }

        private void btnReportManagement_Click(object sender, RoutedEventArgs e)
        {
            ManagerEmployees employees = new ManagerEmployees();
            employees.Show();

            this.Hide();
        }
       
        private void txtAdminFullName_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Xử lý sự kiện khi người dùng nhấn vào tên admin
        }

        private void btnOut_Click(object sender, RoutedEventArgs e)
        {
            LoginForm page = new LoginForm();
            page.Show();
            this.Hide();
        }
    }
}

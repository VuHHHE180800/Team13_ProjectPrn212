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
    /// Interaction logic for LoginForm.xaml
    /// </summary>
    public partial class LoginForm : Window
    {
        private Team13QlbhContext db = new Team13QlbhContext();
        public LoginForm()
        {
            InitializeComponent();
        }

        private void BtnDangNhap_Click(object sender, RoutedEventArgs e)
        {
            string username = txtTaiKhoan.Text;
            string password = passMatKhau.Password;
            Employee employee = db.Employees.FirstOrDefault(e => e.Username == username && e.Password == password);
            if (employee != null)
            {
                Application.Current.Properties["employee"] = employee;
                MessageBox.Show("Đăng nhập thành công !", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                if (employee.RoleId == 1)
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                else if (employee.RoleId == 2)
                {
                    CreatOrderWindow creatOrderWindow = new CreatOrderWindow();
                    creatOrderWindow.Show();
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Đăng nhập thất bại. !", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}

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
    /// <summary>
    /// Interaction logic for ManagerAccountEmployeee.xaml
    /// </summary>
    public partial class ManagerAccountEmployeee : Page
    { 
        private Team13QlbhContext _context;
    
        public ManagerAccountEmployeee()        
        {
            InitializeComponent();
            _context = new Team13QlbhContext();
            load();
           
        
        }

        public void load()
        {
            var employees = _context.Employees.Where(e => e.RoleId==1).ToList();
            lvUsers.ItemsSource = employees;
        }

      

        private void btnActivate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDeactivate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

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
using WinterCherry.Services;

namespace WinterCherry.Windows
{
    /// <summary>
    /// Логика взаимодействия для AdministratorWindow.xaml
    /// </summary>
    public partial class AdministratorWindow : BaseWindow
    {
        public AdministratorWindow()
        {
            InitializeComponent();
        }

        private void Kassa_Click(object sender, RoutedEventArgs e)
        {
            var checkoutWindow = new CheckoutWindow();
            checkoutWindow.Show();
            this.Close();
        }
        private void ProductsControl_Click(object sender, RoutedEventArgs e)
        {
            var productsControlWindow = new ProductsControl(false);
            productsControlWindow.Show();
            this.Close();
        }

        private void Inventarization_Click(object sender, RoutedEventArgs e)
        {
            var inventoryWindow = new InventoryWindow();
            inventoryWindow.Show();
            this.Close();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            UserService.Instance.Logout();
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}

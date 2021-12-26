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
using WinterCherry.Data;
using WinterCherry.Services;

namespace WinterCherry.Windows
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : BaseWindow
    {
        private string login;
        private string password;
        public LoginWindow()
        {
            InitializeComponent();
            DataContext = this;
        }
        public string Login
        {
            get => login;
            set
            {
                login = value;
                OnPropertyChanged();
            }
        }
        public string Password
        {
            get => password;
            set
            {
                password = value;
            }
        }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            Password = passwordText.Password;

            using (var db = new WinterCherryContext())
            {
                var employee = db.Employee.FirstOrDefault(p => p.Login == Login && p.Password == password);
                if (employee == null)
                {
                    
                    MessageBox.Show("Неверный логин или пароль!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                else
                {
                    UserService.Instance.SetEmployee(employee);
                    switch (employee.RoleId)
                    {
                        case 1:
                            {
                                var checkoutWindow = new CheckoutWindow();
                                checkoutWindow.Show();
                                
                            }
                            break;
                        case 2:
                            {
                                var administratorWindow = new AdministratorWindow();
                                administratorWindow.Show();
                            }
                            break;
                        default:
                            break;
                    }
                    this.Close();
                    MessageBox.Show($"Добро пожаловать, {UserService.Instance.CurrentEmployee.FullName}!", "Оповещение", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
        }
    }
}

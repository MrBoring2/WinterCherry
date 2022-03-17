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

namespace WinterCherry.Windows
{
    /// <summary>
    /// Логика взаимодействия для EmployeeWindow.xaml
    /// </summary>
    public partial class EmployeeWindow : BaseWindow
    {
        private string fullName;
        private string login;
        private string password;
        private string phoneNumber;
        private string selectedGender;
        private Role selectedRole;
        private Role selectedTRole;
        public EmployeeWindow()
        {
            InitializeComponent();
            LoadRoles();
            LoadGenders();
            DataContext = this;
            CurrentEmployee = new Employee();
        }
        public EmployeeWindow(Employee employee) : this()
        {
            CurrentEmployee = employee;
            FullName = employee.FullName;
            SelectedRole = Roles.FirstOrDefault(p => p.Id == employee.RoleId);
            SelectedGender = Genders.FirstOrDefault(p => p == employee.Gender);
            PhoneNumber = employee.PhoneNumber;
            Login = employee.Login;
            Password = employee.Password;
        }
        public Employee CurrentEmployee { get; set; }
        public List<Role> Roles { get; set; }
        public List<string> Genders { get; set; }
        public string FullName
        {
            get => fullName;
            set
            {
                fullName = value;
                OnPropertyChanged();
            }
        }
        public string PhoneNumber
        {
            get => phoneNumber;
            set
            {
                phoneNumber = value;
                OnPropertyChanged();
            }
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
                OnPropertyChanged();
            }
        }
        public Role SelectedRole
        {
            get => selectedRole;
            set
            {
                selectedRole = value;
                OnPropertyChanged();
            }
        }
        public string SelectedGender
        {
            get => selectedGender;
            set
            {
                selectedGender = value;
                OnPropertyChanged();
            }
        }
        private void LoadRoles()
        {
            using (var db = new WinterCherryContext())
            {
                Roles = new List<Role>(db.Role);
                SelectedRole = Roles.FirstOrDefault();
            }
        }
        private void LoadGenders()
        {
            Genders = new List<string>
            {
                "мужской",
                "женский"
            };
            SelectedGender = Genders.FirstOrDefault();
        }
        private bool Validate()
        {
            return !string.IsNullOrEmpty(FullName) &&
                !string.IsNullOrEmpty(PhoneNumber) &&
                !string.IsNullOrEmpty(Login) &&
                !string.IsNullOrEmpty(Password);
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                CurrentEmployee.FullName = FullName;
                CurrentEmployee.Gender = SelectedGender;
                CurrentEmployee.RoleId = SelectedRole.Id;
                CurrentEmployee.Password = Password;
                CurrentEmployee.Login = Login;
                this.DialogResult = true;
            }
            else
            {
                MessageBox.Show("Не все данные введены корректно!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

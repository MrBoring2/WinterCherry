using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WinterCherry.Models;
using WinterCherry.Services;

namespace WinterCherry.Windows
{
    /// <summary>
    /// Логика взаимодействия для EmployeesListWindow.xaml
    /// </summary>
    public partial class EmployeesListWindow : BaseWindow
    {
        private bool isAscending;
        private bool isDescending;
        private string search;
        private string selectedRole;
        private int currentPage;
        private int maxItemsOnPage;
        private ObservableCollection<Employee> employees;
        private ObservableCollection<Employee> displayedEmployees;
        private Employee selectedEmployee;
        private SortItem selectedSortItem;
        private Visibility allVisibility;
        private Visibility selectVisibility;
        public EmployeesListWindow(bool isModal)
        {
            InitializeComponent();
            LoadRoles();
            LoadSort();
            InitializeFields();
            LoadEmployees();

            if (isModal)
            {
                SelectVisibility = Visibility.Visible;
                AllVisibility = Visibility.Collapsed;
            }
            else
            {
                SelectVisibility = Visibility.Collapsed;
                AllVisibility = Visibility.Visible;
            }

            DataContext = this;
        }
        public List<SortItem> SortItems { get; set; }
        public List<string> Roles { get; set; }
        public ObservableCollection<Employee> Employees
        {
            get => employees;
            set
            {
                employees = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Employee> DisplayedEmployees
        {
            get => displayedEmployees;
            set
            {
                displayedEmployees = value;
                OnPropertyChanged();
            }
        }

        public Employee SelectedEmployee
        {
            get => selectedEmployee;
            set
            {
                selectedEmployee = value;
                OnPropertyChanged();
            }
        }
        public SortItem SelectedSortItem
        {
            get => selectedSortItem;
            set
            {
                selectedSortItem = value;
                OnPropertyChanged();
                RefreshEmployees();
            }
        }
        public Visibility AllVisibility
        {
            get => allVisibility;
            set
            {
                allVisibility = value;
                OnPropertyChanged();
            }
        }
        public Visibility SelectVisibility
        {
            get => selectVisibility;
            set
            {
                selectVisibility = value;
                OnPropertyChanged();
            }
        }
        public int CurrentPage
        {
            get => currentPage;
            set
            {
                currentPage = value;
                OnPropertyChanged();
                RefreshEmployees();
            }
        }

        public string SelectedRole
        {
            get => selectedRole;
            set
            {
                selectedRole = value;
                OnPropertyChanged();
                RefreshEmployees();
            }
        }
        public string DisplayedPages
        {
            get => $"{CurrentPage + 1}/{MaxPage}";
        }
        public bool IsDescending
        {
            get => isDescending;
            set
            {
                isDescending = value;
                if (Employees != null)
                    RefreshEmployees();
                OnPropertyChanged();
            }
        }
        public bool IsAscending
        {
            get => isAscending;
            set
            {
                isAscending = value;
                if (Employees != null)
                    RefreshEmployees();
                OnPropertyChanged();
            }
        }
        public int MaxPage
        {
            get
            {
                var list = Employees.ToList();
                list = list.Where(p => p.FullName.ToLower().Contains(Search.ToLower())).ToList();

                list = list.Where(p => SelectedRole != "Все" ? p.Role.Name.Equals(SelectedRole) : true).ToList();

                return (int)Math.Ceiling((float)list.Count / (float)maxItemsOnPage) != 0 ? (int)Math.Ceiling((float)list.Count / (float)maxItemsOnPage) : 1;
            }
        }
        /// <summary>
        /// Свойство по возрастанию
        /// </summary>

        public string Search
        {
            get => search;
            set
            {
                search = value;
                OnPropertyChanged();
                RefreshEmployees();
            }
        }
        private void InitializeFields()
        {
            currentPage = 0;
            maxItemsOnPage = 10;
        }
        private void LoadEmployees()
        {
            using (var db = new WinterCherryContext())
            {
                Employees = new ObservableCollection<Employee>(db.Employee.Include("Role"));

                RefreshEmployees();
            }
        }

        public void LoadRoles()
        {
            using (var db = new WinterCherryContext())
            {
                Roles = new List<string>();
                Roles.Add("Все");
                var list = db.Role.ToList();
                list.ForEach(p => Roles.Add(p.Name));
                selectedRole = Roles.FirstOrDefault();
            }
        }

        private void LoadSort()
        {
            SortItems = new List<SortItem>
            {
                new SortItem("ФИО", "FullName"),
                new SortItem("Пол", "Gender")
            };
            selectedSortItem = SortItems.FirstOrDefault();
            isDescending = false;
            isAscending = true;
            search = string.Empty;
        }
        private void RefreshEmployees()
        {

            if (CurrentPage > MaxPage - 1)
            {
                currentPage = MaxPage - 1;
            }

            var list = SortEmployees(Employees);

            list = list.Where(p => p.FullName.ToLower().Contains(Search.ToLower())).ToList();

            list = list.Where(p => SelectedRole != "Все" ? p.Role.Name.Equals(SelectedRole) : true).ToList();

            list = list.Skip(currentPage * maxItemsOnPage).Take(maxItemsOnPage).ToList();

            DisplayedEmployees = new ObservableCollection<Employee>(list);

            OnPropertyChanged(nameof(DisplayedPages));

        }

        private List<Employee> SortEmployees(ICollection<Employee> employees)
        {

            if (IsDescending)
            {
                return employees.OrderByDescending(p => p.GetProperty(SelectedSortItem.Property)).ToList();
            }
            else
            {
                return employees.OrderBy(p => p.GetProperty(SelectedSortItem.Property)).ToList();
            }

        }


        private void IceCreamsList_SizeChanged(object sender, RoutedEventArgs e)
        {
            name.Width = IceCreamsList.ActualWidth / 5 + 100;
            gender.Width = IceCreamsList.ActualWidth / 5 - 50;
            phone.Width = IceCreamsList.ActualWidth / 5 - 80;
            role.Width = IceCreamsList.ActualWidth / 5 - 60;
            login.Width = IceCreamsList.ActualWidth / 5 - 40;
        }

        private void IceCreamsList_Loaded(object sender, RoutedEventArgs e)
        {
            name.Width = IceCreamsList.ActualWidth / 5 + 100;
            gender.Width = IceCreamsList.ActualWidth / 5 - 50;
            phone.Width = IceCreamsList.ActualWidth / 5 - 80;
            role.Width = IceCreamsList.ActualWidth / 5 - 60;
            login.Width = IceCreamsList.ActualWidth / 5 + 40;
        }

        private void ToFirst_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage = 0;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage > 0)
            {
                CurrentPage--;
            }
        }

        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage < MaxPage - 1)
            {
                CurrentPage++;
            }
        }

        private void ToLast_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage = MaxPage - 1;
        }

        private void Ascending_Checked(object sender, RoutedEventArgs e)
        {
            IsAscending = true;
            isDescending = false;
        }

        private void Descending_Checked(object sender, RoutedEventArgs e)
        {
            IsDescending = true;
            isAscending = false;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            var administratorWindow = new AdministratorWindow();
            administratorWindow.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var administratorWindow = new AdministratorWindow();
            administratorWindow.Show();
            this.Close();
        }


        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var employeeWindow = new EmployeeWindow();
            if (employeeWindow.ShowDialog() == true)
            {
                using (var db = new WinterCherryContext())
                {
                    db.Employee.Add(employeeWindow.CurrentEmployee);
                    db.SaveChanges();
                    LoadEmployees();
                    MessageBox.Show("Сотрудник успешно добавлен!", "Оповещение", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedEmployee != null)
            {
                var employeeWindow = new EmployeeWindow(SelectedEmployee);
                if (employeeWindow.ShowDialog() == true)
                {
                    using (var db = new WinterCherryContext())
                    {
                        var employee = db.Employee.FirstOrDefault(p => p.EmployeeId == employeeWindow.CurrentEmployee.EmployeeId);
                        if (employee != null)
                        {
                            db.Entry(employee).CurrentValues.SetValues(employeeWindow.CurrentEmployee);
                            db.SaveChanges();
                            LoadEmployees();
                            MessageBox.Show("Сотрудник успешно изменён!", "Оповещение", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Сначала выберите мороженое!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Select_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedEmployee != null)
            {
                using (var db = new WinterCherryContext())
                {
                    db.Employee.Remove(db.Employee.FirstOrDefault(p => p.EmployeeId == SelectedEmployee.EmployeeId));
                    db.SaveChanges();
                    LoadEmployees();
                    MessageBox.Show("Сотрудник успешно удалён!", "Оповещение", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
    }
}

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
    /// Логика взаимодействия для СheckoutWindow.xaml
    /// </summary>
    public partial class CheckoutWindow : BaseWindow
    {
        private bool isAscending;
        private bool isDescending;
        private string search;
        private string selectedIceCreamType;
        private int currentPage;
        private int maxItemsOnPage;
        private decimal orderPrice;
        private ObservableCollection<IceCream> iceCreams;
        private ObservableCollection<IceCream> displayedIceCreams;
        private ObservableCollection<OrderItem> orderItems;
        private IceCream selectedIceCream;
        private SortItem selectedSortItem;
        private OrderItem selectedOrderItem;

        public CheckoutWindow()
        {
            InitializeComponent();
            LoadTypes();
            LoadSort();
            InitializeFields();
            LoadIceCreams();
            DataContext = this;
        }


        public List<SortItem> SortItems { get; set; }
        public List<string> IceCreamTypes { get; set; }
        public ObservableCollection<IceCream> IceCreams
        {
            get => iceCreams;
            set
            {
                iceCreams = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<IceCream> DisplayedIceCreams
        {
            get => displayedIceCreams;
            set
            {
                displayedIceCreams = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<OrderItem> OrderItems
        {
            get => orderItems;
            set
            {
                orderItems = value;
                OnPropertyChanged();
            }
        }
        public IceCream SelectedIceCream
        {
            get => selectedIceCream;
            set
            {
                selectedIceCream = value;
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
                RefreshIceCreams();
            }
        }
        public OrderItem SelectedOrderItem
        {
            get => selectedOrderItem;
            set
            {
                selectedOrderItem = value;
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
                RefreshIceCreams();
            }
        }
        public decimal OrderPrice
        {
            get => orderPrice;
            set
            {
                orderPrice = value;
                OnPropertyChanged();
            }
        }
        public string SelectedIceCreamType
        {
            get => selectedIceCreamType;
            set
            {
                selectedIceCreamType = value;
                OnPropertyChanged();
                RefreshIceCreams();
            }
        }
        public string EmployeeFullName => UserService.Instance.CurrentEmployee.FullName;
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
                if (IceCreams != null)
                    RefreshIceCreams();
                OnPropertyChanged();
            }
        }
        public bool IsAscending
        {
            get => isAscending;
            set
            {
                isAscending = value;
                if (IceCreams != null)
                    RefreshIceCreams();
                OnPropertyChanged();
            }
        }
        public int MaxPage
        {
            get
            {
                var list = IceCreams.ToList();
                list = list.Where(p => p.Name.ToLower().Contains(Search.ToLower())).ToList();

                list = list.Where(p => SelectedIceCreamType != "Все" ? p.IceCreamType.IceCreamTypeName.Equals(SelectedIceCreamType) : p.IceCreamType.IceCreamTypeName.Contains("")).ToList();

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
                RefreshIceCreams();
            }
        }
        private void InitializeFields()
        {
            currentPage = 0;
            maxItemsOnPage = 10;
            OrderPrice = 0;
            OrderItems = new ObservableCollection<OrderItem>();
        }
        private void LoadIceCreams()
        {
            using (var db = new WinterCherryContext())
            {
                IceCreams = new ObservableCollection<IceCream>(db.IceCream.Include("IceCreamType"));
                RefreshIceCreams();
            }
        }

        public void LoadTypes()
        {
            using (var db = new WinterCherryContext())
            {
                IceCreamTypes = new List<string>();
                IceCreamTypes.Add("Все");
                var list = db.IceCreamType.ToList();
                list.ForEach(p => IceCreamTypes.Add(p.IceCreamTypeName));
                selectedIceCreamType = IceCreamTypes.FirstOrDefault();
            }
        }

        private void LoadSort()
        {
            SortItems = new List<SortItem>
            {
                new SortItem("Название", "Name"),
                new SortItem("Цена", "Price"),
                new SortItem("Вес", "Weight")
            };
            selectedSortItem = SortItems.FirstOrDefault();
            isDescending = false;
            isAscending = true;
            search = string.Empty;
        }
        private void RefreshIceCreams()
        {

            if (CurrentPage > MaxPage - 1)
            {
                currentPage = MaxPage - 1;
            }

            var list = SortIceCreams(IceCreams);

            list = list.Where(p => p.Name.ToLower().Contains(Search.ToLower())).ToList();

            list = list.Where(p => SelectedIceCreamType != "Все" ? p.IceCreamType.IceCreamTypeName.Equals(SelectedIceCreamType) : p.IceCreamType.IceCreamTypeName.Contains("")).ToList();

            list = list.Skip(currentPage * maxItemsOnPage).Take(maxItemsOnPage).ToList();

            DisplayedIceCreams = new ObservableCollection<IceCream>(list);

            OnPropertyChanged(nameof(DisplayedPages));

        }

        private List<IceCream> SortIceCreams(ICollection<IceCream> iceCreams)
        {

            if (IsDescending)
            {
                return iceCreams.OrderByDescending(p => p.GetProperty(SelectedSortItem.Property)).ToList();
            }
            else
            {
                return iceCreams.OrderBy(p => p.GetProperty(SelectedSortItem.Property)).ToList();
            }

        }

        private void UpdateOrdePrice(decimal totalPrice)
        {
            OrderPrice += totalPrice;
        }

        private void IceCreamsList_SizeChanged(object sender, RoutedEventArgs e)
        {
            name.Width = IceCreamsList.ActualWidth / 5 + 50;
            type.Width = IceCreamsList.ActualWidth / 5 - 60;
            price.Width = IceCreamsList.ActualWidth / 5 - 30;
            weight.Width = IceCreamsList.ActualWidth / 5 + 5;
            amount.Width = IceCreamsList.ActualWidth / 5 + 15;
        }

        private void IceCreamsList_Loaded(object sender, RoutedEventArgs e)
        {
            name.Width = IceCreamsList.ActualWidth / 5 + 50;
            type.Width = IceCreamsList.ActualWidth / 5 - 60;
            price.Width = IceCreamsList.ActualWidth / 5 - 30;
            weight.Width = IceCreamsList.ActualWidth / 5 + 5;
            amount.Width = IceCreamsList.ActualWidth / 5 + 15;
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

        private void AddToOrder_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedIceCream != null)
            {
                if (OrderItems.FirstOrDefault(p => p.IceCream.Id == SelectedIceCream.Id) == null)
                {
                    var amountIceCream = new AmountIceCream();
                    if (amountIceCream.ShowDialog() == true)
                    {
                        if (amountIceCream.Amount <= SelectedIceCream.Amount)
                        {
                            var orderItem = new OrderItem(SelectedIceCream, amountIceCream.Amount);
                            OrderItems.Add(orderItem);
                            UpdateOrdePrice(orderItem.TotalPrice);
                        }
                        else
                        {
                            MessageBox.Show("Выбранное количество товара больше, чем есть в магазине!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Это мороженое уже есть в списке!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Сначала выберите мороженое!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }



        private void RemoveFromOrder_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedOrderItem != null)
            {
                UpdateOrdePrice(-SelectedOrderItem.TotalPrice);
                OrderItems.Remove(SelectedOrderItem);
            }
            else
            {
                MessageBox.Show("Сначала выберите мороженое в заказе!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CreateOrder_Click(object sender, RoutedEventArgs e)
        {
            if (OrderItems.Count > 0)
            {
                var order = new Order { EmployeeId = UserService.Instance.CurrentEmployee.EmployeeId, Date = DateTime.Now.ToLocalTime() };
                foreach (var item in OrderItems)
                {
                    order.ProductsInOrder.Add(new ProductsInOrder { Amount = item.Amount, ProductId = item.IceCream.Id });
                }
                using (var db = new WinterCherryContext())
                {
                    try
                    {
                        db.Order.Add(order);

                        foreach (var item in order.ProductsInOrder)
                        {
                            var iceCream = db.IceCream.FirstOrDefault(p => p.Id == item.ProductId);
                            if (iceCream != null)
                            {
                                iceCream.Amount -= item.Amount;
                            }
                        }

                        db.SaveChanges();

                        OrderItems.Clear();
                        OrderPrice = 0;
                        LoadIceCreams();
                        MessageBox.Show("Заказ успешно оформлен!", "Оповещение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка при заказе!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Заказ пуст!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
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
            if (UserService.Instance.CurrentEmployee.RoleId == 1)
            {
                UserService.Instance.Logout();
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
            else
            {
                var administratorWindow = new AdministratorWindow();
                administratorWindow.Show();
                this.Close();
            }
        }
    }
}

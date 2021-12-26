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
    /// Логика взаимодействия для ProductsControl.xaml
    /// </summary>
    public partial class ProductsControl : BaseWindow
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
        private Visibility allVisibility;
        private Visibility selectVisibility;
        public ProductsControl(bool isModal)
        {
            InitializeComponent();
            LoadTypes();
            LoadSort();
            InitializeFields();
            LoadIceCreams();

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
                IceCreams = new ObservableCollection<IceCream>(db.IceCream.Include("IceCreamType").Include("ProductsInStorage").Include("ProductsInStorage.Storage"));

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
            name.Width = IceCreamsList.ActualWidth / 6 + 50;
            type.Width = IceCreamsList.ActualWidth / 6 - 60;
            price.Width = IceCreamsList.ActualWidth / 6 - 30;
            weight.Width = IceCreamsList.ActualWidth / 6 - 80;
            amountInShop.Width = IceCreamsList.ActualWidth / 6 + 40;
            amountInStorages.Width = IceCreamsList.ActualWidth / 6 + 40;
        }

        private void IceCreamsList_Loaded(object sender, RoutedEventArgs e)
        {
            name.Width = IceCreamsList.ActualWidth / 6 + 50;
            type.Width = IceCreamsList.ActualWidth / 6 - 60;
            price.Width = IceCreamsList.ActualWidth / 6 - 30;
            weight.Width = IceCreamsList.ActualWidth / 6 - 80;
            amountInShop.Width = IceCreamsList.ActualWidth / 6 + 40;
            amountInStorages.Width = IceCreamsList.ActualWidth / 6 + 40;
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
            if (UserService.Instance.CurrentEmployee.RoleId == 1)
            {
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var administratorWindow = new AdministratorWindow();
            administratorWindow.Show();
            this.Close();
        }

        private void DeliveryToStorage_Click(object sender, RoutedEventArgs e)
        {
            var deliveryToStorage = new DeliveryToStorageWindow();
            if (deliveryToStorage.ShowDialog() == true)
            {
                using (var db = new WinterCherryContext())
                {
                    db.DeliveryToStorage.Add(deliveryToStorage.Delivery);

                    var storage = db.Storage.Include("ProductsInStorage").FirstOrDefault(p => p.Id == deliveryToStorage.Delivery.StorageId);
                    foreach (var product in deliveryToStorage.Delivery.ProductsInDeliveryToStorage)
                    {
                        var productInStorage = storage.ProductsInStorage.FirstOrDefault(p => p.ProductId == product.ProductId);
                        if (productInStorage != null)
                        {
                            productInStorage.Amount += product.Amount;
                        }
                        else
                        {
                            storage.ProductsInStorage.Add(new ProductsInStorage { StorageId = storage.Id, Amount = product.Amount, ProductId = product.ProductId });
                        }
                    }


                    db.SaveChanges();

                    LoadIceCreams();
                    MessageBox.Show("Поставка успешно заказана!", "Оповещение", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void DeliveryToShop_Click(object sender, RoutedEventArgs e)
        {
            var deliveryToShop = new DeliveryToShopWindow();
            if (deliveryToShop.ShowDialog() == true)
            {
                using (var db = new WinterCherryContext())
                {
                    db.DeliveryToShop.Add(deliveryToShop.Delivery);

                    var storage = db.Storage.Include("ProductsInStorage").FirstOrDefault(p => p.Id == deliveryToShop.Delivery.StorageId);
                    foreach (var product in deliveryToShop.Delivery.ProductsInDeliveryToShop)
                    {
                        var productInStorage = storage.ProductsInStorage.FirstOrDefault(p => p.ProductId == product.ProductId);
                        if (productInStorage != null)
                        {
                            if (productInStorage.Amount - product.Amount >= 0)
                            {
                                var iceCream = db.IceCream.FirstOrDefault(p => p.Id == product.ProductId);
                                productInStorage.Amount -= product.Amount;
                                iceCream.Amount += product.Amount;
                            }
                            else
                            {
                                MessageBox.Show("Не хватает товара на складе!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Товар не найден!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }


                    db.SaveChanges();

                    LoadIceCreams();
                    MessageBox.Show("Поставка успешно заказана!", "Оповещение", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var iceCreamWindow = new IceCreamWindow();
            if (iceCreamWindow.ShowDialog() == true)
            {
                using (var db = new WinterCherryContext())
                {
                    db.IceCream.Add(iceCreamWindow.CurrentIceCream);
                    db.SaveChanges();
                    LoadIceCreams();
                    MessageBox.Show("Мороженое успешно добавлено!", "Оповещение", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedIceCream != null)
            {
                var iceCreamWindow = new IceCreamWindow(SelectedIceCream);
                if (iceCreamWindow.ShowDialog() == true)
                {
                    using (var db = new WinterCherryContext())
                    {
                        var iceCream = db.IceCream.FirstOrDefault(p => p.Id == iceCreamWindow.CurrentIceCream.Id);
                        if (iceCream != null)
                        {
                            iceCream.Name = iceCreamWindow.CurrentIceCream.Name;
                            iceCream.Weight = iceCreamWindow.CurrentIceCream.Weight;
                            iceCream.Price = iceCreamWindow.CurrentIceCream.Price;
                            iceCream.TypeId = iceCreamWindow.CurrentIceCream.TypeId;
                            db.Entry(iceCream).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            LoadIceCreams();
                            MessageBox.Show("Мороженое успешно изменено!", "Оповещение", MessageBoxButton.OK, MessageBoxImage.Information);
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
    }
}

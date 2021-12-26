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

namespace WinterCherry.Windows
{
    /// <summary>
    /// Логика взаимодействия для DeliveryToShop.xaml
    /// </summary>
    public partial class DeliveryToShopWindow : BaseWindow
    {
        private int currentPage;
        private int maxItemsOnPage;
        private string search;
        private string selectedType;
        private Supplier selectedSupplier;
        private StorageItem selectedIceCream;
        private Storage selectedStorage;
        private DeliveryItem selectedDeliveryItem;
        private ObservableCollection<StorageItem> displayedIceCreams;
        private ObservableCollection<DeliveryItem> deliveryItems;
        public DeliveryToShopWindow()
        {
            InitializeComponent();
            InitializeFields();
            LoadTypes();
            LoadStorages();
            LoadSupliers();
            LoadIceCreams();
            DataContext = this;
        }
        public DeliveryToShop Delivery { get; private set; }
        public Supplier SelectedSupplier
        {
            get => selectedSupplier;
            set
            {
                selectedSupplier = value;
                OnPropertyChanged();
            }
        }
        public Storage SelectedStorage
        {
            get => selectedStorage;
            set
            {
                selectedStorage = value;
                OnPropertyChanged();
                LoadIceCreams();
                DeliveryItems.Clear();
            }
        }
        public StorageItem SelectedIceCream
        {
            get => selectedIceCream;
            set
            {
                selectedIceCream = value;
                OnPropertyChanged();
            }
        }
        public DeliveryItem SelectedDeliveryItem
        {
            get => selectedDeliveryItem;
            set
            {
                selectedDeliveryItem = value;
                OnPropertyChanged();
            }
        }
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
        public string SelectedType
        {
            get => selectedType;
            set
            {
                selectedType = value;
                OnPropertyChanged();
                RefreshIceCreams();
            }
        }
        public List<Supplier> Suppliers { get; set; }
        public List<Storage> Storages { get; set; }
        public List<string> IceCreamTypes { get; set; }
        public List<StorageItem> IceCreams { get; set; }
        public ObservableCollection<DeliveryItem> DeliveryItems
        {
            get => deliveryItems;
            set
            {
                deliveryItems = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<StorageItem> DisplayedIceCreams
        {
            get => displayedIceCreams;
            set
            {
                displayedIceCreams = value;
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
        public string DisplayedPages
        {
            get => $"{CurrentPage + 1}/{MaxPage}";
        }
        public int MaxPage
        {
            get
            {
                var list = IceCreams.ToList();
                list = list.Where(p => p.IceCream.Name.ToLower().Contains(Search.ToLower())).ToList();

                list = list.Where(p => SelectedType != "Все" ? p.IceCream.IceCreamType.IceCreamTypeName.Equals(SelectedType) : p.IceCream.IceCreamType.IceCreamTypeName.Contains("")).ToList();

                return (int)Math.Ceiling((float)list.Count / (float)maxItemsOnPage) != 0 ? (int)Math.Ceiling((float)list.Count / (float)maxItemsOnPage) : 1;
            }
        }
        private void InitializeFields()
        {
            search = "";
            currentPage = 0;
            maxItemsOnPage = 10;
            DeliveryItems = new ObservableCollection<DeliveryItem>();
        }
        private void LoadIceCreams()
        {
            using (var db = new WinterCherryContext())
            {
                IceCreams = new List<StorageItem>();
                var storage = db.Storage.Include("ProductsInStorage").Include("ProductsInStorage.IceCream").FirstOrDefault(p => p.Id == SelectedStorage.Id);
                foreach (var item in storage.ProductsInStorage)
                {
                    if(item.Amount > 0)
                    {
                        IceCreams.Add(new StorageItem(item.IceCream, item.Amount));
                    }
                }
                

                RefreshIceCreams();
            }
        }

        private void LoadStorages()
        {
            using (var db = new WinterCherryContext())
            {
                Storages = new List<Storage>(db.Storage.ToList());
                selectedStorage = Storages.FirstOrDefault();
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
                selectedType = IceCreamTypes.FirstOrDefault();
            }
        }
        public void LoadSupliers()
        {
            using (var db = new WinterCherryContext())
            {
                Suppliers = new List<Supplier>(db.Supplier.ToList());
                SelectedSupplier = Suppliers.FirstOrDefault();
            }
        }
        private void RefreshIceCreams()
        {

            if (CurrentPage > MaxPage - 1)
            {
                currentPage = MaxPage - 1;
            }

            var list = IceCreams.OrderBy(p => p.IceCream.Name).ToList();

            list = list.Where(p => p.IceCream.Name.ToLower().Contains(Search.ToLower())).ToList();

            list = list.Where(p => SelectedType != "Все" ? p.IceCream.IceCreamType.IceCreamTypeName.Equals(SelectedType) : p.IceCream.IceCreamType.IceCreamTypeName.Contains("")).ToList();

            list = list.Skip(currentPage * maxItemsOnPage).Take(maxItemsOnPage).ToList();

            DisplayedIceCreams = new ObservableCollection<StorageItem>(list);

            OnPropertyChanged(nameof(DisplayedPages));

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
        private void AddToDelivery_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedIceCream != null)
            {
                if (DeliveryItems.FirstOrDefault(p => p.IceCream.Id == SelectedIceCream.IceCream.Id) == null)
                {
                    var amountIceCream = new AmountIceCream();
                    if (amountIceCream.ShowDialog() == true)
                    {
                        if (amountIceCream.Amount <= SelectedIceCream.Amount)
                        {
                            DeliveryItems.Add(new DeliveryItem(SelectedIceCream.IceCream, amountIceCream.Amount));
                        }
                        else
                        {
                            MessageBox.Show("На складе не хватает товара!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
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

        private void RemoveFromDelivery_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedDeliveryItem != null)
            {
                DeliveryItems.Remove(SelectedDeliveryItem);
            }
            else
            {
                MessageBox.Show("Сначала выберите мороженое в поставке!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CreateDelivery_Click(object sender, RoutedEventArgs e)
        {
            if (DeliveryItems.Count > 0)
            {
                Delivery = new DeliveryToShop { Date = DateTime.Now.ToLocalTime(), StorageId = SelectedStorage.Id };
                Delivery.ProductsInDeliveryToShop = new List<ProductsInDeliveryToShop>();
                foreach (var product in DeliveryItems)
                {
                    Delivery.ProductsInDeliveryToShop.Add(new ProductsInDeliveryToShop { Amount = product.Amount, ProductId = product.IceCream.Id });
                }
                this.DialogResult = true;
            }
            else
            {
                MessageBox.Show("Добавьте товары в поставку!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}


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
    /// Логика взаимодействия для IceCreamWindow.xaml
    /// </summary>
    public partial class IceCreamWindow : BaseWindow
    {
        private string iceCreamName;
        private double weight;
        private decimal price;
        private IceCreamType selectedType;

        public IceCreamWindow()
        {
            InitializeComponent();
            LoadTypes();
            DataContext = this;
            CurrentIceCream = new IceCream();
            CurrentIceCream.Amount = 0;
        }
        public IceCreamWindow(IceCream iceCream) : this()
        {
            CurrentIceCream = iceCream;
            IceCreamName = iceCream.Name;
            Price = iceCream.Price;
            Weight = iceCream.Weight;
            SelectedType = IceCreamTypes.FirstOrDefault(p => p.IceCreamTypeId == iceCream.TypeId);
        }
        public IceCream CurrentIceCream { get; set; }
        public List<IceCreamType> IceCreamTypes { get; set; }
        public string IceCreamName
        {
            get => iceCreamName;
            set
            {
                iceCreamName = value;
                OnPropertyChanged();
            }
        }
        public double Weight
        {
            get => weight;
            set
            {
                weight = value;
                OnPropertyChanged();
            }
        }
        public decimal Price
        {
            get => price;
            set
            {
                price = value;
                OnPropertyChanged();
            }
        }
        public IceCreamType SelectedType
        {
            get => selectedType;
            set
            {
                selectedType = value;
                OnPropertyChanged();
            }
        }

        private void LoadTypes()
        {
            using (var db = new WinterCherryContext())
            {
                IceCreamTypes = new List<IceCreamType>(db.IceCreamType);
                selectedType = IceCreamTypes.FirstOrDefault();
            }
        }
        private bool Validate()
        {
            return !string.IsNullOrEmpty(IceCreamName) &&
                double.TryParse(Weight.ToString(), out double w) &&
                decimal.TryParse(Price.ToString(), out decimal p) &&
                Price > 0;
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                CurrentIceCream.Name = IceCreamName;
                CurrentIceCream.Price = Price;
                CurrentIceCream.Weight = Weight;
                CurrentIceCream.TypeId = SelectedType.IceCreamTypeId;
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

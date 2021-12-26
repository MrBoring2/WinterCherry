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

namespace WinterCherry.Windows
{
    /// <summary>
    /// Логика взаимодействия для AmountIceCream.xaml
    /// </summary>
    public partial class AmountIceCream : BaseWindow
    {
        private int amount;
        public AmountIceCream()
        {
            InitializeComponent();
            DataContext = this;
        }

        public int Amount
        {
            get => amount;
            set
            {
                amount = value;
                OnPropertyChanged();
            }
        }
        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(Amount.ToString(), out int a) && a > 0)
            {
                this.DialogResult = true;
            }
            else
            {
                MessageBox.Show("Неккоректное количество!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CANCEL_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

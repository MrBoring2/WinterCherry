using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using WinterCherry.Models;
using Excel = Microsoft.Office.Interop.Excel;

namespace WinterCherry.Windows
{
    /// <summary>
    /// Логика взаимодействия для InventoryWindow.xaml
    /// </summary>
    public partial class InventoryWindow : BaseWindow
    {
        private decimal factTotalPrice;
        private decimal buhTotalPrice;
        private ObservableCollection<InventoryModel> inventoryModels;
        private InventoryModel selectedInventoryModel;
        public InventoryWindow()
        {
            InitializeComponent();
            InventoryModels = new ObservableCollection<InventoryModel>();
            DataContext = this;
        }
        public decimal FactTotalPrice
        {
            get => factTotalPrice;
            set
            {
                factTotalPrice = value;
                OnPropertyChanged();
            }
        }
        public decimal BuhTotalPrice
        {
            get => buhTotalPrice;
            set
            {
                buhTotalPrice = value;
                OnPropertyChanged();
            }
        }
        public InventoryModel SelectedInventoryModel
        {
            get => selectedInventoryModel;
            set
            {
                selectedInventoryModel = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<InventoryModel> InventoryModels
        {
            get => inventoryModels;
            set
            {
                inventoryModels = value;
                OnPropertyChanged();
            }
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            var administratorWindow = new AdministratorWindow();
            administratorWindow.Show();
            this.Close();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var iceCreamListWindow = new ProductsControl(true);
            if (iceCreamListWindow.ShowDialog() == true)
            {
                if (InventoryModels.FirstOrDefault(p => p.IceCream.Id == iceCreamListWindow.SelectedIceCream.Id) == null)
                {
                    var inventoryModel = new InventoryModel(iceCreamListWindow.SelectedIceCream, iceCreamListWindow.SelectedIceCream.Amount, iceCreamListWindow.SelectedIceCream.Price);
                    inventoryModel.PropertyChanged += InventoryModel_PropertyChanged;
                    InventoryModels.Add(inventoryModel);
                    FactTotalPrice = InventoryModels.Sum(p => p.FactTotalPrice);
                    BuhTotalPrice = InventoryModels.Sum(p => p.BuhTotalPrice);
                }
                else
                {
                    MessageBox.Show("Даный продукт уже есть в списке!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void InventoryModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            FactTotalPrice = InventoryModels.Sum(p => p.FactTotalPrice);
            BuhTotalPrice = InventoryModels.Sum(p => p.BuhTotalPrice);
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedInventoryModel != null)
            {
                InventoryModels.Remove(SelectedInventoryModel);
            }
            else
            {
                MessageBox.Show("Сначала выберите продукт!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        /// <summary>
        /// Формирование Excel документа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async Task Export()
        {
            await Task.Run(() =>
            {
                InventoryModels = new ObservableCollection<InventoryModel>(InventoryModels.OrderBy(p => p.IceCream.Id));
                var application = new Excel.Application();
                application.SheetsInNewWorkbook = 1;
                Excel.Workbook workbook = application.Workbooks.Add(Type.Missing);
                int startRowIndex = 1;
                Excel.Worksheet worksheet = application.Worksheets.Item[1];
                worksheet.Name = "Инвентаризация";
                worksheet.Cells[1][startRowIndex] = "№";
                worksheet.Cells[2][startRowIndex] = "Название";
                worksheet.Cells[3][startRowIndex] = "Количество фактическое";
                worksheet.Cells[4][startRowIndex] = "Количество учётное";
                worksheet.Cells[5][startRowIndex] = "Отклонение";
                worksheet.Cells[6][startRowIndex] = "Цена";
                worksheet.Cells[7][startRowIndex] = "Стоимость фактическая";
                worksheet.Cells[8][startRowIndex] = "Стоимость учётная";
                startRowIndex++;

                foreach (var inventoryModel in InventoryModels)
                {
                    worksheet.Cells[1][startRowIndex] = inventoryModel.IceCream.Id;
                    worksheet.Cells[2][startRowIndex] = inventoryModel.IceCream.Name;
                    worksheet.Cells[3][startRowIndex] = inventoryModel.FactAmount;
                    worksheet.Cells[4][startRowIndex] = inventoryModel.BuhAmount;
                    worksheet.Cells[5][startRowIndex] = inventoryModel.Deviation;
                    worksheet.Cells[6][startRowIndex] = inventoryModel.Price;
                    worksheet.Cells[7][startRowIndex] = inventoryModel.FactTotalPrice;
                    worksheet.Cells[8][startRowIndex] = inventoryModel.BuhTotalPrice;
                    startRowIndex++;
                }
                startRowIndex += 2;


                worksheet.Cells[5][startRowIndex] = "Сумма фактическая:";
                worksheet.Cells[6][startRowIndex].Formula = $"=SUM(G{2}:" + $"G{startRowIndex - 3})";
                worksheet.Cells[7][startRowIndex] = "Сумма учётная:";
                worksheet.Cells[8][startRowIndex].Formula = $"=SUM(H{2}:" + $"H{startRowIndex - 3})";
                worksheet.Cells[5][startRowIndex].Font.Bold = worksheet.Cells[7][startRowIndex].Font.Bold = true;
                Excel.Range rangeBorders = worksheet.Range[worksheet.Cells[1][1], worksheet.Cells[8][startRowIndex - 3]];
                rangeBorders.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle =
                rangeBorders.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle =
                rangeBorders.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle =
                rangeBorders.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle =
                rangeBorders.Borders[Excel.XlBordersIndex.xlInsideHorizontal].LineStyle =
                rangeBorders.Borders[Excel.XlBordersIndex.xlInsideVertical].LineStyle = Excel.XlLineStyle.xlContinuous;

                worksheet.Columns.AutoFit();
                application.Visible = true;
            });
        }
        private async void Spend_Click(object sender, RoutedEventArgs e)
        {
            Task messageTask = Task.Run(() => MessageBox.Show("Отчёт создаётся...", "Подождите", MessageBoxButton.OK, MessageBoxImage.Information));
            Task exportTask = Export();
            await Task.Run(() => Task.WaitAll(messageTask, exportTask));
        }

    }
}

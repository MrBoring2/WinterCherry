using LiveCharts;
using LiveCharts.Wpf;
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

namespace WinterCherry.Windows
{
    /// <summary>
    /// Логика взаимодействия для StatisticsWindow.xaml
    /// </summary>
    public partial class StatisticsWindow : BaseWindow
    {
        private string mostSales;
        private WinterCherryContext _context;
        private SeriesCollection seriesCollection;
        private ObservableCollection<string> labels;
        private string maxSaled;
        private int totalSaled;
        private DateTime startDate;
        private DateTime endDate;
        public StatisticsWindow()
        {
            InitializeComponent();
            _context = new WinterCherryContext();
            StartDate = DateTime.Now - new TimeSpan(24 * 7, 0, 0);
            EndDate = DateTime.Now;
            MaxSaled = "Нет значения";
            DataContext = this;
        }
        public Func<double, string> YFormatter { get; set; }
        public DateTime StartDate
        {
            get => startDate;
            set { startDate = value <= (EndDate == DateTime.MinValue ? value : endDate) ? value : startDate; OnPropertyChanged(); }
        }
        public DateTime EndDate { get => endDate; set { endDate = value >= StartDate ? value : endDate; OnPropertyChanged(); } }
        public SeriesCollection SeriesCollection { get => seriesCollection; set { seriesCollection = value; OnPropertyChanged(); } }
        public string MostSales { get => mostSales; set { mostSales = value; OnPropertyChanged(); } }
        public string MaxSaled { get => maxSaled; set { maxSaled = value; OnPropertyChanged(); } }
        public int TotalSaled { get => totalSaled; set { totalSaled = value; OnPropertyChanged(); } }
        public ObservableCollection<string> Labels { get => labels; set { labels = value; OnPropertyChanged(); } }
        /// <summary>
        /// Сформировать статистику
        /// </summary>
        /// <param name="start">Дата начала</param>
        /// <param name="end">Дата конца</param>
        private void GenerateGenresStatistics(DateTime start, DateTime end)
        {
            TotalSaled = 0;
            if (StartDate.Date <= EndDate.Date && EndDate.Date <= DateTime.Now.Date)
            {
                SeriesCollection = new SeriesCollection();

                var types = _context.IceCreamType.ToList();

                Dictionary<string, int> totalSales = new Dictionary<string, int>();
                MostSales = "Всего продано: ";
                Labels = new ObservableCollection<string>();
                for (DateTime i = start; i <= end; i = i.AddDays(1))
                {
                    Labels.Add(i.ToLongDateString());
                }

                foreach (var type in types)
                {
                    var values = new ChartValues<double>();
                    for (DateTime i = start; i <= end; i = i.AddDays(1))
                    {

                        int sale = 0;
                        foreach (var iceCream in type.IceCream)
                        {
                            sale += iceCream.ProductsInOrder.Where(p => p.Order.Date.Date == i.Date).Sum(p => p.Amount);
                        }

                        if (totalSales.ContainsKey(type.IceCreamTypeName))
                        {
                            totalSales[type.IceCreamTypeName] += sale;
                        }
                        else
                        {
                            totalSales.Add(type.IceCreamTypeName, sale);
                        }

                        values.Add(sale);
                        TotalSaled += sale;
                    }

                    SeriesCollection.Add(new LineSeries
                    {
                        Title = type.IceCreamTypeName,
                        LabelPoint = point => $"Продаж: {point.Y}",
                        Values = values
                    });

                }
                var maxTypeSale = totalSales.FirstOrDefault(p => p.Value == totalSales.Max(d => d.Value));
                MaxSaled = $"{maxTypeSale.Key}: {maxTypeSale.Value}";


                foreach (var item in totalSales)
                {
                    if (item.Key != totalSales.LastOrDefault().Key)
                    {
                        MostSales += $"{item.Key} - {item.Value} шт., ";
                    }
                    else
                    {
                        MostSales += $"{item.Key} - {item.Value} шт.";
                    }
                }

                YFormatter = value => value.ToString("C");
            }
            else
            {
                MessageBox.Show("Выставлена неккоректная дата начала или конца периода!", "Ошибка формирования!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void GenerateStatisticBtn_Click(object sender, RoutedEventArgs e)
        {
            GenerateGenresStatistics(StartDate, EndDate);
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            var administratorWindow = new AdministratorWindow();
            administratorWindow.Show();
            this.Close();
        }
    }
}

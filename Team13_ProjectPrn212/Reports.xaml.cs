using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Team13_ProjectPrn212.Models;

namespace Team13_ProjectPrn212
{
    public partial class Reports : Window
    {
        public ChartValues<int> ProductSales { get; set; }
        public List<string> MonthLabels { get; set; }
        public Func<double, string> YAxisFormatter { get; set; }
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public Reports()
        {
            InitializeComponent();

            NgayTruoc.SelectedDate = new DateTime(2015, 1, 1);
            NgaySau.SelectedDate = DateTime.Now;
            NgayTruocMonth.SelectedDate = new DateTime(2015, 1, 1);
            NgaySauMonth.SelectedDate = DateTime.Now;

            // Initialize the data properties
            ProductSales = new ChartValues<int>();
            MonthLabels = new List<string>();
            YAxisFormatter = value => value.ToString("N0") + " sp";

            DataContext = this;
        }

        private int GetTotalQuantitySold(List<OrderDetail> orderDetails)
        {
            int totalQuantity = 0;
            foreach (OrderDetail od in orderDetails)
            {
                totalQuantity += od.TotalQuantity;
            }
            return totalQuantity;
        }

        private void BtnThongke_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Clear existing data
                ProductSales.Clear();
                MonthLabels.Clear();
                CartesianChart.Series.Clear();

                // Keep from and to as DateTime
                DateTime from = NgayTruoc.SelectedDate ?? new DateTime(2015, 1, 1);
                DateTime to = NgaySau.SelectedDate ?? DateTime.Now;

                using (var db = new Team13QlbhContext())
                {
                    // Get orders within the selected date range and include OrderDetails
                    var orders = db.Orders
                        .Include(o => o.OrderDetails)
                        .Where(x => x.OrderDate >= DateOnly.FromDateTime(from) && x.OrderDate <= DateOnly.FromDateTime(to) && x.StatusId == 2) // Convert DateTime to DateOnly for comparison
                        .OrderBy(x => x.OrderDate)
                        .ToList();

                    // Group by month and calculate the total quantity of products sold
                    var groupedData = orders.GroupBy(x => new { x.OrderDate.Year, x.OrderDate.Month })
                                            .Select(g => new
                                            {
                                                Month = new DateTime(g.Key.Year, g.Key.Month, 1),
                                                TotalQuantity = GetTotalQuantitySold(g.SelectMany(o => o.OrderDetails).ToList())
                                            })
                                            .OrderBy(x => x.Month)
                                            .ToList();

                    // Populate data for the chart
                    foreach (var item in groupedData)
                    {
                        ProductSales.Add(item.TotalQuantity);
                        MonthLabels.Add(item.Month.ToString("MM/yyyy"));
                    }

                    // Create the ColumnSeries
                    SeriesCollection = new SeriesCollection
                    {
                        new ColumnSeries
                        {
                            Title = "Số lượng sản phẩm",
                            Values = ProductSales
                        }
                    };
                }

                CartesianChart.AxisX[0].Labels = MonthLabels;
                CartesianChart.Series = SeriesCollection;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnThongkeMonth_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Clear existing data
                SeriesCollectionMonth.Series.Clear();

                // Keep from and to as DateTime
                DateTime from = NgayTruocMonth.SelectedDate ?? new DateTime(2015, 1, 1);
                DateTime to = NgaySauMonth.SelectedDate ?? DateTime.Now;

                using (var db = new Team13QlbhContext())
                {
                    var orders = db.Orders
                        .Where(x => x.OrderDate >= DateOnly.FromDateTime(from) && x.OrderDate <= DateOnly.FromDateTime(to) && x.StatusId == 2) // Convert DateTime to DateOnly for comparison
                        .OrderBy(x => x.OrderDate)
                        .ToList();

                    // Group by month
                    var groupedData = orders.GroupBy(x => new { x.OrderDate.Year, x.OrderDate.Month })
                                         .Select(g => new
                                         {
                                             Month = new DateTime(g.Key.Year, g.Key.Month, 1),
                                             TotalRevenue = g.SelectMany(x => x.OrderDetails).Sum(od => od.TotalPrice)
                                         })
                                         .OrderBy(x => x.Month)
                                         .ToList();

                    var listMonth = new List<string>();
                    var values = new ChartValues<double>();

                    foreach (var item in groupedData)
                    {
                        listMonth.Add(item.Month.ToString("MMMM"));
                        values.Add(item.TotalRevenue);
                    }

                    SeriesCollection = new SeriesCollection
                    {
                        new LineSeries
                        {
                            Title = "Doanh thu",
                            Values = values,
                            PointGeometry = DefaultGeometries.Circle,
                            PointGeometrySize = 10
                        }
                    };

                    Labels = listMonth.ToArray();
                    YFormatter = value => value.ToString("C"); // Currency format

                    SeriesCollectionMonth.AxisX[0].Labels = Labels;
                    SeriesCollectionMonth.AxisY[0].LabelFormatter = YFormatter;
                    SeriesCollectionMonth.Series = SeriesCollection;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
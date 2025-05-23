using Chromatograph.Service;
using Chromatograph.ViewModels;
using LiveChartsCore.SkiaSharpView.WPF;
using System.Windows;
using System.Windows.Controls;

namespace Chromatograph.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var dialodService = new DialogService();
        dialodService.PrintChartRequested += PrintChart;

        DataContext = new MainViewModel(new ExcelDataService("Content/Data.xlsx"),dialodService);
    }

    private void PrintChart()
    {
        PrintDialog pd = new PrintDialog();

        if (pd.ShowDialog() == true)
        {
            var chartVisual = new CartesianChart
            {
                Series = chart.Series,
                XAxes = chart.XAxes,
                YAxes = chart.YAxes,
                Width = chart.Width,
                Height = chart.Height,
            };

            chartVisual.Measure(new Size(chartVisual.Width, chartVisual.Height));
            chartVisual.Arrange(new Rect(new Point(0, 0), new Size(chartVisual.Width, chartVisual.Height)));

            pd.PrintVisual(chartVisual, "Печать графика");
        }
    }
}
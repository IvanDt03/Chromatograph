using Chromatograph.Service;
using Chromatograph.ViewModels;
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
            pd.PrintVisual(chart, "Печать графика");
        }
    }
}
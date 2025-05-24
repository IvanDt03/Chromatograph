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
        var printDialog = new PrintDialog();
        printDialog.PrintTicket.PageOrientation = System.Printing.PageOrientation.ReverseLandscape;

        if (printDialog.ShowDialog() == true)
        {
            printDialog.PrintVisual(chart, "Печать графика");
        }
    }
}
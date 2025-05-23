using Chromatograph.Service;
using Chromatograph.ViewModels;
using System.Windows;

namespace Chromatograph.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        DataContext = new MainViewModel(new ExcelDataService("Content/Data.xlsx"), new MessageDialogService());
    }
}
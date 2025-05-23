using Chromatograph.Service;
using System;
using System.Windows;

namespace Chromatograph.Views;

public class DialogService : IDialogService
{
    public void ShowMessage(string? message, string title = "Сообщение")
    {
        MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);
    }

    public Action? PrintChartRequested { get; set; }
}


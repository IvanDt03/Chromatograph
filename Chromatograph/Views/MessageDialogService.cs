using Chromatograph.Service;
using System.Windows;

namespace Chromatograph.Views;

public class MessageDialogService : IDialogService
{
    public void ShowMessage(string? message, string title = "Сообщение")
    {
        MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);
    }
}

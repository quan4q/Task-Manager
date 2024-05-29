using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Task_Manager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static SQLDatabase Database { get; } = new();
        public static Notificator Notificator { get; } = new();
    }
}

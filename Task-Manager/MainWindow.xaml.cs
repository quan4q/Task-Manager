using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Task_Manager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Task> Tasks; 

        public MainWindow()
        {
            InitializeComponent();
        }

        private void UpdateTasksList()
        {
            Tasks = App.Database.GetTasks();
            TasksList.ItemsSource = Tasks;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateTasksList();
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            Window TaskWindow = new TaskWindow();
            TaskWindow.Closed += TaskWindow_Closed;
            TaskWindow.ShowDialog();
        }

        private void TaskWindow_Closed(object sender, EventArgs e)
        {
            UpdateTasksList();
        }
    }
}
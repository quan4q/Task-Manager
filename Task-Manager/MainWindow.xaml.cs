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
        public static ObservableCollection<Task> Tasks { get; set; }
        public static SQLDatabase Database = new();

        // NOTE: надо прочитать про кеширование в базах данных.

        public MainWindow()
        {
            InitializeComponent();
        }

        public void UpdateTasksList()
        {
            Tasks = Database.GetTasks();
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
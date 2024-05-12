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
        public static ObservableCollection<Task> Tasks { get; } = [];
        public MainWindow()
        {
            InitializeComponent();

            DataContext = this; // Для подключения Tasks.
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Парочка тестовых задач для проверки отображения и сортировки
            Task task1 = new()
            {
                Id = 1,
                Title = "Сходить на пары",
                Description = "В 18:00 пара по программированию, нельзя пропускать!",
                Deadline = new DateTime(2024, 05, 10, 18, 00, 00),
                Category = "Учёба",
                Priority = "Высокий",
                IsCompleted = false
            };

            Task task2 = new()
            {
                Id = 2,
                Title = "Обсуждение проекта",
                Description = "Нужно встретиться в коворгинге на первом этаже и обсудить детали нашего проекта. ВАЖНО: сортировка, уведомления.",
                Deadline = new DateTime(2024, 05, 5, 17, 00, 00),
                Category = "Работа",
                Priority = "Высокий",
                IsCompleted = false
            };

            Task task3 = new()
            {
                Id = 3,
                Title = "Приготовить еду на завтра",
                Description = "Нашёл в интернете рецепт лазаньи. Говорят вкусная, надо попробовать её приготовить.",
                Deadline = new DateTime(2024, 05, 6, 21, 30, 00),
                Category = "Дом",
                Priority = "Средний",
                IsCompleted = false
            };

            Tasks.Add(task1);
            Tasks.Add(task2);
            Tasks.Add(task3);
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            Window TaskWindow = new TaskWindow();
            TaskWindow.ShowDialog();
        }
    }
}
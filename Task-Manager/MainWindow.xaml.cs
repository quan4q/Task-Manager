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
                Deadline = new DateTime(2025, 01, 1, 21, 30, 00),
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

        public void UpdateTasksList()
        {
            TasksList.ItemsSource = FilterTasks();
        }

        //Применение фильтров к коллекции
        public ObservableCollection<Task> FilterTasks()
        {
            ObservableCollection<Task> filteredTasks = new ObservableCollection<Task>();

            filteredTasks = FilterByPriority(Tasks);
            filteredTasks = FilterByCategory(filteredTasks);
            filteredTasks = FilterByTime(filteredTasks);
            filteredTasks = FilterByTitle(filteredTasks);

            return filteredTasks;
        }

        public ObservableCollection<Task> FilterByPriority(ObservableCollection<Task> tasks)
        {
            ComboBoxItem cbi = (ComboBoxItem)PriorityBox.SelectedItem;
            string filter = cbi.Content.ToString();

            if (filter != "Все")
            {
                return new ObservableCollection<Task>(tasks.Where(t => t.Priority == filter).ToList()); //Почитать про Linq и лямбда функции в шарпах
            }
            else
            {
                return tasks;
            }
        }

        public ObservableCollection<Task> FilterByCategory(ObservableCollection<Task> tasks)
        {
            ComboBoxItem cbi = (ComboBoxItem)CategoryBox.SelectedItem;
            string filter = cbi.Content.ToString();

            if (filter != "Все")
            {
                return new ObservableCollection<Task>(tasks.Where(t => t.Category == filter).ToList());
            }
            else
            {
                return tasks;
            }
        }
        //ВАЖНО СДЕЛАТЬ ПРОВЕРКУ НА ДУРОЧКА В ДЕДЛАЙНЕ!!!
        public ObservableCollection<Task> FilterByTime(ObservableCollection<Task> tasks)
        {
            ComboBoxItem cbi = (ComboBoxItem)Deadline.SelectedItem;
            string filter = cbi.Content.ToString();

            if (filter == "Скоро")
            {
                return new ObservableCollection<Task>(tasks.OrderBy(d => d.Deadline).ToList());
            }
            else
            {
                return new ObservableCollection<Task>(tasks.OrderByDescending(d => d.Deadline).ToList());
            }
        }

        public ObservableCollection<Task> FilterByTitle(ObservableCollection<Task> tasks)
        {
            if(!string.IsNullOrEmpty(SearchBox.Text))
            {
                string substringToSearch = SearchBox.Text.ToLower();
                return new ObservableCollection<Task>(tasks.Where(t => t.Title.ToLower().Contains(substringToSearch)).ToList());
            }
            else
            {
                return tasks;
            }
        }
        //Попробовать сделать лист не null, постоянные проверки это не красиво
        private void PriorityBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TasksList != null)
            {
                UpdateTasksList();
            }
        }

        private void CategoryBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TasksList != null)
            {
                UpdateTasksList();
            }
        }

        private void Deadline_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TasksList != null)
            {
                UpdateTasksList();
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TasksList != null)
            {
                UpdateTasksList();
            }
        }
    }
}
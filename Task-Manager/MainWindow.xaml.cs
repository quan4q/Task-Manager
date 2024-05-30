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
using System.Windows.Threading;
using Notification.Wpf;

namespace Task_Manager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Task> Tasks;

        public MainWindow()
        {
            InitializeComponent();
            UpdateTasksList();
        }

        private void UpdateTasksList()
        {
            Tasks = App.Database.GetTasks();
            TasksList.ItemsSource = FilterTasks();
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            Window TaskWindow = new TaskWindow();
            TaskWindow.Closed += TaskWindow_Closed;
            TaskWindow.ShowDialog();
        }

        //Применение фильтров к коллекции
        public List<Task> FilterTasks()
        {
            List<Task> filteredTasks = [];

            filteredTasks = FilterByPriority(Tasks);
            filteredTasks = FilterByCategory(filteredTasks);
            filteredTasks = FilterByTime(filteredTasks);
            filteredTasks = FilterBySearch(filteredTasks);

            return filteredTasks;
        }

        public List<Task> FilterByPriority(List<Task> tasks)
        {
            ComboBoxItem selectedPriority = (ComboBoxItem)PriorityBox.SelectedItem;
            string priorityFilter = selectedPriority.Content.ToString();

            if (priorityFilter != "Все")
            {
                return new List<Task>(tasks.Where(task => task.Priority == priorityFilter).ToList());
            }
            else
            {
                return tasks;
            }
        }

        public List<Task> FilterByCategory(List<Task> tasks)
        {
            ComboBoxItem selectedCategory = (ComboBoxItem)CategoryBox.SelectedItem;
            string categoryFilter = selectedCategory.Content.ToString();

            if (categoryFilter != "Все" && categoryFilter != "Архив")
            {
                return new List<Task>(tasks.Where(task => (task.Category == categoryFilter) && (task.IsCompleted == false)).ToList());
            }
            else if (categoryFilter == "Архив")
            {
                return new List<Task>(tasks.Where(task => task.IsCompleted == true).ToList());
            }
            else
            {
                return new List<Task>(tasks.Where(task => task.IsCompleted == false).ToList());
            }
        }

        public List<Task> FilterByTime(List<Task> tasks)
        {
            ComboBoxItem selectedSort = (ComboBoxItem)Deadline.SelectedItem;
            string deadlineFilter = selectedSort.Content.ToString();

            if (deadlineFilter == "Скоро")
            {
                return new List<Task>(tasks.OrderBy(task => task.Deadline).ToList());
            }
            else
            {
                return new List<Task>(tasks.OrderByDescending(task => task.Deadline).ToList());
            }
        }

        public List<Task> FilterBySearch(List<Task> tasks)
        {
            if (!string.IsNullOrEmpty(SearchBox.Text))
            {
                string substringToSearch = SearchBox.Text.ToLower();
                return new List<Task>(tasks.Where(task => task.Title.ToLower().Contains(substringToSearch) ||
                task.Description.ToLower().Contains(substringToSearch)).ToList());
            }
            else
            {
                return tasks;
            }
        }
        //Попробовать сделать лист не null, постоянные проверки это не красиво
        private void SortingBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

        private void TaskWindow_Closed(object? sender, EventArgs e)
        {
            UpdateTasksList();
        }

        private void CompleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Task selectedTask = button.DataContext as Task;

            if (selectedTask.IsCompleted == false)
            {
                selectedTask.IsCompleted = true;
            }
            else
            {
                selectedTask.IsCompleted = false;
            }

            App.Database.UpdateTask(selectedTask);
            UpdateTasksList();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Task selectedTask = button.DataContext as Task;

            MessageBoxResult messageBoxAnswer = MessageBox.Show("Вы действительно хотите удалить задачу?", "Подтверждение удаления",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageBoxAnswer == MessageBoxResult.Yes)
            {
                App.Database.DeleteTask(selectedTask.Id);
                UpdateTasksList();
            }
        }

        private void TasksList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Task selectedTask = TasksList.SelectedItem as Task;

            if (TasksList.SelectedItem != null)
            {
                Window TaskWindow = new TaskWindow(selectedTask);
                TaskWindow.Closed += TaskWindow_Closed;
                TaskWindow.ShowDialog();
            }
        }
    }
}
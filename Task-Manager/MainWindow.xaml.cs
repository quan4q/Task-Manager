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
            FilterTasks();
            TasksList.ItemsSource = Tasks;
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            Window TaskWindow = new TaskWindow();
            TaskWindow.Closed += TaskWindow_Closed;
            TaskWindow.ShowDialog();
        }

        //Применение фильтров к коллекции
        private void FilterTasks()
        {
            FilterByPriority();
            FilterByCategory();
            FilterByTime();
            FilterBySearch();
        }

        private void FilterByPriority()
        {
            ComboBoxItem selectedPriority = (ComboBoxItem)PriorityBox.SelectedItem;
            string priorityFilter = selectedPriority.Content.ToString();

            if (priorityFilter != "Все")
            {
                Tasks = Tasks.Where(task => task.Priority == priorityFilter).ToList();
            }
        }

        private void FilterByCategory()
        {
            ComboBoxItem selectedCategory = (ComboBoxItem)CategoryBox.SelectedItem;
            string categoryFilter = selectedCategory.Content.ToString();

            if (categoryFilter != "Все" && categoryFilter != "Архив")
            {
                Tasks = Tasks.Where(task => (task.Category == categoryFilter) && (task.IsCompleted == false)).ToList();
            }
            else if (categoryFilter == "Архив")
            {
                Tasks = Tasks.Where(task => task.IsCompleted == true).ToList();
            }
            else
            {
                Tasks = Tasks.Where(task => task.IsCompleted == false).ToList();
            }
        }

        private void FilterByTime()
        {
            ComboBoxItem selectedSort = (ComboBoxItem)Deadline.SelectedItem;
            string deadlineFilter = selectedSort.Content.ToString();

            if (deadlineFilter == "Скоро")
            {
                Tasks = Tasks.OrderBy(task => task.Deadline).ToList();
            }
            else
            {
                Tasks = Tasks.OrderByDescending(task => task.Deadline).ToList();
            }
        }

        private void FilterBySearch()
        {
            if (!string.IsNullOrEmpty(SearchBox.Text))
            {
                string substringToSearch = SearchBox.Text.ToLower();
                Tasks = Tasks.Where(task => task.Title.ToLower().Contains(substringToSearch) || task.Description.ToLower().Contains(substringToSearch)).ToList();
            }
        }

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
            TasksList.SelectedItem = null;
            UpdateTasksList();
        }

        private void CompleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Task selectedTask = button.DataContext as Task;

            selectedTask.IsCompleted = !selectedTask.IsCompleted;

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
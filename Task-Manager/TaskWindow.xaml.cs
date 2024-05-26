using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Task_Manager
{
    /// <summary>
    /// Логика взаимодействия для TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        private bool IsRedacted;
        private Task RedactedTask;

        public TaskWindow()
        {
            InitializeComponent();

            IsRedacted = false;
        }

        public TaskWindow(Task task)
        {
            InitializeComponent();

            TitleBox.Text = task.Title;
            DescriptionBox.Text = task.Description;
            DeadlineDTP.Value = task.Deadline;
            CategoryBox.Text = task.Category;
            PriorityBox.Text = task.Priority;

            IsRedacted = true;
            RedactedTask = task;
        }

        private void SaveTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleBox.Text) || string.IsNullOrWhiteSpace(CategoryBox.Text) ||
                string.IsNullOrWhiteSpace(PriorityBox.Text) || DeadlineDTP == null)
            {
                MessageBox.Show("ОШИБКА! Пожалуйста, заполните все необходимые поля.");
                return;
            }

            if (DeadlineDTP.Value < DateTime.Now)
            {
                MessageBox.Show("ОШИБКА! Дедлайн не может быть раньше текущий даты.");
                return;
            }

            if (!IsRedacted)
            {
                Task task = new()
                {
                    Title = TitleBox.Text,
                    Description = DescriptionBox.Text,
                    Deadline = DeadlineDTP.Value,
                    Category = CategoryBox.Text,
                    Priority = PriorityBox.Text,
                };

                App.Database.AddTask(task);
            }
            else
            {
                RedactedTask.Title = TitleBox.Text;
                RedactedTask.Description = DescriptionBox.Text;
                RedactedTask.Deadline = DeadlineDTP.Value;
                RedactedTask.Category = CategoryBox.Text;
                RedactedTask.Priority = PriorityBox.Text;

                App.Database.UpdateTask(RedactedTask);
            }

            this.Close();
        }

        private void CancelTaskButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

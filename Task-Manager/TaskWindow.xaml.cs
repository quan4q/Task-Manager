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
        public TaskWindow()
        {
            InitializeComponent();
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

            Task task = new()
            {
                Title = TitleBox.Text,
                Description = DescriptionBox.Text,
                Deadline = DeadlineDTP.Value,
                Category = CategoryBox.Text,
                Priority = PriorityBox.Text,
            };

            App.Database.AddTask(task);

            this.Close();
        }

        private void CancelTaskButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

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

        public static int PriorityToInt(string priority)
        {
            switch (priority)
            {
                case "Низкий": return 0;
                case "Средний": return 1;
                case "Высокий": return 2;
                default: return 0;
            }
        }

        private void SaveTaskButton_Click(object sender, RoutedEventArgs e)
        {
            Task task = new Task
            {
                Title = TitleBox.Text,
                Description = DescriptionBox.Text,
                Deadline = DeadlineDTP.Value,
                Category = CategoryBox.Text,
                Priority = PriorityToInt(PriorityBox.Text),
            };
            
            MainWindow.Tasks.Add(task);

            this.Close();
        }

        private void CancelTaskButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

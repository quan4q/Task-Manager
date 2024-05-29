using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Notification.Wpf;

namespace Task_Manager
{
    public class Notificator
    {
        private readonly DispatcherTimer DeadineTimer = new();
        private readonly NotificationManager NotificationManager = new();
        private readonly Dictionary<TimeSpan, string> Notifications = new()
        {
            { new TimeSpan(0, 59, 0), "Остался 1 час!" },
            { new TimeSpan(2, 59, 0), "Осталось 3 часа!" },
            { new TimeSpan(5, 59, 0), "Осталось 6 часов!" },
            { new TimeSpan(11, 59, 0), "Осталось 12 часов!" },
            { new TimeSpan(23, 59, 0), "Остался 1 день!" },
            { new TimeSpan(1, 23, 59, 0), "Осталось 2 дня!" },
        };

        public Notificator()
        {
            DeadineTimer.Interval = TimeSpan.FromMinutes(1);
            DeadineTimer.Tick += DeadineTimer_Tick;
            DeadineTimer.Start();
        }

        private void ShowNotification(string taskTitle, string timeLeft)
        {
            NotificationContent deadlineNotification = new()
            {
                Title = "ВНИМАНИЕ!",
                Message = $"Задание {taskTitle} близится к дедлайну! " + timeLeft,
                Type = NotificationType.Warning,
            };

            NotificationManager.Show(deadlineNotification);
        }

        private void DeadineTimer_Tick(object? sender, EventArgs e)
        {
            List<Task> tasksList = App.Database.GetTasks();

            foreach (Task task in tasksList)
            {
                if (task.IsCompleted == false)
                {
                    TimeSpan remainingTime = (TimeSpan)(task.Deadline - DateTime.Now);

                    foreach (KeyValuePair<TimeSpan, string> notification in Notifications)
                    {
                        if (remainingTime.Days == notification.Key.Days &&
                            remainingTime.Hours == notification.Key.Hours &&
                            remainingTime.Minutes == notification.Key.Minutes)
                        {
                            ShowNotification(task.Title, notification.Value);
                        }
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace Task_Manager
{
    public class SQLDatabase
    {
        private readonly string connectionString = "Data Source=usertasks.db";

        public SQLDatabase()
        {
            using SqliteConnection connection = new(connectionString);
            connection.Open();

            SqliteCommand command = new(@"CREATE TABLE IF NOT EXISTS tasks (
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        Title TEXT, Description TEXT, Deadline DATETIME,
                                        Category TEXT, Priority TEXT, IsCompleted INTEGER)", connection);
            command.ExecuteNonQuery();
        }

        public ObservableCollection<Task> GetTasks()
        {
            using SqliteConnection connection = new(connectionString);
            connection.Open();

            using SqliteCommand command = new("SELECT * FROM tasks", connection);
            using SqliteDataReader reader = command.ExecuteReader();

            ObservableCollection<Task> tasks = [];

            while (reader.Read())
            {
                tasks.Add(new Task()
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Description = reader.GetString(2),
                    Deadline = reader.GetDateTime(3),
                    Category = reader.GetString(4),
                    Priority = reader.GetString(5),
                    IsCompleted = reader.GetBoolean(6),
                });
            }

            return tasks;
        }

        /* TO-DO: 
            метод для добавления задачи, 
            метод для удаления задачи, 
            метод для обновления задачи,
            общий рефакторинг
        */
    }
}

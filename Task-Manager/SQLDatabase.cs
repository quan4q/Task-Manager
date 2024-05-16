﻿using System;
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

        public void AddTask(Task task)
        {
            using SqliteConnection connection = new(connectionString);
            connection.Open();

            string formattedDeadline = task.Deadline.Value.ToString("yyyy-MM-dd HH:mm:ss");

            using SqliteCommand command = new($@"INSERT INTO tasks (
                                    Title, Description, Deadline,
                                    Category, Priority, IsCompleted) VALUES (
                                    '{task.Title}', '{task.Description}',
                                    '{formattedDeadline}', '{task.Category}', '{task.Priority}',
                                    '{task.IsCompleted}')", connection);

            command.ExecuteNonQuery();
        }

        public void DeleteTask(int taskId)
        {
            using SqliteConnection connection = new(connectionString);
            connection.Open();

            using SqliteCommand command = new($"DELETE FROM tasks WHERE Id='{taskId}'", connection);
            command.ExecuteNonQuery();
        }

        public void UpdateTask(Task task)
        {
            using SqliteConnection connection = new(connectionString);
            connection.Open();

            string formattedDeadline = task.Deadline.Value.ToString("yyyy-MM-dd HH:mm:ss");

            using SqliteCommand command = new($@"UPDATE tasks SET
                                                Title='{task.Title}', Description='{task.Description}',
                                                Deadline='{formattedDeadline}', Category='{task.Category}',
                                                Priority='{task.Priority}', IsCompleted='{task.IsCompleted}'
                                                WHERE Id='{task.Id}'", connection);

            command.ExecuteNonQuery();
        }

        /* TO-DO:
            общий рефакторинг
        */
    }
}

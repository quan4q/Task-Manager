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
        private readonly string ConnectionString = "Data Source=usertasks.db";
        private readonly List<Task> CachedTasks = [];
        private bool IsCached = false;

        public SQLDatabase() 
        {
            InitializeDatabase();
        }

        private SqliteConnection GetConnection()
        {
            SqliteConnection connection = new(ConnectionString);
            connection.Open();
            return connection;
        }

        private void InitializeDatabase()
        {
            using SqliteConnection connection = GetConnection();

            SqliteCommand command = new(@"CREATE TABLE IF NOT EXISTS tasks (
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        Title TEXT, Description TEXT, Deadline DATETIME,
                                        Category TEXT, Priority TEXT, IsCompleted INTEGER)", connection);
            command.ExecuteNonQuery();
        }

        public List<Task> GetTasks()
        {
            if (IsCached) 
            {
                return CachedTasks;
            }
            
            UpdateCache();

            return CachedTasks;
        }

        public void AddTask(Task task)
        {
            using SqliteConnection connection = GetConnection();
            string formattedDeadline = task.Deadline.ToString("yyyy-MM-dd HH:mm:ss");

            using SqliteCommand command = new($@"INSERT INTO tasks (
                                                Title, Description, Deadline, Category,
                                                Priority, IsCompleted) VALUES (
                                                '{task.Title}', '{task.Description}',
                                                '{formattedDeadline}', '{task.Category}', '{task.Priority}',
                                                {task.IsCompleted})", connection);

            command.ExecuteNonQuery();
            IsCached = false;
        }

        public void DeleteTask(int taskId)
        {
            using SqliteConnection connection = GetConnection();
            using SqliteCommand command = new($"DELETE FROM tasks WHERE Id={taskId}", connection);

            command.ExecuteNonQuery();
            IsCached = false;
        }

        public void UpdateTask(Task task)
        {
            using SqliteConnection connection = GetConnection();
            string formattedDeadline = task.Deadline.ToString("yyyy-MM-dd HH:mm:ss");

            using SqliteCommand command = new($@"UPDATE tasks SET
                                                Title='{task.Title}', Description='{task.Description}',
                                                Deadline='{formattedDeadline}', Category='{task.Category}',
                                                Priority='{task.Priority}', IsCompleted={task.IsCompleted}
                                                WHERE Id={task.Id}", connection);

            command.ExecuteNonQuery();
            IsCached = false;
        }

        private void UpdateCache() 
        {
            using SqliteConnection connection = GetConnection();
            using SqliteCommand command = new("SELECT * FROM tasks", connection);
            using SqliteDataReader reader = command.ExecuteReader();

            CachedTasks.Clear();

            while (reader.Read())
            {
                CachedTasks.Add(new Task()
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

            IsCached = true;
        }
    }
}

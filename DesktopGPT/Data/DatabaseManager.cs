using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Windows;

namespace DesktopGPT.Data
{
    public class DatabaseManager
    {
        private static readonly object _lock = new object();

        public static void InitializeDatabase()
        {
            try
            {
                string scriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/SQLiteScripts/setup.sql");
                ExecuteScript(scriptPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initilizing database: {ex.Message}");
            }
                
        }

        private static void ExecuteScript(string scriptPath)
        {
            try
            {
                if (!File.Exists(scriptPath))
                {
                    throw new FileNotFoundException($"SQL script not found: {scriptPath}");
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Error executing script: {ex.Message}");
            }

            // Open connection
            SQLiteConnection connection = new SQLiteConnection("Data Source=desktop_gpt.db;Version=3;");

            try
            {
                // Execute Script
                var script = File.ReadAllText(scriptPath);
                using (var command = new SQLiteCommand(script, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show($"Error executing script: {ex.Message}");
            }
        }

        public static void ExecuteNonQuery(string query)
        {
            // Open connection
            SQLiteConnection connection = new SQLiteConnection("Data Source=desktop_gpt.db;Version=3;");
            connection.Open();

            // Create SQL Command
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = query;

            // Execute Command
            command.ExecuteNonQuery();

            // Close connection
            connection.Close();
        }

        public static List<Dictionary<string, object>> ExecuteReader(string query)
        {
            // Open connection
            SQLiteConnection connection = new SQLiteConnection("Data Source=desktop_gpt.db;Version=3;");
            connection.Open();

            // Create SQL Command
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = query;

            // Execute Command and Read Response
            SQLiteDataReader reader = command.ExecuteReader();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            while (reader.Read())
            {
                Dictionary<string, object> row = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string columnName = reader.GetName(i);
                    object columnValue = reader.GetValue(i);
                    row[columnName] = columnValue;
                }
                rows.Add(row);
            }
            reader.Close();

            // Close connection
            connection.Close();

            return rows;
        }
    }
}

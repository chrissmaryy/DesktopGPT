using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopGPT.Data
{
    public class UserRepository
    {

        public static void InsertUserInfo(string api_key, string shortcut_key, string shortcut_modifiers, double temperature)
        {
            string query = @$"
                INSERT INTO User_Info
                (
                    api_key,
                    temperature,
                    shortcut_key,
                    shortcut_modifiers
                )
                VALUES
                (
                    @api_key,
                    @temperature,
                    @shortcut_key,
                    @shortcut_modifiers
                );
            ";

            var parameters = new Dictionary<string, object>
            {
                { "@api_key", api_key },
                { "@temperature", temperature },
                { "@shortcut_key", shortcut_key },
                { "@shortcut_modifiers", shortcut_modifiers }
            };

            DatabaseManager.ExecuteNonQuery(query, parameters);
        }

        public static void UpdateUserInfo(string api_key, float temperature, string shortcut_key, string shortcut_modifiers)
        {
            string query = @$"
                UPDATE User_Info
                SET
                    api_key = @api_key,
                    temperature = @temperature,
                    shortcut_key = @shortcut_key,
                    shortcut_modifiers = @shortcut_modifiers
                WHERE user_id = @user_id;
            ";

            var parameters = new Dictionary<string, object>
            {
                { "@api_key", api_key },
                { "@temperature", temperature },
                { "@shortcut_key", shortcut_key },
                { "@shortcut_modifiers", shortcut_modifiers },
                { "@user_id", 1 }
            };

            DatabaseManager.ExecuteNonQuery(query, parameters);
        }

        public static (string Key, string Modifiers) LoadShortcut()
        {
            var rows = DatabaseManager.ExecuteReader("SELECT shortcut_key, shortcut_modifiers FROM User_Info LIMIT 1;");

            // Check if any data is available
            if (!rows.Any())
            {
                // Return default shortcut if no data found
                return ("O", "Control + Shift");
            }

            var row = rows[0];
            return ((string)row["shortcut_key"], (string)row["shortcut_modifiers"]);
        }
    }
}

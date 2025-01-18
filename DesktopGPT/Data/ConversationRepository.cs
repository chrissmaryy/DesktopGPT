using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopGPT.Data
{
    public class ConversationRepository
    {
        public static void InsertChat(string chat_name, SQLiteConnection connection)
        {
            string query = @$"
                INSERT INTO Chats
                (
                    chat_name
                )
                VALUES
                (
                    '{chat_name}'
                );
            ";

            DatabaseManager.ExecuteNonQuery(query);
        }
    }
}

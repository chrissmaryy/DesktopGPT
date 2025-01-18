using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SQLite;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace DesktopGPT.Data
{
    public class MessageRepository
    {

        public static void InsertMessage(int message_id, int chat_id, string model, string role, string content, SQLiteConnection connection)
        {
            string query = @$"
                INSERT INTO Messages
                (
                    message_id,
                    chat_id,
                    model,
                    role,
                    content
                )
                VALUES
                (
                    '{message_id}',
                    '{chat_id}',
                    '{model}',
                    '{role}',
                    '{content}'
                );
            ";

            DatabaseManager.ExecuteNonQuery(query);
        }
    }
}

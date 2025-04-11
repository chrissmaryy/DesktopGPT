using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopGPT.Data
{
    public class ChatRepository
    {
        public static void InsertChat(string chat_name)
        {
            string query = @$"
                INSERT INTO Chats
                (
                    chat_name
                )
                VALUES
                (
                    @chat_name
                );
            ";

            var parameters = new Dictionary<string, object>
            {
                { "@chat_name", chat_name }
            };

            DatabaseManager.ExecuteNonQuery(query, parameters);
        }

        public static void UpdateChat(string chat_name, int chat_id)
        {
            string query = @$"
                UPDATE Chats
                SET
                    chat_name = @chat_name
                WHERE chat_id = @chat_id;
            ";

            var parameters = new Dictionary<string, object>
            {
                { "@chat_name", chat_name },
                { "@chat_id", chat_id }
            };

            DatabaseManager.ExecuteNonQuery(query, parameters);
        }

        public static List<Dictionary<string, object>> GetChatList()
        {
            string query = @$"
                SELECT chat_id, chat_name
                FROM Chats
            ";

            List<Dictionary<string, object>> chat_list = DatabaseManager.ExecuteReader(query);

            return chat_list;
        }

        public static void DeleteChat(int chat_id)
        {
            MessageRepository.DeleteMessages(chat_id);

            string query = @$"
                DELETE FROM Chats
                WHERE chat_id = {chat_id};
            ";

            DatabaseManager.ExecuteNonQuery(query);
        }
    }
}

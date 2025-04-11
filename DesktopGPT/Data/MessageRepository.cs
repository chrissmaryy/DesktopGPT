using System;
using System.Collections.Generic;
using System.Data;
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

        public static void InsertMessage(int chat_id, string model, string role, string content, DateTime timestamp)
        {
            string query = @$"
                INSERT INTO Messages
                (
                    chat_id,
                    model,
                    role,
                    content,
                    timestamp
                )
                VALUES
                (
                    @message_id,
                    @chat_id,
                    @model,
                    @role,
                    @content,
                    @timestamp
                );
            ";

            var parameters = new Dictionary<string, object>
            {
                { "@chat_id", chat_id },
                { "@model", model },
                { "@role", role },
                { "@content", content },
                { "@timestamp", timestamp }
            };

            DatabaseManager.ExecuteNonQuery(query, parameters);
        }

        public static List<Dictionary<string, object>> GetMessages(int chat_id)
        {
            string query = @$"
                SELECT role, content
                FROM Messages
                WHERE chat_id = {chat_id};
            ";

            List<Dictionary<string, object>> messages_list = DatabaseManager.ExecuteReader(query);

            return messages_list;
        }

        public static void DeleteMessages(int chat_id)
        {
            string query = @$"
                DELETE FROM Messages
                WHERE chat_id = {chat_id};
            ";

            DatabaseManager.ExecuteNonQuery(query);
        }


        public static List<Dictionary<string, object>> GetTokenInfo(string model)
        {
            string query = @$"
                SELECT max_tokens, token_buffer
                FROM Token_Info
                WHERE model = {model};
            ";

            List<Dictionary<string, object>> token_info = DatabaseManager.ExecuteReader(query);

            return token_info;
        }
    }
}

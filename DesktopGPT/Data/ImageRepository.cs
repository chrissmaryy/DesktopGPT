using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace DesktopGPT.Data
{
    public class ImageRepository
    {
        public static void InsertImageRequest(int chat_id, string prompt, string size, string quality, int n, Blob image_blob, string request_type, SQLiteConnection connection)
        {
            string query = @$"
                INSERT INTO Image_Requests
                (
                    chat_id,
                    prompt,
                    size,
                    quality,
                    n,
                    image_blob,
                    request_type
                )
                VALUES
                (
                    '{chat_id}',
                    '{prompt}',
                    '{size}',
                    '{quality}',
                    '{n}',
                    '{image_blob}',
                    '{request_type}'
                );
            ";

            DatabaseManager.ExecuteNonQuery(query);
        }

        public static void InsertImage(int image_id, int image_request_id, string image_b64, SQLiteConnection connection)
        {
            string query = @$"
                INSERT INTO Images
                (
                    image_id,
                    image_request_id,
                    image_b64
                )
                VALUES
                (
                    '{image_id}',
                    '{image_request_id}',
                    '{image_b64}'
                );
            ";

            DatabaseManager.ExecuteNonQuery(query);
        }
    }
}

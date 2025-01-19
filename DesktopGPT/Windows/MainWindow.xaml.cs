
using System.Data.SQLite;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using DesktopGPT.Data;
using System.Diagnostics;
using DesktopGPT.Windows;

namespace DesktopGPT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            if (IsUserInfoEmpty())
            {
                UserInfoWindow userInfoWindow = new UserInfoWindow();

                // Show the dialog
                bool? result = userInfoWindow.ShowDialog();

                // If the user closes the window or cancels input, shut down the application
                if (result != true) // `true` means user saved data successfully
                {
                    Application.Current.Shutdown();
                    return; // Prevent further execution
                }
            }
        }

        public bool IsUserInfoEmpty()
        {
            string connectionString = "Data Source=desktop_gpt.db;Version=3;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                string countQuery = "SELECT COUNT(*) FROM User_Info;";

                using (SQLiteCommand command = new SQLiteCommand(countQuery, connection))
                {
                    long count = (long)command.ExecuteScalar();
                    return count == 0;
                }
            }
        }
    }
}
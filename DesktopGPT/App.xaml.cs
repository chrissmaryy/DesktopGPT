using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Windows;
using System.Windows.Input;
using DesktopGPT.Data;
using DesktopGPT.Windows;
using GlobalHotKey;

namespace DesktopGPT
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private HotKeyManager? _hotKeyManager;
        private HotKey? _currentHotKey;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                DatabaseManager.InitializeDatabase();
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Error initilizing database: {ex.Message}");
            }

            _hotKeyManager = new HotKeyManager();
            RegisterHotKey();
        }

        private void RegisterHotKey()
        {
            if (_currentHotKey != null)
            {
                _hotKeyManager.Unregister(_currentHotKey);
                _currentHotKey = null;
            }

            // Load shortcut from the database
            var shortcut = UserRepository.LoadShortcut();
            var key = (Key)Enum.Parse(typeof(Key), shortcut.Key);
            // Parse modifiers
            var modifiers = ModifierKeys.None;
            foreach (var modifier in shortcut.Modifiers.Split(new[] { '+', ' ' }, StringSplitOptions.RemoveEmptyEntries))
            {
                modifiers |= (ModifierKeys)Enum.Parse(typeof(ModifierKeys), modifier);
            }

            // Register the hotkey
            _hotKeyManager.Register(key, modifiers);
            _hotKeyManager.KeyPressed += OnHotKeyPressed;
        }

        private void OnHotKeyPressed(object sender, KeyPressedEventArgs e)
        {
            // Open the WPF Window
            var mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}

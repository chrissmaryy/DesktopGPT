using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DesktopGPT.Data;

namespace DesktopGPT.Windows
{
    /// <summary>
    /// Interaction logic for UserInfoWindow.xaml
    /// </summary>
    public partial class UserInfoWindow : Window
    {
        private Key _key; // Stores the key pressed
        private ModifierKeys _modifiers; // Stores the modifiers (Ctrl, Shift, etc.)

        public UserInfoWindow()
        {
            InitializeComponent();

            // Load existing shortcut
            var shortcut = UserRepository.LoadShortcut();
            ShortcutInput.Text = $"{shortcut.Modifiers} + {shortcut.Key}";
        }

        private void ShortcutInput_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Capture the key and modifiers
            _key = e.Key == Key.System ? e.SystemKey : e.Key; // Handle Alt key correctly
            _modifiers = Keyboard.Modifiers;

            // Display the combo in the TextBox
            var shortcutText = new StringBuilder();
            if (_modifiers.HasFlag(ModifierKeys.Control)) shortcutText.Append("Control + ");
            if (_modifiers.HasFlag(ModifierKeys.Shift)) shortcutText.Append("Shift + ");
            if (_modifiers.HasFlag(ModifierKeys.Alt)) shortcutText.Append("Alt + ");
            shortcutText.Append(_key.ToString());

            ShortcutInput.Text = shortcutText.ToString();

            // Prevent default behavior
            e.Handled = true;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string api_key = APIKey_Input.Password;
            if (_key == Key.None && _modifiers == ModifierKeys.None)
            {
                _modifiers = ModifierKeys.Control | ModifierKeys.Shift;
                _key = Key.O;
            }

            UserRepository.InsertUserInfo(api_key, _key.ToString(), _modifiers.ToString().Replace(",", "+"), 1.0);

            this.DialogResult = true;
            this.Close();
        }
    }
}

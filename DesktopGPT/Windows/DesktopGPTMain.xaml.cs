using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.IO;
using DesktopGPT.Data;


namespace DesktopGPT
{
    /// <summary>
    /// Interaction logic for DesktopGPTMain.xaml
    /// </summary>
    public partial class DesktopGPTMain : Page
    {
        public DesktopGPTMain ()
        {
            InitializeComponent();
            SetupComboBoxes();
        }

        private void SetupComboBoxes()
        {
            Chats.ItemsSource = new List<string> { "Convo 1", "Convo 2", "Convo 3", "Convo 4" };

            var models = new List<string>
            {
                "gpt-4o", "gpt-4o-mini", "o1", "o1-mini", "gpt-4-turbo", "gpt-4", "gpt-3.5-turbo"
            };

            var image_models = new List<string>
            {
                "No Image Generation", "dall-e-2", "dall-e-3"
            };

            // Set the ComboBox ItemsSource to the list of models
            ModelComboBox.ItemsSource = models;
            ImageComboBox.ItemsSource = image_models;

            // Optionally, set a default selected item
            ModelComboBox.SelectedIndex = 0; // Select the first item by default
            ImageComboBox.SelectedIndex = 0;
        }

        private void Chats_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Chats.SelectedItem is string selectedItem)
            {
                
            }
        }

        private void PencilIcon_Click(object sender, RoutedEventArgs e)
        {
            // Add logic for when the pencil icon is clicked
            // Get the parent ListBoxItem
            var button = sender as Button;
            var parentListBoxItem = button?.DataContext;

            MessageBox.Show($"Edit clicked for item: {parentListBoxItem}");
        }

        private void Models_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox? comboBox = sender as ComboBox;
            if (comboBox?.SelectedItem != null)
            {
                string? selectedItem = comboBox.SelectedItem.ToString();
            }
        }

        private void ImageModels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox? comboBox = sender as ComboBox;
            if (comboBox?.SelectedItem != null)
            {
                string? selectedItem = comboBox.SelectedItem.ToString();
            }
        }

        private void UserMessage_KeyDown(object sender, KeyEventArgs e)
        {
            // If the Enter key is pressed
            if (e.Key == Key.Enter)
            {
                // If Shift is NOT pressed (i.e., the user didn't want a new line)
                if (!Keyboard.IsKeyDown(Key.LeftShift) && !Keyboard.IsKeyDown(Key.RightShift))
                {
                    string message = UserChatBox.Text.Trim();

                    // If there's a message (non-empty), process it
                    if (!string.IsNullOrEmpty(message) && message != "Enter your message...")
                    {
                        // Process the message (e.g., display or send it)
                        AddChatBubble(message, isUser: true);

                        // Clear the TextBox after sending the message
                        UserChatBox.Clear();

                        // Optionally, set focus back to the TextBox for convenience
                        UserChatBox.Focus();
                    }

                    // Mark the event as handled to prevent the default behavior (inserting a new line)
                    e.Handled = true;
                }
            }
        }

        private void AddChat_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("New Chat added");
        }

        private void AddChatBubble(string message, bool isUser)
        {
            // Define custom colors
            string userBubbleColor = "#616166";  // User bubble color
            string botBubbleColor = "#303034";   // Bot bubble color

            // Convert hex to Brush
            BrushConverter brushConverter = new BrushConverter();
            Brush? backgroundColor = brushConverter.ConvertFromString(isUser ? userBubbleColor : botBubbleColor) as Brush;
            
            // Create a TextBlock for the chat message
            TextBlock chatText = new TextBlock
            {
                Text = message,
                FontSize = 16,
                Padding = new Thickness(10),
                Margin = new Thickness(0,5,30,5),
                TextWrapping = TextWrapping.Wrap,
                Background = backgroundColor,
                Foreground = Brushes.White,
                HorizontalAlignment = isUser ? HorizontalAlignment.Right : HorizontalAlignment.Left,
                MaxWidth = 300,
            };

            Border chatBubble = new Border
            {
                Background = backgroundColor,
                CornerRadius = new CornerRadius(10),
                Child = chatText,
                Margin = isUser ? new Thickness(30, 10, 30, 5) : new Thickness(0, 10, 30, 5),
                HorizontalAlignment = isUser ? HorizontalAlignment.Right : HorizontalAlignment.Left,
            };

            // Add the TextBlock to the chat panel
            ChatPanel.Children.Add(chatBubble);

            // Auto-scroll to the bottom of the chat
            ScrollToBottom();
        }

        private void ScrollToBottom()
        {
            if (ChatPanel.Parent is ScrollViewer scrollViewer)
            {
                scrollViewer.ScrollToEnd();
            }
        }

        private void UserChatBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (UserChatBox.Text == "Enter your message...")
            {
                UserChatBox.Clear();
                UserChatBox.Foreground = new SolidColorBrush(Colors.White); // Set text color back to black
            }
        }

        private void UserChatBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(UserChatBox.Text))
            {
                UserChatBox.Text = "Enter your message..."; // Placeholder text when lost focus
                UserChatBox.Foreground = new SolidColorBrush(Colors.LightGray); // Placeholder color
            }
        }

        private void UserChatBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Page_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBox? textBox = Keyboard.FocusedElement as TextBox;
            if (textBox != null)
            {
                TraversalRequest tRequest = new TraversalRequest(FocusNavigationDirection.Next);
                textBox.MoveFocus(tRequest);
            }
        }

        private void UserInfoButton_Click(object sender, RoutedEventArgs e)
        {
            
            // Toggle the visibility of the popup
            UserInfo.IsOpen = !UserInfo.IsOpen;
            
        }

        private void SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve user input from the textboxes
            string api_key = APIKey_Input.Password;
            string temp = Temperature_Input.Text;
            string key_input = KeyTextBox_Input.Text;
            string modifiers_input = Modifiers_Input.Text;

            // Handle the input (e.g., save to configuration or update UI)
            MessageBox.Show($"API Key: {api_key}\nTemperature: {temp}\nKey Input: {key_input}\nModifiers Input: {modifiers_input}", "Settings Saved");

            // Close the popup
            UserInfo.IsOpen = false;
        }

        private void Temperature_Input_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Regular expression to allow digits, a single decimal point, and optional negative sign
            var regex = new Regex(@"^-?\d*\.?\d*$");
            e.Handled = !regex.IsMatch(((TextBox)sender).Text.Insert(((TextBox)sender).SelectionStart, e.Text));
        }

        // Handle invalid states (e.g., multiple dots, empty input)
        private void Temperature_Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;

            if (string.IsNullOrWhiteSpace(textBox.Text)) return;

            if (!double.TryParse(textBox.Text, out _))
            {
                MessageBox.Show("Invalid input. Please enter a valid number.");
                textBox.Text = string.Empty;
            }
        }

    }
}

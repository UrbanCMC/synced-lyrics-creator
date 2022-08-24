using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace SyncedLyricsCreator.Controls
{
    /// <summary>
    /// Interaction logic for MessageBox.xaml
    /// </summary>
    public partial class MessageBox : Window
    {
        private int pressedButton = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageBox"/> class.
        /// </summary>
        public MessageBox() => InitializeComponent();

        /// <summary>
        /// Handles the Click event for the first button
        /// </summary>
        /// <param name="sender">The event sender</param>
        /// <param name="e">The event args</param>
        public void FirstButton_Click(object sender, RoutedEventArgs e)
        {
            pressedButton = 1;

            Close();
        }

        /// <summary>
        /// Gets a value indicating the number of the button that was pressed on the dialog
        /// </summary>
        /// <returns>
        /// An integer, specifying the number of the button that was pressed, or -1 if no button was pressed
        /// </returns>
        public int GetPressedButton() => pressedButton;

        /// <summary>
        /// Handles the Click event for the second button
        /// </summary>
        /// <param name="sender">The event sender</param>
        /// <param name="e">The event args</param>
        public void SecondButton_Click(object sender, RoutedEventArgs e)
        {
            pressedButton = 2;

            Close();
        }

        /// <summary>
        /// Handles the Click event for the third button
        /// </summary>
        /// <param name="sender">The event sender</param>
        /// <param name="e">The event args</param>
        public void ThirdButton_Click(object sender, RoutedEventArgs e)
        {
            pressedButton = 3;

            Close();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            FirstButton = this.FindControl<Button>("FirstButton");
            Image = this.FindControl<Image>("Image");
            MessageTextBlock = this.FindControl<TextBlock>("MessageTextBlock");
            SecondButton = this.FindControl<Button>("SecondButton");
            ThirdButton = this.FindControl<Button>("ThirdButton");
        }
    }
}

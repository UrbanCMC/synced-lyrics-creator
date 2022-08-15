using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using SyncedLyricsCreator.Helpers.Dialog.MessageBox;

namespace SyncedLyricsCreator.Controls
{
    /// <summary>
    /// Custom message box implementation that shows a WPF window instead of the default from Windows Forms
    /// </summary>
    internal class CustomMessageBox
    {
        private readonly MessageBox messageBox;
        private readonly MessageBoxSettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomMessageBox"/> class.
        /// </summary>
        /// <param name="settings">The settings for the message box</param>
        public CustomMessageBox(MessageBoxSettings settings)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));

            messageBox = new MessageBox();

            SetUpButtons();
            SetUpIcon();
            SetUpText();
        }

        /// <summary>
        /// Shows the message box
        /// </summary>
        /// <param name="owner">The window the message box should be shown for</param>
        /// <returns>A <see cref="MessageBoxResult"/> specifying the result selected by the user</returns>
        public async Task<MessageBoxResult> Show(Window owner)
        {
            await messageBox.ShowDialog(owner);
            return GetMessageBoxResult(messageBox.GetPressedButton());
        }

        private static Bitmap GetSystemIconImageSource(System.Drawing.Icon icon)
        {
            using var stream = new MemoryStream();
            icon.Save(stream);
            stream.Seek(0, SeekOrigin.Begin);

            return new Bitmap(stream);
        }

        private MessageBoxResult GetMessageBoxResult(int messageBoxButton)
        {
            if (messageBoxButton == -1)
            {
                // Handle no pressed button the same as the user canceling via ESC
                if (messageBox.FirstButton.IsCancel)
                {
                    messageBoxButton = 1;
                }
                else if (messageBox.SecondButton.IsCancel)
                {
                    messageBoxButton = 2;
                }
                else
                {
                    messageBoxButton = 3;
                }
            }

            switch (settings.Button)
            {
                case MessageBoxButton.OK:
                    if (messageBoxButton == 1)
                    {
                        return MessageBoxResult.OK;
                    }

                    throw new InvalidOperationException("Invalid button pressed in message box");

                case MessageBoxButton.OKCancel:
                    return messageBoxButton switch
                    {
                        1 => MessageBoxResult.OK
                        , 2 => MessageBoxResult.Cancel
                        , _ => throw new InvalidOperationException("Invalid button pressed in message box")
                    };

                case MessageBoxButton.YesNoCancel:
                    return messageBoxButton switch
                    {
                        1 => MessageBoxResult.Yes
                        , 2 => MessageBoxResult.No
                        , _ => MessageBoxResult.Cancel
                    };

                case MessageBoxButton.YesNo:
                    return messageBoxButton switch
                    {
                        1 => MessageBoxResult.Yes
                        , 2 => MessageBoxResult.No
                        , _ => throw new InvalidOperationException("Invalid button pressed in message box")
                    };

                default:
                    throw new InvalidOperationException("This case should never be hit. Something went very wrong.");
            }
        }

        private void SetUpButtons()
        {
            // Set available buttons
            switch (settings.Button)
            {
                case MessageBoxButton.OK:
                    messageBox.FirstButton.Content = settings.ButtonOkText;
                    messageBox.FirstButton.IsCancel = true;

                    messageBox.SecondButton.IsVisible = false;
                    messageBox.ThirdButton.IsVisible = false;
                    break;

                case MessageBoxButton.OKCancel:
                    messageBox.FirstButton.Content = settings.ButtonOkText;
                    messageBox.SecondButton.Content = settings.ButtonCancelText;
                    messageBox.SecondButton.IsCancel = true;

                    messageBox.ThirdButton.IsVisible = false;
                    break;

                case MessageBoxButton.YesNoCancel:
                    messageBox.FirstButton.Content = settings.ButtonYesText;
                    messageBox.SecondButton.Content = settings.ButtonNoText;
                    messageBox.ThirdButton.Content = settings.ButtonCancelText;
                    messageBox.ThirdButton.IsCancel = true;
                    break;

                case MessageBoxButton.YesNo:
                    messageBox.FirstButton.Content = settings.ButtonYesText;
                    messageBox.SecondButton.Content = settings.ButtonNoText;
                    messageBox.SecondButton.IsCancel = true;

                    messageBox.ThirdButton.IsVisible = false;
                    break;
            }

            // Set default button
            switch (settings.DefaultResult)
            {
                case MessageBoxResult.OK:
                    if (settings.Button != MessageBoxButton.OK && settings.Button != MessageBoxButton.OKCancel)
                    {
                        throw new ArgumentException(
                            "The specified default result doesn't match one of the available buttons"
                            , nameof(settings));
                    }

                    messageBox.FirstButton.IsDefault = true;
                    break;

                case MessageBoxResult.Cancel:
                    switch (settings.Button)
                    {
                        case MessageBoxButton.OKCancel:
                            messageBox.SecondButton.IsDefault = true;
                            break;
                        case MessageBoxButton.YesNoCancel:
                            messageBox.ThirdButton.IsDefault = true;
                            break;
                        default:
                            throw new ArgumentException(
                                "The specified default result doesn't match one of the available buttons"
                                , nameof(settings));
                    }

                    break;

                case MessageBoxResult.Yes:
                    if (settings.Button != MessageBoxButton.YesNo && settings.Button != MessageBoxButton.YesNoCancel)
                    {
                        throw new ArgumentException(
                            "The specified default result doesn't match one of the available buttons"
                            , nameof(settings));
                    }

                    messageBox.FirstButton.IsDefault = true;
                    break;

                case MessageBoxResult.No:
                    if (settings.Button != MessageBoxButton.YesNo && settings.Button != MessageBoxButton.YesNoCancel)
                    {
                        throw new ArgumentException(
                            "The specified default result doesn't match one of the available buttons"
                            , nameof(settings));
                    }

                    messageBox.SecondButton.IsDefault = true;
                    break;

                case MessageBoxResult.None:
                default:
                    break;
            }
        }

        private void SetUpIcon()
        {
            switch (settings.Icon)
            {
                case MessageBoxImage.Error:
                    messageBox.Image.Source = GetSystemIconImageSource(System.Drawing.SystemIcons.Error);
                    break;

                case MessageBoxImage.Question:
                    messageBox.Image.Source = GetSystemIconImageSource(System.Drawing.SystemIcons.Question);
                    break;

                case MessageBoxImage.Warning:
                    messageBox.Image.Source = GetSystemIconImageSource(System.Drawing.SystemIcons.Warning);
                    break;

                case MessageBoxImage.Information:
                    messageBox.Image.Source = GetSystemIconImageSource(System.Drawing.SystemIcons.Information);
                    break;

                case MessageBoxImage.None:
                default:
                    messageBox.Image.IsVisible = false;
                    break;
            }
        }

        private void SetUpText()
        {
            messageBox.Title = settings.Caption;
            messageBox.MessageTextBlock.Text = settings.MessageBoxText;
        }
    }
}

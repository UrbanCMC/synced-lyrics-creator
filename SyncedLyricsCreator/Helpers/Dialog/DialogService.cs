using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using SyncedLyricsCreator.Controls;
using SyncedLyricsCreator.Helpers.Dialog.MessageBox;
using SyncedLyricsCreator.ViewModels;
using Application = Avalonia.Application;
using Window = Avalonia.Controls.Window;

namespace SyncedLyricsCreator.Helpers.Dialog
{
    /// <summary>
    /// Helper service that manages showing custom dialog windows based on their view models
    /// </summary>
    internal static class DialogService
    {
        private static Window Owner => (Application.Current!.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)!.MainWindow;

        /// <summary>
        /// Shows a dialog
        /// </summary>
        /// <param name="dialogViewModel">The view model of the dialog</param>
        public static void Show(ViewModelBase dialogViewModel)
        {
            var dialog = BuildWindow(dialogViewModel);
            dialog.Show(Owner);
        }

        /// <summary>
        /// Shows a modal-dialog
        /// </summary>
        /// <param name="dialogViewModel">The view model of the dialog</param>
        /// <returns>The result of the dialog</returns>
        public static async Task<bool?> ShowDialog(IModalDialogViewModel dialogViewModel)
        {
            var dialog = BuildWindow(dialogViewModel as ViewModelBase);
            dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            await dialog.ShowDialog(Owner);

            return dialogViewModel.DialogResult;
        }

        /// <summary>
        /// Shows a message box
        /// </summary>
        /// <param name="text">The window text</param>
        /// <param name="caption">The window caption</param>
        /// <param name="buttons">The buttons to show</param>
        /// <param name="icon">The icon to display</param>
        /// <param name="defaultResult">The default result to select</param>
        /// <param name="buttonCancelText">The text to override the Cancel button text with</param>
        /// <param name="buttonNoText">The text to override the No button text with</param>
        /// <param name="buttonOkText">The text to override the Ok button text with</param>
        /// <param name="buttonYesText">The text to override the Yes button text with</param>
        /// <returns>The result of the message box</returns>
        public static async Task<MessageBoxResult> ShowMessageBox(
            string text
            , string caption = ""
            , MessageBoxButton buttons = MessageBoxButton.OK
            , MessageBoxImage icon = MessageBoxImage.None
            , MessageBoxResult defaultResult = MessageBoxResult.None
            , string buttonCancelText = ""
            , string buttonNoText = ""
            , string buttonOkText = ""
            , string buttonYesText = "") => await ShowMessageBox(
            new MessageBoxSettings
            {
                Button = buttons
                , ButtonCancelText = buttonCancelText
                , ButtonNoText = buttonNoText
                , ButtonOkText = buttonOkText
                , ButtonYesText = buttonYesText
                , Caption = caption
                , DefaultResult = defaultResult
                , Icon = icon
                , MessageBoxText = text
            });

        /// <summary>
        /// Shows a message box
        /// </summary>
        /// <param name="settings">The settings to build the message box from</param>
        /// <returns>The result of the message box</returns>
        public static async Task<MessageBoxResult> ShowMessageBox(MessageBoxSettings settings)
        {
            var messageBox = new CustomMessageBox(settings);
            return await messageBox.Show(Owner);
        }

        private static Window BuildWindow(ViewModelBase? viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            var name = viewModel.GetType().FullName?.Replace("ViewModel", "View");
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Failed to get a type name for the specified view model.", nameof(viewModel));
            }

            var type = Type.GetType(name);
            if (type == null)
            {
                throw new InvalidOperationException($"Failed to find type '{name}' to instantiate.");
            }

            if (Activator.CreateInstance(type) is not Window window)
            {
                throw new InvalidOperationException($"The view of type '{name}' is not of type Window.");
            }

            window.DataContext = viewModel;

            return window;
        }
    }
}

using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using SyncedLyricsCreator.ViewModels;

namespace SyncedLyricsCreator
{
    /// <summary>
    /// Locates the view class matching a specified view model
    /// </summary>
    public class ViewLocator : IDataTemplate
    {
        /// <inheritdoc/>
        public Control Build(object? data)
        {
            if (data == null)
            {
                return new TextBlock { Text = "No data" };
            }

            var name = data.GetType().FullName!.Replace("ViewModel", "View");
            var type = Type.GetType(name);

            if (type != null)
            {
                return (Control)Activator.CreateInstance(type)!;
            }

            return new TextBlock { Text = "Not Found: " + name };
        }

        /// <inheritdoc/>
        public bool Match(object? data) => data is ViewModelBase;
    }
}

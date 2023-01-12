using System.Reactive;
using Avalonia;
using Avalonia.Controls;

namespace SyncedLyricsCreator.Behaviors;

/// <summary>
/// Static class containing attached properties for <see cref="Window"/> objects
/// </summary>
public static class WindowProperties
{
    /// <summary>
    /// Gets the dependency property for the <c>ForceClose</c> property
    /// </summary>
    public static readonly AttachedProperty<bool> ForceCloseProperty
        = AvaloniaProperty.RegisterAttached<Window, Window, bool>("ForceClose", defaultValue: false);

    static WindowProperties() => ForceCloseProperty.Changed.Subscribe(new AnonymousObserver<AvaloniaPropertyChangedEventArgs>(ForceClosePropertyChanged));

    /// <summary>
    /// Gets the value of the <c>ForceClose</c> property
    /// </summary>
    /// <param name="obj">The dependency object to get the value on</param>
    /// <returns>The value of the specified objects <c>ForceClose</c> property</returns>
    public static bool GetForceClose(Window obj) => obj.GetValue(ForceCloseProperty);

    /// <summary>
    /// Sets the value of the <c>ForceClose</c> property
    /// </summary>
    /// <param name="obj">The dependency object to set the value on</param>
    /// <param name="value">The value to set the property to</param>
    public static void SetForceClose(Window obj, bool value) => obj.SetValue(ForceCloseProperty, value);

    private static void ForceClosePropertyChanged(AvaloniaPropertyChangedEventArgs args)
    {
        var newValue = args.NewValue as bool?;
        if (args.Sender is Window window && newValue == true)
        {
            window.Close();
        }
    }
}

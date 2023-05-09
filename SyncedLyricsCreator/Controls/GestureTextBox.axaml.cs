using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace SyncedLyricsCreator.Controls;

/// <summary>
/// Defines a user control for viewing and setting the gesture used for a hotkey
/// </summary>
public partial class GestureTextBox : UserControl
{
    /// <summary>
    /// Defines the direct property for the <see cref="Gesture"/> property
    /// </summary>
    public static readonly DirectProperty<GestureTextBox, KeyGesture> GestureProperty
        = AvaloniaProperty.RegisterDirect<GestureTextBox, KeyGesture>(
            nameof(Gesture)
            , x => x.Gesture
            , (x, v) => x.Gesture = v
            , new KeyGesture(Key.None)
            , BindingMode.TwoWay);

    private TextBox textBox = null!;
    private KeyGesture gesture = null!;

    /// <summary>
    /// Initializes a new instance of the <see cref="GestureTextBox"/> class
    /// </summary>
    public GestureTextBox()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Gets or sets the currently displayed gesture
    /// </summary>
    public KeyGesture Gesture
    {
        get => gesture;
        set
        {
            SetAndRaise(GestureProperty, ref gesture, value);
            UpdateText();
        }
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        textBox = this.FindControl<TextBox>("TextBox")!;
    }

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        e.Handled = true;
        Gesture = new KeyGesture(e.Key, e.KeyModifiers);
    }

    private void UpdateText()
    {
        textBox.Text = Gesture.ToString();
    }
}

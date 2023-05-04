using System;
using Avalonia;
using Avalonia.Xaml.Interactivity;
using AvaloniaEdit;

namespace SyncedLyricsCreator.Behaviors;

/// <summary>
/// Defines behaviors to bind to a TextEditor's properties
/// </summary>
public class TextEditorBindingBehavior : Behavior<TextEditor>
{
    /// <summary>
    /// Defines the styled property for the <see cref="CaretOffset"/> property
    /// </summary>
    public static readonly StyledProperty<int> CaretOffsetProperty = AvaloniaProperty.Register<TextEditorBindingBehavior, int>(nameof(CaretOffset));

    /// <summary>
    /// Defines the styled property for the <see cref="Text"/> property
    /// </summary>
    public static readonly StyledProperty<string> TextProperty = AvaloniaProperty.Register<TextEditorBindingBehavior, string>(nameof(Text));

    private TextEditor? editor;

    /// <summary>
    /// Gets or sets the caret offset of the editor
    /// </summary>
    public int CaretOffset
    {
        get => GetValue(CaretOffsetProperty);
        set => SetValue(CaretOffsetProperty, value);
    }

    /// <summary>
    /// Gets or sets the text of the editor
    /// </summary>
    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <inheritdoc />
    protected override void OnAttached()
    {
        base.OnAttached();

        if (AssociatedObject is not { } textEditor)
        {
            return;
        }

        editor = textEditor;
        editor.TextChanged += TextChanged;
        editor.TextArea.Caret.PositionChanged += CaretOffsetChanged;
        this.GetObservable(CaretOffsetProperty).Subscribe(CaretOffsetPropertyChanged);
        this.GetObservable(TextProperty).Subscribe(TextPropertyChanged);
    }

    /// <inheritdoc />
    protected override void OnDetaching()
    {
        base.OnDetaching();

        if (editor == null)
        {
            return;
        }

        editor.TextChanged -= TextChanged;
        editor.TextArea.Caret.PositionChanged -= CaretOffsetChanged;
    }

    private void CaretOffsetChanged(object? sender, EventArgs e)
    {
        if (editor is { Document: { } })
        {
            CaretOffset = editor.CaretOffset;
        }
    }

    private void CaretOffsetPropertyChanged(int caretOffset)
    {
        if (editor is not { Document: { } })
        {
            return;
        }

        editor.CaretOffset = caretOffset;
    }

    private void TextChanged(object? sender, EventArgs e)
    {
        if (editor is { Document: { } })
        {
            Text = editor.Document.Text;
        }
    }

    private void TextPropertyChanged(string? text)
    {
        if (editor is not { Document: { } } || text == null || string.Equals(text, editor.Document.Text, StringComparison.Ordinal))
        {
            return;
        }

        var caretOffset = editor.CaretOffset;
        editor.Document.Text = text;
        editor.CaretOffset = Math.Min(caretOffset, editor.Document.TextLength);
    }
}

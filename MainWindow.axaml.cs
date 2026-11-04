using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace notatnik;

public partial class MainWindow : Window
{
    private readonly ObservableCollection<string> _notes = new();

    public MainWindow()
    {
        InitializeComponent();
        NotesList.ItemsSource = _notes;
    }

    private void SaveButton_Click(object? sender, RoutedEventArgs e)
    {
        var title = TitleBox.Text?.Trim() ?? string.Empty;
        var content = ContentBox.Text?.Trim() ?? string.Empty;

        if (string.IsNullOrEmpty(title) && string.IsNullOrEmpty(content))
            return;

        var fileName = $"notatka_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
        var fileContent = $"{title}\n{content}";
        File.WriteAllText(fileName, fileContent);

        _notes.Add($"{title} ({fileName})");

        TitleBox.Text = string.Empty;
        ContentBox.Text = string.Empty;
    }
}

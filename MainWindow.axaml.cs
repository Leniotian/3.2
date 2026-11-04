using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace notatnik;

public partial class MainWindow : Window
{
    private const string NotesFilePath = "notatki.md";
    private readonly ObservableCollection<string> _notes = new();

    public MainWindow()
    {
        InitializeComponent();
        NotesList.ItemsSource = _notes;
        LoadNotes();
    }

    private void LoadNotes()
    {
        _notes.Clear();
        if (!File.Exists(NotesFilePath))
            return;

        var lines = File.ReadAllLines(NotesFilePath, Encoding.UTF8);
        foreach (var line in lines)
        {
            if (line.StartsWith("## "))
            {
                _notes.Add(line.Substring(3));
            }
        }
    }

    private void SaveButton_Click(object? sender, RoutedEventArgs e)
    {
        var title = TitleBox.Text?.Trim() ?? string.Empty;
        var content = ContentBox.Text?.Trim() ?? string.Empty;

        if (string.IsNullOrEmpty(title) && string.IsNullOrEmpty(content))
            return;

        var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        var newEntry = $"## {timestamp} — {title}\n{content}\n";

        var existingContent = File.Exists(NotesFilePath) ? File.ReadAllText(NotesFilePath, Encoding.UTF8) : string.Empty;
        var updatedContent = string.IsNullOrEmpty(existingContent)
            ? newEntry
            : newEntry + "\n" + existingContent.TrimStart('\r', '\n');

        File.WriteAllText(NotesFilePath, updatedContent, Encoding.UTF8);

        _notes.Insert(0, $"{timestamp} — {title}");

        TitleBox.Text = string.Empty;
        ContentBox.Text = string.Empty;
    }
}

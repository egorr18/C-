using System;
using System.Collections.Generic;
using System.Linq;

public class BaseItem
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }

    public BaseItem()
    {
        CreatedAt = DateTime.Now;
    }
}

public class Note : BaseItem
{
    public string Title { get; set; }
    public string Text { get; set; }
}

public class NoteService
{
    private List<Note> notes = new List<Note>();
    private int nextId = 1;

    public void AddNote(string title, string text)
    {
        Note note = new Note
        {
            Id = nextId++,
            Title = title,
            Text = text
        };

        notes.Add(note);
    }

    public Note GetNote(int id)
    {
        return notes.FirstOrDefault(n => n.Id == id);
    }

    public List<Note> GetAll()
    {
        return notes;
    }
}

class Program
{
    static void Main()
    {
        NoteService service = new NoteService();

        service.AddNote("Покупки", "Купити молоко, хліб, сир");
        service.AddNote("План дня", "Зробити лабораторну");

        foreach (var note in service.GetAll())
        {
            Console.WriteLine($"{note.Id}. {note.Title} — {note.Text}");
        }
    }
}
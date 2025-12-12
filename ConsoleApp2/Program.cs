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

    public virtual void Display()
    {
        Console.WriteLine($"ID: {Id}, Created: {CreatedAt}");
    }
}

public class Note : BaseItem
{
    public string Title { get; set; }
    public string Text { get; set; }

    public override void Display()
    {
        Console.WriteLine($"Note #{Id}: {Title} — {Text}");
    }
}

public class TaskItem : BaseItem
{
    public string Description { get; set; }
    public bool IsDone { get; set; }

    public override void Display()
    {
        Console.WriteLine($"Task #{Id}: {Description} | Done: {IsDone}");
    }
}

public class Reminder : BaseItem
{
    public string Message { get; set; }
    public DateTime RemindAt { get; set; }

    public override void Display()
    {
        Console.WriteLine($"Reminder #{Id}: {Message} | At: {RemindAt}");
    }
}

public class ItemService
{
    private List<BaseItem> items = new List<BaseItem>();
    private int nextId = 1;

    public void AddItem(BaseItem item)
    {
        item.Id = nextId++;
        items.Add(item);
    }

    public BaseItem GetItem(int id)
    {
        return items.FirstOrDefault(i => i.Id == id);
    }

    public List<BaseItem> GetAll()
    {
        return items;
    }
}

class Program
{
    static void Main()
    {
        ItemService service = new ItemService();

        service.AddItem(new Note { Title = "Покупки", Text = "Купити молоко, хліб, сир" });
        service.AddItem(new TaskItem { Description = "Зробити лабораторну", IsDone = false });
        service.AddItem(new Reminder { Message = "Подзвонити мамі", RemindAt = DateTime.Now.AddHours(2) });

        foreach (var item in service.GetAll())
        {
            item.Display();
        }
    }
}

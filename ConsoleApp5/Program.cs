using System;
using System.Collections.Generic;

public abstract class StoreItem
{
    public string Name { get; set; }
    public decimal Price { get; set; }

    public StoreItem(string name, decimal price)
    {
        if (price < 0)
            throw new ArgumentException("Price cannot be negative!");

        Name = name;
        Price = price;
    }

    public abstract void DisplayInfo();

    public virtual void Buy(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.");

        Console.WriteLine($"{quantity} item(s) \"{Name}\" purchased for {Price * quantity} ₴.");
    }
}

public class FoodItem : StoreItem
{
    public DateTime ExpirationDate { get; set; }

    public FoodItem(string name, decimal price, DateTime expiration)
        : base(name, price)
    {
        ExpirationDate = expiration;
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"Food: {Name}, Price: {Price} ₴, Expires: {ExpirationDate.ToShortDateString()}");
    }
}

public class ElectronicItem : StoreItem
{
    public int WarrantyMonths { get; set; }

    public ElectronicItem(string name, decimal price, int warranty)
        : base(name, price)
    {
        WarrantyMonths = warranty;
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"Electronics: {Name}, Price: {Price} ₴, Warranty: {WarrantyMonths} months");
    }
}

public class StoreManager
{
    public delegate void ItemEventHandler(StoreItem item);
    
    public event ItemEventHandler ItemAdded;
    public event ItemEventHandler ItemRemoved;

    private List<StoreItem> items = new List<StoreItem>();

    public void AddItem(StoreItem item)
    {
        try
        {
            items.Add(item);
            ItemAdded?.Invoke(item);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding item: {ex.Message}");
        }
    }

    public void RemoveItem(StoreItem item)
    {
        try
        {
            if (!items.Contains(item))
                throw new Exception("Item does not exist in store.");

            items.Remove(item);
            ItemRemoved?.Invoke(item);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error removing item: {ex.Message}");
        }
    }

    public IEnumerable<StoreItem> GetAll()
    {
        return items;
    }
}

public class StoreLogger
{
    public void OnItemAdded(StoreItem item)
    {
        try
        {
            Console.WriteLine($"[LOG] Added: {item.Name} ({item.Price} ₴)");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Logger error: {ex.Message}");
        }
    }

    public void OnItemRemoved(StoreItem item)
    {
        try
        {
            Console.WriteLine($"[LOG] Removed: {item.Name}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Logger error: {ex.Message}");
        }
    }
}

public class NotificationService
{
    public void SendAddNotification(StoreItem item)
    {
        Console.WriteLine($"[NOTIFY] New item in store: {item.Name}");
    }

    public void SendRemoveNotification(StoreItem item)
    {
        Console.WriteLine($"[NOTIFY] Item removed from store: {item.Name}");
    }
}

class Program
{
    static void Main()
    {
        StoreManager manager = new StoreManager();
        StoreLogger logger = new StoreLogger();
        NotificationService notifier = new NotificationService();
        
        manager.ItemAdded += logger.OnItemAdded;
        manager.ItemAdded += notifier.SendAddNotification;

        manager.ItemRemoved += logger.OnItemRemoved;
        manager.ItemRemoved += notifier.SendRemoveNotification;

        StoreItem milk = new FoodItem("Milk", 29.5m, DateTime.Now.AddDays(3));
        StoreItem laptop = new ElectronicItem("Laptop", 39999m, 24);

        manager.AddItem(milk);
        manager.AddItem(laptop);
        manager.RemoveItem(milk);

        Console.WriteLine("\n=== CURRENT STORE ITEMS ===");
        foreach (var item in manager.GetAll())
        {
            item.DisplayInfo();
        }
    }
}

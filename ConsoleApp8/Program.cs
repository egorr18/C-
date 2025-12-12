using System;
using System.Collections.Generic;
using System.Linq;

public abstract class StoreItem
{
    public string Name { get; set; }
    public decimal Price { get; set; }

    protected StoreItem(string name, decimal price)
    {
        Name = name;
        Price = price;
    }

    public abstract void DisplayInfo();
}

public class FoodItem : StoreItem
{
    public DateTime ExpirationDate { get; set; }

    public FoodItem(string name, decimal price, DateTime exp)
        : base(name, price)
    {
        ExpirationDate = exp;
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"Food: {Name}, Price: {Price} ₴, Exp: {ExpirationDate.ToShortDateString()}");
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
    private List<StoreItem> items = new List<StoreItem>();

    public void AddItem(StoreItem item)
    {
        items.Add(item);
    }

    public IEnumerable<StoreItem> GetAll()
    {
        return items;
    }

    public IEnumerable<StoreItem> GetExpensiveItems(decimal minPrice)
    {
        foreach (var item in items)
        {
            if (item.Price >= minPrice)
                yield return item;
        }
    }

    public IEnumerable<FoodItem> GetExpiringSoon(int days)
    {
        foreach (var item in items)
        {
            if (item is FoodItem food)
            {
                if ((food.ExpirationDate - DateTime.Now).TotalDays <= days)
                    yield return food;
            }
        }
    }
}

class Program
{
    static void Main()
    {
        StoreManager manager = new StoreManager();

        manager.AddItem(new FoodItem("Milk", 29.5m, DateTime.Now.AddDays(2)));
        manager.AddItem(new FoodItem("Cheese", 155m, DateTime.Now.AddDays(12)));
        manager.AddItem(new ElectronicItem("Laptop", 39999m, 24));
        manager.AddItem(new ElectronicItem("Headphones", 1999m, 12));
        manager.AddItem(new FoodItem("Yogurt", 25m, DateTime.Now.AddDays(1)));


        Console.WriteLine("\n=== ALL ITEMS ===");
        foreach (var item in manager.GetAll())
            item.DisplayInfo();

        Console.WriteLine("\n=== EXPENSIVE ITEMS (>= 1000 ₴) ===");

        foreach (var item in manager.GetExpensiveItems(1000))
            item.DisplayInfo();

        Console.WriteLine("\n=== FOOD EXPIRING WITHIN 3 DAYS ===");

        foreach (var food in manager.GetExpiringSoon(3))
            food.DisplayInfo();

        var allItems = manager.GetAll();

        var names = allItems.Select(i => i.Name);

        Console.WriteLine("\n=== LINQ: NAMES ===");
        foreach (var n in names)
            Console.WriteLine(n);

        var cheapItems = allItems.Where(i => i.Price < 100);

        Console.WriteLine("\n=== LINQ: CHEAP ITEMS (< 100₴) ===");
        foreach (var i in cheapItems)
            i.DisplayInfo();

        var sorted = allItems.OrderBy(i => i.Price);

        Console.WriteLine("\n=== LINQ: SORTED BY PRICE ===");
        foreach (var i in sorted)
            i.DisplayInfo();

        var avg = allItems.Average(i => i.Price);
        Console.WriteLine($"\nAverage price: {avg} ₴");

        var grouped = allItems.GroupBy(i => i.GetType().Name);

        Console.WriteLine("\n=== LINQ: GROUPED BY TYPE ===");
        foreach (var group in grouped)
        {
            Console.WriteLine($"Group: {group.Key}");
            foreach (var item in group)
                item.DisplayInfo();
        }
    }
}

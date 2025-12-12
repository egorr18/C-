using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public abstract class StoreItem
{
    public string Name { get; }
    public decimal Price { get; }

    protected StoreItem(string name, decimal price)
    {
        Name = name;
        Price = price;
    }

    public abstract void DisplayInfo();
}

public class FoodItem : StoreItem
{
    public DateTime ExpirationDate { get; }

    public FoodItem(string name, decimal price, DateTime exp)
        : base(name, price)
    {
        ExpirationDate = exp;
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"Food: {Name}, Price: {Price}, Exp: {ExpirationDate.ToShortDateString()}");
    }
}

public class ElectronicItem : StoreItem
{
    public int Warranty { get; }

    public ElectronicItem(string name, decimal price, int warranty)
        : base(name, price)
    {
        Warranty = warranty;
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"Electronics: {Name}, Price: {Price}, Warranty: {Warranty} months");
    }
}

public class StoreManager
{
    private readonly List<StoreItem> items = new List<StoreItem>();

    public void AddItem(StoreItem item)
    {
        items.Add(item);
    }

    public IEnumerable<StoreItem> GetAll() => items;

    public async Task<List<StoreItem>> LoadItemsAsync(CancellationToken token)
    {
        Console.WriteLine("Loading items...");

        await Task.Delay(2000, token);

        Console.WriteLine("Items loaded.");
        return items;
    }

    public async Task<List<StoreItem>> FindExpensiveAsync(decimal minPrice, CancellationToken token)
    {
        Console.WriteLine($"Searching for items >= {minPrice}...");

        await Task.Delay(1500, token);

        return items.Where(i => i.Price >= minPrice).ToList();
    }

    public async Task<decimal> CalculateTotalAsync(CancellationToken token)
    {
        Console.WriteLine("Calculating total price...");

        await Task.Delay(1200, token);

        decimal sum = items.Sum(i => i.Price);
        return sum;
    }
}

class Program
{
    static async Task Main()
    {
        StoreManager manager = new StoreManager();

        manager.AddItem(new FoodItem("Milk", 29.5m, DateTime.Now.AddDays(2)));
        manager.AddItem(new FoodItem("Cheese", 150m, DateTime.Now.AddDays(10)));
        manager.AddItem(new ElectronicItem("Laptop", 40000m, 24));
        manager.AddItem(new ElectronicItem("Headphones", 1999m, 12));

        CancellationTokenSource cts = new CancellationTokenSource();

        try
        {
            var loadTask = manager.LoadItemsAsync(cts.Token);
            var expensiveTask = manager.FindExpensiveAsync(1000, cts.Token);
            var sumTask = manager.CalculateTotalAsync(cts.Token);

            Console.WriteLine("Running async tasks...");

            var loaded = await loadTask;
            Console.WriteLine("\n=== LOADED ITEMS ===");
            foreach (var item in loaded) item.DisplayInfo();

            var expensive = await expensiveTask;
            Console.WriteLine("\n=== EXPENSIVE ITEMS ===");
            foreach (var item in expensive) item.DisplayInfo();

            var total = await sumTask;
            Console.WriteLine($"\nTotal price = {total} ₴");
        }
        catch (TaskCanceledException)
        {
            Console.WriteLine("Operation was canceled!");
        }
    }
}

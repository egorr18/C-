using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

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
        Console.WriteLine($"Food: {Name}, {Price} ₴, Exp: {ExpirationDate.ToShortDateString()}");
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
        Console.WriteLine($"Electronics: {Name}, {Price} ₴, Warranty: {WarrantyMonths} months");
    }
}

public class StoreManager
{
    private List<StoreItem> items = new List<StoreItem>();

    private readonly Mutex mutex = new Mutex();

    public void AddItem(StoreItem item)
    {
        mutex.WaitOne();
        try
        {
            items.Add(item);
            Console.WriteLine($"[ADD] Added {item.Name} ({item.Price} ₴)");
        }
        finally
        {
            mutex.ReleaseMutex();
        }
    }

    public IEnumerable<StoreItem> GetAll()
    {
        return items;
    }
    public decimal CalculateTotalPriceParallel()
    {
        decimal sum = 0;

        Parallel.ForEach(items, item =>
        {
            Interlocked.Add(ref Unsafe.As<decimal, int>(ref sum), (int)item.Price);
        });

        return sum;
    }

    public void RemoveExpensiveItemsParallel(decimal limit)
    {
        Parallel.ForEach(items.ToList(), item =>
        {
            if (item.Price > limit)
            {
                mutex.WaitOne();
                try
                {
                    items.Remove(item);
                    Console.WriteLine($"[REMOVE] Removed {item.Name}");
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            }
        });
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
        manager.AddItem(new ElectronicItem("Smartphone", 19999m, 12));

        Task t1 = Task.Run(() =>
        {
            for (int i = 0; i < 5; i++)
                manager.AddItem(new FoodItem($"Food{i}", i + 10, DateTime.Now.AddDays(1)));
        });

        Task t2 = Task.Run(() =>
        {
            for (int i = 0; i < 5; i++)
                manager.AddItem(new ElectronicItem($"Device{i}", 1000 + i, 12));
        });

        Task.WaitAll(t1, t2);

        Console.WriteLine("\n=== PARALLEL REMOVE ITEMS > 10 000 ===");
        manager.RemoveExpensiveItemsParallel(10000);

        Console.WriteLine("\n=== PARALLEL SUM ===");
        decimal total = manager.CalculateTotalPriceParallel();
        Console.WriteLine($"Total price: {total} ₴");

        Console.WriteLine("\n=== FINAL ITEMS ===");
        foreach (var item in manager.GetAll())
            item.DisplayInfo();
    }
}

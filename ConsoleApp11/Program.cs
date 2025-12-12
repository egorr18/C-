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
    public int WarrantyMonths { get; }

    public ElectronicItem(string name, decimal price, int warranty)
        : base(name, price)
    {
        WarrantyMonths = warranty;
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"Electronics: {Name}, Price: {Price}, Warranty: {WarrantyMonths} months");
    }
}

public class StoreManager
{
    private readonly List<StoreItem> items = new List<StoreItem>();

    public void AddItem(StoreItem item) => items.Add(item);
    public IEnumerable<StoreItem> GetAll() => items;
}

public enum OrderState
{
    Created,
    Processing,
    Packed,
    Shipped,
    Completed,
    Cancelled
}

public class Order
{
    public int Id { get; }
    public OrderState State { get; private set; }

    public Order(int id)
    {
        Id = id;
        State = OrderState.Created;
    }

    private void Print()
    {
        Console.WriteLine($"[ORDER {Id}] State: {State}");
    }

    public async Task StartProcessingAsync()
    {
        if (State != OrderState.Created)
            throw new InvalidOperationException("Cannot process from current state.");

        Console.WriteLine("Order: starting processing...");
        await Task.Delay(1000);
        State = OrderState.Processing;
        Print();
    }

    public async Task PackAsync()
    {
        if (State != OrderState.Processing)
            throw new InvalidOperationException("Cannot pack before processing.");

        Console.WriteLine("Order: packing...");
        await Task.Delay(1000);
        State = OrderState.Packed;
        Print();
    }

    public async Task ShipAsync()
    {
        if (State != OrderState.Packed)
            throw new InvalidOperationException("Cannot ship before packing.");

        Console.WriteLine("Order: shipping...");
        await Task.Delay(1000);
        State = OrderState.Shipped;
        Print();
    }

    public async Task CompleteAsync()
    {
        if (State != OrderState.Shipped)
            throw new InvalidOperationException("Cannot complete before shipping.");

        Console.WriteLine("Order: completing...");
        await Task.Delay(1000);
        State = OrderState.Completed;
        Print();
    }

    public async Task CancelAsync()
    {
        if (State == OrderState.Completed)
            throw new InvalidOperationException("Cannot cancel completed order.");

        Console.WriteLine("Order: cancelling...");
        await Task.Delay(600);
        State = OrderState.Cancelled;
        Print();
    }
}

class Program
{
    static async Task Main()
    {
        StoreManager manager = new StoreManager();
        manager.AddItem(new FoodItem("Milk", 29.5m, DateTime.Now.AddDays(2)));
        manager.AddItem(new ElectronicItem("Laptop", 40000m, 24));

        Console.WriteLine("=== ITEMS IN STORE ===");
        foreach (var item in manager.GetAll())
            item.DisplayInfo();

        Console.WriteLine("\n=== ORDER STATE MACHINE DEMO ===");

        Order order = new Order(1);

        try
        {
            await order.StartProcessingAsync();
            await order.PackAsync();
            await order.ShipAsync();
            await order.CompleteAsync();

            Console.WriteLine("\nTrying to cancel completed order...");
            await order.CancelAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        Console.WriteLine("\n=== END ===");
    }
}

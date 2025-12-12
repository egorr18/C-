using System;
using System.Collections.Generic;

public abstract class StoreItem
{
    public string Name { get; set; }
    public decimal Price { get; set; }

    protected StoreItem(string name, decimal price)
    {
        if (price < 0)
            throw new ArgumentException("Price cannot be negative!");

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

    public delegate void ItemProcessor(StoreItem item);

    public void AddItem(StoreItem item)
    {
        items.Add(item);
    }

    public void ProcessItems(ItemProcessor processor)
    {
        foreach (var item in items)
        {
            try
            {
                processor(item);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Delegate error: {ex.Message}");
            }
        }
    }

    public IEnumerable<StoreItem> GetAll() => items;
}

class Program
{
    static void Main()
    {
        StoreManager manager = new StoreManager();

        manager.AddItem(new FoodItem("Milk", 29.5m, DateTime.Now.AddDays(3)));
        manager.AddItem(new ElectronicItem("Laptop", 39999m, 24));
        manager.AddItem(new FoodItem("Cheese", 155m, DateTime.Now.AddDays(10)));

        StoreManager.ItemProcessor printInfo = (item) =>
        {
            item.DisplayInfo();
        };

        Console.WriteLine("\n=== DEL: DISPLAY INFO ===");
        manager.ProcessItems(printInfo);

        StoreManager.ItemProcessor checkPrice = (item) =>
        {
            if (item.Price > 1000)
                Console.WriteLine($"{item.Name} — дорогий товар!");
            else
                Console.WriteLine($"{item.Name} — недорогий товар.");
        };

        Console.WriteLine("\n=== DEL: CHECK PRICE ===");
        manager.ProcessItems(checkPrice);
        
        StoreManager.ItemProcessor discountCalculator = (item) =>
        {
            decimal discount = item.Price * 0.1m;
            Console.WriteLine($"{item.Name}: 10% Discount = {discount} ₴");
        };

        Console.WriteLine("\n=== DEL: DISCOUNT CALCULATOR ===");
        manager.ProcessItems(discountCalculator);
    }
}

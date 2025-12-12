using System;
using System.Collections.Generic;

public abstract class StoreItem : ICloneable, IComparable<StoreItem>
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

    public virtual object Clone()
    {
        return this.MemberwiseClone();
    }

    public int CompareTo(StoreItem other)
    {
        if (other == null) return 1;
        return this.Price.CompareTo(other.Price);
    }
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
    
    public override object Clone()
    {
        return new FoodItem(Name, Price, ExpirationDate);
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

    public override object Clone()
    {
        return new ElectronicItem(Name, Price, WarrantyMonths);
    }
}

public class StoreManager
{
    private List<StoreItem> items = new List<StoreItem>();

    public void AddItem(StoreItem item)
    {
        items.Add(item);
    }

    public List<StoreItem> GetAll()
    {
        return items;
    }
}

class Program
{
    static void Main()
    {
        StoreManager manager = new StoreManager();

        manager.AddItem(new FoodItem("Milk", 29.5m, DateTime.Now.AddDays(3)));
        manager.AddItem(new ElectronicItem("Laptop", 39999m, 24));
        manager.AddItem(new FoodItem("Cheese", 155m, DateTime.Now.AddDays(10)));
        manager.AddItem(new ElectronicItem("Smartphone", 19999m, 12));

        Console.WriteLine("=== ORIGINAL LIST ===");
        foreach (var item in manager.GetAll())
            item.DisplayInfo();

        Console.WriteLine("\n=== SORTED BY PRICE ===");
        var sorted = new List<StoreItem>(manager.GetAll());
        sorted.Sort();

        foreach (var item in sorted)
            item.DisplayInfo();

        Console.WriteLine("\n=== CLONE TEST ===");

        StoreItem original = manager.GetAll()[0];
        StoreItem clone = (StoreItem)original.Clone();

        Console.WriteLine("\nOriginal:");
        original.DisplayInfo();

        Console.WriteLine("Clone:");
        clone.DisplayInfo();

        Console.WriteLine($"\nReferenceEquals: {ReferenceEquals(original, clone)}");
    }
}

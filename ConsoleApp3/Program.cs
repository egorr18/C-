using System;
using System.Collections.Generic;

public abstract class StoreItem
{
    public string Name { get; set; }
    public decimal Price { get; set; }

    public StoreItem(string name, decimal price)
    {
        Name = name;
        Price = price;
    }

    public abstract void DisplayInfo();

    public virtual void Buy()
    {
        Console.WriteLine($"Item \"{Name}\" purchased for {Price} ₴.");
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
        Console.WriteLine($"Food: {Name}, Price: {Price} ₴, Expiration: {ExpirationDate.ToShortDateString()}");
    }

    public override void Buy()
    {
        Console.WriteLine($"Food \"{Name}\" purchased. Check expiration date!");
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

    public override void Buy()
    {
        Console.WriteLine($"Electronic device \"{Name}\" purchased. Warranty included!");
    }
}

public class ClothesItem : StoreItem
{
    public string Size { get; set; }

    public ClothesItem(string name, decimal price, string size)
        : base(name, price)
    {
        Size = size;
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"Clothes: {Name}, Price: {Price} ₴, Size: {Size}");
    }

    public override void Buy()
    {
        Console.WriteLine($"Clothes \"{Name}\" purchased. Don’t forget to try it on!");
    }
}

class Program
{
    static void Main()
    {
        List<StoreItem> store = new List<StoreItem>();

        store.Add(new FoodItem("Apple", 12.5m, DateTime.Now.AddDays(7)));
        store.Add(new ElectronicItem("Smartphone", 13999m, 24));
        store.Add(new ClothesItem("T-Shirt", 499m, "L"));

        foreach (var item in store)
        {
            item.DisplayInfo();
            item.Buy();
            Console.WriteLine();
        }
    }
}

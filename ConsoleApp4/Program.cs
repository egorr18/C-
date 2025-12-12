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

    public virtual void CheckAvailability()
    {
        Console.WriteLine($"Item \"{Name}\" is available.");
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

    public override void Buy(int quantity)
    {
        try
        {
            if (DateTime.Now > ExpirationDate)
                throw new Exception("Cannot buy expired food.");

            base.Buy(quantity);
            Console.WriteLine("Please store food properly.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error buying food: {ex.Message}");
        }
    }

    public override void CheckAvailability()
    {
        if (DateTime.Now > ExpirationDate)
        {
            Console.WriteLine($"Food \"{Name}\" is expired and not available.");
        }
        else {
            Console.WriteLine($"Food \"{Name}\" is fresh and available.");
        }
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

    public override void Buy(int quantity)
    {
        try
        {
            if (quantity > 5)
                throw new Exception("Cannot buy more than 5 electronic items at once.");

            base.Buy(quantity);
            Console.WriteLine("Warranty card issued.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error buying electronics: {ex.Message}");
        }
    }

    public override void CheckAvailability()
    {
        Console.WriteLine($"Electronics \"{Name}\" require checking warehouse stock.");
    }
}

public class LuxuryItem : StoreItem
{
    public bool RequiresPassport { get; set; }

    public LuxuryItem(string name, decimal price, bool requiresPassport)
        : base(name, price)
    {
        RequiresPassport = requiresPassport;
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"Luxury Item: {Name}, Price: {Price} ₴, Passport needed: {RequiresPassport}");
    }

    public override void Buy(int quantity)
    {
        try
        {
            if (RequiresPassport == false)
                throw new Exception("Luxury item must require passport verification.");

            Console.WriteLine($"VIP purchase confirmed: {Name} × {quantity}");
            base.Buy(quantity);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Luxury purchase error: {ex.Message}");
        }
    }

    public override void CheckAvailability()
    {
        Console.WriteLine($"Luxury item \"{Name}\" available only on request.");
    }
}

class Program
{
    static void Main()
    {
        List<StoreItem> store = new List<StoreItem>();

        store.Add(new FoodItem("Milk", 29.5m, DateTime.Now.AddDays(3)));
        store.Add(new ElectronicItem("Laptop", 39999m, 24));
        store.Add(new LuxuryItem("Golden Watch", 150000m, true));

        foreach (var item in store)
        {
            item.DisplayInfo();
            item.CheckAvailability();
            item.Buy(2);
            Console.WriteLine();
        }
    }
}

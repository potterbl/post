using System;
using System.Collections.Generic;

// Базовий клас Відправлення (абстрактний)
public abstract class Shipment
{
    public int Id { get; set; }
    public string Sender { get; set; }
    public string Receiver { get; set; }
    public double Weight { get; set; }
    public double Cost { get; set; }

    public Shipment(int id, string sender, string receiver, double weight)
    {
        Id = id;
        Sender = sender;
        Receiver = receiver;
        Weight = weight;
    }

    public abstract void CalculateCost();
}

// Клас Лист, успадкований від Відправлення
public class Letter : Shipment
{
    public string Content { get; set; }

    public Letter(int id, string sender, string receiver, double weight, string content)
        : base(id, sender, receiver, weight)
    {
        Content = content;
    }

    public override void CalculateCost()
    {
        // Розрахунок вартості відправлення для листа
        // Наприклад, базуючись на вазі і типі доставки
        Cost = Weight * 0.2;
    }
}

// Клас Посилка, успадкований від Відправлення
public class Package : Shipment
{
    public bool IsFragile { get; set; }

    public Package(int id, string sender, string receiver, double weight, bool isFragile)
        : base(id, sender, receiver, weight)
    {
        IsFragile = isFragile;
    }

    public override void CalculateCost()
    {
        // Розрахунок вартості відправлення для посилки
        // Наприклад, базуючись на вазі, хрупкості та типі доставки
        Cost = Weight * 0.5 + (IsFragile ? 10 : 0);
    }
}

// Інтерфейс для взаємодії з Поштовим відділенням
public interface IPostOffice
{
    void SendShipment(Shipment shipment);
    Shipment ReceiveShipment(int id);
}

// Клас Поштове відділення
public class PostOffice : IPostOffice
{
    private List<Shipment> shipments = new List<Shipment>();

    public void SendShipment(Shipment shipment)
    {
        shipment.CalculateCost(); // Розрахунок вартості
        shipments.Add(shipment);
    }

    public Shipment ReceiveShipment(int id)
    {
        Shipment shipment = shipments.Find(s => s.Id == id);
        if (shipment != null)
        {
            shipments.Remove(shipment);
        }
        return shipment;
    }
}

class Program
{
    static void Main(string[] args)
    {
        PostOffice postOffice = new PostOffice();

        while (true)
        {
            Console.WriteLine("Поштове відділення");
            Console.WriteLine("1. Відправити лист");
            Console.WriteLine("2. Відправити посилку");
            Console.WriteLine("3. Отримати відправлення");
            Console.WriteLine("4. Вихід");

            Console.Write("Оберіть опцію: ");
            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    SendLetter(postOffice);
                    break;
                case 2:
                    SendPackage(postOffice);
                    break;
                case 3:
                    ReceiveShipment(postOffice);
                    break;
                case 4:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                    break;
            }
        }
    }

    static void SendLetter(PostOffice postOffice)
    {
        Console.Write("Введіть ID: ");
        int id = int.Parse(Console.ReadLine());
        Console.Write("Введіть відправника: ");
        string sender = Console.ReadLine();
        Console.Write("Введіть отримувача: ");
        string receiver = Console.ReadLine();
        Console.Write("Введіть вагу (грами): ");
        double weight = double.Parse(Console.ReadLine());
        Console.Write("Введіть зміст листа: ");
        string content = Console.ReadLine();

        Letter letter = new Letter(id, sender, receiver, weight, content);
        postOffice.SendShipment(letter);
        Console.WriteLine("Лист відправлено!");
    }

    static void SendPackage(PostOffice postOffice)
    {
        Console.Write("Введіть ID: ");
        int id = int.Parse(Console.ReadLine());
        Console.Write("Введіть відправника: ");
        string sender = Console.ReadLine();
        Console.Write("Введіть отримувача: ");
        string receiver = Console.ReadLine();
        Console.Write("Введіть вагу (грами): ");
        double weight = double.Parse(Console.ReadLine());
        Console.Write("Чи є посилка хрупкою? (Так/Ні): ");
        bool isFragile = Console.ReadLine().Equals("Так", StringComparison.OrdinalIgnoreCase);

        Package package = new Package(id, sender, receiver, weight, isFragile);
        postOffice.SendShipment(package);
        Console.WriteLine("Посилка відправлена!");
    }

    static void ReceiveShipment(PostOffice postOffice)
    {
        Console.Write("Введіть ID відправлення, яке ви бажаєте отримати: ");
        int id = int.Parse(Console.ReadLine());

        Shipment shipment = postOffice.ReceiveShipment(id);
        if (shipment != null)
        {
            Console.WriteLine("Відправлення отримано:");
            Console.WriteLine($"ID: {shipment.Id}");
            Console.WriteLine($"Відправник: {shipment.Sender}");
            Console.WriteLine($"Отримувач: {shipment.Receiver}");
            Console.WriteLine($"Вага: {shipment.Weight} г");
            Console.WriteLine($"Вартість: {shipment.Cost} грн");
        }
        else
        {
            Console.WriteLine("Відправлення з таким ID не знайдено.");
        }
    }
}

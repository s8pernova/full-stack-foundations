public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public int Stock { get; set; }
}

public class Program
{
    public static void Main()
    {
        System.Globalization.CultureInfo.DefaultThreadCurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");

        var inventory = new List<Product>();
        int nextId = 1;
        bool running = true;

        while (running)
        {
            Console.WriteLine();
            Console.WriteLine("=== Inventory Management System ===");
            Console.WriteLine("1) Add product");
            Console.WriteLine("2) Update stock (sell/restock)");
            Console.WriteLine("3) View all products");
            Console.WriteLine("4) Remove product");
            Console.WriteLine("5) Exit");
            Console.WriteLine();

            int choice = ReadInt("Choose an option (1-5): ");

            switch (choice)
            {
                case 1:
                    AddProduct(inventory, ref nextId);
                    break;
                case 2:
                    UpdateStock(inventory);
                    break;
                case 3:
                    DisplayProducts(inventory);
                    break;
                case 4:
                    RemoveProduct(inventory);
                    break;
                case 5:
                    running = false;
                    Console.WriteLine("Exiting...");
                    break;
                default:
                    Console.WriteLine("Invalid option. Pick 1-5.");
                    break;
            }
        }
    }

    private static void AddProduct(List<Product> inventory, ref int nextId)
    {
        Console.WriteLine();
        Console.WriteLine("--- Add Product ---");

        string name = ReadNonEmptyString("Name: ");
        decimal price = ReadDecimal("Price: ");
        int stock = ReadInt("Stock quantity: ");

        if (price < 0)
        {
            Console.WriteLine("Price cannot be negative. Product not added.");
            return;
        }

        if (stock < 0)
        {
            Console.WriteLine("Stock cannot be negative. Product not added.");
            return;
        }

        var p = new Product
        {
            Id = nextId++,
            Name = name,
            Price = price,
            Stock = stock
        };

        inventory.Add(p);
        Console.WriteLine($"Added product ID {p.Id}.");
    }

    private static void UpdateStock(List<Product> inventory)
    {
        Console.WriteLine();
        Console.WriteLine("--- Update Stock ---");

        if (inventory.Count == 0)
        {
            Console.WriteLine("No products yet.");
            return;
        }

        int id = ReadInt("Enter product ID: ");
        Product? product = FindById(inventory, id);

        if (product == null)
        {
            Console.WriteLine("Product not found.");
            return;
        }

        Console.WriteLine($"Selected: {product.Name} (Stock: {product.Stock})");
        Console.WriteLine("1) Sell (decrease stock)");
        Console.WriteLine("2) Restock (increase stock)");

        int action = ReadInt("Choose 1 or 2: ");

        if (action == 1)
        {
            int qty = ReadInt("Quantity sold: ");
            if (qty <= 0)
            {
                Console.WriteLine("Quantity must be > 0.");
                return;
            }

            if (qty > product.Stock)
            {
                Console.WriteLine("Not enough stock to sell that many.");
                return;
            }

            product.Stock -= qty;
            Console.WriteLine($"Sold {qty}. New stock: {product.Stock}");
        }
        else if (action == 2)
        {
            int qty = ReadInt("Quantity restocked: ");
            if (qty <= 0)
            {
                Console.WriteLine("Quantity must be > 0.");
                return;
            }

            product.Stock += qty;
            Console.WriteLine($"Restocked {qty}. New stock: {product.Stock}");
        }
        else
        {
            Console.WriteLine("Invalid action.");
        }
    }

    private static void RemoveProduct(List<Product> inventory)
    {
        Console.WriteLine();
        Console.WriteLine("--- Remove Product ---");

        if (inventory.Count == 0)
        {
            Console.WriteLine("No products to remove.");
            return;
        }

        int id = ReadInt("Enter product ID to remove: ");
        Product? product = FindById(inventory, id);

        if (product == null)
        {
            Console.WriteLine("Product not found.");
            return;
        }

        inventory.Remove(product);
        Console.WriteLine($"Removed product ID {id}.");
    }

    private static void DisplayProducts(List<Product> inventory)
    {
        Console.WriteLine();
        Console.WriteLine("--- All Products ---");

        if (inventory.Count == 0)
        {
            Console.WriteLine("No products yet.");
            return;
        }

        for (int i = 0; i < inventory.Count; i++)
        {
            Product p = inventory[i];
            Console.WriteLine(
                $"ID: {p.Id} | Name: {p.Name} | Price: {p.Price:C} | Stock: {p.Stock}"
            );
        }
    }

    private static Product? FindById(List<Product> inventory, int id)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].Id == id)
            {
                return inventory[i];
            }
        }
        return null;
    }

    private static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int value))
            {
                return value;
            }

            Console.WriteLine("Enter a valid whole number.");
        }
    }

    private static decimal ReadDecimal(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (decimal.TryParse(input, out decimal value))
            {
                return value;
            }

            Console.WriteLine("Enter a valid decimal number.");
        }
    }

    private static string ReadNonEmptyString(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(input))
            {
                return input.Trim();
            }

            Console.WriteLine("Value cannot be empty.");
        }
    }
}

using Railway.Core;

namespace Railway.Sample;

class Program
{

    static void Main(string[] args)
    {
        var productService = new Service.ProductService(new Repository.ProductRepository());

        // Test successful creation
        Console.WriteLine("=== Testing Successful Creation ===");
        var successResult = productService.Create("Gaming Laptop", 1299.99m, 5)
            .Tap(product => Console.WriteLine($"✅ Created product: {product.Name} - ${product.Price}"))
            .OnFailure(error => Console.WriteLine($"❌ Failed to create product: {error}"));

        Console.WriteLine($"Final result - IsSuccess: {successResult.IsSuccess}");
        Console.WriteLine();

        // Test failure case - invalid price (negative)
        Console.WriteLine("=== Testing Failure Case (Negative Price) ===");
        var failureResult = productService.Create("Invalid Product", -50.0m, 10)
            .Tap(product => Console.WriteLine($"✅ Created product: {product.Name} - ${product.Price}"))
            .OnFailure(error => Console.WriteLine($"❌ Failed to create product: {error}"));

        Console.WriteLine($"Final result - IsSuccess: {failureResult.IsSuccess}");
        Console.WriteLine();

        // Test failure case - empty name
        Console.WriteLine("=== Testing Failure Case (Empty Name) ===");
        var emptyNameResult = productService.Create("", 99.99m, 10)
            .Tap(product => Console.WriteLine($"✅ Created product: {product.Name} - ${product.Price}"))
            .OnFailure(error => Console.WriteLine($"❌ Failed to create product: {error}"));

        Console.WriteLine($"Final result - IsSuccess: {emptyNameResult.IsSuccess}");


        Console.Read();
    }

}

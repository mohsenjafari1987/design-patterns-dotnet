using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Railway.Sample.Service;
using Railway.Sample.Model;

namespace Railway.Sample.UserInterface
{
    internal class ProductUserInterface
    {
        private readonly ProductService _productService;

        public ProductUserInterface(ProductService productService)
        {
            _productService = productService;
        }

        public void Start()
        {
            Console.Clear();
            Console.WriteLine("🛍️  Welcome to Product Management System");
            Console.WriteLine("=========================================");

            bool keepRunning = true;
            while (keepRunning)
            {
                DisplayMainMenu();
                var choice = GetUserChoice();

                switch (choice)
                {
                    case 1:
                        CreateProduct();
                        break;
                    case 2:
                        ViewAllProducts();
                        break;
                    case 3:
                        ViewProductById();
                        break;
                    case 4:
                        SearchProductsByName();
                        break;
                    case 5:
                        UpdateProductPrice();
                        break;
                    case 6:
                        UpdateProductStock();
                        break;
                    case 7:
                        RenameProduct();
                        break;
                    case 8:
                        DeleteProduct();
                        break;
                    case 9:
                        keepRunning = false;
                        break;
                    default:
                        Console.WriteLine("❌ Invalid choice! Please try again.");
                        break;
                }

                if (keepRunning)
                {
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                }
            }

            Console.WriteLine("\n👋 Thank you for using Product Management System!");
        }

        private void DisplayMainMenu()
        {
            Console.Clear();
            Console.WriteLine("🛍️  Product Management System");
            Console.WriteLine("=============================");
            Console.WriteLine();
            Console.WriteLine("1️⃣  Create New Product");
            Console.WriteLine("2️⃣  View All Products");
            Console.WriteLine("3️⃣  View Product by ID");
            Console.WriteLine("4️⃣  Search Products by Name");
            Console.WriteLine("5️⃣  Update Product Price");
            Console.WriteLine("6️⃣  Update Product Stock");
            Console.WriteLine("7️⃣  Rename Product");
            Console.WriteLine("8️⃣  Delete Product");
            Console.WriteLine("9️⃣  Exit");
            Console.WriteLine();
            Console.Write("Please select an option (1-9): ");
        }

        private int GetUserChoice()
        {
            while (true)
            {
                var input = Console.ReadLine();
                if (int.TryParse(input, out int choice) && choice >= 1 && choice <= 9)
                {
                    return choice;
                }
                Console.Write("❌ Invalid input! Please enter a number between 1-9: ");
            }
        }

        private void CreateProduct()
        {
            Console.Clear();
            Console.WriteLine("➕ Create New Product");
            Console.WriteLine("====================");

            var name = GetStringInput("Enter product name: ");
            var price = GetDecimalInput("Enter product price: $");
            var stock = GetIntInput("Enter initial stock quantity: ");

            var result = _productService.Create(name, price, stock);

            if (result.IsSuccess)
            {
                Console.WriteLine($"✅ Product created successfully!");
                Console.WriteLine($"📦 {result.Value!.ToString()}");
            }
            else
            {
                Console.WriteLine($"❌ Failed to create product: {result.Error.Message}");
            }
        }

        private void ViewAllProducts()
        {
            Console.Clear();
            Console.WriteLine("📋 All Products");
            Console.WriteLine("===============");

            var result = _productService.GetAll();

            if (result.IsSuccess)
            {
                var products = result.Value!.ToList();
                if (products.Any())
                {
                    Console.WriteLine($"Found {products.Count} product(s):");
                    Console.WriteLine();
                    Console.WriteLine("📦 ID".PadRight(40) + "Name".PadRight(20) + "Price".PadRight(12) + "Stock");
                    Console.WriteLine(new string('-', 80));

                    foreach (var product in products)
                    {
                        Console.WriteLine($"📦 {product.Id.ToString().Substring(0, 8)}...".PadRight(40) +
                                        $"{product.Name}".PadRight(20) +
                                        $"${product.Price:F2}".PadRight(12) +
                                        $"{product.Stock}");
                    }
                }
                else
                {
                    Console.WriteLine("📭 No products found.");
                }
            }
            else
            {
                Console.WriteLine($"❌ Failed to retrieve products: {result.Error.Message}");
            }
        }

        private void ViewProductById()
        {
            Console.Clear();
            Console.WriteLine("🔍 View Product by ID");
            Console.WriteLine("====================");

            var idInput = GetStringInput("Enter product ID (full GUID or first 8 characters): ");
            
            // Try to find product by partial ID if not a full GUID
            var result = TryGetProductById(idInput);

            if (result.IsSuccess)
            {
                Console.WriteLine("✅ Product found:");
                Console.WriteLine($"📦 {result.Value!.ToString()}");
            }
            else
            {
                Console.WriteLine($"❌ {result.Error.Message}");
            }
        }

        private void SearchProductsByName()
        {
            Console.Clear();
            Console.WriteLine("🔎 Search Products by Name");
            Console.WriteLine("=========================");

            var searchTerm = GetStringInput("Enter search term: ");

            var result = _productService.SearchByName(searchTerm);

            if (result.IsSuccess)
            {
                var products = result.Value!.ToList();
                if (products.Any())
                {
                    Console.WriteLine($"✅ Found {products.Count} product(s) matching '{searchTerm}':");
                    Console.WriteLine();
                    foreach (var product in products)
                    {
                        Console.WriteLine($"📦 {product.ToString()}");
                    }
                }
                else
                {
                    Console.WriteLine($"📭 No products found matching '{searchTerm}'.");
                }
            }
            else
            {
                Console.WriteLine($"❌ Search failed: {result.Error.Message}");
            }
        }

        private void UpdateProductPrice()
        {
            Console.Clear();
            Console.WriteLine("💰 Update Product Price");
            Console.WriteLine("======================");

            var idInput = GetStringInput("Enter product ID: ");
            var productResult = TryGetProductById(idInput);

            if (!productResult.IsSuccess)
            {
                Console.WriteLine($"❌ {productResult.Error.Message}");
                return;
            }

            var product = productResult.Value!;
            Console.WriteLine($"📦 Current product: {product.ToString()}");
            
            var newPrice = GetDecimalInput($"Enter new price (current: ${product.Price:F2}): $");

            var result = _productService.UpdatePrice(product.Id, newPrice);

            if (result.IsSuccess)
            {
                Console.WriteLine($"✅ Price updated successfully!");
                Console.WriteLine($"📦 {result.Value!.ToString()}");
            }
            else
            {
                Console.WriteLine($"❌ Failed to update price: {result.Error.Message}");
            }
        }

        private void UpdateProductStock()
        {
            Console.Clear();
            Console.WriteLine("📦 Update Product Stock");
            Console.WriteLine("======================");

            var idInput = GetStringInput("Enter product ID: ");
            var productResult = TryGetProductById(idInput);

            if (!productResult.IsSuccess)
            {
                Console.WriteLine($"❌ {productResult.Error.Message}");
                return;
            }

            var product = productResult.Value!;
            Console.WriteLine($"📦 Current product: {product.ToString()}");
            
            var stockChange = GetIntInput($"Enter stock change (current: {product.Stock}, positive to add, negative to remove): ");

            var result = _productService.UpdateStock(product.Id, stockChange);

            if (result.IsSuccess)
            {
                Console.WriteLine($"✅ Stock updated successfully!");
                Console.WriteLine($"📦 {result.Value!.ToString()}");
            }
            else
            {
                Console.WriteLine($"❌ Failed to update stock: {result.Error.Message}");
            }
        }

        private void RenameProduct()
        {
            Console.Clear();
            Console.WriteLine("✏️  Rename Product");
            Console.WriteLine("==================");

            var idInput = GetStringInput("Enter product ID: ");
            var productResult = TryGetProductById(idInput);

            if (!productResult.IsSuccess)
            {
                Console.WriteLine($"❌ {productResult.Error.Message}");
                return;
            }

            var product = productResult.Value!;
            Console.WriteLine($"📦 Current product: {product.ToString()}");
            
            var newName = GetStringInput($"Enter new name (current: '{product.Name}'): ");

            var result = _productService.RenameProduct(product.Id, newName);

            if (result.IsSuccess)
            {
                Console.WriteLine($"✅ Product renamed successfully!");
                Console.WriteLine($"📦 {result.Value!.ToString()}");
            }
            else
            {
                Console.WriteLine($"❌ Failed to rename product: {result.Error.Message}");
            }
        }

        private void DeleteProduct()
        {
            Console.Clear();
            Console.WriteLine("🗑️  Delete Product");
            Console.WriteLine("==================");

            var idInput = GetStringInput("Enter product ID: ");
            var productResult = TryGetProductById(idInput);

            if (!productResult.IsSuccess)
            {
                Console.WriteLine($"❌ {productResult.Error.Message}");
                return;
            }

            var product = productResult.Value!;
            Console.WriteLine($"📦 Product to delete: {product.ToString()}");
            
            var confirmation = GetStringInput("⚠️  Are you sure you want to delete this product? (yes/no): ");

            if (confirmation.ToLower() == "yes" || confirmation.ToLower() == "y")
            {
                var result = _productService.DeleteProduct(product.Id);

                if (result.IsSuccess)
                {
                    Console.WriteLine($"✅ Product deleted successfully!");
                }
                else
                {
                    Console.WriteLine($"❌ Failed to delete product: {result.Error.Message}");
                }
            }
            else
            {
                Console.WriteLine("❌ Deletion cancelled.");
            }
        }

        private Railway.Core.Result<Product> TryGetProductById(string idInput)
        {
            // Try to parse as full GUID first
            if (Guid.TryParse(idInput, out Guid fullId))
            {
                return _productService.GetById(fullId);
            }

            // If not a full GUID, try to find by partial ID (first 8 characters)
            var allProductsResult = _productService.GetAll();
            if (!allProductsResult.IsSuccess)
            {
                return Railway.Core.Result<Product>.Failure(allProductsResult.Error);
            }

            var matchingProduct = allProductsResult.Value!
                .FirstOrDefault(p => p.Id.ToString().StartsWith(idInput, StringComparison.OrdinalIgnoreCase));

            if (matchingProduct != null)
            {
                return Railway.Core.Result<Product>.Success(matchingProduct);
            }

            return Railway.Core.Result<Product>.Failure(new Railway.Core.Error("PRODUCT_NOT_FOUND", $"No product found with ID starting with '{idInput}'."));
        }

        private string GetStringInput(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input.Trim();
                }
                Console.WriteLine("❌ Input cannot be empty. Please try again.");
            }
        }

        private decimal GetDecimalInput(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine();
                if (decimal.TryParse(input, out decimal value))
                {
                    return value;
                }
                Console.WriteLine("❌ Invalid decimal number. Please try again.");
            }
        }

        private int GetIntInput(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine();
                if (int.TryParse(input, out int value))
                {
                    return value;
                }
                Console.WriteLine("❌ Invalid integer number. Please try again.");
            }
        }
    }
}

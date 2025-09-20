using Railway.Core;
using Railway.Sample.Model;

namespace Railway.Sample.Repository
{
    public class ProductRepository
    {
        private readonly Dictionary<Guid, Product> _products;
        public ProductRepository()
        {
            _products = new Dictionary<Guid, Product>();
            SeedInitialData();
        }

        public Result<Product> GetById(Guid id)
        {
            return _products.TryGetValue(id, out var product)
                ? Result<Product>.Success(product)
                : Result<Product>.Failure(new Error("PRODUCT_NOT_FOUND", $"Product with ID {id} was not found."));
        }

        public Result<IEnumerable<Product>> GetAll()
        {
            return Result<IEnumerable<Product>>.Success(_products.Values.AsEnumerable());
        }

        public Result<Product> Add(Product product)
        {
            _products.Add(product.Id, product);
            return Result<Product>.Success(product);
        }

        public Result<Product> Update(Product product)
        {
            if (!_products.ContainsKey(product.Id))
            {
                return Result<Product>.Failure(new Error("PRODUCT_NOT_FOUND", $"Product with ID {product.Id} was not found."));
            }

            _products[product.Id] = product;
            return Result<Product>.Success(product);
        }

        public Result Delete(Guid id)
        {
            if (!_products.ContainsKey(id))
            {
                return Result.Failure(new Error("PRODUCT_NOT_FOUND", $"Product with ID {id} was not found."));
            }

            _products.Remove(id);
            return Result.Success();
        }

        public Result<IEnumerable<Product>> FindByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Result<IEnumerable<Product>>.Failure(new Error("INVALID_SEARCH", "Search name cannot be empty."));
            }

            var matchingProducts = _products.Values
                .Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                .AsEnumerable();

            return Result<IEnumerable<Product>>.Success(matchingProducts);
        }

        private void SeedInitialData()
        {
            // Add some initial sample products
            var sampleProducts = new[]
            {
                ("Laptop", 999.99m, 10),
                ("Mouse", 29.99m, 50),
                ("Keyboard", 79.99m, 25),
                ("Monitor", 299.99m, 15),
                ("Headphones", 149.99m, 30),
                ("Webcam", 89.99m, 20),
                ("Smartphone", 699.99m, 8),
                ("Tablet", 399.99m, 12),
                ("Speaker", 199.99m, 18),
                ("Charger", 24.99m, 40)
            };

            foreach (var (name, price, stock) in sampleProducts)
            {
                Product.Create(name, price, stock)
                    .Bind(p => Add(p));
            }
        }
    }
}

using Railway.Core;

namespace Railway.Sample.Model
{
    public class Product
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = default!;
        public decimal Price { get; private set; }
        public int Stock { get; private set; }

        private static Product Empty => new Product
        {
            Name = "",
            Price = 0,
            Stock = 0
        };

        public static Result<Product> Create(string name, decimal price, int stock)
        {
            return Result<Product>.Create(Empty)
                .Bind(r => r.UpdatePrice(price))
                .Bind(r => r.UpdateStock(stock))
                .Bind(r => r.Rename(name));
        }

        public Result<Product> UpdatePrice(decimal newPrice)
        {
            return Result<decimal>.Create(newPrice)
                .Ensure(price => price >= 0, new Error("INVALID_PRICE", "Price cannot be negative."))
                .Map(price =>
                {
                    Price = price;
                    return this;
                });
        }

        public Result<Product> UpdateStock(int quantity)
        {
            return Result<int>.Create(quantity)
                .Ensure(qty => Stock + qty >= 0, new Error("INSUFFICIENT_STOCK", "Not enough stock available."))
                .Map(qty =>
                {
                    Stock += qty;
                    return this;
                });
        }

        public Result<Product> Rename(string newName)
        {
            return Result<string>.Create(newName)
                .Ensure(name => !string.IsNullOrWhiteSpace(name), new Error("INVALID_NAME", "Name cannot be empty."))
                .Map(name =>
                {
                    Name = name;
                    return this;
                });
        }
    }
}

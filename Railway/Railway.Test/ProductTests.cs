using Railway.Core;
using Railway.Sample.Model;

namespace Railway.Test
{
    public class ProductTests
    {
        #region Product Creation Tests

        [Fact]
        public void Create_WithValidData_ShouldReturnSuccessResult()
        {
            // Arrange
            var name = "Gaming Laptop";
            var price = 1299.99m;
            var stock = 5;

            // Act
            var result = Product.Create(name, price, stock);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(name, result.Value.Name);
            Assert.Equal(price, result.Value.Price);
            Assert.Equal(stock, result.Value.Stock);
            Assert.NotEqual(Guid.Empty, result.Value.Id);
        }

        [Fact]
        public void Create_WithNegativePrice_ShouldReturnFailureResult()
        {
            // Arrange
            var name = "Gaming Laptop";
            var price = -100m;
            var stock = 5;

            // Act
            var result = Product.Create(name, price, stock);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("INVALID_PRICE", result.Error.Code);
            Assert.Equal("Price cannot be negative.", result.Error.Message);
        }

        [Fact]
        public void Create_WithEmptyName_ShouldReturnFailureResult()
        {
            // Arrange
            var name = "";
            var price = 1299.99m;
            var stock = 5;

            // Act
            var result = Product.Create(name, price, stock);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("INVALID_NAME", result.Error.Code);
            Assert.Equal("Name cannot be empty.", result.Error.Message);
        }

        [Fact]
        public void Create_WithWhitespaceOnlyName_ShouldReturnFailureResult()
        {
            // Arrange
            var name = "   ";
            var price = 1299.99m;
            var stock = 5;

            // Act
            var result = Product.Create(name, price, stock);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("INVALID_NAME", result.Error.Code);
            Assert.Equal("Name cannot be empty.", result.Error.Message);
        }

        [Fact]
        public void Create_WithZeroPrice_ShouldReturnSuccessResult()
        {
            // Arrange
            var name = "Free Sample";
            var price = 0m;
            var stock = 10;

            // Act
            var result = Product.Create(name, price, stock);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(price, result.Value!.Price);
        }

        #endregion

        #region UpdatePrice Tests

        [Fact]
        public void UpdatePrice_WithValidPrice_ShouldReturnSuccessResult()
        {
            // Arrange
            var product = Product.Create("Test Product", 100m, 5).Value!;
            var newPrice = 150m;

            // Act
            var result = product.UpdatePrice(newPrice);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(newPrice, result.Value!.Price);
        }

        [Fact]
        public void UpdatePrice_WithNegativePrice_ShouldReturnFailureResult()
        {
            // Arrange
            var product = Product.Create("Test Product", 100m, 5).Value!;
            var newPrice = -50m;

            // Act
            var result = product.UpdatePrice(newPrice);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("INVALID_PRICE", result.Error.Code);
            Assert.Equal("Price cannot be negative.", result.Error.Message);
        }

        [Fact]
        public void UpdatePrice_WithZeroPrice_ShouldReturnSuccessResult()
        {
            // Arrange
            var product = Product.Create("Test Product", 100m, 5).Value!;
            var newPrice = 0m;

            // Act
            var result = product.UpdatePrice(newPrice);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(newPrice, result.Value!.Price);
        }

        #endregion

        #region UpdateStock Tests

        [Fact]
        public void UpdateStock_WithPositiveQuantity_ShouldIncreaseStock()
        {
            // Arrange
            var product = Product.Create("Test Product", 100m, 5).Value!;
            var initialStock = product.Stock;
            var quantity = 10;

            // Act
            var result = product.UpdateStock(quantity);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(initialStock + quantity, result.Value!.Stock);
        }

        [Fact]
        public void UpdateStock_WithNegativeQuantity_ShouldDecreaseStock()
        {
            // Arrange
            var product = Product.Create("Test Product", 100m, 10).Value!;
            var initialStock = product.Stock;
            var quantity = -5;

            // Act
            var result = product.UpdateStock(quantity);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(initialStock + quantity, result.Value!.Stock);
        }

        [Fact]
        public void UpdateStock_WithQuantityThatResultsInNegativeStock_ShouldReturnFailureResult()
        {
            // Arrange
            var product = Product.Create("Test Product", 100m, 5).Value!;
            var quantity = -10; // This would result in -5 stock

            // Act
            var result = product.UpdateStock(quantity);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("INSUFFICIENT_STOCK", result.Error.Code);
            Assert.Equal("Not enough stock available.", result.Error.Message);
        }

        [Fact]
        public void UpdateStock_WithQuantityThatResultsInZeroStock_ShouldReturnSuccessResult()
        {
            // Arrange
            var product = Product.Create("Test Product", 100m, 5).Value!;
            var quantity = -5; // This would result in 0 stock

            // Act
            var result = product.UpdateStock(quantity);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(0, result.Value!.Stock);
        }

        #endregion

        #region Rename Tests

        [Fact]
        public void Rename_WithValidName_ShouldReturnSuccessResult()
        {
            // Arrange
            var product = Product.Create("Old Name", 100m, 5).Value!;
            var newName = "New Product Name";

            // Act
            var result = product.Rename(newName);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(newName, result.Value!.Name);
        }

        [Fact]
        public void Rename_WithEmptyName_ShouldReturnFailureResult()
        {
            // Arrange
            var product = Product.Create("Old Name", 100m, 5).Value!;
            var newName = "";

            // Act
            var result = product.Rename(newName);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("INVALID_NAME", result.Error.Code);
            Assert.Equal("Name cannot be empty.", result.Error.Message);
        }

        [Fact]
        public void Rename_WithWhitespaceOnlyName_ShouldReturnFailureResult()
        {
            // Arrange
            var product = Product.Create("Old Name", 100m, 5).Value!;
            var newName = "   ";

            // Act
            var result = product.Rename(newName);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("INVALID_NAME", result.Error.Code);
            Assert.Equal("Name cannot be empty.", result.Error.Message);
        }

        [Fact]
        public void Rename_WithNullName_ShouldReturnFailureResult()
        {
            // Arrange
            var product = Product.Create("Old Name", 100m, 5).Value!;
            string newName = null!;

            // Act
            var result = product.Rename(newName);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("INVALID_NAME", result.Error.Code);
            Assert.Equal("Name cannot be empty.", result.Error.Message);
        }

        #endregion

        #region ToString Tests

        [Fact]
        public void ToString_ShouldReturnFormattedString()
        {
            // Arrange
            var product = Product.Create("Gaming Laptop", 1299.99m, 5).Value!;

            // Act
            var result = product.ToString();

            // Assert
            Assert.Contains("Gaming Laptop", result);
            Assert.Contains("$1299.99", result);
            Assert.Contains("Stock: 5", result);
            Assert.Contains(product.Id.ToString("D"), result);
        }

        #endregion

        #region Fluent Chaining Tests

        [Fact]
        public void FluentChaining_MultipleOperations_ShouldWorkCorrectly()
        {
            // Arrange & Act
            var result = Product.Create("Test Product", 100m, 10)
                .Bind(p => p.UpdatePrice(150m))
                .Bind(p => p.UpdateStock(-5))
                .Bind(p => p.Rename("Updated Product"));

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Updated Product", result.Value!.Name);
            Assert.Equal(150m, result.Value.Price);
            Assert.Equal(5, result.Value.Stock);
        }

        [Fact]
        public void FluentChaining_WithFailureInMiddle_ShouldStopAndReturnFailure()
        {
            // Arrange & Act
            var result = Product.Create("Test Product", 100m, 10)
                .Bind(p => p.UpdatePrice(-50m)) // This should fail
                .Bind(p => p.UpdateStock(-5))
                .Bind(p => p.Rename("Updated Product"));

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("INVALID_PRICE", result.Error.Code);
        }

        #endregion
    }
}
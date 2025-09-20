using Railway.Core;
using Railway.Sample.Model;
using Railway.Sample.Repository;
using Railway.Sample.Service;

namespace Railway.Test
{
    public class ProductServiceTests
    {
        private ProductService CreateProductService()
        {
            return new ProductService(new ProductRepository());
        }

        #region Create Tests

        [Fact]
        public void Create_WithValidData_ShouldReturnSuccessResult()
        {
            // Arrange
            var service = CreateProductService();
            var name = "Gaming Laptop";
            var price = 1299.99m;
            var stock = 5;

            // Act
            var result = service.Create(name, price, stock);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(name, result.Value.Name);
            Assert.Equal(price, result.Value.Price);
            Assert.Equal(stock, result.Value.Stock);
            Assert.NotEqual(Guid.Empty, result.Value.Id);
        }

        [Fact]
        public void Create_WithInvalidPrice_ShouldReturnFailureResult()
        {
            // Arrange
            var service = CreateProductService();
            var name = "Gaming Laptop";
            var price = -100m;
            var stock = 5;

            // Act
            var result = service.Create(name, price, stock);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("INVALID_PRICE", result.Error.Code);
        }

        [Fact]
        public void Create_WithInvalidName_ShouldReturnFailureResult()
        {
            // Arrange
            var service = CreateProductService();
            var name = "";
            var price = 100m;
            var stock = 5;

            // Act
            var result = service.Create(name, price, stock);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("INVALID_NAME", result.Error.Code);
        }

        #endregion

        #region GetById Tests

        [Fact]
        public void GetById_WithExistingProduct_ShouldReturnSuccessResult()
        {
            // Arrange
            var service = CreateProductService();
            var createResult = service.Create("Test Product", 100m, 10);
            var productId = createResult.Value!.Id;

            // Act
            var result = service.GetById(productId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(productId, result.Value.Id);
            Assert.Equal("Test Product", result.Value.Name);
        }

        [Fact]
        public void GetById_WithNonExistentProduct_ShouldReturnFailureResult()
        {
            // Arrange
            var service = CreateProductService();
            var nonExistentId = Guid.NewGuid();

            // Act
            var result = service.GetById(nonExistentId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("PRODUCT_NOT_FOUND", result.Error.Code);
        }

        #endregion

        #region GetAll Tests

        [Fact]
        public void GetAll_ShouldReturnAllProducts()
        {
            // Arrange
            var service = CreateProductService();
            
            // Act
            var result = service.GetAll();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            // The repository seeds initial data, so there should be products
            Assert.True(result.Value.Count() > 0);
        }

        [Fact]
        public void GetAll_AfterAddingProducts_ShouldIncludeNewProducts()
        {
            // Arrange
            var service = CreateProductService();
            var initialCount = service.GetAll().Value!.Count();
            
            service.Create("New Product 1", 100m, 5);
            service.Create("New Product 2", 200m, 10);

            // Act
            var result = service.GetAll();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(initialCount + 2, result.Value!.Count());
        }

        #endregion

        #region UpdatePrice Tests

        [Fact]
        public void UpdatePrice_WithExistingProduct_ShouldReturnSuccessResult()
        {
            // Arrange
            var service = CreateProductService();
            var createResult = service.Create("Test Product", 100m, 10);
            var productId = createResult.Value!.Id;
            var newPrice = 150m;

            // Act
            var result = service.UpdatePrice(productId, newPrice);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(newPrice, result.Value!.Price);
            
            // Verify the product is actually updated in the repository
            var updatedProduct = service.GetById(productId);
            Assert.Equal(newPrice, updatedProduct.Value!.Price);
        }

        [Fact]
        public void UpdatePrice_WithNonExistentProduct_ShouldReturnFailureResult()
        {
            // Arrange
            var service = CreateProductService();
            var nonExistentId = Guid.NewGuid();
            var newPrice = 150m;

            // Act
            var result = service.UpdatePrice(nonExistentId, newPrice);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("PRODUCT_NOT_FOUND", result.Error.Code);
        }

        [Fact]
        public void UpdatePrice_WithInvalidPrice_ShouldReturnFailureResult()
        {
            // Arrange
            var service = CreateProductService();
            var createResult = service.Create("Test Product", 100m, 10);
            var productId = createResult.Value!.Id;
            var invalidPrice = -50m;

            // Act
            var result = service.UpdatePrice(productId, invalidPrice);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("INVALID_PRICE", result.Error.Code);
        }

        #endregion

        #region UpdateStock Tests

        [Fact]
        public void UpdateStock_WithPositiveQuantity_ShouldIncreaseStock()
        {
            // Arrange
            var service = CreateProductService();
            var createResult = service.Create("Test Product", 100m, 10);
            var productId = createResult.Value!.Id;
            var initialStock = createResult.Value.Stock;
            var quantity = 5;

            // Act
            var result = service.UpdateStock(productId, quantity);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(initialStock + quantity, result.Value!.Stock);
        }

        [Fact]
        public void UpdateStock_WithNegativeQuantity_ShouldDecreaseStock()
        {
            // Arrange
            var service = CreateProductService();
            var createResult = service.Create("Test Product", 100m, 10);
            var productId = createResult.Value!.Id;
            var initialStock = createResult.Value.Stock;
            var quantity = -3;

            // Act
            var result = service.UpdateStock(productId, quantity);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(initialStock + quantity, result.Value!.Stock);
        }

        [Fact]
        public void UpdateStock_WithInsufficientStock_ShouldReturnFailureResult()
        {
            // Arrange
            var service = CreateProductService();
            var createResult = service.Create("Test Product", 100m, 5);
            var productId = createResult.Value!.Id;
            var quantity = -10; // Trying to reduce by more than available

            // Act
            var result = service.UpdateStock(productId, quantity);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("INSUFFICIENT_STOCK", result.Error.Code);
        }

        [Fact]
        public void UpdateStock_WithNonExistentProduct_ShouldReturnFailureResult()
        {
            // Arrange
            var service = CreateProductService();
            var nonExistentId = Guid.NewGuid();
            var quantity = 5;

            // Act
            var result = service.UpdateStock(nonExistentId, quantity);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("PRODUCT_NOT_FOUND", result.Error.Code);
        }

        #endregion

        #region RenameProduct Tests

        [Fact]
        public void RenameProduct_WithValidName_ShouldReturnSuccessResult()
        {
            // Arrange
            var service = CreateProductService();
            var createResult = service.Create("Old Name", 100m, 10);
            var productId = createResult.Value!.Id;
            var newName = "New Product Name";

            // Act
            var result = service.RenameProduct(productId, newName);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(newName, result.Value!.Name);
            
            // Verify the product is actually updated in the repository
            var updatedProduct = service.GetById(productId);
            Assert.Equal(newName, updatedProduct.Value!.Name);
        }

        [Fact]
        public void RenameProduct_WithInvalidName_ShouldReturnFailureResult()
        {
            // Arrange
            var service = CreateProductService();
            var createResult = service.Create("Old Name", 100m, 10);
            var productId = createResult.Value!.Id;
            var invalidName = "";

            // Act
            var result = service.RenameProduct(productId, invalidName);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("INVALID_NAME", result.Error.Code);
        }

        [Fact]
        public void RenameProduct_WithNonExistentProduct_ShouldReturnFailureResult()
        {
            // Arrange
            var service = CreateProductService();
            var nonExistentId = Guid.NewGuid();
            var newName = "New Name";

            // Act
            var result = service.RenameProduct(nonExistentId, newName);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("PRODUCT_NOT_FOUND", result.Error.Code);
        }

        #endregion

        #region DeleteProduct Tests

        [Fact]
        public void DeleteProduct_WithExistingProduct_ShouldReturnSuccessResult()
        {
            // Arrange
            var service = CreateProductService();
            var createResult = service.Create("Test Product", 100m, 10);
            var productId = createResult.Value!.Id;

            // Act
            var result = service.DeleteProduct(productId);

            // Assert
            Assert.True(result.IsSuccess);
            
            // Verify the product is actually deleted from the repository
            var getResult = service.GetById(productId);
            Assert.False(getResult.IsSuccess);
            Assert.Equal("PRODUCT_NOT_FOUND", getResult.Error.Code);
        }

        [Fact]
        public void DeleteProduct_WithNonExistentProduct_ShouldReturnFailureResult()
        {
            // Arrange
            var service = CreateProductService();
            var nonExistentId = Guid.NewGuid();

            // Act
            var result = service.DeleteProduct(nonExistentId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("PRODUCT_NOT_FOUND", result.Error.Code);
        }

        #endregion

        #region SearchByName Tests

        [Fact]
        public void SearchByName_WithExistingPartialName_ShouldReturnMatchingProducts()
        {
            // Arrange
            var service = CreateProductService();
            service.Create("Gaming Laptop", 1000m, 5);
            service.Create("Gaming Mouse", 50m, 20);
            service.Create("Office Chair", 200m, 10);

            // Act
            var result = service.SearchByName("Gaming");

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            var products = result.Value.ToList();
            Assert.Equal(2, products.Count);
            Assert.All(products, p => Assert.Contains("Gaming", p.Name));
        }

        [Fact]
        public void SearchByName_WithNonExistentName_ShouldReturnEmptyResult()
        {
            // Arrange
            var service = CreateProductService();

            // Act
            var result = service.SearchByName("NonExistentProduct");

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value);
        }

        [Fact]
        public void SearchByName_WithEmptyName_ShouldReturnFailureResult()
        {
            // Arrange
            var service = CreateProductService();

            // Act
            var result = service.SearchByName("");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("INVALID_SEARCH", result.Error.Code);
        }

        [Fact]
        public void SearchByName_WithWhitespaceOnlyName_ShouldReturnFailureResult()
        {
            // Arrange
            var service = CreateProductService();

            // Act
            var result = service.SearchByName("   ");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("INVALID_SEARCH", result.Error.Code);
        }

        [Fact]
        public void SearchByName_IsCaseInsensitive()
        {
            // Arrange
            var service = CreateProductService();
            service.Create("Gaming Laptop", 1000m, 5);

            // Act
            var result = service.SearchByName("gaming");

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            var products = result.Value.ToList();
            Assert.Single(products);
            Assert.Equal("Gaming Laptop", products[0].Name);
        }

        #endregion

        #region Integration Tests

        [Fact]
        public void CompleteWorkflow_CreateUpdateSearchDelete_ShouldWorkCorrectly()
        {
            // Arrange
            var service = CreateProductService();

            // Act & Assert - Create
            var createResult = service.Create("Test Product", 100m, 10);
            Assert.True(createResult.IsSuccess);
            var productId = createResult.Value!.Id;

            // Act & Assert - Update Price
            var updatePriceResult = service.UpdatePrice(productId, 150m);
            Assert.True(updatePriceResult.IsSuccess);
            Assert.Equal(150m, updatePriceResult.Value!.Price);

            // Act & Assert - Update Stock
            var updateStockResult = service.UpdateStock(productId, -3);
            Assert.True(updateStockResult.IsSuccess);
            Assert.Equal(7, updateStockResult.Value!.Stock);

            // Act & Assert - Rename
            var renameResult = service.RenameProduct(productId, "Updated Test Product");
            Assert.True(renameResult.IsSuccess);
            Assert.Equal("Updated Test Product", renameResult.Value!.Name);

            // Act & Assert - Search
            var searchResult = service.SearchByName("Updated");
            Assert.True(searchResult.IsSuccess);
            Assert.Contains(searchResult.Value!, p => p.Id == productId);

            // Act & Assert - Delete
            var deleteResult = service.DeleteProduct(productId);
            Assert.True(deleteResult.IsSuccess);

            // Act & Assert - Verify deletion
            var getResult = service.GetById(productId);
            Assert.False(getResult.IsSuccess);
            Assert.Equal("PRODUCT_NOT_FOUND", getResult.Error.Code);
        }

        [Fact]
        public void MultipleProductOperations_ShouldNotInterfereWithEachOther()
        {
            // Arrange
            var service = CreateProductService();

            // Act - Create multiple products
            var product1 = service.Create("Product 1", 100m, 10);
            var product2 = service.Create("Product 2", 200m, 20);
            var product3 = service.Create("Product 3", 300m, 30);

            // Assert
            Assert.True(product1.IsSuccess);
            Assert.True(product2.IsSuccess);
            Assert.True(product3.IsSuccess);

            // Act - Update different products
            var updateResult1 = service.UpdatePrice(product1.Value!.Id, 150m);
            var updateResult2 = service.UpdateStock(product2.Value!.Id, -5);
            var renameResult3 = service.RenameProduct(product3.Value!.Id, "Renamed Product 3");

            // Assert
            Assert.True(updateResult1.IsSuccess);
            Assert.True(updateResult2.IsSuccess);
            Assert.True(renameResult3.IsSuccess);

            // Verify each product maintains its own state
            var finalProduct1 = service.GetById(product1.Value.Id);
            var finalProduct2 = service.GetById(product2.Value.Id);
            var finalProduct3 = service.GetById(product3.Value.Id);

            Assert.Equal(150m, finalProduct1.Value!.Price);
            Assert.Equal(15, finalProduct2.Value!.Stock); // 20 - 5
            Assert.Equal("Renamed Product 3", finalProduct3.Value!.Name);
        }

        #endregion
    }
}
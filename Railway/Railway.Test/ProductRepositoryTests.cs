using Railway.Core;
using Railway.Sample.Model;
using Railway.Sample.Repository;

namespace Railway.Test
{
    public class ProductRepositoryTests
    {
        private ProductRepository CreateRepository()
        {
            return new ProductRepository();
        }

        #region Add Tests

        [Fact]
        public void Add_WithValidProduct_ShouldReturnSuccessResult()
        {
            // Arrange
            var repository = CreateRepository();
            var product = Product.Create("Test Product", 100m, 10).Value!;

            // Act
            var result = repository.Add(product);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(product.Id, result.Value!.Id);
            Assert.Equal(product.Name, result.Value.Name);
        }

        [Fact]
        public void Add_ShouldMakeProductRetrievable()
        {
            // Arrange
            var repository = CreateRepository();
            var product = Product.Create("Test Product", 100m, 10).Value!;

            // Act
            var addResult = repository.Add(product);
            var getResult = repository.GetById(product.Id);

            // Assert
            Assert.True(addResult.IsSuccess);
            Assert.True(getResult.IsSuccess);
            Assert.Equal(product.Id, getResult.Value!.Id);
        }

        #endregion

        #region GetById Tests

        [Fact]
        public void GetById_WithExistingProduct_ShouldReturnSuccessResult()
        {
            // Arrange
            var repository = CreateRepository();
            var product = Product.Create("Test Product", 100m, 10).Value!;
            repository.Add(product);

            // Act
            var result = repository.GetById(product.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(product.Id, result.Value.Id);
            Assert.Equal(product.Name, result.Value.Name);
            Assert.Equal(product.Price, result.Value.Price);
            Assert.Equal(product.Stock, result.Value.Stock);
        }

        [Fact]
        public void GetById_WithNonExistentProduct_ShouldReturnFailureResult()
        {
            // Arrange
            var repository = CreateRepository();
            var nonExistentId = Guid.NewGuid();

            // Act
            var result = repository.GetById(nonExistentId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("PRODUCT_NOT_FOUND", result.Error.Code);
            Assert.Contains(nonExistentId.ToString(), result.Error.Message);
        }

        #endregion

        #region GetAll Tests

        [Fact]
        public void GetAll_ShouldReturnAllProducts()
        {
            // Arrange
            var repository = CreateRepository();
            var initialCount = repository.GetAll().Value!.Count();

            var product1 = Product.Create("Product 1", 100m, 10).Value!;
            var product2 = Product.Create("Product 2", 200m, 20).Value!;
            repository.Add(product1);
            repository.Add(product2);

            // Act
            var result = repository.GetAll();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            var products = result.Value.ToList();
            Assert.Equal(initialCount + 2, products.Count);
            Assert.Contains(products, p => p.Id == product1.Id);
            Assert.Contains(products, p => p.Id == product2.Id);
        }

        [Fact]
        public void GetAll_WithEmptyRepository_ShouldReturnSeededData()
        {
            // Arrange & Act
            var repository = CreateRepository();
            var result = repository.GetAll();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            // Repository seeds initial data, so it should not be empty
            Assert.True(result.Value.Count() > 0);
        }

        #endregion

        #region Update Tests

        [Fact]
        public void Update_WithExistingProduct_ShouldReturnSuccessResult()
        {
            // Arrange
            var repository = CreateRepository();
            var product = Product.Create("Original Name", 100m, 10).Value!;
            repository.Add(product);

            var updatedProduct = product.Rename("Updated Name").Value!;

            // Act
            var result = repository.Update(updatedProduct);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Updated Name", result.Value!.Name);

            // Verify the update persisted
            var retrievedProduct = repository.GetById(product.Id);
            Assert.Equal("Updated Name", retrievedProduct.Value!.Name);
        }

        [Fact]
        public void Update_WithNonExistentProduct_ShouldReturnFailureResult()
        {
            // Arrange
            var repository = CreateRepository();
            var product = Product.Create("Test Product", 100m, 10).Value!;
            // Note: Not adding the product to repository

            // Act
            var result = repository.Update(product);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("PRODUCT_NOT_FOUND", result.Error.Code);
            Assert.Contains(product.Id.ToString(), result.Error.Message);
        }

        [Fact]
        public void Update_ShouldReplaceExistingProduct()
        {
            // Arrange
            var repository = CreateRepository();
            var originalProduct = Product.Create("Original", 100m, 10).Value!;
            repository.Add(originalProduct);

            var updatedProduct = originalProduct
                .UpdatePrice(200m)
                .Bind(p => p.UpdateStock(5))
                .Bind(p => p.Rename("Updated"))
                .Value!;

            // Act
            var updateResult = repository.Update(updatedProduct);
            var retrievedProduct = repository.GetById(originalProduct.Id);

            // Assert
            Assert.True(updateResult.IsSuccess);
            Assert.True(retrievedProduct.IsSuccess);
            Assert.Equal("Updated", retrievedProduct.Value!.Name);
            Assert.Equal(200m, retrievedProduct.Value.Price);
            Assert.Equal(15, retrievedProduct.Value.Stock); // 10 + 5
        }

        #endregion

        #region Delete Tests

        [Fact]
        public void Delete_WithExistingProduct_ShouldReturnSuccessResult()
        {
            // Arrange
            var repository = CreateRepository();
            var product = Product.Create("Test Product", 100m, 10).Value!;
            repository.Add(product);

            // Act
            var result = repository.Delete(product.Id);

            // Assert
            Assert.True(result.IsSuccess);

            // Verify the product is actually deleted
            var getResult = repository.GetById(product.Id);
            Assert.False(getResult.IsSuccess);
            Assert.Equal("PRODUCT_NOT_FOUND", getResult.Error.Code);
        }

        [Fact]
        public void Delete_WithNonExistentProduct_ShouldReturnFailureResult()
        {
            // Arrange
            var repository = CreateRepository();
            var nonExistentId = Guid.NewGuid();

            // Act
            var result = repository.Delete(nonExistentId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("PRODUCT_NOT_FOUND", result.Error.Code);
            Assert.Contains(nonExistentId.ToString(), result.Error.Message);
        }

        [Fact]
        public void Delete_ShouldNotAffectOtherProducts()
        {
            // Arrange
            var repository = CreateRepository();
            var product1 = Product.Create("Product 1", 100m, 10).Value!;
            var product2 = Product.Create("Product 2", 200m, 20).Value!;
            repository.Add(product1);
            repository.Add(product2);

            // Act
            var deleteResult = repository.Delete(product1.Id);

            // Assert
            Assert.True(deleteResult.IsSuccess);

            // Verify product1 is deleted but product2 remains
            var getResult1 = repository.GetById(product1.Id);
            var getResult2 = repository.GetById(product2.Id);

            Assert.False(getResult1.IsSuccess);
            Assert.True(getResult2.IsSuccess);
            Assert.Equal(product2.Id, getResult2.Value!.Id);
        }

        #endregion

        #region FindByName Tests

        [Fact]
        public void FindByName_WithExactMatch_ShouldReturnMatchingProducts()
        {
            // Arrange
            var repository = CreateRepository();
            var product = Product.Create("Gaming Laptop", 1000m, 5).Value!;
            repository.Add(product);

            // Act
            var result = repository.FindByName("Gaming Laptop");

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            var products = result.Value.ToList();
            Assert.Contains(products, p => p.Id == product.Id);
        }

        [Fact]
        public void FindByName_WithPartialMatch_ShouldReturnMatchingProducts()
        {
            // Arrange
            var repository = CreateRepository();
            var laptop = Product.Create("Gaming Laptop", 1000m, 5).Value!;
            var mouse = Product.Create("Gaming Mouse", 50m, 20).Value!;
            var chair = Product.Create("Office Chair", 200m, 10).Value!;
            
            repository.Add(laptop);
            repository.Add(mouse);
            repository.Add(chair);

            // Act
            var result = repository.FindByName("Gaming");

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            var products = result.Value.ToList();
            Assert.Equal(2, products.Count);
            Assert.Contains(products, p => p.Id == laptop.Id);
            Assert.Contains(products, p => p.Id == mouse.Id);
            Assert.DoesNotContain(products, p => p.Id == chair.Id);
        }

        [Fact]
        public void FindByName_IsCaseInsensitive()
        {
            // Arrange
            var repository = CreateRepository();
            var product = Product.Create("Gaming Laptop", 1000m, 5).Value!;
            repository.Add(product);

            // Act
            var result = repository.FindByName("gaming");

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            var products = result.Value.ToList();
            Assert.Contains(products, p => p.Id == product.Id);
        }

        [Fact]
        public void FindByName_WithNonExistentName_ShouldReturnEmptyResult()
        {
            // Arrange
            var repository = CreateRepository();

            // Act
            var result = repository.FindByName("NonExistentProduct");

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value);
        }

        [Fact]
        public void FindByName_WithEmptyName_ShouldReturnFailureResult()
        {
            // Arrange
            var repository = CreateRepository();

            // Act
            var result = repository.FindByName("");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("INVALID_SEARCH", result.Error.Code);
            Assert.Equal("Search name cannot be empty.", result.Error.Message);
        }

        [Fact]
        public void FindByName_WithWhitespaceOnlyName_ShouldReturnFailureResult()
        {
            // Arrange
            var repository = CreateRepository();

            // Act
            var result = repository.FindByName("   ");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("INVALID_SEARCH", result.Error.Code);
            Assert.Equal("Search name cannot be empty.", result.Error.Message);
        }

        [Fact]
        public void FindByName_WithNullName_ShouldReturnFailureResult()
        {
            // Arrange
            var repository = CreateRepository();

            // Act
            var result = repository.FindByName(null!);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("INVALID_SEARCH", result.Error.Code);
        }

        #endregion

        #region Seeded Data Tests

        [Fact]
        public void Constructor_ShouldSeedInitialData()
        {
            // Arrange & Act
            var repository = CreateRepository();
            var allProducts = repository.GetAll();

            // Assert
            Assert.True(allProducts.IsSuccess);
            var products = allProducts.Value!.ToList();
            Assert.True(products.Count >= 10); // Should have at least the seeded products

            // Verify some expected seeded products exist
            Assert.Contains(products, p => p.Name.Contains("Laptop"));
            Assert.Contains(products, p => p.Name.Contains("Mouse"));
            Assert.Contains(products, p => p.Name.Contains("Keyboard"));
        }

        [Fact]
        public void SeededData_ShouldBeValidProducts()
        {
            // Arrange
            var repository = CreateRepository();
            var allProducts = repository.GetAll();

            // Act & Assert
            Assert.True(allProducts.IsSuccess);
            var products = allProducts.Value!.ToList();

            foreach (var product in products)
            {
                Assert.NotEqual(Guid.Empty, product.Id);
                Assert.False(string.IsNullOrWhiteSpace(product.Name));
                Assert.True(product.Price >= 0);
                Assert.True(product.Stock >= 0);
            }
        }

        #endregion

        #region Integration Tests

        [Fact]
        public void CompleteRepositoryWorkflow_ShouldWorkCorrectly()
        {
            // Arrange
            var repository = CreateRepository();
            var initialCount = repository.GetAll().Value!.Count();

            // Act & Assert - Add
            var product = Product.Create("Integration Test Product", 99.99m, 15).Value!;
            var addResult = repository.Add(product);
            Assert.True(addResult.IsSuccess);

            // Act & Assert - GetAll should include new product
            var allProductsAfterAdd = repository.GetAll();
            Assert.Equal(initialCount + 1, allProductsAfterAdd.Value!.Count());

            // Act & Assert - GetById
            var getResult = repository.GetById(product.Id);
            Assert.True(getResult.IsSuccess);
            Assert.Equal(product.Name, getResult.Value!.Name);

            // Act & Assert - Update
            var updatedProduct = product.UpdatePrice(149.99m).Value!;
            var updateResult = repository.Update(updatedProduct);
            Assert.True(updateResult.IsSuccess);
            Assert.Equal(149.99m, updateResult.Value!.Price);

            // Act & Assert - FindByName
            var searchResult = repository.FindByName("Integration");
            Assert.True(searchResult.IsSuccess);
            Assert.Contains(searchResult.Value!, p => p.Id == product.Id);

            // Act & Assert - Delete
            var deleteResult = repository.Delete(product.Id);
            Assert.True(deleteResult.IsSuccess);

            // Act & Assert - Verify deletion
            var getAfterDeleteResult = repository.GetById(product.Id);
            Assert.False(getAfterDeleteResult.IsSuccess);

            // Act & Assert - GetAll should return to original count
            var allProductsAfterDelete = repository.GetAll();
            Assert.Equal(initialCount, allProductsAfterDelete.Value!.Count());
        }

        #endregion
    }
}
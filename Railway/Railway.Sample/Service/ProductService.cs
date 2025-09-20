using Railway.Core;
using Railway.Sample.Model;
using Railway.Sample.Repository;

namespace Railway.Sample.Service
{
    public class ProductService
    {
        private readonly ProductRepository _repository;

        public ProductService(ProductRepository repository)
        {
            _repository = repository;
        }

        public Result<Product> Create(string name, decimal price, int stock)
        {
            return Product.Create(name, price, stock)
                .Bind(product => _repository.Add(product));
        }

        public Result<Product> GetById(Guid id)
        {
            return _repository.GetById(id);
        }

        public Result<IEnumerable<Product>> GetAll()
        {
            return _repository.GetAll();
        }

        public Result<Product> UpdatePrice(Guid productId, decimal newPrice)
        {
            return _repository.GetById(productId)
                .Bind(product => product.UpdatePrice(newPrice))
                .Bind(updatedProduct => _repository.Update(updatedProduct));
        }

        public Result<Product> UpdateStock(Guid productId, int quantity)
        {
            return _repository.GetById(productId)
                .Bind(product => product.UpdateStock(quantity))
                .Bind(updatedProduct => _repository.Update(updatedProduct));
        }

        public Result<Product> RenameProduct(Guid productId, string newName)
        {
            return _repository.GetById(productId)
                .Bind(product => product.Rename(newName))
                .Bind(updatedProduct => _repository.Update(updatedProduct));
        }

        public Result DeleteProduct(Guid productId)
        {
            return _repository.Delete(productId);
        }

        public Result<IEnumerable<Product>> SearchByName(string name)
        {
            return _repository.FindByName(name);
        }
    }
}

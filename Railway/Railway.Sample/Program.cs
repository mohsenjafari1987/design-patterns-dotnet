using Railway.Core;
using Railway.Sample.UserInterface;

namespace Railway.Sample;

class Program
{
    static void Main(string[] args)
    {
        var productService = new Service.ProductService(new Repository.ProductRepository());
        var userInterface = new ProductUserInterface(productService);
        
        userInterface.Start();
    }
}

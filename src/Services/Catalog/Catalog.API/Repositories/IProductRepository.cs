using Catalog.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.API.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<IEnumerable<Product>> GetProductsByName(string name);
        Task<IEnumerable<Product>> GetProductsByCategory(string categoryName);
        Task<Product> GetProduct(string id);

        Task CreateProduct(Product product);
        Task<bool> UpdateProduct(string id, Product product);
        Task<bool> DeleteProduct(string id);
    }
}

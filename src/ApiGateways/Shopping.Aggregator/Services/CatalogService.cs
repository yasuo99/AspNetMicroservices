using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _httpClient;
        public CatalogService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<CatalogModel>> GetCatalog()
        {
            var response = await _httpClient.GetAsync("/api/v1/Catalog");

            List<CatalogModel> catalog = await response.ReadContentAs<List<CatalogModel>>();
            return catalog;
        }

        public async Task<CatalogModel> GetCatalog(string id)
        {
            var response = await _httpClient.GetAsync($"/api/v1/Catalog/{id}");

            CatalogModel catalog = await response.ReadContentAs<CatalogModel>();
            return catalog;
        }

        public async Task<IEnumerable<CatalogModel>> GetCatalogWithCategory(string category)
        {
            var response = await _httpClient.GetAsync($"/api/v1/Catalog/GetProductsByCategory/{category}");

            List<CatalogModel> catalog = await response.ReadContentAs<List<CatalogModel>>();
            return catalog;
            throw new NotImplementedException();
        }
    }
}

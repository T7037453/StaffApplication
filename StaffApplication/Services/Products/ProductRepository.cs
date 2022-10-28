
using System.Net;

namespace StaffApplication.Services.Products
{
    public class ProductRepository : IProductsRepository
    {
        private readonly HttpClient _client;
        public ProductRepository(HttpClient client)
        {
            client.BaseAddress = new System.Uri("http://localhost:7061/");
            client.Timeout = TimeSpan.FromSeconds(5);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client = client;
        }
        public async Task<ProductDto> GetProductAsync(int id)
        {
           var response = await _client.GetAsync("api/products/" + id);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;

            }
            response.EnsureSuccessStatusCode();
            var product = await response.Content.ReadAsAsync<ProductDto>();
            return product;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync(string name)
        {
            var uri = "api/products?brand=TestBrand1";
            if (name != null)
            {
                uri = uri + "&brand =" + name;

            }
            var response= await _client.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var products = await response.Content.ReadAsAsync<IEnumerable<ProductDto>>();
            return products;
        }
    }
}

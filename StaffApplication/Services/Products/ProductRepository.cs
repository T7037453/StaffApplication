using System;
using System.Net;
using System.Net.Http.Headers;

namespace StaffApplication.Services.Products;

public class ProductRepository : IProductsRepository
{
    //private readonly HttpClient _client;
    private readonly IHttpClientFactory _clientFactory;
    private readonly IConfiguration _configuration;


    public ProductRepository(IHttpClientFactory clientFactory,
                             IConfiguration configuration)
    {
        //var baseUrl = configuration["WebServices:Products:BaseURL"];
        //client.BaseAddress = new System.Uri(baseUrl);
        //client.Timeout = TimeSpan.FromSeconds(5);
        //client.DefaultRequestHeaders.Add("Accept", "application/json");
        //_client = client;

        _clientFactory = clientFactory;
        _configuration = configuration;
    }

    record TokenDto(string access_token, string token_type, int expires_in)
    public async Task<ProductDto> GetProductAsync(int id)
    {
        //var response = await _client.GetAsync("/products/" + id);
        var tokenClient = _clientFactory.CreateClient();

        var authBaseAddress = _configuration["Auth:Authority"];
        tokenClient.BaseAddress = new Uri(authBaseAddress);

        var tokenParams = new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
            { "client_id", _configuration["Auth:ClientId"] },
            { "client_secret", _configuration["Auth:ClientSecret"] },
            { "audience", _configuration["Services:Values:AuthAudience"] },
        };

        var tokenFrom = new FormUrlEncodedContent(tokenParams);
        var tokenResponse = await tokenClient.PostAsync("oauth/token", tokenFrom);
        tokenResponse.EnsureSuccessStatusCode();
        var tokenInfo = await tokenResponse.Content.ReadFromJsonAsync<TokenDto>();

        var client = _clientFactory.CreateClient();

        var serviceBaseAddress = _configuration["Services:Values:BaseAddress"];
        client.BaseAddress = new Uri(serviceBaseAddress);
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", tokenInfo?.access_token);

        var response = await client.GetAsync("api/values");
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsAsync<ProductDto>();
        return result;


    //    if (response.StatusCode == HttpStatusCode.NotFound)
    //    {
    //        return null;

    //    }
    //    //response.EnsureSuccessStatusCode();
    //    var product = await response.Content.ReadAsAsync<ProductDto>();
    //    return product;
    //}




    public async Task<IEnumerable<ProductDto>> GetProductsAsync(string name)
    {
        var uri = "/products?description=Test_Desc";
        if (name != null)
        {
            uri = uri + "&name=" + name;

        }
        var response = await _client.GetAsync(uri);
        response.EnsureSuccessStatusCode();
        var products = await response.Content.ReadAsAsync<IEnumerable<ProductDto>>();
        return products;
    }
}

using Microsoft.Extensions.Caching.Memory;
using Polly;
using Polly.Retry;
using StaffApplication.Services.Cache;
using System;
using System.Net;
using System.Net.Http.Headers;

namespace StaffApplication.Services.Reviews;

public class ReviewsService : IReviewsService
{
    //private readonly HttpClient _client;
    private readonly IHttpClientFactory _clientFactory;
    private readonly IConfiguration _configuration;
    private readonly IMemoryCache _cache;
    private readonly IAsyncPolicy<HttpResponseMessage> _retryPolicy =
        Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .OrResult(x => x.StatusCode is >= HttpStatusCode.InternalServerError || x.StatusCode == HttpStatusCode.RequestTimeout)
            .RetryAsync(5);


    public ReviewsService(IHttpClientFactory clientFactory,
                             IConfiguration configuration,
                             IMemoryCache cache)
    {
        //var baseUrl = configuration["WebServices:Products:BaseURL"];
        //client.BaseAddress = new System.Uri(baseUrl);
        //client.Timeout = TimeSpan.FromSeconds(5);
        //client.DefaultRequestHeaders.Add("Accept", "application/json");
        //_client = client;

        _clientFactory = clientFactory;
        _configuration = configuration;
        _cache = cache;
    }

    record TokenDto(string access_token, string token_type, int expires_in);
    public async Task<ReviewDto> GetReviewAsync(int id)
    {
        if (_cache.TryGetValue("ProductsList", out IEnumerable<ReviewDto?> reviewList)) { return reviewList.ToList().FirstOrDefault(pl => pl.Id == id); };
        //var response = await _client.GetAsync("/products/" + id);
        var tokenClient = _clientFactory.CreateClient();

        var authBaseAddress = _configuration["Auth:Authority"];
        tokenClient.BaseAddress = new Uri(authBaseAddress);

        var tokenParams = new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
            { "client_id", _configuration["Auth:ClientId"] },
            { "client_secret", _configuration["Auth:ClientSecret"] },
            { "audience", _configuration["WebServices:Reviews:AuthAudience"] },
        };

        var tokenFrom = new FormUrlEncodedContent(tokenParams);
        var tokenResponse = await tokenClient.PostAsync("oauth/token", tokenFrom);
        tokenResponse.EnsureSuccessStatusCode();
        var tokenInfo = await tokenResponse.Content.ReadFromJsonAsync<TokenDto>();

        var client = _clientFactory.CreateClient();

        var serviceBaseAddress = _configuration["WebServices:Reviews:BaseURL"];
        client.BaseAddress = new Uri(serviceBaseAddress);
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", tokenInfo?.access_token);


        HttpResponseMessage response = await _retryPolicy.ExecuteAsync(() => client.GetAsync("/reviews/" + id));
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsAsync<ReviewDto>();
        return result;


        //    if (response.StatusCode == HttpStatusCode.NotFound)
        //    {
        //        return null;

        //    }
        //    //response.EnsureSuccessStatusCode();
        //    var product = await response.Content.ReadAsAsync<ProductDto>();
        //    return product;
        //}


    }

    public async Task<IEnumerable<ReviewDto>> GetReviewsAsync(int id)
    {
        var tokenClient = _clientFactory.CreateClient();

        var authBaseAddress = _configuration["Auth:Authority"];
        tokenClient.BaseAddress = new Uri(authBaseAddress);

        var tokenParams = new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id", _configuration["Auth:ClientId"] },
                { "client_secret", _configuration["Auth:ClientSecret"] },
                { "audience", _configuration["WebServices:Reviews:AuthAudience"] },
            };

        var tokenFrom = new FormUrlEncodedContent(tokenParams);
        var tokenResponse = await tokenClient.PostAsync("oauth/token", tokenFrom);
        tokenResponse.EnsureSuccessStatusCode();
        var tokenInfo = await tokenResponse.Content.ReadFromJsonAsync<TokenDto>();

        var client = _clientFactory.CreateClient();

        var serviceBaseAddress = _configuration["WebServices:Reviews:BaseURL"];

        var uri = "/reviews";
        if (id != null)
        {
            uri = uri + "?id=" + id;
        }

        client.BaseAddress = new Uri(serviceBaseAddress);
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", tokenInfo?.access_token);

        HttpResponseMessage response = await _retryPolicy.ExecuteAsync(() => client.GetAsync(uri));
        //response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsAsync<IEnumerable<ReviewDto>>();
        return result;

    }

    public Task<ReviewDto> PostReviewAsync(ReviewDto review)
    {
        throw new NotImplementedException();
    }
}


using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Polly;
using StaffApplication.Models;
using StaffApplication.Services.Products;
using System.Net;
using System.Net.Http.Headers;

namespace StaffApplication.Services.Accounts
{
    public class AccountService : IAccountsService
    {

        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;
        private readonly IAsyncPolicy<HttpResponseMessage> _retryPolicy =
        Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .OrResult(x => x.StatusCode is >= HttpStatusCode.InternalServerError || x.StatusCode == HttpStatusCode.RequestTimeout)
            .RetryAsync(5);

        public AccountService(IHttpClientFactory clientFactory,
                             IConfiguration configuration,
                             IMemoryCache cache)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
            _cache = cache;
        }
        record TokenDto(string access_token, string token_type, int expires_in);
        public async Task<IEnumerable<AccountDto>> GetAccountsAsync()
        {
            var tokenClient = _clientFactory.CreateClient();

            var authBaseAddress = _configuration["Auth:Authority"];
            tokenClient.BaseAddress = new Uri(authBaseAddress);

            var tokenParams = new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id", _configuration["Auth:ClientId"] },
                { "client_secret", _configuration["Auth:ClientSecret"] },
                { "audience", _configuration["WebServices:Accounts:AuthAudience"] },
            };

            var tokenFrom = new FormUrlEncodedContent(tokenParams);
            var tokenResponse = await tokenClient.PostAsync("oauth/token", tokenFrom);
            tokenResponse.EnsureSuccessStatusCode();
            var tokenInfo = await tokenResponse.Content.ReadFromJsonAsync<TokenDto>();

            var client = _clientFactory.CreateClient();

            var serviceBaseAddress = _configuration["WebServices:Accounts:BaseURL"];
            client.BaseAddress = new Uri(serviceBaseAddress);
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", tokenInfo?.access_token);

            HttpResponseMessage response = await _retryPolicy.ExecuteAsync(() => client.GetAsync("/accounts"));
            //response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsAsync<IEnumerable<AccountDto>>();
            return result;
        }

        public async Task<AccountDto> GetAccountAsync(string id)
        {
            var tokenClient = _clientFactory.CreateClient();

            var authBaseAddress = _configuration["Auth:Authority"];
            tokenClient.BaseAddress = new Uri(authBaseAddress);

            var tokenParams = new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id", _configuration["Auth:ClientId"] },
                { "client_secret", _configuration["Auth:ClientSecret"] },
                { "audience", _configuration["WebServices:Accounts:AuthAudience"] },
            };

            var tokenFrom = new FormUrlEncodedContent(tokenParams);
            var tokenResponse = await tokenClient.PostAsync("oauth/token", tokenFrom);
            tokenResponse.EnsureSuccessStatusCode();
            var tokenInfo = await tokenResponse.Content.ReadFromJsonAsync<TokenDto>();

            var client = _clientFactory.CreateClient();

            var serviceBaseAddress = _configuration["WebServices:Accounts:BaseURL"];
            client.BaseAddress = new Uri(serviceBaseAddress);
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", tokenInfo?.access_token);

            HttpResponseMessage response = await _retryPolicy.ExecuteAsync(() => client.GetAsync("/accounts/" + id));
            //response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsAsync<AccountDto>();
            return result;
        }

        public async Task<AccountsCreationViewModel> CreateAccountAsync(AccountsCreationViewModel account)
        {
            var tokenClient = _clientFactory.CreateClient();

            var authBaseAddress = _configuration["Auth:Authority"];
            tokenClient.BaseAddress = new Uri(authBaseAddress);

            var tokenParams = new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id", _configuration["Auth:ClientId"] },
                { "client_secret", _configuration["Auth:ClientSecret"] },
                { "audience", _configuration["WebServices:Accounts:AuthAudience"] },
            };

            var tokenFrom = new FormUrlEncodedContent(tokenParams);
            var tokenResponse = await tokenClient.PostAsync("oauth/token", tokenFrom);
            tokenResponse.EnsureSuccessStatusCode();
            var tokenInfo = await tokenResponse.Content.ReadFromJsonAsync<TokenDto>();

            var client = _clientFactory.CreateClient();

            var AccountParams = new Dictionary<string, string>
            {
                {"user_id", account.user_id  },
                {"username", account.username},
                {"name", account.name },
                {"nickname", account.nickname },
                {"email", account.email},
                {"password", account.password},
                {"connection", "Username-Password-Authentication"}

            };

            var serviceBaseAddress = _configuration["WebServices:Accounts:BaseURL"];
            client.BaseAddress = new Uri(serviceBaseAddress);
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", tokenInfo?.access_token);

            HttpResponseMessage response = await _retryPolicy.ExecuteAsync(() => client.PostAsJsonAsync("/accounts", AccountParams));
            //response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsAsync<AccountsCreationViewModel>();
            return result;
        }

        public async Task<AccountDto> DeleteAccountAsync(string id)
        {
            var tokenClient = _clientFactory.CreateClient();

            var authBaseAddress = _configuration["Auth:Authority"];
            tokenClient.BaseAddress = new Uri(authBaseAddress);

            var tokenParams = new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id", _configuration["Auth:ClientId"] },
                { "client_secret", _configuration["Auth:ClientSecret"] },
                { "audience", _configuration["WebServices:Accounts:AuthAudience"] },
            };

            var tokenFrom = new FormUrlEncodedContent(tokenParams);
            var tokenResponse = await tokenClient.PostAsync("oauth/token", tokenFrom);
            tokenResponse.EnsureSuccessStatusCode();
            var tokenInfo = await tokenResponse.Content.ReadFromJsonAsync<TokenDto>();

            var client = _clientFactory.CreateClient();

            var serviceBaseAddress = _configuration["WebServices:Accounts:BaseURL"];
            client.BaseAddress = new Uri(serviceBaseAddress);
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", tokenInfo?.access_token);

            HttpResponseMessage response = await _retryPolicy.ExecuteAsync(() => client.DeleteAsync("/accounts/" + id));
            //response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsAsync<AccountDto>();
            return result;
        }

        public async Task<AccountsCreationViewModel> EditAccountAsync(AccountsCreationViewModel account, string id)
        {
            var tokenClient = _clientFactory.CreateClient();

            var authBaseAddress = _configuration["Auth:Authority"];
            tokenClient.BaseAddress = new Uri(authBaseAddress);

            var tokenParams = new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id", _configuration["Auth:ClientId"] },
                { "client_secret", _configuration["Auth:ClientSecret"] },
                { "audience", _configuration["WebServices:Accounts:AuthAudience"] },
            };

            var tokenFrom = new FormUrlEncodedContent(tokenParams);
            var tokenResponse = await tokenClient.PostAsync("oauth/token", tokenFrom);
            tokenResponse.EnsureSuccessStatusCode();
            var tokenInfo = await tokenResponse.Content.ReadFromJsonAsync<TokenDto>();

            var client = _clientFactory.CreateClient();

            var AccountParams = new Dictionary<string, string>
            {
                {"user_id", account.user_id  },
                {"username", account.username},
                {"name", account.name },
                {"nickname", account.nickname },
                {"email", account.email},
                {"password", account.password},
                {"connection", "Username-Password-Authentication"}

            };

            var serviceBaseAddress = _configuration["WebServices:Accounts:BaseURL"];
            client.BaseAddress = new Uri(serviceBaseAddress);
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", tokenInfo?.access_token);

            HttpResponseMessage response = await _retryPolicy.ExecuteAsync(() => client.PutAsJsonAsync("/accounts" + id, AccountParams));
            //response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsAsync<AccountsCreationViewModel>();
            return result;
        }

    }
}




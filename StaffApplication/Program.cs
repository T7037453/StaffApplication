using StaffApplication.Services.Products;
using Auth0.AspNetCore.Authentication;
using Polly;
using Polly.Extensions.Http;
using StaffApplication.Services.Cache;
using StaffApplication.Services.Reviews;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);


if (builder.Environment.IsDevelopment())
{
    builder.Services.AddTransient<IProductsRepository, FakeProductsRepository>();
    builder.Services.AddTransient<IReviewsService, FakeReviewsService>();
}
else
{
    builder.Services.AddHttpClient<ProductRepository>()
                    .AddPolicyHandler(GetRetryPolicy());
    builder.Services.AddTransient<IProductsRepository, ProductRepository>();

    builder.Services.AddHttpClient<ReviewsService>()
                    .AddPolicyHandler(GetRetryPolicy());
    builder.Services.AddTransient<IReviewsService, ReviewsService>();
}

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuth0WebAppAuthentication(options => {
    options.Domain = builder.Configuration["Auth:Domain"];
    options.ClientId = builder.Configuration["Auth:ClientId"];
});


builder.Services.AddSingleton<Cache>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();


app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

 IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
        .WaitAndRetryAsync(5, retryAttempt =>
            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}

//IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
//{
//    return HttpPolicyExtensions
//        .HandleTransientHttpError()
//        .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
//}

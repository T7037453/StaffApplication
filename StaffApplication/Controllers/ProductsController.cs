using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Polly.Retry;
using StaffApplication.Models;
using StaffApplication.Services.Products;
using StaffApplication.Services.Reviews;

namespace StaffApplication.Controllers;

public class ProductsController : Controller
{
    private readonly ILogger _logger;
    private readonly IProductsRepository _productsRepository;
    private readonly IReviewsService _reviewsService;
    private readonly IHttpClientFactory _clientFactory;

    public ProductsController(ILogger<ProductsController> logger, 
                              IProductsRepository productsRepository,
                              IReviewsService reviewsService)
    {
        _logger = logger;
        _productsRepository = productsRepository;
        _reviewsService = reviewsService;
    }

    // GET: /products/
    public async Task<IActionResult> Index([FromQuery] string? name)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);

        }

        IEnumerable<ProductDto> products = null;
        try
        {
            products = await _productsRepository.GetProductsAsync(name);

        }
        catch
        {
            _logger.LogWarning("Exception occured using the Product Repository");
            
            products = Array.Empty<ProductDto>();

        }
        return View(products.ToList());
    }

    // GET: /products/details/{id}
    [Authorize]
    public async Task<IActionResult> Details (int? id)
    {
        if (id == null)
        {
            return BadRequest();
        }

        var product = new ProductDto();
        var ViewModel = new ProductDetailsViewModel();
        IEnumerable<ReviewDto> reviews;

        try
        {
            product = await _productsRepository.GetProductAsync(id.Value);
            ViewModel = new ProductDetailsViewModel();
            ViewModel.product = product;
            
            if (product == null)
            {
                
            }
            

        }
        catch
        {
            _logger.LogWarning("Exception occured using the Products Repository");
            ViewModel.product = product;
        }
        try
        {
            reviews = await _reviewsService.GetReviewsAsync(id.Value);
            ViewModel = new ProductDetailsViewModel();
            ViewModel.Reviews = reviews;

            if (reviews == null)
            {
                
            }
        }
        catch
        {
            _logger.LogWarning("Exception occured using the Reviews Service");
            ViewModel.Reviews = Array.Empty<ReviewDto>();
            
        }

        return View(ViewModel);
    }
}

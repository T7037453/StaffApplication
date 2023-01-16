using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Polly.Retry;
using StaffApplication.Models;
using StaffApplication.Services.Accounts;
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
    public async Task<IActionResult> Index([FromQuery] string? name, bool update)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);

        }

        IEnumerable<ProductDto> products = null;
        try
        {
            products = await _productsRepository.GetProductsAsync(name, update);

        }
        catch
        {
            _logger.LogWarning("Exception occured using the Product Repository");
            
            products = Array.Empty<ProductDto>();

        }
        return View(products.ToList());
    }

    // GET: /products/details/{id}
    [Authorize (Roles ="Management")]
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

    public ActionResult Create()
    {
        var ViewModel = new ProductDto();
        return View(ViewModel);
    }

    // POST: ProductsController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductDto account)
    {
        bool update = false;
        try
        {
            account = await _productsRepository.CreateProductAsync(account);
            update = true;
        }
        catch
        {
            _logger.LogWarning("Exception occured using the Accounts Service");
            update = false;

        }
        return RedirectToAction("Index", update);

    }

    //Get Delete
    public ActionResult Delete(ProductDto product)
    {
        var ViewModel = new ProductDto();
        ViewModel.Id = product.Id;
        return View(ViewModel);
    }

    //Post Delete
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        bool update = false;
        var product = new ProductDto();
        try
        {
            product = await _productsRepository.DeleteProductAsync(id);
            update = true;
        }
        catch
        {
            _logger.LogWarning("Exception occured using the Accounts Service");
            update = false;

        }
        return RedirectToAction("Index", update);

    }

    //Get Edit
    public ActionResult Edit(ProductDto product)
    {
        var ViewModel = new ProductDto();
        ViewModel.Id = product.Id;
        return View(ViewModel);
    }

    //Post Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Product product, int id)
    {
        bool update = false;
        try
        {
            product = await _productsRepository.EditProductAsync(product, id);
            update = true;
        }
        catch
        {
            _logger.LogWarning("Exception occured using the Products Service");
            update = false;

        }
        return RedirectToAction("Index", update);

    }
}

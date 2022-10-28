using System;
using System.Collection.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore;
using Microsoft.AspNetCore.Mvc;
using StaffApplication.Services.Products;

namespace StaffApplication.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ILogger _logger;
        private readonly IProductsRepository _productsRepository;

        public ProductsController(Ilogger<ProductsController> logger, 
                                  IProductsRepository productsRepository)
        {
            _logger = logger;
            _productsRepository = productsRepository;
        }

        // GET: /products/
        public async Task<IActionResult> Index([FromQuery] string? brand)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }

            IEnumberable<ProductDto> products = null;
            try
            {
                products = await _productsRepository.GetProductsAsync(brand);
            }
            catch
            {
                _logger.LogWarning("Exception occured using the Product Repository");
                products = Array.Empty<ProductDto>();
            }
            return View(products.ToList());
        }

        // GET: /products/details/{id}
        public async Task<IActionResult> Details (int id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            try
            {
                var product = await _productsRepository.GetProductsAsync(id.Value);
                if (product == null)
                {
                    return NotFound();
                }
                return View(product);

            }
            catch
            {
                _logger.LogWarning("Exception occured using the Products Repository");
                return StatusCode(StatusCodes.Status503ServiceUnavailable);
            }
        }
    }
}

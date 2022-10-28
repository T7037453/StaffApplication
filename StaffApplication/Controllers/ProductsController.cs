using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StaffApplication.Services.Products;

namespace StaffApplication.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ILogger _logger;
        private readonly IProductsRepository _productsRepository;

        public ProductsController(ILogger<ProductsController> logger, 
                                  IProductsRepository productsRepository)
        {
            _logger = logger;
            _productsRepository = productsRepository;
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
        public async Task<IActionResult> Details (int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            try
            {
                var product = await _productsRepository.GetProductAsync(id.Value);
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

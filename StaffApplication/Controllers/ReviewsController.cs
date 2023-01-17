using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using StaffApplication.Services.Products;
using StaffApplication.Services.Reviews;

namespace StaffApplication.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ILogger _logger;
        private readonly IReviewsService _reviewsService;

        public ReviewsController(ILogger<ReviewsController> logger,
                                 IReviewsService reviewsService)
        {
            _logger = logger;
            _reviewsService = reviewsService;
        }
        // GET: ReviewsController
        public async Task<IActionResult> Index(int id, bool update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }

            IEnumerable<ReviewDto> reviews = null;
            update = false;
            try
            {
                reviews = await _reviewsService.GetReviewsAsync(id, update);
                update = true;
            }
            catch
            {
                _logger.LogWarning("Exception occured using the Reviews Service");

                reviews = Array.Empty<ReviewDto>();

            }
            return View(reviews.ToList());
        }

        // GET: ReviewsController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var review = new ReviewDto();
            try
            {
                review = await _reviewsService.GetReviewAsync(id);
         
            }
            catch
            {

            }
            
            return View(review);
        }

        // GET: ReviewsController/Create
        public async Task<IActionResult> Create(int productId)
        {
            var ViewModel = new ReviewDto();
            ViewModel.productId = productId;
            return View(ViewModel);
        }

        // POST: ReviewsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReviewDto review)
        {
            bool update = false;
            try
            {
                review = await _reviewsService.CreateReviewAsync(review);
            }
            catch
            {
                _logger.LogWarning("Exception occured using the Reviews Service");
                update = false;
            }
            return RedirectToAction("Index", "Products");
        }

        // GET: ReviewsController/Edit/5
        public ActionResult Edit(ReviewDto review)
        {
            var ViewModel = new ReviewDto();
            ViewModel.productId = review.productId;
            return View(ViewModel);
        }

        // POST: ReviewsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async  Task<IActionResult> Edit(ReviewDto review, int id)
        {
            bool update = false;


            try
            {
                review = await _reviewsService.EditReviewAsync(review, id);
                update = true;

            }
            catch
            {
                _logger.LogWarning("Exception occured using the Reviews Service");
                update = false;
            }

            return RedirectToAction("Index", "Products");
        }

        // GET: ReviewsController/Delete/5
        public ActionResult Delete(ReviewDto review)
        {
            var ViewModel = new ReviewDto();
            ViewModel.productId = review.productId;
            return View(ViewModel);
        }

        // POST: ReviewsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int productId)
        {
            bool update = false;
            var review = new ReviewDto();
            

            try
            {
                review = await _reviewsService.DeleteReviewAsync(id);
                update = true;
                
            }
            catch
            {
                _logger.LogWarning("Exception occured using the Reviews Service");
                update = false;
            }

            return RedirectToAction("Index", "Products");
        }
    }
}

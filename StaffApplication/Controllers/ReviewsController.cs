using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Index(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }

            IEnumerable<ReviewDto> reviews = null;
            try
            {
                reviews = await _reviewsService.GetReviewsAsync(id);
            }
            catch
            {
                _logger.LogWarning("Exception occured using the Reviews Service");

                reviews = Array.Empty<ReviewDto>();

            }
            return View(reviews.ToList());
        }

        // GET: ReviewsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ReviewsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReviewsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ReviewsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ReviewsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ReviewsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ReviewsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

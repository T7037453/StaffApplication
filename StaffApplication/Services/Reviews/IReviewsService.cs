using StaffApplication.Services.Products;

namespace StaffApplication.Services.Reviews
{
    public interface IReviewsService
    {
        Task<IEnumerable<ReviewDto>> GetReviewsAsync(int id);

        Task<ReviewDto> GetReviewAsync(int id);

        Task<ReviewDto> PostReviewAsync(ReviewDto review);
    }
}

using StaffApplication.Services.Products;

namespace StaffApplication.Services.Reviews
{
    public interface IReviewsService
    {
        Task<IEnumerable<ReviewDto>> GetReviewsAsync(int id);

        Task<ReviewDto> GetReviewAsync(int id);

        Task<ReviewDto> CreateReviewAsync(ReviewDto review);

        Task<ReviewDto> DeleteReviewAsync(int id);

        Task<ReviewDto> EditReviewAsync(ReviewDto review, int id);
    }
}

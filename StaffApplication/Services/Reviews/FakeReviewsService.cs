using System;

namespace StaffApplication.Services.Reviews;


public class FakeReviewsService : IReviewsService
{
    private readonly ReviewDto[] _reviews =
    {
        new ReviewDto {Id = 1, Title = "TestReview1", productId = 1, productReviewContent = "TestReview1", productReviewRating = 1, displayReview = true, anonymized = false, firstName = "TestReviewUser1"},
        new ReviewDto {Id = 2, Title = "TestReview2", productId = 1, productReviewContent = "TestReview2", productReviewRating = 1, displayReview = true, anonymized = false, firstName = "TestReviewUser2"},
        new ReviewDto {Id = 3, Title = "TestReview3", productId = 2, productReviewContent = "TestReview3", productReviewRating = 3, displayReview = true, anonymized = false, firstName = "TestReviewUser3"},
        new ReviewDto {Id = 4, Title = "TestReview4", productId = 2, productReviewContent = "TestReview4", productReviewRating = 3, displayReview = true, anonymized = false, firstName = "TestReviewUser4"},
        new ReviewDto {Id = 5, Title = "TestReview5", productId = 3, productReviewContent = "TestReview5", productReviewRating = 5, displayReview = true, anonymized = false, firstName = "TestReviewUser5"},
        new ReviewDto {Id = 6, Title = "TestReview6", productId = 3, productReviewContent = "TestReview6", productReviewRating = 5, displayReview = true, anonymized = false, firstName = "TestReviewUser6"},
    };

    public Task<ReviewDto> CreateReviewAsync(ReviewDto review)
    {
        throw new NotImplementedException();
    }

    public Task<ReviewDto> DeleteReviewAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ReviewDto> EditReviewAsync(ReviewDto review, int id)
    {
        throw new NotImplementedException();
    }

    public Task<ReviewDto> GetReviewAsync(int id)
    {
        var review = _reviews.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(review);
    }


    public Task<IEnumerable<ReviewDto>> GetReviewsAsync(int id, bool update)
    {
        var reviews = _reviews.AsEnumerable();
        if (id != null)
        {
            reviews = null;
        }
        return Task.FromResult(reviews);
    }


}

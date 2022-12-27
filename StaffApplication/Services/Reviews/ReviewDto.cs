namespace StaffApplication.Services.Reviews
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int productId { get; set; }
        public string productReviewContent { get; set; }
        public int productReviewRating { get; set; }
        public bool displayReview { get; set; }
        public bool anonymized { get; set; }
        public string firstName { get; set; }
    }
}

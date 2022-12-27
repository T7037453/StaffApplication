using StaffApplication.Services.Products;
using StaffApplication.Services.Reviews;

namespace StaffApplication.Models
{
    public class ProductDetailsViewModel
    {
        public ProductDto product { get; set; }
        public IEnumerable<ReviewDto> Reviews { get; set; }
        public ReviewDto Review { get; set; }

    }
}

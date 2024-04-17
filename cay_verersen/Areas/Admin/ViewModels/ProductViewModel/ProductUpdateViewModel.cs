using System.ComponentModel.DataAnnotations;

namespace cay_verersen.Areas.Admin.ViewModels.ProductViewModel
{
    public class ProductUpdateViewModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public IFormFile? Image { get; set; }
        public int CategoryId { get; set; }
    }
}

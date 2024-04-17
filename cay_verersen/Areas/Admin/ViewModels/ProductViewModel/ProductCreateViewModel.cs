using System.ComponentModel.DataAnnotations;

namespace cay_verersen.Areas.Admin.ViewModels.ProductViewModel
{
    public class ProductCreateViewModel
    {
        [Required(ErrorMessage = "Productun adını daxil etmək lazımdır")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Productun description daxil etmək lazımdır")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Productun qiymətini daxil etmək lazımdır")]
        public double Price { get; set; }
        [Required]
        public IFormFile Image { get; set; }
        public int CategoryId { get; set; }
    }
}

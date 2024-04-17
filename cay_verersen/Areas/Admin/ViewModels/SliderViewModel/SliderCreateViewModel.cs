using System.ComponentModel.DataAnnotations;

namespace cay_verersen.Areas.Admin.ViewModels.SliderViewModel
{
    public class SliderCreateViewModel
    {
        [Required(ErrorMessage = "Sliderin title daxil edin")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Sliderin description daxil edin")]
        public string Description { get; set; }
        [Required]
        public IFormFile Image { get; set; }
    }
}

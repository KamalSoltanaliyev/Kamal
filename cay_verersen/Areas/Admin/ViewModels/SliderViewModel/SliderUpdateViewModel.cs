using System.ComponentModel.DataAnnotations;

namespace cay_verersen.Areas.Admin.ViewModels.SliderViewModel
{
    public class SliderUpdateViewModel
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile? Image { get; set; }
    }
}

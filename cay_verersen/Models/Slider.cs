using System.ComponentModel.DataAnnotations.Schema;

namespace cay_verersen.Models
{
    public class Slider
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        //[NotMapped]
        public string Image { get; set; }
    }
}

using AdvanceEshop.Repository.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdvanceEshop.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [StringLength(150)]
        public string CategoryName { get; set; }
        [StringLength(300)]
        public string CategoryPhoto { get; set; } = "noname.jpg";
        public int CategoryOrder { get; set; }

        [NotMapped]
        [FileExtension]
        public IFormFile CategoryUpLoadImage { get; set; }
    }
}

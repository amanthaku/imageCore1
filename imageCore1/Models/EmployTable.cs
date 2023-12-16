using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace imageCore1.Models
{
    public class EmployTable
    {
        public int Id { get; set; }
        [Required]

        public string Brand { get; set; }
        [Required]
        public string Description { get; set; }

        public string path { get; set; }
        [NotMapped]
        [Display (Name ="Choose image ")]
        public  IFormFile  ImagePath { get; set; }
    }
}

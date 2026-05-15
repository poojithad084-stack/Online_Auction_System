using System.ComponentModel.DataAnnotations;

namespace CategoryService.Models
{
    public class Category
    {
        [Key]
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public string status { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}

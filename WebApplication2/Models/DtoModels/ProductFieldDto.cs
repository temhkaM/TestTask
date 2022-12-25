using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models.DtoModels
{
    public class ProductFieldDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string Description { get; set; }

        [Required]
        public int CategoryId { get; set; }
        
        public CategoryEditDto Category { get; set; }

        public List<ProductCategoryFieldDto> Fields { get; set; }
    }
}

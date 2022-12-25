using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models.DtoModels
{
    public class ProductEditDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string Description { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
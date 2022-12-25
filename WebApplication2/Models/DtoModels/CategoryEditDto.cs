using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models.DtoModels
{
    public class CategoryEditDto
    {
        [Required]
        public string Name { get; set; }

        
        public string Description { get; set; }
    }
}

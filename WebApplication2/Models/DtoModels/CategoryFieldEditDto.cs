using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models.DtoModels
{
    public class CategoryFieldEditDto
    {
        [Required]
        [MaxLength(12, ErrorMessage = "Строка должна быть короче 12 символов")]
        public string FieldName { get; set; }
    }
}

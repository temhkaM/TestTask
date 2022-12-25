using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class CategoryField
    {
        [Column("CategoryFieldId")]
        [Key]
        public int Id { get; set; } = default;

        [Required]
        [MaxLength(12, ErrorMessage = "Строка должна быть короче 12 символов")]
        public string FieldName { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
    }
}

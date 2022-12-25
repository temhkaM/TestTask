using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication2.Models.DtoModels;

namespace WebApplication2.Models
{
    public class ProductCategoryField
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [ForeignKey(nameof(CategoryField))]
        public int CategoryFieldId { get; set; }
        public CategoryField CategoryField { get; set; }

        public string FieldValue { get; set; } = "";
    }
}

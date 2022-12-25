using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication2.Models.DtoModels;

namespace WebApplication2.Models
{
    public class ProductCategoryFieldDto
    {
        [Required]
        public int CategoryFieldId { get; set; }

        [Required]
        public string CategoryField { get; set; }

        public string FieldValue { get; set; }
    }
}

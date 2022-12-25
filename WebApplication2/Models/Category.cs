using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class Category
    {
        [Column("CategoryId")]
        [Key]
        public int Id { get; set; } 

        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }

        public List<CategoryField> CategoryFields { get; set; } = new List<CategoryField>();
        //public ICollection<Product> Products { get; set; }
    }
}

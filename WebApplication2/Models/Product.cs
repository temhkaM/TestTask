using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebApplication2.Models
{
    public class Product
    {
        [Column("ProductId")]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(24, ErrorMessage = "������ ������ ���� ������ 50 ��������")]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [MaxLength(100, ErrorMessage = "������ ������ ���� ������ 100 ��������")]
        public string? Description { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
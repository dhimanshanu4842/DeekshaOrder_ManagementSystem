using System.ComponentModel.DataAnnotations;

namespace OrderManagementSystem.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
    }
}

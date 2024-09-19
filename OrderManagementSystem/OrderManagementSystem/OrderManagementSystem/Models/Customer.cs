using System.ComponentModel.DataAnnotations;

namespace OrderManagementSystem.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}

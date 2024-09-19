using Microsoft.AspNetCore.Mvc.Rendering;

namespace OrderManagementSystem.Models
{
    public class OrderViewModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public IEnumerable<SelectListItem> Product { get; set; }
    }
}

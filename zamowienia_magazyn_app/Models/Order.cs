using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace zamowienia_magazyn_app.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Display(Name = "Data zamówienia")]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        public int ClientId { get; set; }
        
        [ForeignKey("ClientId")]
        [Display(Name = "Klient")]
        public Client? Client { get; set; }

        [Display(Name = "Status")]
        public OrderStatus Status { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        
        [Display(Name = "Suma")]
        public decimal TotalAmount => OrderItems.Sum(x => x.Quantity * x.UnitPrice);
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace zamowienia_magazyn_app.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Proszę podać nazwę produktu.")]
        [Display(Name = "Nazwa Produktu")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Opis")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Cena jest wymagana.")]
        [Range(0.01, 1000000.00, ErrorMessage = "Cena musi być większa od 0.")]
        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Cena")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Ilość jest wymagana.")]
        [Range(0, 10000, ErrorMessage = "Ilość nie może być ujemna.")]
        [Display(Name = "Ilość w magazynie")]
        public int StockQuantity { get; set; }
    }
}

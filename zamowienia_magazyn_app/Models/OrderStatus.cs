namespace zamowienia_magazyn_app.Models
{
    public enum OrderStatus
    {
        [System.ComponentModel.DataAnnotations.Display(Name = "Nowe")]
        New,
        [System.ComponentModel.DataAnnotations.Display(Name = "W trakcie realizacji")]
        Processing,
        [System.ComponentModel.DataAnnotations.Display(Name = "Wysłane")]
        Shipped,
        [System.ComponentModel.DataAnnotations.Display(Name = "Dostarczone")]
        Delivered,
        [System.ComponentModel.DataAnnotations.Display(Name = "Anulowane")]
        Cancelled
    }
}

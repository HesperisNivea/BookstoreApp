using Labb2_DbFirst_Template.Models;

namespace BookstoreApp.Models;

public class ShopModel
{
    public int Id { get; set; }

    public string ShopName { get; set; } = null!;

    public string ShopAdress { get; set; } = null!;

   // public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

   // public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
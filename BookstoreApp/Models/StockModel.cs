using Labb2_DbFirst_Template.Models;
using Labb2_DbFirst_Template.Repositories;

namespace BookstoreApp.Models;

public class StockModel
{
    public int ShopId { get; set; }

    public string Isbn13 { get; set; } = null!;

    public int? Amount { get; set; }

    public string ShopName { get; set; } 

    public string BookTitle { get; set; }



}
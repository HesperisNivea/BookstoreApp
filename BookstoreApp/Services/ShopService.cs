using BookstoreApp.Models;
using Labb2_DbFirst_Template.Repositories;

namespace BookstoreApp.Services;

public class ShopService
{
    private readonly ShopRepository _shopRepository = new ();

    public List<ShopModel> GetAll
        ()
    {
        return _shopRepository.GetAll().Select(
            shop => new ShopModel()
        {
                Id = shop.Id,
                ShopAdress = shop.ShopAdress,
                ShopName = shop.ShopName
        }).ToList();
    }

}
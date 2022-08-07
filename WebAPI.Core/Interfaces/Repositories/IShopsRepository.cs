using WebAPI.Core.Dto.Shops;
using WebAPI.Core.Entities;

namespace WebAPI.Core.Interfaces.Repositories
{
    public interface IShopsRepository
    {
        bool AddShops(Shops shops);
        bool UpdateShops(int id, Shops shops);
        Shops GetShopDetailById(int id);
        bool DeleteShopById(int id);
        ShopListResponseDTO GetShopsList(ShopListRequest shops);
    }
}

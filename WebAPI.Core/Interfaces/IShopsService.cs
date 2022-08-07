using System.Threading.Tasks;
using WebAPI.Core.Dto.Shops;
using WebAPI.Core.Entities;

namespace WebAPI.Core.Interfaces
{
    public interface IShopsService
    {
        Task<bool> AddShops(Shops shops);
        Task<bool> UpdateShops(int id, Shops shops);
        Task<Shops> GetShopDetailById(int id);
        Task<bool> DeleteShopById(int id);
        Task<ShopListResponseDTO> GetShopsList(ShopListRequest shops);
    }
}

using System.Threading.Tasks;
using WebAPI.Core.Dto.Shops;
using WebAPI.Core.Entities;
using WebAPI.Core.Interfaces;
using WebAPI.Core.Interfaces.Repositories;

namespace WebAPI.Infrastructure.Services
{
    public class ShopsService : IShopsService
    {
        private readonly IShopsRepository _shopsRepository;

        public ShopsService(IShopsRepository shopsRepository)
        {
            _shopsRepository = shopsRepository;
        }
        public async Task<bool> AddShops(Shops shops)
        {
            var result = await Task.Run(() => _shopsRepository.AddShops(shops));
            return result;
        }

        public async Task<bool> UpdateShops(int id, Shops shops)
        {
            var result = await Task.Run(() => _shopsRepository.UpdateShops(id, shops));
            return result;
        }

        public async Task<Shops> GetShopDetailById(int id)
        {
            var shopDetail = await Task.Run(() => _shopsRepository.GetShopDetailById(id));
            return shopDetail;
        }

        public async Task<bool> DeleteShopById(int id)
        {
            return await Task.Run(() => _shopsRepository.DeleteShopById(id));
        }

        public async Task<ShopListResponseDTO> GetShopsList(ShopListRequest shopsListRequest)
        {
            var shopList = await Task.Run(() => _shopsRepository.GetShopsList(shopsListRequest));
            return shopList;
        }
    }
}

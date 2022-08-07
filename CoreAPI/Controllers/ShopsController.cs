using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.Core.Dto.Common;
using WebAPI.Core.Dto.Shops;
using WebAPI.Core.Entities;
using WebAPI.Core.Interfaces;
using WebAPI.Infrastructure.Common;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopsController : ControllerBase
    {
        private IShopsService _shopsService;
        private readonly IMapper _mapper;
        private ILoggerService _logger;

        public ShopsController(IShopsService shopsService, IMapper mapper, ILoggerService logger)
        {
            _shopsService = shopsService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost("AddShops")]
        public async Task<IActionResult> AddShops([FromBody] Shops request)
        {
            _logger.LogInfo("Executing AddShops endpoint.");
            ShopsResponseDTO response = new ShopsResponseDTO();
            bool result = await _shopsService.AddShops(request);
            if (!result)
            {
                response.success = false;
                response.responseCode = (int)ResponseMessages.RecordNotFound;
                response.responseMessage = StringEnum.GetStringValue(ResponseMessages.RecordNotFound);
                response.data = null;
            }
            else
            {
                response.success = true;
                response.responseCode = (int)ResponseMessages.RequestExecutedSuccessfully;
                response.responseMessage = StringEnum.GetStringValue(ResponseMessages.RequestExecutedSuccessfully);
                response.data = result;
            }
            _logger.LogInfo($"Executed AddShops endpoint with success: {response.success}.");

            return Ok(response);
        }

        [HttpPut("UpdateShops/{id}")]
        public async Task<IActionResult> UpdateShops(int id, [FromBody] Shops request)
        {
            _logger.LogInfo("Executing UpdateShops endpoint.");
            ShopsResponseDTO response = new ShopsResponseDTO();
            bool result = await _shopsService.UpdateShops(id, request);
            if (!result)
            {
                response.success = false;
                response.responseCode = (int)ResponseMessages.RecordNotFound;
                response.responseMessage = StringEnum.GetStringValue(ResponseMessages.RecordNotFound);
                response.data = null;
            }
            else
            {
                response.success = true;
                response.responseCode = (int)ResponseMessages.RequestExecutedSuccessfully;
                response.responseMessage = StringEnum.GetStringValue(ResponseMessages.RequestExecutedSuccessfully);
                response.data = result;
            }
            _logger.LogInfo($"Executed UpdateShops endpoint with success: {response.success}.");

            return Ok(response);
        }

        [HttpGet("GetShopDetail/{id}")]
        public async Task<ActionResult<ShopsResponseDTO>> GetShopDetailById(int id)
        {
            _logger.LogInfo("Executing GetShopDetailById endpoint.");
            ShopsResponseDTO response = new ShopsResponseDTO();
            var shopDetail = await _shopsService.GetShopDetailById(id);
            if (shopDetail == null)
            {
                response.success = false;
                response.responseCode = (int)ResponseMessages.RecordNotFound;
                response.responseMessage = StringEnum.GetStringValue(ResponseMessages.RecordNotFound);
                response.data = null;
            }
            else
            {
                response.success = true;
                response.responseCode = (int)ResponseMessages.RequestExecutedSuccessfully;
                response.responseMessage = StringEnum.GetStringValue(ResponseMessages.RequestExecutedSuccessfully);
                response.data = shopDetail;
            }

            _logger.LogInfo($"Executed GetShopDetailById endpoint for Id : {id}.");
            return Ok(response);
        }

        [HttpDelete("DeleteShop/{id}")]
        public async Task<ActionResult> DeleteShopById(int id)
        {
            _logger.LogInfo("Executing DeleteShopById endpoint.");
            CommonResponse response = new CommonResponse();
            if (await _shopsService.GetShopDetailById(id) == null)
            {
                response.success = false;
                response.responseCode = (int)ResponseMessages.DoesNotExists;
                response.responseMessage = StringEnum.GetStringValue(ResponseMessages.DoesNotExists);
            }

            if (await _shopsService.DeleteShopById(id))
            {
                response.success = true;
                response.responseCode = (int)ResponseMessages.RequestExecutedSuccessfully;
                response.responseMessage = StringEnum.GetStringValue(ResponseMessages.RequestExecutedSuccessfully);
            }
            else
            {
                response.success = false;
                response.responseCode = (int)ResponseMessages.DoesNotExists;
                response.responseMessage = StringEnum.GetStringValue(ResponseMessages.DoesNotExists);
            }
            _logger.LogInfo($"Executed DeleteShopById endpoint for Id : {id}.");
            return Ok(response);
        }

        [HttpGet("GetShopsList")]
        [AllowAnonymous]
        public async Task<ActionResult<ShopListResponseDTO>> GetShopsList([FromQuery] ShopListRequest request)
        {
            bool success = false;
            _logger.LogInfo("Executing GetShopsList endpoint.");
            ShopListResponseDTO response = new ShopListResponseDTO();
            response = await _shopsService.GetShopsList(request);

            if (response.data.Count == 0)
            {
                response.success = false;
                response.responseCode = (int)ResponseMessages.RecordNotFound;
                response.responseMessage = StringEnum.GetStringValue(ResponseMessages.RecordNotFound);
                response.data = null;
            }
            else
            {
                response.success = true;
                response.responseCode = (int)ResponseMessages.RequestExecutedSuccessfully;
                response.responseMessage = StringEnum.GetStringValue(ResponseMessages.RequestExecutedSuccessfully);
            }

            _logger.LogInfo($"Executed GetShopsList endpoint with success: {success}.");

            return Ok(response);
        }
    }
}

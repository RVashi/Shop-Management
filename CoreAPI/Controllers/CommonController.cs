using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using WebAPI.Core.Interfaces;
using WebAPI.Helpers;
using WebAPI.Infrastructure.Common;
using WebAPI.Models;
using WebAPI.Core.Dto.Upload;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private ICommonService _commonService;
        private ILoggerService _logger;
        public CommonController(ICommonService commonService, ILoggerService logger)
        {
            _commonService = commonService;
            _logger = logger;
        }

        [HttpPost("UploadFile")]
        public async Task<ActionResult<AmazonResponseObject>> UploadFile([FromForm] FileModel fileModel)
        {
            string folder = string.Empty;
            string AWSBucket = string.Empty;
            AmazonResponseObject objFile = null;

            if (!string.IsNullOrEmpty(fileModel.FileName))
            {
                if (fileModel.FileUploadedFor.ToLower() == "products")
                {
                    folder = "Products";
                    AWSBucket = "eccproductimages";
                }
                else if (fileModel.FileUploadedFor.ToLower() == "banners")
                {
                    folder = "Banners";
                    AWSBucket = "eccbannerimages";
                }

                AmazonAWSHelper helper = new AmazonAWSHelper();

                using (var memoryStream = new MemoryStream())
                {
                    await fileModel.FormFile.CopyToAsync(memoryStream);

                    // Upload the file if less than 2 MB
                    if (memoryStream.Length < 2097152)
                    {
                        objFile = helper.UploadFile(AWSBucket, folder, fileModel.FileName, memoryStream);
                    }
                    else
                    {
                        ModelState.AddModelError("File", "The file is too large.");
                    }
                }
            }
            return objFile;
        }

        [HttpPost("UploadFileToServer")]
        public async Task<IActionResult> UploadFileToServer([FromBody] UploadRequest request)
        {
            _logger.LogInfo("Executing UploadFileToServer endpoint.");
            UploadResponse result = new UploadResponse();

            var uploadResponseDetail = await _commonService.UploadFileToServer(request);

            if (!uploadResponseDetail.success)
            {
                result.success = false;
                result.responseCode = (int)ResponseMessages.FileAlreadyExists;
                result.responseMessage = uploadResponseDetail.imageUrl; //StringEnum.GetStringValue(ResponseMessages.FileAlreadyExists);
                result.data = null;
            }
            else
            {
                result.success = true;
                result.responseCode = (int)ResponseMessages.RequestExecutedSuccessfully;
                result.responseMessage = uploadResponseDetail.imageUrl;
                result.data = uploadResponseDetail;
            }

            _logger.LogInfo($"Executed UploadFileToServer endpoint for Id : {request.imageName}.");
            return Ok(result);
        }

        [HttpPost("UploadCkFile")]
        public async Task<IActionResult> UploadCkFile([FromForm] CommonFileModel fileModel)
        {
            string finalBase64String = string.Empty;
            CommonUploadFileResponse result = new CommonUploadFileResponse();
            if (fileModel.upload != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await fileModel.upload.CopyToAsync(memoryStream);

                    // Upload the file if less than 2 MB
                    if (memoryStream.Length < 2097152)
                    {
                        var fileBytes = memoryStream.ToArray();
                        finalBase64String = Convert.ToBase64String(fileBytes);
                    }
                }
                UploadRequest request = new UploadRequest();
                request.folder = "contenido";
                request.image = finalBase64String;
                request.imageName = fileModel.upload.FileName;
                var uploadResponseDetail = await _commonService.UploadFileToServer(request);

                if (!uploadResponseDetail.success)
                {
                    result.success = false;
                    result.uploaded = 0;
                    result.error = StringEnum.GetStringValue(ResponseMessages.FileAlreadyExists);
                    result.url = null;
                }
                else
                {
                    result.success = true;
                    result.fileName = uploadResponseDetail.imageUrl.Split("/")[1];
                    result.uploaded = 1;
                    result.error = StringEnum.GetStringValue(ResponseMessages.RequestExecutedSuccessfully); 
                    result.url = string.Format(@"{0}/{1}", "https://content.eccediciones.com", uploadResponseDetail.imageUrl);
                }


                _logger.LogInfo($"Executed UploadFileToServer endpoint for Id : {request.imageName}.");
            }
            else
            {
                result.success = false;
                result.uploaded = 0;
                result.error = StringEnum.GetStringValue(ResponseMessages.PleaseSelectAFile);
                result.url = null;
            }
            return Ok(result);
        }
    }
}

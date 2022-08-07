using MailChimp.Net;
using MailChimp.Net.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Core.Dto.Common;
using WebAPI.Core.Dto.Upload;
using WebAPI.Core.Interfaces;
using WebAPI.Core.Interfaces.Repositories;

namespace WebAPI.Infrastructure.Services
{
    public class CommonService : ICommonService
    {
        private readonly ICommonRepository _commonRepository;

        public CommonService(ICommonRepository commonRepository)
        {
            _commonRepository = commonRepository;
        }
        public async Task<UploadFileResponse> UploadFileToServer(UploadRequest request)
        {
            var uploadFileResponse = await Task.Run(() => _commonRepository.UploadFileToServer(request));
            return uploadFileResponse;
        }
    }
}

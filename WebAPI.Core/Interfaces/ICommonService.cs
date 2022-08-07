using System.Threading.Tasks;
using WebAPI.Core.Dto.Upload;

namespace WebAPI.Core.Interfaces
{
    public interface ICommonService
    {
        Task<UploadFileResponse> UploadFileToServer(UploadRequest request);
    }
}

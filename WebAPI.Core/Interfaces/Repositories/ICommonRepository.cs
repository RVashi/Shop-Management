using WebAPI.Core.Dto.Upload;

namespace WebAPI.Core.Interfaces.Repositories
{
    public interface ICommonRepository
    {
        UploadFileResponse UploadFileToServer(UploadRequest request);
    }
}

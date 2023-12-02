using FileUpload.ViewModels.ResultView;
using FileUpload.Entities;

namespace FileUpload.Services
{
    public interface IFileService
    {
        public Task<ResultViewModel> PostFileAsync(IFormFile fileData);

        public Task<ResultViewModel> PostMultiFileAsync(List<FileUploadModel> fileData);

        public Task<ResultViewModel> DownloadFileById(int fileName);
    }
}

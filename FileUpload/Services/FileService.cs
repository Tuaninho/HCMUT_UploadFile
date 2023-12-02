
using FileUpload.Data;
using FileUpload.Entities;
using Microsoft.EntityFrameworkCore;
using FileUpload.ViewModels.ResultView;

namespace FileUpload.Services
{
    public class FileService : IFileService
    {
        private readonly DbContextClass dbContextClass;

        public FileService(DbContextClass dbContextClass)
        {
            this.dbContextClass = dbContextClass;
        }

        public async Task<ResultViewModel> PostFileAsync(IFormFile fileData)
        {
            ResultViewModel model = new ResultViewModel();
            try
            {
                var fileDetails = new FileDetails()
                {
                    ID = 0,
                    FileName = fileData.FileName
                };

                

                if (fileData != null)
                {
                    using (var stream = new MemoryStream())
                    {
                        fileData.CopyTo(stream);
                        fileDetails.FileData = stream.ToArray();
                    }
                }


                var result = dbContextClass.FileDetails.Add(fileDetails);
                await dbContextClass.SaveChangesAsync();
                model.response = new ResponseFileModel()
                {
                    ID = fileDetails.ID,
                    FileName = fileDetails.FileName,
                    FileData = fileDetails.FileData
                };
            }
            catch (Exception ex)
            {
                model.status = 0;
                model.message = "Lỗi phát sinh " + ex.Message.ToString();
            }
            return model;
        }

        public async Task<ResultViewModel> PostMultiFileAsync(List<FileUploadModel> fileData)
        {
            ResultViewModel model = new ResultViewModel();

            try
            {
                foreach(FileUploadModel file in fileData)
                {
                    var fileDetails = new FileDetails()
                    {
                        ID = 0,
                        FileName = file.FileDetails.FileName,
                        FileType = file.FileType,
                    };

                    using (var stream = new MemoryStream())
                    {
                        file.FileDetails.CopyTo(stream);
                        fileDetails.FileData = stream.ToArray();
                    }

                    var result = dbContextClass.FileDetails.Add(fileDetails);
                }             
                await dbContextClass.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                model.status = 0;
                model.message = "Lỗi phát sinh " + ex.Message.ToString();
            }
            return model;
        }

        public async Task<ResultViewModel> DownloadFileById(int Id)
        {
            ResultViewModel model = new ResultViewModel();
            try
            {
                var file =  dbContextClass.FileDetails.Where(x => x.ID == Id).FirstOrDefaultAsync();

                var content = new System.IO.MemoryStream(file.Result.FileData);
                var path = Path.Combine(
                   Directory.GetCurrentDirectory(), "FileDownloaded",
                   file.Result.FileName);

                await CopyStream(content, path);

                model.status = 1;
                model.message = "Download file success!";
            }
            catch (Exception ex)
            {
                model.status = -1;
                model.message = "Service không hoạt động " + ex.Message.ToString();
            }
            return model;
        }
 
        public async Task CopyStream(Stream stream, string downloadPath)
        {
            using (var fileStream = new FileStream(downloadPath, FileMode.Create, FileAccess.Write))
            {
               await stream.CopyToAsync(fileStream);
            }
        }
    }
}

using FileUpload.Entities;

namespace FileUpload.ViewModels.ResultView
{
    public class ResponseFileModel
    {
        public int ID { get; set; }
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
    }
}
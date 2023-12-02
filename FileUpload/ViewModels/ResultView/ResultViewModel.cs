namespace FileUpload.ViewModels.ResultView
{
    public class ResultViewModel
    {
        public ResultViewModel()
        {
            status = 1; //1 success; -1: error
            message = "success";

        }

        public int status { get; set; }
        public string message { get; set; }
        public object response { get; set; }
        public int totalRecord { get; set; }
    }
}

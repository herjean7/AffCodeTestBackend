namespace AffynBackend.Models
{
    public class Transaction
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public Result Result { get; set; }
    }


    public class Result
    {
        public int IsError { get; set; }
        public string ErrDescription { get; set; }
    }
}

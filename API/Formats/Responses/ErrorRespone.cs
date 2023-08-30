namespace API.Formats.Return
{
    public class ErrorRespone
    {
        public required int ErrorCode { get; set; }
        public required string Message { get; set; }
    }
}

namespace API.DTOs.Error
{
    public class UnauthorizedExceptionDto : Exception
    {
        public UnauthorizedExceptionDto(string message) : base(message)
        {
        }
    }
}

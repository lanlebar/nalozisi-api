namespace API.DTOs.Error
{
    public class NotFoundExceptionDto : Exception
    {
        public NotFoundExceptionDto(string message) : base(message)
        {
        }
    }
}

namespace API.Models.Auth
{
    public class UserRegistrationModel
    {
        public string UserName { get; set; }
        public string PasswordBcrypt { get; set; }
        public string Email { get; set; }
    }
}

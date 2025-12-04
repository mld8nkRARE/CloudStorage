namespace Server.Dtos.Auth
{
    public class AuthResultDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expires { get; set; }
    }
}

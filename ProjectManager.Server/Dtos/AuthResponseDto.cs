namespace ProjectManager.Server.Dtos
{
    public class AuthResponseDto
    {
        public string JwtToken { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
    }
}

namespace Core.Application.DTOs
{
    public class AuthenticationResult
    {
        public bool Succeeded { get; set; }
        public Dictionary<string, string> Errors { get; set; }
    }
}

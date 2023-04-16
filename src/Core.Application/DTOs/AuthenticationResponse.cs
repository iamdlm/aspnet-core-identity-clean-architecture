namespace Core.Application.DTOs
{
    public class AuthenticationResponse
    {
        public bool Succeeded { get; set; }
        public Dictionary<string, string> Errors { get; set; }
    }
}

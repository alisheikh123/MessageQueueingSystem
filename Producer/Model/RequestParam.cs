namespace Producor_Web_API.Model
{
    public class RequestParam
    {
        public string? Payload { get; set; }
        public string? RequestMethod { get; set; }
        public string? RequestUrl { get; set; }
        public string? Token { get; set; } = string.Empty;

    }
}

namespace ConsumerAPI.Model
{
    public class RequestParameter
    {

        public GeneralHeaders? GeneralHeaders { get; set; }
        public RequestHeader? RequestHeaders { get; set; }
        public ResponseHeaders? ResponseHeaders { get; set; }
        public string? Payload { get; set; }
    }

    public class GeneralHeaders
    {
        public string RequestMethod { get; set; }
        public string RequestUrl { get; set; }
        public string StatusCode { get; set; }
        public string RemoteAddress { get; set; }
        public string ReferrerPolicy { get; set; }


    }
    public class RequestHeader
    {
        public string UserAgent { get; set; }
        public string ContentType { get; set; }
        public string Authorization { get; set; }
    }

    public class ResponseHeaders
    {
        public string ContentType { get; set; }
        public string ContentLength { get; set; }
        public string CacheControl { get; set; }
        public string SetCookie { get; set; }
    }
}

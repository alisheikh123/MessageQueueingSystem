using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualBasic;
using RabbitMQ.Client;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Buffers.Text;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Text.Encodings.Web;
using System;
using static Producor_Web_API.Model.GeneralHeaders;

namespace Producor_Web_API.Model
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

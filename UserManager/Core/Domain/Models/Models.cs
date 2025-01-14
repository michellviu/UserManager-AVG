using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Models
{
    public class Response
    {
        public string Message { get; set; }
    }
    public class RequestUserLogin
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class RequestUserRegister
    {
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
    }

    public class ResponseLogin
    {
        public string username { get; set; }
        public string token { get; set; }
    }
}

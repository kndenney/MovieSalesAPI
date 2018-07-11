using System;
using System.Collections.Generic;
using System.Text;

namespace MovieSalesAPILogic.Authorization
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public DateTime expiration { get; set; }
        public string Username { get; set; }
    }
}

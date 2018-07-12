using System;
using System.Collections.Generic;
using System.Text;

namespace MovieSalesAPILogic.Authorization
{
    public class TokenResponse : ITokenResponse
    {
        public string Token { get; set; }
        public DateTime expiration { get; set; }
        public string Username { get; set; }
    }

    public interface ITokenResponse
    {
        string Token { get; set; }
        DateTime expiration { get; set; }
        string Username { get; set; }
    }
}

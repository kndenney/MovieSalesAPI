using System;

namespace MovieSalesAPILogic
{
    public class TokenRequest : ITokenRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public interface ITokenRequest
    {
        string Username { get; set; }
        string Password { get; set; }
    }
}

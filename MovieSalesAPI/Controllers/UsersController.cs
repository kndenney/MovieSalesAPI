using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MovieSalesAPI.Data.User;
using MovieSalesAPILogic;
using MovieSalesAPILogic.Authorization;

namespace MovieSalesAPI.Controllers
{
    /// <summary>
    /// Authenticate and authorize users to the movies API and data.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private IUserData _userData;

        /// <summary>
        /// Controller that authorizes a JWT token for users
        /// </summary>
        /// <param name="configuration"></param>
        public UsersController(
            IConfiguration configuration,
            IUserData userData)
        {
            _configuration = configuration;
            _userData = userData;
        }

        //https://jonhilton.net/2017/10/11/secure-your-asp.net-core-2.0-api-part-1---issuing-a-jwt/
        //https://auth0.com/blog/securing-asp-dot-net-core-2-applications-with-jwts/

        // POST: api/Authorize
        /// <summary>
        /// Create a JWT token for the user
        /// </summary>
        /// <param name="request">Token Request posted in the form of Username and Password</param>
        [Route("token")]
        [AllowAnonymous]
        [HttpPost]
        public IEnumerable<ITokenResponse> CreateToken([FromBody] TokenRequest request)
        {
            return new MovieSalesAPILogic.Authorization.Token(_configuration).CreateToken(request);
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="request">User Request posted in the form of Username and Password</param>
        [AllowAnonymous]
        [HttpPost]
        public IEnumerable<IUser> CreateUserAccount([FromBody] User request)
        {
            return _userData.CreateUserAccount(request);
        }


    }
}

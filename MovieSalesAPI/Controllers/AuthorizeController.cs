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
using MovieSalesAPILogic;
using MovieSalesAPILogic.Authorization;

namespace MovieSalesAPI.Controllers
{
    /// <summary>
    /// Authenticate and authorize users to the movies API and data.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Controller that authorizes a JWT token for users
        /// </summary>
        /// <param name="configuration"></param>
        public AuthorizeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //https://jonhilton.net/2017/10/11/secure-your-asp.net-core-2.0-api-part-1---issuing-a-jwt/
        //https://auth0.com/blog/securing-asp-dot-net-core-2-applications-with-jwts/

        // POST: api/Authorize
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request">Token Request posted in the form of Username and Password</param>
        [Route("create/token")]
        [AllowAnonymous]
        [HttpPost]
        public List<ITokenResponse> CreateToken([FromBody] TokenRequest request)
        {
            return new MovieSalesAPILogic.Authorization.Token(_configuration).CreateToken(request);
        }
    }
}

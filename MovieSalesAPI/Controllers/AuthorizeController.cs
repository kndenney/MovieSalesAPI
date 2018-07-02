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
using Microsoft.IdentityModel.Tokens;
using MovieSalesAPILogic;

namespace MovieSalesAPI.Controllers
{
    /// <summary>
    /// Authenticate and authorize users to the movies API and data.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
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
        public string CreateToken([FromBody] TokenRequest request)
        {
            //The username and password would be pulled either from a secure server
            //or database in our class library codebase or some other DAL
            if (request.Username == "Jon" && request.Password == "Again, not for production use, DEMO ONLY!")
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, request.Username)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecurityKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                // _config["Jwt:Issuer"]
                //We should create a QA URL and Prod URL in the appsettings.json
                //then use that here
                //in the 'issuer' and 'audience' portion
                //whereby the issueing server is whatever we need it to be
                //should it be diference than the audience server (audience being
                //being the Angular app or front end whatever domain name
                //and the issuing server could be a different domain name altogether
                //like token.domain.com
                //and audienc would just be domain.com etc.

                var token = new JwtSecurityToken(
                    issuer: "yourdomain.com",
                    audience: "yourdomain.com",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }

            return "";
        }
    }
}

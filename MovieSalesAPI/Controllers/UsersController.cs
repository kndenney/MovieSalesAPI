using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MovieSalesAPI.Data.User;
using MovieSalesAPI.Shared;
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
        [HttpPost] //IEnumerable<IUser>
        public dynamic CreateUserAccount([FromBody] User request)
        {
            //https://www.carlrippon.com/integrating-validation-in-angular-2-and-asp-net-core/
            //I think the above example uses reactive forms which is what we want
            //this link - http://jasonwatmore.com/post/2018/05/10/angular-6-reactive-forms-validation-example
            //shows how to do reactive and has a link on how to do template driven which is what we curenlty have
            //but the funky syntax will be hard to explain to students I think
                if (!ModelState.IsValid)
                {

                    return _userData.CreateUserAccount(request);
                }

                var results = _userData.CreateUserAccount(request);

                //If there is an exception from the method we will want to
                //create an exception class and fill it with data from the exception
                //and we can then return that data instead of whatever the method
                //would normally return if there was no exception
                if (results.Exception != null)
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, new ExceptionJson
                    {
                        Exception = results.Exception,
                        Stacktrace = results.Stacktrace,
                        InnerException = (results.InnerException != null) ? results.InnerException.ToString() : ""
                    });
                } else
                {
                    return results;
                }
        }
    }
}

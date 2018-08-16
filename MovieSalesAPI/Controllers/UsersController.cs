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

            try
            {
                if (!ModelState.IsValid)
                {

                    return _userData.CreateUserAccount(request);
                }

                return _userData.CreateUserAccount(request);
            }
            catch(Exception ex)
            {
                // By not allowing our data class code to have a try/catch
                // it forces our middleware not to assign a status code yet if 
                // we have an exception. It will wait to assign a status code until it
                // tries the code and then if the code breaks it will enter this exception handler
                // whereby we assign a BadRequest status and return the error
                // in this way we can have a consistent experience and return a consistent JSON
                // object. Otherwise, if we had a try catch in the data class, it would error there
                // but would have already assigned a status code of OK 200 with an HTTP call of 200.
                // And while technically true it shows that the HTTP call went through OK
                // but then our data  class errors and we have an issue.

                return StatusCode((int)HttpStatusCode.InternalServerError, (new ExceptionJson
                {
                    Exception = ex.Message,
                    Stacktrace = ex.StackTrace,
                    InnerException = (ex.InnerException != null) ? ex.InnerException.ToString() : ""
                }));
            }
        }
    }
}

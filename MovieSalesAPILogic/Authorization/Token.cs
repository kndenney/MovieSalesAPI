using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MovieSalesAPILogic.Authorization
{
    public class Token
    {
        private readonly IConfiguration _configuration;

        public Token(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool Authorize(TokenRequest request)
        {
            if (request.Username == _configuration["JWTUsername"] && request.Password == _configuration["JWTPassword"])
            {
                return true;
            }
            return false;
        }

        public string CreateToken(TokenRequest request)
        {
            //The username and password would be pulled either from a secure server
            //or database in our class library codebase or some other DAL
            if (Authorize(request))
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, request.Username)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTKey"]));
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

                string issuer = "";
                string audience = "";
                switch (_configuration["Environment"])
                {
                    case "Development":
                        issuer = _configuration["DevelopmentIssuerURL"];
                        audience = _configuration["DevelopmentAudienceURL"];
                        break;
                    case "UAT":
                        issuer = _configuration["UATIssuerURL"];
                        audience = _configuration["UATAudienceURL"];
                        break;
                    case "Production":
                        issuer = _configuration["ProductionIssuerURL"];
                        audience = _configuration["ProductionAudienceURL"];
                        break;
                    default:
                        issuer = "";
                        audience = "";
                        break;
                }

                var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }

            return "";
        }


    }
}

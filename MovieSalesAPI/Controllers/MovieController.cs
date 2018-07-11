using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieSalesAPI.Data;

namespace MovieSalesAPI.Controllers
{
    /// <summary>
    /// Retrieve and interact with movie data.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : Controller
    {

        private readonly IMovieData _movieData;

        /// <summary>
        /// Class Declaration
        /// </summary>
        /// <param name="movieData"></param>
        public MovieController(
            IMovieData movieData
        )
        {
            _movieData = movieData;
        }
        // GET: api/Movie
        /// <summary>
        /// Retrieve all movie details
        /// </summary>
        /// <returns>Returns a list of movie details.</returns>
        [Authorize(Policy = "APIMovieAccess")]
        [HttpGet]
        [Route("all/movies")]
        public List<Movie> GetAllMovieDetails
            (
                [FromHeader] string authorization
            )
        {

            //Access the Claim.Name value
            //var userName = User.Identity.Name;
            /* List<Movie> movies = new List<Movie>
             {
                 new Movie
                 {
                     Name = "Test",
                     Description = "Test"
                 }
             };

             return movies;*/

            if (User.Identity.IsAuthenticated)
            {
                return _movieData.RetrieveAllMovies(User.Identity.Name);
            } else
            {
                return null;
            }
        }

        // GET: api/Movie/5
        [HttpGet("{id}", Name = "Get")]
        public string GetSpecificMovieDetails(int id)
        {
            return "value";
        }

        // POST: api/Movie
        [HttpPost]
        public void PostMovieToDatabase([FromBody] string value)
        {
        }

        // PUT: api/Movie/5
        [HttpPut("{id}")]
        public void PutMovieToDatabase(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void DeleteMovieFromDatabase(int id)
        {
        }
    }
}

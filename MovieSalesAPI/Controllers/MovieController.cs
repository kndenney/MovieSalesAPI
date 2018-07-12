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
    [Route("[controller]")]
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
        [Route("all")]
        public List<IMovie> GetAllMovieDetails
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
            }
            else
            {
                return null;
            }
        }

        // GET: Movie/id
        /// <summary>
        /// Retrieve specific movie
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public string GetSpecificMovieDetailsById(int id)
        {
            return "value";
        }


        // GET: Movie/name
        /// <summary>
        /// Retrieve specific movie by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{name}")]
        public string GetSpecificMovieDetailsByName(string name)
        {
            return "value";
        }


        // POST: Movie/create
        [HttpPost]
        public void PostMovieToDatabase([FromBody] string value)
        {
        }

        // PUT: Movie/update
        [HttpPut]
        [Route("{id}")]
        public void PutMovieToDatabase(int id, [FromBody] string value)
        {
        }

        // DELETE: Movie/id
        [HttpDelete]
        [Route("{id}")]
        public void DeleteMovieFromDatabaseById(int id)
        {
        }

        // DELETE: Movie/name
        [HttpDelete]
        [Route("{name}")]
        public void DeleteMovieFromDatabaseByName(string name)
        {
        }
    }
}

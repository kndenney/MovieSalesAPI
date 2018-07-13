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
                return _movieData.GetAllMovieDetails(User.Identity.Name);
            }
            else
            {
                return null;
            }
        }

        // GET: Movie/movieid
        /// <summary>
        /// Retrieve specific movie
        /// </summary>
        /// <param name="movieid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{movieid}")]
        public List<IMovie> GetSpecificMovieDetailsById(int movieid)
        {
            if (User.Identity.IsAuthenticated)
            {
                return _movieData.GetSpecificMovieDetailsById(movieid, User.Identity.Name);
            }
            else
            {
                return null;
            }
        }


        // GET: Movie/moviename
        /// <summary>
        /// Retrieve specific movie by name
        /// </summary>
        /// <param name="moviename"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{moviename}")]
        public List<IMovie> GetSpecificMovieDetailsByName(string moviename)
        {
            if (User.Identity.IsAuthenticated)
            {
                return _movieData.GetSpecificMovieDetailsByName(moviename, User.Identity.Name);
            }
            else
            {
                return null;
            }
        }


        // POST: Movie/create
        [HttpPost]
        public List<IMovie> SaveMovieToDatabase([FromBody] IMovie movie)
        {
            return null;
        }

        // PUT: Movie/update
        [HttpPut]
        [Route("{id}")]
        public List<IMovie> UpdateMovieInDatabase(int id, [FromBody] IMovie movie)
        {
            return null;
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

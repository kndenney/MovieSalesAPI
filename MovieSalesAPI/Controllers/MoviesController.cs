﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieSalesAPI.Data;

namespace MovieSalesAPI.Controllers
{
    /// <summary>
    /// Retrieve and interact with movie data.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class MoviesController : Controller
    {
        //Good practices to keep in mind:
        //https://blog.mwaysolutions.com/2014/06/05/10-best-practices-for-better-restful-api/

        private readonly IMovieData _movieData;

        /// <summary>
        /// Class Declaration
        /// </summary>
        /// <param name="movieData"></param>
        public MoviesController(
            IMovieData movieData
        )
        { 
            _movieData = movieData;
        }

        ///We may want to independently update a movie, that does not belong to a specific user
        ///The updates to the name of the movie, MPAA rating etc
        ///should be independent of the user that the movie belongs
        ///for instance, two users with the same movie shouldn't have differnt MPAA ratings
        ///or different descriptions for the same movie id etc.
        #region MOVIE ACTIONS - NOT SPECIFIC TO A USER

            //We should be able to:
            //Add a new movie by itself
            //Update a movie by itself
            //Patch a movie by itself
            //Delete a movie (mark it inactive - meaning take it from the available movies for purchase)
            //but it does not delete the movie entirely from the database; just sets a flag called 'available' to 0 or 1
            //etc.
           
        #endregion

        #region USERS API ACTIONS

        // GET: api/Movie
        /// <summary>
        /// Retrieve all of a users movies
        /// </summary>
        /// <returns>Returns users movies</returns>
        [Authorize(Policy = "APIMovieAccess")]
        [HttpGet]
        [Route("users")]
        public IEnumerable<IMovie> GetUsersMovies
        (
            // [FromHeader] string authorization
        )
        {

            //Access the Claim.Name value
            //var userName = User.Identity.Name;
            /* IEnumerable<Movie> movies = new IEnumerable<Movie>
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
                return _movieData.GetUsersMovies(User.Identity.Name);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieve all the movies in the system that are available for purchase
        /// </summary>
        /// <returns>List of movies</returns>
        [Authorize(Policy = "APIMovieAccess")]
        [HttpGet]
        [Route("all")]
        public IActionResult GetAllMoviesIncludingUsers()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Ok(_movieData.GetAllMoviesIncludingUsers(User.Identity.Name));
            }
            else
            {
                return null;
            }
        }

        // GET: Movies/users/movieid
        /// <summary>
        /// Retrieve specific movie for a user by movie id
        /// </summary>
        /// <param name="movieid">Movie Id</param>
        /// <returns>Return a movie</returns>
        [Authorize(Policy = "APIMovieAccess")]
        [HttpGet]
        [Route("{movieid}")]
        public IEnumerable<IMovie> GetSpecificUsersMovieDetailsById([FromRoute] int movieid)
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

        // GET: Movies/users/moviename
        /// <summary>
        /// Retrieve specific users movie by name
        /// </summary>
        /// <param name="moviename">Movie Name</param>
        /// <returns>Return a movie</returns>
        [Authorize(Policy = "APIMovieAccess")]
        [HttpGet]
        [Route("{moviename}")]
        public IEnumerable<IMovie> GetSpecificUserMovieDetailsByName([FromRoute] string moviename)
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


        // POST: Movies/movie
        /// <summary>
        /// Create a new movie
        /// </summary>
        /// <param name="movie">Movie</param>
        /// <returns>Return the movie created</returns>
        [Authorize(Policy = "APIMovieAccess")]
        [HttpPost]
        public IEnumerable<IMovie> SaveMovieToDatabase([FromBody] Movie movie) // IMovie movie)
        {
            return _movieData.SaveMovieToDatabase(movie);
        }


        // POST: Movies/users/movie
        /// <summary>
        /// Create a new movie
        /// </summary>
        /// <param name="movie">Movie</param>
        /// <param name="username">username</param>
        /// <returns>Return the movie created</returns>
        [Authorize(Policy = "APIMovieAccess")]
        [Route("users/{movie}")]
        [HttpPost]
        public IEnumerable<IMovie> SaveUsersMovieToDatabase(
            [FromBody] int movieid, 
            [FromRoute] string username)
        {
            _movieData.SaveUsersMovieToDatabase(movieid, User.Identity.Name);
            return null;
        }



        // PUT: Movies/id
        /// <summary>
        /// Update an entire movie
        /// </summary>
        /// <param name="id"></param>
        /// <param name="movie"></param>
        /// <returns>Return the movie that was updated</returns>
        [Authorize(Policy = "APIMovieAccess")]
        [HttpPut]
        [Route("{id}")]
        public IEnumerable<IMovie> UpdateEntireMovieInDatabaseById([FromRoute] int id, [FromBody] Movie movie)
        {
            return null;
        }

        // PUT: Movies/moviename
        /// <summary>
        /// Update movie
        /// </summary>
        /// <param name="moviename"></param>
        /// <param name="movie"></param>
        /// <returns>Return updated movie</returns>
        [Authorize(Policy = "APIMovieAccess")]
        [HttpPut]
        [Route("{moviename}")]
        public IEnumerable<IMovie> UpdateEntireMovieInDatabaseByName([FromRoute] string moviename, [FromBody] Movie movie)
        {
            return null;
        }

        //PATCH Example: https://dotnetcoretutorials.com/2017/11/29/json-patch-asp-net-core/
        //https://kimsereyblog.blogspot.com/2017/11/implement-patch-on-asp-net-core-with.html
        //If you do not have the PATCH Nuget package
        //Open Tools > Package Manager Console
        //Do this command:
        //Install-Package Microsoft.AspNetCore.JsonPatch

        //PATCH: Movies/movie/id
        /// <summary>
        /// Update a portion movie details
        /// </summary>
        /// <param name="movieid"></param>
        /// <param name="moviePatch"></param>
        /// <returns>Return the updated movie</returns>
        [Authorize(Policy = "APIMovieAccess")]
        [HttpPatch]
        [Route("{id}")]
        public IEnumerable<IMovie> UpdatePartialMovieInDatabaseById([FromRoute]int movieid, [FromBody]JsonPatchDocument<IMovie> moviePatch)
        {
            //The idea is that you pull: somemoviefromdatabase
            //From the database
            //Then apply the 'moviePatch' to it
            //then save it back to the database
            //then reutrn the newly changed somemoviefromdatabase
            //object tback to the user

            IEnumerable<IMovie> someMovie = _movieData.GetSpecificMovieDetailsById(movieid, User.Identity.Name);
            moviePatch.ApplyTo(someMovie.First());

            _movieData.SaveMovieToDatabase(someMovie.First());
            return someMovie;
        }

        // DELETE: Movies/id
        /// <summary>
        /// Delete a movie by id
        /// </summary>
        /// <param name="id"></param>
        [Authorize(Policy = "APIMovieAccess")]
        [HttpDelete]
        [Route("{id}")]
        public void DeleteMovieFromDatabaseById([FromRoute] int id)
        {
        }

        // DELETE: Movies/name
        /// <summary>
        /// Delete a movie by name - *Could delete more than one movie with same name
        /// </summary>
        /// <param name="name"></param>
        [Authorize(Policy = "APIMovieAccess")]
        [HttpDelete]
        [Route("{name}")]
        public void DeleteMovieFromDatabaseByName([FromRoute] string name)
        {
            
        }

        #endregion

    }
}

using Dapper;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MovieSalesAPI.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSalesAPI.Data
{

    public class MovieData : IMovieData
    {
        private IDistributedCache _memoryCache;
        private string CacheName;

        //Maybe the list of movies updates only once a night via some feed into the database
        //and so you can set a cache because throughout a single day
        //the data will be the same until the next day after midnight, in this example
        private TimeSpan untilMidnight = DateTime.Today.AddDays(1) - DateTime.Now;
        private string connectionString = "";
        private IOptions<AppConfig> _config;
        DistributedCacheEntryOptions cacheExpirationOptions = new DistributedCacheEntryOptions();

        public MovieData(
          IDistributedCache memoryCache,
          IOptions<AppConfig> config
        )
        {
            _memoryCache = memoryCache;
            _config = config;

            cacheExpirationOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(untilMidnight.TotalMinutes);
        }

        //Example of Http REST Web API:
        //https://medium.com/@maheshi.gunarathne1994/web-api-using-asp-net-core-2-0-and-entity-framework-core-with-mssql-59d30f33ff64
        //https://apihandyman.io/api-design-tips-and-tricks-getting-creating-updating-or-deleting-multiple-resources-in-one-api-call/

        #region GET REQUESTS

        /// <summary>
        /// Retrieve all the users movies
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Return a list of all movies for the user.</returns>
        public async Task<IEnumerable<IMovie>> GetUsersMovies
        (
            string username
        )
        {
            try
            {
                CacheName = "Movies" + username;

                //If the cache exists return it
                
                if (_memoryCache.GetObject(CacheName) != null)
                {
                    return (IEnumerable<IMovie>)await _memoryCache.GetObject(CacheName);
                }

                //Retrieve the movies from the database based on the username
                //of the JWT token username
                using (var connection = new SqlConnection(_config.Value.ConnectionString))
                {
                    DynamicParameters parameters = new DynamicParameters();

                    parameters.Add("@username", dbType: DbType.AnsiString, value: username, direction: ParameterDirection.Input);

                    var results = await connection.QueryAsync<Movie>("RetrieveUsersMovies", parameters, commandType: CommandType.StoredProcedure);

                    //The cache was empty therefore set a new cache object
                    await _memoryCache.SetObject(CacheName, results, cacheExpirationOptions);

                    return results;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieve all movies including users movies
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<IEnumerable<IMovie>> GetAllMoviesIncludingUsers
        (
            string username
        )
        {
            try
            {
                CacheName = "AllMovies" + username;

                if (_memoryCache.GetObject(CacheName) != null)
                {
                    return (IEnumerable<IMovie>)await _memoryCache.GetObject(CacheName);
                }

                //Retrieve the movies from the database based on the username
                //of the JWT token username
                using (var connection = new SqlConnection(_config.Value.ConnectionString))
                {
                    DynamicParameters parameters = new DynamicParameters();

                    parameters.Add("@username", dbType: DbType.AnsiString, value: username, direction: ParameterDirection.Input);

                    var results = connection.Query<Movie>("RetrieveAllMoviesIncludingUsers", parameters, commandType: CommandType.StoredProcedure);

                    //The cache was empty therefore set a new cache object
                    await _memoryCache.SetObject(CacheName, results, cacheExpirationOptions);

                    return results;
                }
            }
            catch (Exception ex)
            {
                //return Enumerable.Empty<IMovie>();
                return new List<IMovie>();
            }
        }

        /// <summary>
        /// Retrieve a movie by id
        /// </summary>
        /// <param name="movieid"></param>
        /// <param name="username"></param>
        /// <returns>Return a movie</returns>
        public async Task<IEnumerable<IMovie>> GetSpecificMovieDetailsById
        (
            int movieid,
            string username
        )
        {
            try
            {
                CacheName = "Movies" + username;

                //If the cache exists return it
                if (_memoryCache.GetObject(CacheName) != null)
                {
                    return (IEnumerable<IMovie>)_memoryCache.GetObject(CacheName);
                }

                //Retrieve the movies from the database based on the username
                //of the JWT token username
                using (var connection = new SqlConnection(_config.Value.ConnectionString))
                {
                    DynamicParameters parameters = new DynamicParameters();

                    parameters.Add("@user", dbType: DbType.AnsiString, value: username, direction: ParameterDirection.Input);

                    var results = await connection.QueryAsync<Movie>("sprocName", parameters, commandType: CommandType.StoredProcedure);

                    //The cache was empty therefore set a new cache object
                    await _memoryCache.SetObject(CacheName, results, cacheExpirationOptions);

                    return results;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieve a movie by name
        /// </summary>
        /// <param name="moviename"></param>
        /// <param name="username"></param>
        /// <returns>Return a movie</returns>
        public async Task<IEnumerable<IMovie>> GetSpecificMovieDetailsByName
        (
            string moviename,
            string username
        )
        {
            try
            {
                CacheName = "Movies" + username;

                //If the cache exists return it
                if (_memoryCache.GetObject(CacheName) != null)
                {
                    return (IEnumerable<IMovie>)await _memoryCache.GetObject(CacheName);
                }

                //Retrieve the movies from the database based on the username
                //of the JWT token username
                using (var connection = new SqlConnection(_config.Value.ConnectionString))
                {
                    DynamicParameters parameters = new DynamicParameters();

                    parameters.Add("@user", dbType: DbType.AnsiString, value: username, direction: ParameterDirection.Input);
                    parameters.Add("@moviename", dbType: DbType.AnsiString, value: moviename, direction: ParameterDirection.Input);

                    var results = connection.Query<Movie>("sprocName", parameters, commandType: CommandType.StoredProcedure);

                    //The cache was empty therefore set a new cache object
                    await _memoryCache.SetObject(CacheName, results.ToByteArray(), cacheExpirationOptions);

                    return results;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region PUT/POST/PATCH REQUESTS

        /// <summary>
        /// Save a movie
        /// </summary>
        /// <param name="movie"></param>
        /// <param name="username"></param>
        /// <returns>Return the movie saved</returns>
        public IEnumerable<IMovie> SaveMovieToDatabase
        (
            IMovie movie
        )
        {
            try
            {
                CacheName = "SaveMovieToDatabase" + movie.moviename + movie.imageurl;

                //If the cache exists return it
                if (_memoryCache.GetObject(CacheName) != null)
                {
                    return null; // return (IEnumerable<IMovie>)await _memoryCache.GetObject(CacheName);
                }

                //Retrieve the movies from the database based on the username
                //of the JWT token username
                using (var connection = new SqlConnection(_config.Value.ConnectionString))
                {
                    DynamicParameters parameters = new DynamicParameters();

                    parameters.Add("@moviename", dbType: DbType.AnsiString, value: movie.moviename, direction: ParameterDirection.Input);
                    parameters.Add("@price", dbType: DbType.Double, value: movie.price, direction: ParameterDirection.Input);
                    parameters.Add("@availableforpurchase", dbType: DbType.Int32, value: movie.availableforpurchase, direction: ParameterDirection.Input);
                    parameters.Add("@theaterreleasedate", dbType: DbType.DateTime, value: movie.theaterreleasedate, direction: ParameterDirection.Input);
                    parameters.Add("@discreleasedate", dbType: DbType.DateTime, value: movie.discreleasedate, direction: ParameterDirection.Input);
                    parameters.Add("@mpaarating", dbType: DbType.AnsiString, value: movie.mpaarating, direction: ParameterDirection.Input);
                    parameters.Add("@imageurl", dbType: DbType.AnsiString, value: movie.imageurl, direction: ParameterDirection.Input);
                    parameters.Add("@movielength", dbType: DbType.Time, value: movie.movielength, direction: ParameterDirection.Input);
                    parameters.Add("@lastmodified", dbType: DbType.DateTime, value: movie.lastmodified, direction: ParameterDirection.Input);
                    parameters.Add("@modifiedby", dbType: DbType.AnsiString, value: movie.modifiedby, direction: ParameterDirection.Input);

                    var results = connection.Query<Movie>("InsertMovie", parameters, commandType: CommandType.StoredProcedure);

                    //The cache was empty therefore set a new cache object
                    _memoryCache.Set(CacheName, results.ToByteArray(), cacheExpirationOptions);

                    return results.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Save a movie
        /// </summary>
        /// <param name="movie"></param>
        /// <param name="username"></param>
        /// <returns>Return the movie saved</returns>
        public IEnumerable<IMovie> SaveUsersMovieToDatabase
        (
            int movieid,
            string username
        )
        {
            return null;
        }

        /// <summary>
        /// Update a movie
        /// </summary>
        /// <param name="id"></param>
        /// <param name="movie"></param>
        /// <param name="username"></param>
        /// <returns>Return movie that was updated</returns>
        public IEnumerable<IMovie> UpdateMovieInDatabase
        (
            int id,
            IMovie movie,
            string username
        )
        {
            return null;
        }

        #endregion
    }

    public interface IMovieData
    {
        Task<IEnumerable<IMovie>> GetUsersMovies
        (
            string username
        );

        Task<IEnumerable<IMovie>> GetAllMoviesIncludingUsers
        (
            string username
        );

        Task<IEnumerable<IMovie>> GetSpecificMovieDetailsById
        (
            int movieid,
            string username
        );

        Task<IEnumerable<IMovie>> GetSpecificMovieDetailsByName
        (
            string moviename,
            string username
        );


        IEnumerable<IMovie> SaveMovieToDatabase
        (
           IMovie movie
        );

        IEnumerable<IMovie> SaveUsersMovieToDatabase
        (
           int movieid,
           string username
        );

        IEnumerable<IMovie> UpdateMovieInDatabase
        (
            int id,
            IMovie movie,
            string username
        );
    }
}

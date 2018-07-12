using Dapper;
using Microsoft.Extensions.Caching.Memory;
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
        private IMemoryCache _memoryCache;
        private string CacheName;

        //Maybe the list of movies updates only once a night via some feed into the database
        //and so you can set a cache because throughout a single day
        //the data will be the same until the next day after midnight, in this example
        private TimeSpan untilMidnight = DateTime.Today.AddDays(1) - DateTime.Now;

        public MovieData(
          IMemoryCache memoryCache
        )
        {
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Retrieve all the users movies
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Return a list of all movies for the user.</returns>
        public List<IMovie> RetrieveAllMovies(
                string username
            )
        {
            try
            {
                CacheName = "Movies" + username;

                //If the cache exists return it
                if (_memoryCache.TryGetValue(CacheName, out List<IMovie> movies))
                {
                    return movies;
                }

                //Retrieve the movies from the database based on the username
                //of the JWT token username
                using (var connection = new SqlConnection("test"))
                {
                    DynamicParameters parameters = new DynamicParameters();

                    parameters.Add("@user", dbType: DbType.AnsiString, value: username, direction: ParameterDirection.Input);

                    var results = connection.Query<IMovie>("sprocName", parameters, commandType: CommandType.StoredProcedure);

                    //The cache was empty therefore set a new cache object
                    _memoryCache.Set(
                        CacheName,
                        results,
                        new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(
                            DateTimeOffset.UtcNow.AddMinutes(
                                untilMidnight.TotalMinutes
                            )
                        )
                    );

                    return results.ToList();
                }
            }
            catch(Exception ex)
            {
                return null;
            }         
        }
    }

    public interface IMovieData
    {
        List<IMovie> RetrieveAllMovies(
             string username
         );
    }
}

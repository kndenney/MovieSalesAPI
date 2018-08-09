using Dapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MovieSalesAPI.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSalesAPI.Data.User
{
    public class UserData : IUserData
    {
        private IMemoryCache _memoryCache;
        private string CacheName;

        private TimeSpan untilMidnight = DateTime.Today.AddDays(1) - DateTime.Now;
        private string connectionString = "";
        private IOptions<AppConfig> _config;

        public UserData(
          IMemoryCache memoryCache,
          IOptions<AppConfig> config
        )
        {
            _memoryCache = memoryCache;
            _config = config;
            connectionString = _config.Value.ConnectionString;
        }

        /// <summary>
        /// Create a users account
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Return the user</returns>
        public IEnumerable<IUser> CreateUserAccount
        (
            IUser user
        )
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(connectionString))
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        DynamicParameters parameters = new DynamicParameters();

                        parameters.Add("@username", dbType: DbType.AnsiString, value: user.Username, direction: ParameterDirection.Input);
                        parameters.Add("@password", dbType: DbType.AnsiString, value: user.Password, direction: ParameterDirection.Input);

                        var results = connection.Query<User>("CreateUserAccount", parameters, commandType: CommandType.StoredProcedure);

                        return results;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

    public interface IUserData
    {
        IEnumerable<IUser> CreateUserAccount(IUser user);
    }
}

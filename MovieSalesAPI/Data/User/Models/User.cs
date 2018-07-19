using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSalesAPI.Data.User
{
    public class User : IUser
    {
        public string Username { get; set; }
        public string Name { get; set; }
    }

    public interface IUser
    {
        string Username { get; set; }
        string Name { get; set; }
    }
}

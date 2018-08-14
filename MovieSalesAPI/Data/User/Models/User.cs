using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSalesAPI.Data.User
{
    public class User : IUser
    {
        [Required]
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public interface IUser
    {
        string Username { get; set; }
        string Password { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSalesAPI.Data.User
{
    public class User : IUser
    {
        [BindRequired]
        public string Username { get; set; }
        [BindRequired]
        public string Password { get; set; }
    }

    public interface IUser
    {
        string Username { get; set; }
        string Password { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSalesAPI.Data.User
{
    public class EndUsersname : IEndUsersname
    {
        public string Username { get; set; }
    }

    public interface IEndUsersname
    {
        string Username { get; set; }
    }
}

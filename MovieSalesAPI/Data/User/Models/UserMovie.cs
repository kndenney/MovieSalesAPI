using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSalesAPI.Data.User.Models
{
    public class UserMovie
    {
        public int Userid { get; set; }
        public int Movieid { get; set; }
        public DateTime DatePurchased { get; set; }
        public double PurchasePrice { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSalesAPI
{
    /// <summary>
    /// Model class for movie details
    /// </summary>
    public class Movie : IMovie
    {
        public string movieid { get; set; }
        public string moviename { get; set; }
        public double price { get; set; }
        public DateTime theaterreleasedate { get; set; }
        public DateTime discreleasedate { get; set; }
        public string mpaarating { get; set; }
        public string imageurl { get; set; }
        public TimeSpan movielength { get; set; }
        public DateTime lastmodified { get; set; }
        public string modifiedby { get; set; }
    }

    public interface IMovie
    {
         string movieid { get; set; }
         string moviename { get; set; }
         double price { get; set; }
         DateTime theaterreleasedate { get; set; }
         DateTime discreleasedate { get; set; }
         string mpaarating { get; set; }
         string imageurl { get; set; }
         TimeSpan movielength { get; set; }
         DateTime lastmodified { get; set; }
         string modifiedby { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSalesAPI
{
    /// <summary>
    /// Model class for movie details
    /// </summary>
    public class Movie : IMovie
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public interface IMovie
    {
        string Name { get; set; }
        string Description { get; set; }
    }
}

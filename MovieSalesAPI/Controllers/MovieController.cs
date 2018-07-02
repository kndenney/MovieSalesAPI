﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MovieSalesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        // GET: api/Movie
        [HttpGet]
        public IEnumerable<string> GetAllMovieDetails()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Movie/5
        [HttpGet("{id}", Name = "Get")]
        public string GetSpecificMovieDetails(int id)
        {
            return "value";
        }

        // POST: api/Movie
        [HttpPost]
        public void PostMovieToDatabase([FromBody] string value)
        {
        }

        // PUT: api/Movie/5
        [HttpPut("{id}")]
        public void PutMovieToDatabase(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void DeleteMovieFromDatabase(int id)
        {
        }
    }
}

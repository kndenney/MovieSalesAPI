using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MovieSalesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        /// <summary>
        /// Retrieve all values that I have created
        /// </summary>
        /// <returns>Action result of list of values</returns>
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            //throw new System.ArgumentException("Parameter cannot be null", "original");
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        /// <summary>
        /// Retrieve specific movie.
        /// </summary>
        /// <param name="id">Movie ID</param>
        /// <returns>Movie</returns>
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "kyledenney";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

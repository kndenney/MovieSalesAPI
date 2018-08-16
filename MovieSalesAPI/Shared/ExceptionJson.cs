using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSalesAPI.Shared
{
    public class ExceptionJson
    {
        private string exception;
        private string innerexception;
        private string stacktrace;

        public string Exception { get; set; }
        public string Stacktrace { get; set; }

        public string InnerException { get; set; }
    }
}

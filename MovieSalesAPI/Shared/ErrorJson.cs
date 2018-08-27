using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSalesAPI.Shared
{
    public class ErrorJson
    {
        private string error;
        private string internalcode;

        public string Error { get; set; }
        public string Internalcode { get; set; }
    }
}

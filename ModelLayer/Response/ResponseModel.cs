using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Response
{
    public class ResponseModel<T>
    {
       
<<<<<<< HEAD
        public int StatusCode { get; set; }
=======
       // public int StatusCode { get; set; }
>>>>>>> c8ef75a48d6d0f2109f56342f5abb787d9323a7f
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
    }
}

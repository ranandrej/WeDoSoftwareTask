using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    public class Result<T>
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public T? Data { get; set; }

        public static Result<T> Fail(string error)
            => new() { Success = false, Error = error };

        public static Result<T> Ok(T data)
            => new() { Success = true, Data = data };
    }
}

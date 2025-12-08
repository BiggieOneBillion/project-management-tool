using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.CORE.Interfaces
{
   public interface IResult<T>
{
    bool Success { get; set; }
    string Message { get; set; }
    T? Data { get; set; }
}
}
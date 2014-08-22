using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mp3.Infrastructure
{
    public interface IConverter<T> where T:class
    {
        T Convert(T obj);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHapiSampleApplication.Models
{
    public interface IParser<T>
    {
        T Parse(string message);
        Patient Convert(T message);
    }
}

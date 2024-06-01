using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier;

public class ShittifierArgumentException : Exception
{
    public ShittifierArgumentException(string message) : base(message) { }
}
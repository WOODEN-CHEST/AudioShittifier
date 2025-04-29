using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier;

public class ShittifyException : Exception
{
    public ShittifyException(string? message) : base(message) { }
}
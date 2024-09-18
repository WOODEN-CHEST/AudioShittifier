using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Layout;

public class ModifierLayoutException : Exception
{
    public ModifierLayoutException(string? message) : base(message) { }
}
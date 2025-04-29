using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Layout;

public class ModifierParseException : Exception
{
    public ModifierParseException(string? message) : base(message) { }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Layout;

public class ModifierBuildException : Exception
{
    public ModifierBuildException(string? message) : base(message) { }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Modifiers.Layout;

public class ModifierLayoutException : Exception
{
    public ModifierLayoutException(string message) : base($"Invalid modifier layout! {message}") { }
}
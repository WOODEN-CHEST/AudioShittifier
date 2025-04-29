using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier;

public class ShittifyCompleteEventArgs
{
    // Fields.
    public string FilePath { get; }
    public int FileNumber{ get; }
    public int MaxFileNumber { get; }


    // Constructors.
    public ShittifyCompleteEventArgs(string filePath, int fileNumber, int maxFileNumber)
    {
        FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        FileNumber = fileNumber;
        MaxFileNumber = maxFileNumber;
    }
}
using System;

namespace ZCore.Modules.SexyCompressors.ResourceStreamBundle.Exceptions
{
// Exception thrown when HeadLength differs from Expected

public class FileDesc_InvalidHeadLengthException(long pos, ushort lengthRead, ushort expectedLength) 
: Exception(string.Format(errorMsg, pos, lengthRead, expectedLength) )
{
public long Position { get; } = pos;

public ushort HeadLength { get; } = lengthRead;

// Error MSG (should load it from LocStrings)

private const string errorMsg = "Invalid Header Length for FileDescription, found at: {0}\n" +

"Length obtained: {1} - Expected: {2}";
}

}
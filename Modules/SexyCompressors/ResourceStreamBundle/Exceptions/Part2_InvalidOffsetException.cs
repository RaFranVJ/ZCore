using System;

namespace ZCore.Modules.SexyCompressors.ResourceStreamBundle.Exceptions
{
// Exception thrown when Offset read for Part2 in RSB differs from Expected

public class Part2_InvalidOffsetException(long pos, uint offsetRead, uint expectedOffset) 
: Exception(string.Format(errorMsg, pos, offsetRead, expectedOffset) )
{
public long Position { get; } = pos;

public uint OffsetRead { get; } = offsetRead;

// Error MSG (should load it from LocStrings)

private const string errorMsg = "Invalid Offset for Part2, found at: {0}\n" +

"Offset read: {1} - Expected: {2}";
}

}
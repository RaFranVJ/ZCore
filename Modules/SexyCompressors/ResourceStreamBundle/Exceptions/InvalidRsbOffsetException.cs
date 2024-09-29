using System;

namespace ZCore.Modules.SexyCompressors.ResourceStreamBundle.Exceptions
{
// Exception thrown when RSB Offset exceeds the Limit

public class InvalidRsbOffsetException(long pos, uint endOffset) : Exception(string.Format(errorMsg, pos, endOffset) )
{
public long Position { get; } = pos;

public long EndOffset{ get; } = endOffset;

// Error MSG (should load it from LocStrings)

private const string errorMsg = "Invalid RSB Offset: {0} (Expected: {1})";
}

}
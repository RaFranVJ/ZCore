using System;

namespace ZCore.Modules.SexyCompressors.ResourceStreamBundle.Exceptions
{
// Exception thrown when RsgNumber read differs from Expected

public class InvalidRsgNumberException(long pos, uint numberRead, uint expectedNumber) 
: Exception(string.Format(errorMsg, pos, numberRead, expectedNumber) )
{
public long Position{ get; } = pos;

public uint NumberRead{ get; } = numberRead;

// Error MSG (should load it from LocStrings)

private const string errorMsg = "Invalid RSG Number, found at: {0}\n" +

"Number read: {1} - Expected: {2}";
}

}
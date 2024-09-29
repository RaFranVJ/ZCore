using System.Collections.Generic;
using ZCore.Modules.SexyCompressors.ResourceStreamBundle.Exceptions;

namespace ZCore.Modules.SexyCompressors.ResourceStreamBundle
{
/// <summary> Performs some useful Tasks for RSB Files </summary>

public static class RsbHelper
{
// Check if End Offset is Reached

public static void CheckEndOffset(BinaryStream targetStream, uint endOffset)
{

if(targetStream.Position != endOffset)         
throw new InvalidRsbOffsetException(targetStream.Position, endOffset);

}

// Add Key to StrPool

public static uint ThrowInPool(BinaryStream sourceStream, Dictionary<string, uint> pool, string key, Endian endian)
{

if(!pool.TryGetValue(key, out uint value))
{
value = (uint)sourceStream.Position;
pool.Add(key, value);

sourceStream.WriteStringUntilZero(key, default, endian);
}

return value;
}

}

}
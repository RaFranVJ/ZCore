using System;

namespace ZCore.Modules.SexyCompressors.ResourceStreamBundle.Exceptions
{
// Exception thrown when RSB seems to be Corrupted

public class CorruptedRsbException(long pos) : Exception($"This RSB seems to be Corrupted")
{
// File Position

public long Position { get; } = pos;
}

}
using System;

namespace ZCore.Modules.SexyCompressors.ResourceStreamGroup.Exceptions
{
// Exception thrown when FileList Offset differs from Expected

public class FileList_InvalidOffsetException(long offset, uint expected) 
: Exception($"Invalid Offset for FileList: {offset}, Expected {expected} for RSG")
{
public long Offset{ get; } = offset;

}

}
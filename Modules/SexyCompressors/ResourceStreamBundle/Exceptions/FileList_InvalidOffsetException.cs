using System;

namespace ZCore.Modules.SexyCompressors.ResourceStreamBundle.Exceptions
{
// Exception thrown when FileList Offset differs from Expected

public class FileList_InvalidOffsetException(uint ver, uint offset, uint expected) 
: Exception(string.Format(errorMsg, ver, offset, expected) )
{
public long FileVersion{ get; } = ver;

public uint OffsetRead{ get; } = offset;

// Error MSG (should load it from LocStrings)

private const string errorMsg = "Invalid Offset for FileList: {1}\nExpected {2} for v{0}";
}

}
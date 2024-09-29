using System;

namespace ZCore.Serializables.ArgumentsInfo.SexyCompressor.Smf.Integrity
{
/// <summary> Determines how Integrity Check should be Done. </summary>

[Flags]

public enum IntegrityCheckType
{
/// <summary> File Integrity should be Skipped. </summary>
None = 0,

/// <summary> File Integrity should be Verified by the Hash inside Tags. </summary>
Tags = 4,

/// <summary> File Integrity should be Verified by the Size expressed in Hexadecimal. </summary>
HexSize = 8,

/// <summary> File Integrity should be Verified by the Adler32 Checksum. </summary>
Adler32 = 32,

/// <summary> File Integrity should be Verified by Tags and Size. </summary>
CompareTagsAndSize = Tags | HexSize,

/// <summary> File Integrity should be Verified by Tags and Adler32. </summary>
CompareTagsAndAdler32 = Tags | Adler32,

/// <summary> File Integrity should be Verified by Size and Adler32. </summary>
CompareSizeAndAdler32 = HexSize | Adler32,

/// <summary> A Full Scope will be Done in order to Check File Integrity. </summary>
FullScope = Tags | HexSize | Adler32
}

}
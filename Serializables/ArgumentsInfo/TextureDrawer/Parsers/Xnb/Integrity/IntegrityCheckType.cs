using System;

namespace ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.Xnb.Integrity
{
/// <summary> Determines how Integrity Check should be Done on XNB Files. </summary>

[Flags]

public enum IntegrityCheckType
{
/// <summary> File Integrity should be Skipped. </summary>
None = 0,

/// <summary> File Integrity should be Verified by Comparing TextureSize. </summary>
TextureComparisson = 1,

/// <summary> File Integrity should be Verified by Comparing XnbSize. </summary>
XnbComparisson = 2,

/// <summary> A Full Scope will be Done in order to Check File Integrity. </summary>
FullScope = TextureComparisson | XnbComparisson
}

}
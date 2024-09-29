namespace ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.Txz
{
/// <summary> Determines how TXZ Files should be Parsed. </summary>

public enum TxzFormat : ushort
{
/** <summary> TXZ won't use a Specific Encoding. </summary>
<remarks> Using this Format will raise an Exception. </remarks> */

None,

/// <summary> TXZ will use ABGR8888 </summary>
ABGR8888,

/// <summary> TXZ will use RGBA4444 </summary>
RGBA4444,

/// <summary> TXZ will use RGBA5551 </summary>
RGBA5551,

/// <summary> TXZ will use RGB565. </summary>
RGB565,
}

}
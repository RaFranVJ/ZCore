namespace ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.UTex
{
/// <summary> Determines how TEX Files should be Parsed. </summary>

public enum UTexFormat : ushort
{
/** <summary> TXZ Files won't use a Specific Encoding. </summary>
<remarks> Using this Format will raise an Exception. </remarks> */

None,

/// <summary> TEX Files will use ABGR8888 </summary>
ABGR8888,

/// <summary> TEX Files will use RGBA4444 </summary>
RGBA4444,

/// <summary> TEX Files will use RGBA5551 </summary>
RGBA5551,

/// <summary> TEX Files will use RGB565. </summary>
RGB565,
}

}
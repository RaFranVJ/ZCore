namespace ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.SexyTex
{
/// <summary> Determines how TEX Files (SexyTexture) should be Parsed. </summary>

public enum SexyTexFormat : int
{
/** <summary> TEX Files won't use a Specific Encoding. </summary>
<remarks> Using this Format will raise an Exception. </remarks> */

None,

/// <summary> TEX Files will use ARGB8888 </summary>	
ARGB8888,

/// <summary> TEX Files will use ARGB4444 </summary>
ARGB4444,

/// <summary> TEX Files will use ARGB1555 </summary>
ARGB1555,

/// <summary> TEX Files will use RGB565 </summary>
RGB565,

/// <summary> TEX Files will use ABGR8888 </summary>
ABGR8888,

/// <summary> TEX Files will use RGBA4444 </summary>
RGBA4444,

/// <summary> TEX Files will use RGBA5551 </summary>
RGBA5551,

/// <summary> TEX Files will use XRGB8888 </summary>
XRGB8888,

/// <summary> TEX Files will use LA88 </summary>
LA88,

/** <summary> TEX Files will use LUT8 </summary>

<remarks This Format seems to be Invalid in game because the Expression
<c>(uint)(format - 2) > 8</c> will always Return a Positive Number <remarks> */

LUT8
}

}
namespace ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.Xnb
{
/// <summary> Determines how XNB Files should be Parsed. </summary>

public enum XnbFormat : uint
{
/// <summary> XNB Files will use ABGR8888 </summary>
ABGR8888,

/// <summary> XNB Files will use RGBA4444 </summary>
RGBA4444,

/// <summary> XNB Files will use RGB565 </summary>
RGB565,

/// <summary> XNB Files will use RGBA5551 </summary>
RGBA5551,

/// <summary> XNB Files will use DXT1 in the RGB Order. </summary>
DXT1_RGB,

/// <summary> XNB Files will use DXT2 in the RGBA Order. </summary>
DXT2_RGBA,

/// <summary> XNB Files will use DXT3 in the RGBA Order. </summary>
DXT3_RGBA,

/// <summary> XNB Files will use DXT4 in the RGBA Order. </summary>
DXT4_RGBA,

/// <summary> XNB Files will use DXT5 in the RGBA Order. </summary>
DXT5_RGBA
}

}
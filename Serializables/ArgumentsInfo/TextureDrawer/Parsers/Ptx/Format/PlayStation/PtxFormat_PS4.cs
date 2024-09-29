namespace ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.Ptx.Format.PlayStation
{
/// <summary> Determines how PopCap Texture (PTX) Files should be Parsed for PS4. </summary>

public enum PtxFormat_PS4 : uint
{
/// <summary> PTX Files will use RGBA4444 </summary>
RGBA4444,

/// <summary> PTX Files will use RGB565 </summary>
RGB565,

/// <summary> PTX Files will use RGBA5551 </summary>
RGBA5551,

/// <summary> PTX Files will use ABGR8888 </summary>
ABGR8888,

/// <summary> PTX Files will use DXT5 in the RGBA Order (Morton Algorithm, Tiled) </summary>
DXT5_RGBA_MortonBlock = 5,

/// <summary> PTX Files will use L8 </summary>
L8,

/// <summary> PTX Files will use LA88 </summary>
LA88,

/// <summary> PTX Files will use DXT1 in the RGB Order </summary>
DXT1_RGB = 35,

/// <summary> PTX Files will use DXT3 in the RGBA Order </summary>
DXT3_RGBA,

/// <summary> PTX Files will use DXT5 in the RGBA Order </summary>
DXT5_RGBA
}

}
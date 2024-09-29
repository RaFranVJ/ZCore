namespace ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.Ptx.Format.PlayStation
{
/// <summary> Determines how PopCap Texture (PTX) Files should be Parsed for PSP. </summary>

public enum PtxFormat_PSP : uint
{
/// <summary> PTX Files will use RGBA4444 </summary>
RGBA4444,

/// <summary> PTX Files will use RGB565 </summary>
RGB565,

/// <summary> PTX Files will use RGBA5551 </summary>
RGBA5551,

/// <summary> PTX Files will use DXT1 in the RGB Order </summary>
DXT1_RGB = 35,

/// <summary> PTX Files will use DXT3 in the RGBA Order </summary>
DXT3_RGBA,

/// <summary> PTX Files will use DXT5 in the RGBA Order </summary>
DXT5_RGBA,

/// <summary> PTX Files will use ATITC in the RGB Order </summary>
ATITC_RGB,
		
/// <summary> PTX Files will use ATITC in the RGBA Order </summary>		
ATITC_RGBA,

/// <summary> PTX Files will use XRGB8888 (with Alpha) </summary>		
XRGB8888_A8 = 149
}

}
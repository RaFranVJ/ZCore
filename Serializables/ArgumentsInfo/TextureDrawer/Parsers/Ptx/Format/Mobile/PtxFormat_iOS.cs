namespace ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.Ptx.Format.Mobile
{
/// <summary> Determines how PopCap Texture (PTX) Files should be Parsed for iOS. </summary>

public enum PtxFormat_iOS : uint
{
/// <summary> PTX Files will use ARGB8888 </summary>
ARGB8888,

/// <summary> PTX Files will use RGBA4444 </summary>
RGBA4444,

/// <summary> PTX Files will use RGBA5551 </summary>
RGBA5551 = 3,

/// <summary> PTX Files will use RGBA4444 (Tiled) </summary>
RGBA4444_Block = 21,

/// <summary> PTX Files will use RGBA5551 (Tiled) </summary>
RGBA5551_Block = 23,

/// <summary> PTX Files will use PVRTC in the RGBA Order (4-bits per Pixel) </summary> 
PVRTC_4BPP_RGBA = 30,

/// <summary> PTX Files will use PVRTC in the RGB Order (4-bits per Pixel, with Alpha) </summary>
PVRTC_4BPP_RGB_A8 = 148
}

}
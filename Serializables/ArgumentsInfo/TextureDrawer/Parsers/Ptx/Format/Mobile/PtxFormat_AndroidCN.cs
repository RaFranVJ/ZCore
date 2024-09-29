namespace ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.Ptx.Format.Mobile
{
/// <summary> Determines how PopCap Texture (PTX) Files should be Parsed for Android (Chinese Version). </summary>

public enum PtxFormat_AndroidCN : uint
{
/// <summary> PTX Files will use ABGR8888 </summary>
ABGR8888,

/// <summary> PTX Files will use RGBA4444 </summary>
RGBA4444,

/// <summary> PTX Files will use RGB565 </summary>
RGB565,

/// <summary> PTX Files will use RGBA5551 </summary>
RGBA5551,

/// <summary> PTX Files will use RGBA4444 (Tiled) </summary>
RGBA4444_Block = 21,

/// <summary> PTX Files will use RGB565 (Tiled) </summary>
RGB565_Block,

/// <summary> PTX Files will use RGBA5551 (Tiled) </summary>
RGBA5551_Block,

/// <summary> PTX Files will use ETC1 in the RGB Order (with an Alpha Palette) </summary>
ETC1_RGB_A_Palette = 147
}

}
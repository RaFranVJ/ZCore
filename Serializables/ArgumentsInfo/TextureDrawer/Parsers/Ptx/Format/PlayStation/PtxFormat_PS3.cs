namespace ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.Ptx.Format.PlayStation
{
/** <summary> Determines how PopCap Texture (PTX) Files should be Parsed for PS3. </summary>

<remarks> Format must be Parsed as a String of 4 Chars in the PTX Stream, <para>

</para>so enum Names must meet this Limitations. </remarks> */

public enum PtxFormat_PS3
{
/// <summary> PTX Files will use DXT5 (RGBA Order) </summary>
DXT5,

/// <summary> PTX Files will use ABGR (8888 Pixels) </summary>
ABGR,

/// <summary> PTX Files will use L8 </summary>
L008,

/// <summary> PTX Files will use PVRTC (2-bits per Pixel, RGBA Order) </summary> 
PVR2,

/// <summary> PTX Files will use DXT1 (RGB Order) </summary>
DXT1,

/// <summary> PTX Files will use DXT2 (RGBA Order) </summary>
DXT2,

/// <summary> PTX Files will use DXT3 (RGBA Order) </summary>
DXT3,

/// <summary> PTX Files will use DXT4 (RGBA Order) </summary>
DXT4
}

}
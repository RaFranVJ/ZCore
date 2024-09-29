using ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions.PopCapTexture;
using ZCore.Modules.SexyCompressors.ResourceStreamGroup.Exceptions;

namespace ZCore.Modules.SexyCompressors.ResourceStreamGroup.Definitions.Resources
{
/// <summary> Represents a Resource inside a RSB Stream </summary>

public class PtxInfoForRsg
{
/** <summary> Gets or Sets the Texture ID. </summary>
<returns> The TextureID </returns> */

public int TextureID{ get; set; }

/** <summary> Gets or Sets the Texture Width. </summary>
<returns> The TextureWidth. </returns> */

public int TextureWidth{ get; set; }

/** <summary> Gets or Sets the Texture Height. </summary>
<returns> The TextureHeight. </returns> */

public int TextureHeight{ get; set; }

/// <summary> Creates a new Instance of the <c>PtxInfoForRsg</c> </summary>

public PtxInfoForRsg()
{
}

/// <summary> Creates a new Instance of the <c>PtxInfoForRsg</c> </summary>

public PtxInfoForRsg(int id, int width, int height)
{
TextureID = id;

TextureWidth = width;
TextureHeight = height;
}

// Validate PTX Info

public static void Validate(PtxParamsForRsb manifest, PtxInfoForRsg info, string path)
{

if(manifest.TextureWidth != info!.TextureWidth)
throw new InvalidPacketWidthException(info!.TextureWidth, manifest.TextureWidth, path);

if(manifest.TextureHeight != info!.TextureHeight)
throw new InvalidPacketHeightException(info!.TextureHeight, manifest.TextureHeight, path);

}

}

}
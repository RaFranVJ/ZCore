namespace ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions.PopCapTexture
{
/// <summary> Represents some Params for a PopCapTexture (PTX) that was Extracted from a RSB Stream </summary>

public class PtxParams
{
/** <summary> Gets or Sets the TextureFormat. </summary>
<returns> The TextureFormat </returns> */

public uint TextureFormat{ get; set; }

/** <summary> Gets or Sets the TexturePitch. </summary>
<returns> The TexturePitch </returns> */

public int TexturePitch{ get; set; }

/** <summary> Gets or Sets the AlphaSize. </summary>
<returns> The AlphaSize </returns> */

public int? AlphaSize{ get; set; }

/** <summary> Gets or Sets the AlphaChannel that was used. </summary>
<returns> The AlphaChannel </returns> */

public PtxAlphaChannel? AlphaChannel{ get; set; }

/// <summary> Creates a new Instance of the <c>PtxParams</c> </summary>

public PtxParams()
{
}

/// <summary> Creates a new Instance of the <c>PtxParams</c> </summary>

public PtxParams(uint format, int pitch, int? aSize, PtxAlphaChannel? aChannel)
{
TextureFormat = format;
TexturePitch = pitch;

AlphaSize = aSize;
AlphaChannel = aChannel;
}

}

}
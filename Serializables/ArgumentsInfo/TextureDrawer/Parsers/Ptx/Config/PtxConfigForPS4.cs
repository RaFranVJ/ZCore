using ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.Ptx.Format.PlayStation;

namespace ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.Ptx.Config
{
/// <summary> Groups additional Options for the PTX Parser (used on PS4). </summary>

public class PtxConfigForPS4 : GenericImgSettings_Metadata<PtxFormat_PS4>
{
/** <summary> Gets or Sets the Endian Order used when Parsing Files. </summary>
<returns> The Endian Order. </returns> */

public bool CompareTextureSizeOnDecoding{ get; set; }

/// <summary> Creates a new Instance of the <c>PtxConfigForPS4</c>. </summary>

public PtxConfigForPS4()
{
CompareTextureSizeOnDecoding = true;
}

}

}
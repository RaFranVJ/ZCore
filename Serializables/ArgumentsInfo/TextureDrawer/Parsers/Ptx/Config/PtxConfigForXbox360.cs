using ZCore.Serializables.ArgumentsInfo.FileManager.Metadata;
using ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.Ptx.Format.PlayStation;

namespace ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.Ptx.Config
{
/// <summary> Groups additional Options for the PTX Parser (used on Xbox360). </summary>

public class PtxConfigForXbox360 : ParamGroupInfo
{
/** <summary> Gets or Sets the Endian Order used when Parsing Files. </summary>
<returns> The Endian Order. </returns> */

public Endian Endianness{ get; set; }

/** <summary> Gets or Sets some Info related to how the Info inside SMF Files should be Loaded. </summary>
<returns> The Import Config. </returns> */

public MetadataImportParams MetadataImportConfig{ get; set; }

/** <summary> Gets or Sets some Info related to how the Info inside SMF Files should be Saved. </summary>
<returns> The Export Config. </returns> */

public MetadataExportParams MetadataExportConfig{ get; set; }

/// <summary> Creates a new Instance of the <c>PtxConfigForPS4</c>. </summary>

public PtxConfigForXbox360()
{
MetadataImportConfig = new();
MetadataExportConfig = new();
}

}

}
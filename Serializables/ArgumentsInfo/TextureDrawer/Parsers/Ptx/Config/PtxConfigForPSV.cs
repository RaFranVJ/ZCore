using iVersion = ZCore.Modules.TextureDrawer.Parsers.PackedTexture.PlayStation.PSV.PtxVersion;

using ZCore.Serializables.ArgumentsInfo.FileManager.Archive;
using ZCore.Serializables.ArgumentsInfo.FileManager.Metadata;

namespace ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.Ptx.Config
{
/// <summary> Groups additional Options for the PTX Parser (used on PSV). </summary>

public class PtxConfigForPSV : ParamGroupInfo
{
/** <summary> Gets or Sets the Endian Order used when Parsing Files. </summary>
<returns> The Endian Order. </returns> */

public Endian Endianness{ get; set; }

/** <summary> Gets or Sets the Endian Order used when Parsing Files. </summary>
<returns> The Endian Order. </returns> */

public bool CompareFileSizeOnDecoding{ get; set; }

/** <summary> Gets or Sets some Options related to PTX Files. </summary>
<returns> The PTX Version. </returns> */

public FileVersionDetails<uint> PtxVersion{ get; set; }

/** <summary> Gets or Sets some Info related to how the Info inside PTX Files should be Loaded. </summary>
<returns> The Import Config. </returns> */

public MetadataImportParams MetadataImportConfig{ get; set; }

/** <summary> Gets or Sets some Info related to how the Info inside PTX Files should be Saved. </summary>
<returns> The Export Config. </returns> */

public MetadataExportParams MetadataExportConfig{ get; set; }

/// <summary> Creates a new Instance of the <c>PtxConfigForPSV</c>. </summary>

public PtxConfigForPSV()
{
CompareFileSizeOnDecoding = true;
PtxVersion = new(iVersion.ExpectedVersions.MaxValue);

MetadataImportConfig = new();
MetadataExportConfig = new();
}

/// <summary> Checks each nullable Field of the <c>PtxSettings</c> Instance and Validates it, in case it's <c>null</c>. </summary>

public override void CheckForNullFields()
{
PtxConfigForPSV defaultOptions = new();

#region ======== Set default Values to Null Fields ========

MetadataImportConfig ??= defaultOptions.MetadataImportConfig;
MetadataExportConfig ??= defaultOptions.MetadataExportConfig;

#endregion

MetadataImportConfig.CheckForNullFields();
MetadataExportConfig.CheckForNullFields();
}

}

}
using ZCore.Serializables.ArgumentsInfo.Compressor;
using ZCore.Serializables.ArgumentsInfo.FileManager.Metadata;

namespace ZCore.Serializables.ArgumentsInfo.SexyCompressor.ArcV
{
/// <summary> Groups options related to the ARCV Compressor. </summary>

public class ArcvSettings : GenericCompressorInfo
{
/** <summary> Gets or Sets the Endian Order used when Parsing Files. </summary>
<returns> The Endian Order. </returns> */

public Endian Endianness{ get; set; }

/** <summary> Gets or Sets the Workspace Name used when Packing or Unpacking Files. </summary>
<returns> The Workspace Name. </returns> */

public string WorkspaceName{ get; set; }

/** <summary> Gets or Sets some Info related to how the Adler32 Bytes should be Calculated. </summary>
<returns> The Adler32 Bytes Info. </returns> */

public ArcvStyleInfo StyleInfo{ get; set; }

/** <summary> Gets or Sets some Info related to how the Info inside ARCV Files should be Loaded. </summary>
<returns> The Import Config. </returns> */

public MetadataImportParams MetadataImportConfig{ get; set; }

/** <summary> Gets or Sets some Info related to how the Info inside ARCV Files should be Saved. </summary>
<returns> The Export Config. </returns> */

public MetadataExportParams MetadataExportConfig{ get; set; }

/** <summary> Gets or Sets a Boolean that Determines if the Extracted Files should Use Binary Names. </summary>
<returns> <c>true</c> if Binary Names should be Used; otherwise, <c>false</c>. </returns> */

public bool UseBinNamesOnDecompression{ get; set; }

/// <summary> Creates a new Instance of the <c>ArcvInfo</c>. </summary>

public ArcvSettings()
{
WorkspaceName = "NDS";
StyleInfo = new();

MetadataImportConfig = new();
MetadataExportConfig = new();

UseBinNamesOnDecompression = true;
}

/// <summary> Checks each nullable Field of the instance and Validates it, in case it's <c>null</c>. </summary>

public override void CheckForNullFields()
{
ArcvSettings defaultOptions = new();

#region ======== Set default Values to Null Fields ========

WorkspaceName ??= defaultOptions.WorkspaceName;
StyleInfo ??= defaultOptions.StyleInfo;
MetadataImportConfig ??= defaultOptions.MetadataImportConfig;
MetadataExportConfig ??= defaultOptions.MetadataExportConfig;

#endregion

StyleInfo.CheckForNullFields();
MetadataImportConfig.CheckForNullFields();
MetadataExportConfig.CheckForNullFields();

BufferSizeForIOTasks = BufferSizeRange.CheckParamRange(BufferSizeForIOTasks);
}

}

}
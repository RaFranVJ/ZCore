using ZCore.Serializables.ArgumentsInfo.Compressor;
using ZCore.Serializables.ArgumentsInfo.FileManager.Metadata;

namespace ZCore.Serializables.ArgumentsInfo.SexyCompressor.Rsb
{
/// <summary> Groups options related to the RSB Compressor. </summary>

public class RsbSettings : GenericCompressorInfo
{
/** <summary> Gets or Sets the Endian Order used when Parsing Files. </summary>
<returns> The Endian Order. </returns> */

public Endian Endianness{ get; set; }

// Packer Info here

/** <summary> Gets or Sets some Info related to how the RSB Files should be Extracted. </summary>
<returns> The ExtractParams. </returns> */

public RsbExtractParams ExtractParams{ get; set; }

/** <summary> Gets or Sets some Info related to how the Info inside RSB Files should be Saved. </summary>
<returns> The Export Config. </returns> */

public MetadataExportParams MetadataExportConfig{ get; set; }

/** <summary> Gets or Sets a Boolean that Determines if Compat should be Forced to be Fit into expected. </summary>
<returns> <b>true</b> or <b>false</b>. </returns> */

public bool AdaptCompatibilityBetweenVersions{ get; set; }

// b

public StringCase StrCaseForResNames{ get; set; }

/// <summary> Creates a new Instance of the <c>SmfInfo</c>. </summary>

public RsbSettings()
{
BufferSizeForIOTasks = BufferSizeRange.MaxValue;

ExtractParams = new();
MetadataExportConfig = new();

AdaptCompatibilityBetweenVersions = true;
}

/// <summary> Checks each nullable Field of the <c>RsbInfo</c> Instance and Validates it, in case it's <c>null</c>. </summary>

public override void CheckForNullFields()
{
RsbSettings defaultOptions = new();

#region ======== Set default Values to Null Fields ========

ExtractParams ??= defaultOptions.ExtractParams;
MetadataExportConfig ??= defaultOptions.MetadataExportConfig;

#endregion

MetadataExportConfig.CheckForNullFields();

BufferSizeForIOTasks = BufferSizeRange.CheckParamRange(BufferSizeForIOTasks);
}

}

}
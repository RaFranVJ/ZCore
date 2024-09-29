using ZCore.Serializables.ArgumentsInfo.Compressor;

namespace ZCore.Serializables.ArgumentsInfo.SexyCompressor.Rsg
{
/// <summary> Groups options related to the RSG Compressor. </summary>

public class RsgSettings : GenericCompressorInfo
{
/** <summary> Gets or Sets the Endian Order used when Parsing Files. </summary>
<returns> The Endian Order. </returns> */

public Endian Endianness{ get; set; }

// Packer Info here

/** <summary> Gets or Sets some Info related to how the RSG Files should be Extracted. </summary>
<returns> The ExtractParams. </returns> */

public RsgExtractParams ExtractParams{ get; set; }

/** <summary> Gets or Sets a Boolean that Determines if Compat should be Forced to be Fit into expected. </summary>
<returns> <b>true</b> or <b>false</b>. </returns> */

public bool AdaptCompatibilityBetweenVersions{ get; set; }

// b

public StringCase StrCaseForFileNames{ get; set; }

/// <summary> Creates a new Instance of the <c>RsgInfo</c>. </summary>

public RsgSettings()
{
BufferSizeForIOTasks = BufferSizeRange.MaxValue;

ExtractParams = new();
AdaptCompatibilityBetweenVersions = true;
}

/// <summary> Checks each nullable Field of the <c>RsgInfo</c> Instance and Validates it, in case it's <c>null</c>. </summary>

public override void CheckForNullFields()
{
RsgSettings defaultOptions = new();

#region ======== Set default Values to Null Fields ========

ExtractParams ??= defaultOptions.ExtractParams;

#endregion

BufferSizeForIOTasks = BufferSizeRange.CheckParamRange(BufferSizeForIOTasks);
}

}

}
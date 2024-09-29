using ZCore.Serializables.ArgumentsInfo.Compressor;
using ZCore.Serializables.ArgumentsInfo.FileManager.Metadata;
using ZCore.Serializables.ArgumentsInfo.FileSecurity.Integrity;
using ZCore.Serializables.ArgumentsInfo.SexyCompressor.Smf.Integrity;

namespace ZCore.Serializables.ArgumentsInfo.SexyCompressor.Smf
{
/// <summary> Groups options related to the SMF Compressor. </summary>

public class SmfSettings : GenericCompressorInfo
{
/** <summary> Gets or Sets the Endian Order used when Parsing Files. </summary>
<returns> The Endian Order. </returns> */

public Endian Endianness{ get; set; }

/** <summary> Gets or Sets the Number of Hex Bytes to Use when Parsing the File Size. </summary>
<returns> The Block Factor. </returns> */

public int NumberOfHexBytes{ get; set; }

/** <summary> Gets or Sets some Info related to how the Adler32 Bytes should be Calculated. </summary>
<returns> The Adler32 Bytes Info. </returns> */

public Adler32BytesInfo Adler32Info{ get; set; }

/** <summary> Gets or Sets some Info related to how SMF Tag should be Generated. </summary>
<returns> <b>true</b> if a Tag should be Generated after Compressing SMF Files; otherwise, <b>false</b>. </returns> */

public SmfTagInfo TagConfig{ get; set; }

/** <summary> Gets or Sets some Info related to how the Adler32 Bytes should be Calculated. </summary>
<returns> The SMF Integrity Info. </returns> */

public SmfIntegrityInfo IntegrityConfig{ get; set; }

/** <summary> Gets or Sets some Info related to how the Info inside SMF Files should be Loaded. </summary>
<returns> The Import Config. </returns> */

public MetadataImportParams MetadataImportConfig{ get; set; }

/** <summary> Gets or Sets some Info related to how the Info inside SMF Files should be Saved. </summary>
<returns> The Export Config. </returns> */

public MetadataExportParams MetadataExportConfig{ get; set; }

/** <summary> Gets or Sets a Boolean that Determines if SMF Ext should be Removed after Decompression. </summary>
<returns> <b>true</b> or <b>false</b>. </returns> */

public bool RemoveSmfExtAfterDecompression{ get; set; }

/// <summary> Creates a new Instance of the <c>SmfInfo</c>. </summary>

public SmfSettings()
{
BufferSizeForIOTasks = BufferSizeRange.MaxValue;

NumberOfHexBytes = 4;
Adler32Info = new();

TagConfig = new();
IntegrityConfig = new();

MetadataImportConfig = new();
MetadataExportConfig = new();
}

/// <summary> Checks each nullable Field of the <c>SmfInfo</c> Instance and Validates it, in case it's <c>null</c>. </summary>

public override void CheckForNullFields()
{
SmfSettings defaultOptions = new();

#region ======== Set default Values to Null Fields ========

Adler32Info ??= defaultOptions.Adler32Info;
TagConfig ??= defaultOptions.TagConfig;
IntegrityConfig ??= defaultOptions.IntegrityConfig;
MetadataImportConfig ??= defaultOptions.MetadataImportConfig;
MetadataExportConfig ??= defaultOptions.MetadataExportConfig;

#endregion

TagConfig.CheckForNullFields();
MetadataImportConfig.CheckForNullFields();
MetadataExportConfig.CheckForNullFields();

BufferSizeForIOTasks = BufferSizeRange.CheckParamRange(BufferSizeForIOTasks);
}

}

}
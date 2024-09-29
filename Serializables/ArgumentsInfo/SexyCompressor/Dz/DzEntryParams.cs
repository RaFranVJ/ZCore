using ZCore.Serializables.ArgumentsInfo.Compressor;
using ZCore.Serializables.ArgumentsInfo.Compressor.BZip2;
using ZCore.Serializables.ArgumentsInfo.Compressor.Lzma;

namespace ZCore.Serializables.ArgumentsInfo.SexyCompressor.Dz
{
/// <summary> Groups options related to the DZ Entries and their Compression Methods. </summary>

public class DzEntryParams : DataBlockInfo
{
/** <summary> Gets or Sets the Compress Info related to the GZip Algorithm. </summary>
<returns> The GZip Compress Info. </returns> */

public GenericCompressorInfo ConfigForZLibEntries{ get; set; }

/** <summary> Gets or Sets the Compress Info related to the BZip2 Algorithm. </summary>
<returns> The BZip2 Compress Info. </returns> */

public BZip2Settings ConfigForBZip2Entries{ get; set; }

/** <summary> Gets or Sets the Compress Info related to the LZMA Algorithm. </summary>
<returns> The LZMA Compress Info. </returns> */

public LzmaSettings ConfigForLzmaEntries{ get; set; }

/** <summary> Gets or Sets a Value that Determines the amount of Zeroes to Write for Empty Entries. </summary>
<returns> The Padding Value. </returns> */

public uint PaddingForEmptyEntries{ get; set; }

/// <summary> Creates a new Instance of the <c>ArcvInfo</c>. </summary>

public DzEntryParams()
{
ConfigForZLibEntries = new();
ConfigForBZip2Entries = new();

ConfigForLzmaEntries = new();
PaddingForEmptyEntries = 81920;
}

/// <summary> Checks each nullable Field of the instance and Validates it, in case it's <c>null</c>. </summary>

public override void CheckForNullFields()
{
DzEntryParams defaultOptions = new();

#region ======== Set default Values to Null Fields ========

ConfigForZLibEntries ??= defaultOptions.ConfigForZLibEntries;
ConfigForBZip2Entries ??= defaultOptions.ConfigForBZip2Entries;
ConfigForLzmaEntries ??= defaultOptions.ConfigForLzmaEntries;

#endregion

ConfigForZLibEntries.CheckForNullFields();
ConfigForBZip2Entries.CheckForNullFields();
ConfigForLzmaEntries.CheckForNullFields();

BufferSizeForIOTasks = BufferSizeRange.CheckParamRange(BufferSizeForIOTasks);
}

}

}
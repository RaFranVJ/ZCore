using System.Collections.Generic;
using ZCore.Serializables.ArgumentsInfo.FileManager.Archive;
using ZCore.Serializables.ArgumentsInfo.FileManager.Metadata;

namespace ZCore.Serializables.ArgumentsInfo.SexyCompressor.Dz
{
/// <summary> Groups options related to the DZ Compressor. </summary>

public class DzSettings : ParamGroupInfo
{
/** <summary> Gets or Sets the Endian Order used when Parsing Files. </summary>
<returns> The Endian Order. </returns> */

public Endian Endianness{ get; set; }

/** <summary> Gets or Sets the Encoding used when Parsing Strings. </summary>
<returns> The Text Encoding. </returns> */

public string TextEncoding{ get; set; }

/** <summary> Gets or Sets a Value that Determines the Maximum number of Chunks expected in the Stream. </summary>
<returns> The Max ChunkIndex. </returns> */

public ushort MaxChunkIndex{ get; set; }

/** <summary> Gets or Sets a Dictionary that Maps each Extention with its CompressionFlags. </summary>
<returns> The Extension Methods. </returns> */

public Dictionary<string, CompressionFlags> ExtensionMethods{ get; set; }

// a

public DzEntryParams EntryParams{ get; set; }

/** <summary> Gets or Sets some Info related to how the Info inside ARCV Files should be Loaded. </summary>
<returns> The Import Config. </returns> */

public MetadataImportParams MetadataImportConfig{ get; set; }

/** <summary> Gets or Sets some Info related to how the Info inside ARCV Files should be Saved. </summary>
<returns> The Export Config. </returns> */

public MetadataExportParams MetadataExportConfig{ get; set; }

/** <summary> Gets or Sets some Options related of the DZ Compressor (Default Version is 0). </summary>
<returns> The Parser Version. </returns> */

public FileVersionDetails<byte> CompressorVersion{ get; set; }

/// <summary> Creates a new Instance of the <c>ArcvInfo</c>. </summary>

public DzSettings()
{
TextEncoding = EncodeHelper.ANSI.ToString();
MaxChunkIndex = 0xFFFF;

ExtensionMethods = new()
{

{ ".compiled", CompressionFlags.ReadOnly },
{ ".jpg", CompressionFlags.ReadOnly },
{ ".png", CompressionFlags.ReadOnly },
{ ".txt", CompressionFlags.ZLib },

};

EntryParams = new();
MetadataImportConfig = new();

MetadataExportConfig = new();
CompressorVersion = new();
}

/// <summary> Checks each nullable Field of the instance and Validates it, in case it's <c>null</c>. </summary>

public override void CheckForNullFields()
{
DzSettings defaultOptions = new();

#region ======== Set default Values to Null Fields ========

TextEncoding ??= defaultOptions.TextEncoding;
MetadataImportConfig ??= defaultOptions.MetadataImportConfig;
MetadataExportConfig ??= defaultOptions.MetadataExportConfig;

#endregion

MetadataImportConfig.CheckForNullFields();
MetadataExportConfig.CheckForNullFields();
}

}

}
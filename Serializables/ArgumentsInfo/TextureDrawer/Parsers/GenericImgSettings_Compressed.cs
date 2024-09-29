using ZCore.Serializables.ArgumentsInfo.Compressor;
using ZCore.Serializables.ArgumentsInfo.FileManager.Metadata;

namespace ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers
{
/// <summary> Groups options related to the SexyTex Parser. </summary>

public class GenericImgSettings_Compressed<T> : GenericCompressorInfo where T : struct, System.Enum
    {
/** <summary> Gets or Sets the Endian Order used when Parsing Files. </summary>
<returns> The Endian Order. </returns> */

public Endian Endianness{ get; set; }

/** <summary> Gets or Sets the Format used when Encoding TEX Images. </summary>
<returns> The TXZ Format. </returns> */

public T ImageFormatForEncoding{ get; set; }

/** <summary> Gets or Sets some Info related to how the Info inside SMF Files should be Loaded. </summary>
<returns> The Import Config. </returns> */

public MetadataImportParams MetadataImportConfig{ get; set; }

/** <summary> Gets or Sets some Info related to how the Info inside SMF Files should be Saved. </summary>
<returns> The Export Config. </returns> */

public MetadataExportParams MetadataExportConfig{ get; set; }

/// <summary> Creates a new Instance of the <c>TxzSettings</c>. </summary>

public GenericImgSettings_Compressed()
{
MetadataImportConfig = new();
MetadataExportConfig = new();
}

}

}
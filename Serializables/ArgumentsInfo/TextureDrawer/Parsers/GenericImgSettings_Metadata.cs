using ZCore.Serializables.ArgumentsInfo.FileManager.Metadata;

namespace ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers
{
/// <summary> Groups some Base Settings used on the Image Parsers. </summary>

public class GenericImgSettings_Metadata<T> : ParamGroupInfo where T : struct, System.Enum
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

/// <summary> Creates a new Instance of the <c>GenericImgSettings</c>. </summary>

public GenericImgSettings_Metadata()
{
MetadataImportConfig = new();
MetadataExportConfig = new();
}

/// <summary> Creates a new Instance of the <c>GenericImgSettings</c>. </summary>

public GenericImgSettings_Metadata(T format)
{
ImageFormatForEncoding = format;

MetadataImportConfig = new();
MetadataExportConfig = new();
}

}

}
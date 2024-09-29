using ZCore.Serializables.ArgumentsInfo.FileManager.Archive;

namespace ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.SexyTex
{
/// <summary> Groups options related to the SexyTex Parser. </summary>

public class SexyTexSettings : GenericImgSettings_Compressed<SexyTexFormat>
{
/** <summary> Gets or Sets some Options related to DDS (Default Version is 0). </summary>
<returns> The DDS Version. </returns> */

public FileVersionDetails<uint> ParserVersion{ get; set; }

/// <summary> Creates a new Instance of the <c>TxzSettings</c>. </summary>

public SexyTexSettings()
{
ImageFormatForEncoding = SexyTexFormat.ARGB8888;

ParserVersion = new();
}

/// <summary> Checks each nullable Field of the <c>XnbSettings</c> Instance and Validates it, in case it's <c>null</c>. </summary>

public override void CheckForNullFields()
{
SexyTexSettings defaultOptions = new();

#region ======== Set default Values to Null Fields ========

ParserVersion ??= defaultOptions.ParserVersion;
MetadataImportConfig ??= defaultOptions.MetadataImportConfig;
MetadataExportConfig ??= defaultOptions.MetadataExportConfig;

#endregion

MetadataImportConfig.CheckForNullFields();
MetadataExportConfig.CheckForNullFields();

ImageFormatForEncoding = ImageFormatForEncoding == SexyTexFormat.None ? SexyTexFormat.ARGB8888 : ImageFormatForEncoding;
BufferSizeForIOTasks = BufferSizeRange.CheckParamRange(BufferSizeForIOTasks);
}

}

}
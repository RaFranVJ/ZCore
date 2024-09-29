namespace ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.UTex
{
/// <summary> Groups options related to the UTex Parser. </summary>

public class UTexSettings : GenericImgSettings_Metadata<UTexFormat>
{
/// <summary> Creates a new Instance of the <c>UTexFormat</c>. </summary>

public UTexSettings()
{
ImageFormatForEncoding = UTexFormat.ABGR8888;
}

///<summary> Checks each nullable Field of the <c>SexyTexSettings</c> Instance and Validates it, in case it's <c>null</c>. </summary>

public override void CheckForNullFields()
{
UTexSettings defaultOptions = new();

#region ======== Set default Values to Null Fields ========

MetadataImportConfig ??= defaultOptions.MetadataImportConfig;
MetadataExportConfig ??= defaultOptions.MetadataExportConfig;

#endregion

MetadataImportConfig.CheckForNullFields();
MetadataExportConfig.CheckForNullFields();

ImageFormatForEncoding = ImageFormatForEncoding == UTexFormat.None ? UTexFormat.ABGR8888 : ImageFormatForEncoding;
}

}

}
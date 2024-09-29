namespace ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.Txz
{
/// <summary> Groups options related to the TXZ Parser. </summary>

public class TxzSettings : GenericImgSettings_Compressed<TxzFormat>
{
/// <summary> Creates a new Instance of the <c>TxzSettings</c>. </summary>

public TxzSettings()
{
ImageFormatForEncoding = TxzFormat.ABGR8888;
}

///<summary> Checks each nullable Field of the <c>TxzSettings</c> Instance and Validates it, in case it's <c>null</c>. </summary>

public override void CheckForNullFields()
{
TxzSettings defaultOptions = new();

#region ======== Set default Values to Null Fields ========

MetadataImportConfig ??= defaultOptions.MetadataImportConfig;
MetadataExportConfig ??= defaultOptions.MetadataExportConfig;

#endregion

MetadataImportConfig.CheckForNullFields();
MetadataExportConfig.CheckForNullFields();

ImageFormatForEncoding = ImageFormatForEncoding == TxzFormat.None ? TxzFormat.ABGR8888 : ImageFormatForEncoding;
BufferSizeForIOTasks = BufferSizeRange.CheckParamRange(BufferSizeForIOTasks);
}

}

}
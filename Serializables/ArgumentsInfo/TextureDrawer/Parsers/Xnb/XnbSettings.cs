using iVersion = ZCore.Modules.TextureDrawer.Parsers.XnaGameStudio.VersionCheck.XnbVersion;

using ZCore.Serializables.ArgumentsInfo.FileManager.Archive;
using ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.Xnb.Integrity;

namespace ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.Xnb
{
/// <summary> Groups options related to the XNB Parser. </summary>

public class XnbSettings : GenericImgSettings_Metadata<XnbFormat>
{
/** <summary> Gets or Sets the Target Platform used for XNB Files. </summary>
<returns> The Target Platform. </returns> */

public XnbPlatform TargetPlatform{ get; set; }

/** <summary> Gets or Sets some Info related to how the Adler32 Bytes should be Calculated. </summary>
<returns> The XNB Integrity Info. </returns> */

public XnbIntegrityInfo IntegrityConfig{ get; set; }

/** <summary> Gets or Sets some Options related to XNB Files (Default Version is 5). </summary>
<returns> The XNB Version. </returns> */

public FileVersionDetails<ushort> XnbVersion{ get; set; }

/** <summary> Gets or Sets some Options related to DDS (Default Version is 0). </summary>
<returns> The DDS Version. </returns> */

public FileVersionDetails<uint> DdsVersion{ get; set; }

/// <summary> Creates a new Instance of the <c>XnbSettings</c>. </summary>

public XnbSettings()
{
TargetPlatform = XnbPlatform.WindowsPhone;
ImageFormatForEncoding = XnbFormat.ABGR8888;

IntegrityConfig = new();
XnbVersion = new(iVersion.ExpectedVersions.MaxValue);

DdsVersion = new();
}

/// <summary> Checks each nullable Field of the <c>XnbSettings</c> Instance and Validates it, in case it's <c>null</c>. </summary>

public override void CheckForNullFields()
{
XnbSettings defaultOptions = new();

#region ======== Set default Values to Null Fields ========

IntegrityConfig ??= defaultOptions.IntegrityConfig;
XnbVersion ??= defaultOptions.XnbVersion;
DdsVersion ??= defaultOptions.DdsVersion;
MetadataImportConfig ??= defaultOptions.MetadataImportConfig;
MetadataExportConfig ??= defaultOptions.MetadataExportConfig;

#endregion

MetadataImportConfig.CheckForNullFields();
MetadataExportConfig.CheckForNullFields();

TargetPlatform = TargetPlatform == XnbPlatform.Default ? XnbPlatform.WindowsPhone : TargetPlatform;
}

}

}
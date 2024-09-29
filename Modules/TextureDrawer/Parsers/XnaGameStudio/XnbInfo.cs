using ZCore.Modules.TextureDrawer.Parsers.XnaGameStudio.VersionCheck;
using ZCore.Serializables.ArgumentsInfo.FileManager.Archive;
using ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.Xnb;

namespace ZCore.Modules.TextureDrawer.Parsers.XnaGameStudio
{
/// <summary> Represents Info for a XNB Image. </summary>

public class XnbInfo : MetaModel<XnbInfo>
{
/** <summary> The Info of a XNB File. </summary>

<remarks> It Occupies 157 Bytes in the Stream which Specify the Version of the Xna Assembly </remarks> */

private static readonly byte[] m_AssemblyInfo = new byte[0x9D] { 0x01, 0x94, 0x01, 0x4D, 0x69, 0x63, 0x72, 0x6F, 0x73, 0x6F, 0x66, 0x74, 0x2E, 0x58, 0x6E, 0x61, 0x2E, 0x46, 0x72, 0x61, 0x6D, 0x65, 0x77, 0x6F, 0x72, 0x6B, 0x2E, 0x43, 0x6F, 0x6E, 0x74, 0x65, 0x6E, 0x74, 0x2E, 0x54, 0x65, 0x78, 0x74, 0x75, 0x72, 0x65, 0x32, 0x44, 0x52, 0x65, 0x61, 0x64, 0x65, 0x72, 0x2C, 0x20, 0x4D, 0x69, 0x63, 0x72, 0x6F, 0x73, 0x6F, 0x66, 0x74, 0x2E, 0x58, 0x6E, 0x61, 0x2E, 0x46, 0x72, 0x61, 0x6D, 0x65, 0x77, 0x6F, 0x72, 0x6B, 0x2E, 0x47, 0x72, 0x61, 0x70, 0x68, 0x69, 0x63, 0x73, 0x2C, 0x20, 0x56, 0x65, 0x72, 0x73, 0x69, 0x6F, 0x6E, 0x3D, 0x34, 0x2E, 0x30, 0x2E, 0x30, 0x2E, 0x30, 0x2C, 0x20, 0x43, 0x75, 0x6C, 0x74, 0x75, 0x72, 0x65, 0x3D, 0x6E, 0x65, 0x75, 0x74, 0x72, 0x61, 0x6C, 0x2C, 0x20, 0x50, 0x75, 0x62, 0x6C, 0x69, 0x63, 0x4B, 0x65, 0x79, 0x54, 0x6F, 0x6B, 0x65, 0x6E, 0x3D, 0x38, 0x34, 0x32, 0x63, 0x66, 0x38, 0x62, 0x65, 0x31, 0x64, 0x65, 0x35, 0x30, 0x35, 0x35, 0x33, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };

/** <summary> Gets or Sets the PlatformID of the XNB. </summary>
<returns> The PlatformID. </returns> */

public XnbPlatform PlatformID{ get; set; }

/** <summary> Gets or Sets the XNB Version. </summary>
<returns> The File Version. </returns> */

public ushort FileVersion{ get; set; }

/** <summary> Gets or Sets the XNB File Size. </summary>
<returns> The XnbSize. </returns> */

public int XnbSize{ get; set; }

/** <summary> Gets or Sets the Version of the DDS (DirectDraw Surface). </summary>
<returns> The DDS Version. </returns> */

public uint SurfaceVersion{ get; set; }

/** <summary> Gets or Sets the Texture Width. </summary>
<returns> The TextureWidth. </returns> */

public int TextureWidth{ get; set; }

/** <summary> Gets or Sets the Texture Height. </summary>
<returns> The TextureHeight. </returns> */

public int TextureHeight{ get; set; }

/** <summary> Gets or Sets the Texture Format. </summary>
<returns> The TextureFormat. </returns> */
		
public XnbFormat TextureFormat{ get; set; }

/** <summary> Gets or Sets the Texture Size. </summary>
<returns> The TextureSize. </returns> */

public int TextureSize{ get; set; }

/// <summary> Creates a new Instance of the <c>XnbInfo</c>. </summary>

public XnbInfo()
{
}

/// <summary> Creates a new Instance of the <c>XnbInfo</c>. </summary>

public XnbInfo(int width, int height)
{
PlatformID = XnbPlatform.WindowsPhone;

TextureWidth = width;
TextureHeight = height;

TextureFormat = XnbFormat.ABGR8888;
TextureSize = TextureHelper.CalculateTextureSize(TextureWidth, TextureHeight, 2);
}

/// <summary> Creates a new Instance of the <c>XnbInfo</c>. </summary>

public XnbInfo(int width, int height, XnbPlatform platform, XnbFormat format)
{
PlatformID = platform;

TextureWidth = width;
TextureHeight = height;

TextureFormat = format;
TextureSize = TextureHelper.CalculateTextureSize(TextureWidth, TextureHeight, 2);
}

// Read Info from BinaryStream

public static XnbInfo ReadBin(BinaryStream bs, Endian endian = default, bool adaptVer_Xnb = true, bool adaptVer_Dds = true)
{

XnbInfo fileInfo = new()
{
PlatformID = (XnbPlatform)bs.ReadByte(),
FileVersion = XnbVersion.Read(bs, endian, adaptVer_Xnb),
XnbSize = bs.ReadInt(endian)
};

bs.CompareBytes<GenericValueMismatchException<byte[]>>(m_AssemblyInfo);

fileInfo.SurfaceVersion = DdsVersion.Read(bs, endian, adaptVer_Dds);
fileInfo.TextureWidth = bs.ReadInt(endian);
fileInfo.TextureHeight = bs.ReadInt(endian);
fileInfo.TextureFormat = (XnbFormat)bs.ReadUInt(endian);
fileInfo.TextureSize = bs.ReadInt(endian);
			
return fileInfo;
}

// Write Info to BinaryStream

public void WriteBin(BinaryStream bs, FileVersionDetails<ushort> xnbVer, FileVersionDetails<uint> ddsVer, 
Endian endian = default)
{
XnbSize = (XnbSize <= 0) ? (int)bs.Length : XnbSize;
TextureSize = (TextureSize <= 0) ? TextureHelper.CalculateTextureSize(TextureWidth, TextureHeight, 2) : TextureSize;

FileVersion = xnbVer.VersionNumber;
SurfaceVersion = ddsVer.VersionNumber;

bs.WriteByte( (byte)PlatformID);

XnbVersion.Write(bs, endian, xnbVer.VersionNumber, xnbVer.AdaptCompatibilityBetweenVersions);  
  
bs.WriteInt(XnbSize, endian);

bs.Write(m_AssemblyInfo);
DdsVersion.Write(bs, endian, ddsVer.VersionNumber, ddsVer.AdaptCompatibilityBetweenVersions);

bs.WriteInt(TextureWidth, endian);
bs.WriteInt(TextureHeight, endian);

bs.WriteUInt( (uint)TextureFormat, endian);
bs.WriteInt(TextureSize, endian);
}

}

}
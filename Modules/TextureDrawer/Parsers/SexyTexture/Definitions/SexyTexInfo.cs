using ZCore.Modules.TextureDrawer.Parsers.SexyTexure;
using ZCore.Serializables.ArgumentsInfo.FileManager.Archive;
using ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.SexyTex;

namespace ZCore.Modules.TextureDrawer.Parsers.SexyTexture.Definitions
{
// <summary> Represents Info for a SexyTexture. </summary>

public class SexyTexInfo : MetaModel<SexyTexInfo>
{
/** <summary> Gets or Sets the SexyTex Version. </summary>
<returns> The File Version. </returns> */

public uint FileVersion{ get; set; }

/** <summary> Gets or Sets the Texture Width. </summary>
<returns> The TextureWidth. </returns> */

public int TextureWidth{ get; set; }

/** <summary> Gets or Sets the Texture Height. </summary>
<returns> The TextureHeight. </returns> */

public int TextureHeight{ get; set; }

/** <summary> Gets or Sets the Texture Format. </summary>
<returns> The TextureFormat. </returns> */
	
public SexyTexFormat TextureFormat{ get; set; }

/** <summary> Gets or Sets a Boolean that Determines if Texture should Use Compression or not. </summary>
<returns> <b>true</b> if Data should be Compressed; <b>false</b> otherwise. </returns> */
	
public bool CompressData{ get; set; }

/** <summary> Gets or Sets the Type of Compression used in the SexyTexture. </summary>
<returns> The Compression Type. </returns> */

public CompressionFlags CompressionType{ get; set; }

/** <summary> Gets or Sets the Size of the SexyTexture after Compression. </summary>
<returns> The Size Compressed; 0 if the Field <c>CompressData</c> is <bfalse</b>. </returns> */

public int SizeCompressed{ get; set; }

/// <summary> Reserved Field for SexyTex, don't know what Stands for. </summary>

public uint ReservedField{ get; set; }

/// <summary> Reserved Field for SexyTex, don't know what Stands for. </summary>

public uint ReservedField2{ get; set; }

/// <summary> Reserved Field for SexyTex, don't know what Stands for. </summary>

public uint ReservedField3{ get; set; }

/// <summary> Creates a new Instance of the <c>SexyTexInfo</c>. </summary>

public SexyTexInfo()
{
}

/// <summary> Creates a new Instance of the <c>SexyTexInfo</c>. </summary>

public SexyTexInfo(int width, int height)
{
TextureWidth = width;
TextureHeight = height;

TextureFormat = SexyTexFormat.ARGB8888;
}

/// <summary> Creates a new Instance of the <c>SexyTexInfo</c>. </summary>

public SexyTexInfo(int width, int height, SexyTexFormat format)
{
TextureWidth = width;
TextureHeight = height;

TextureFormat = format;
}

// Read Info from BinaryStream

public static SexyTexInfo ReadBin(BinaryStream bs, bool adaptVer, Endian endian = default)
{

return new()
{
FileVersion = SexyTexVersion.Read(bs, endian, adaptVer),

TextureWidth = bs.ReadInt(endian),
TextureHeight = bs.ReadInt(endian),

TextureFormat = (SexyTexFormat)bs.ReadInt(endian),
CompressData = bs.ReadUInt(endian) == 1,

CompressionType = (CompressionFlags)bs.ReadUInt(endian),
SizeCompressed = bs.ReadInt(endian),

ReservedField = bs.ReadUInt(endian),
ReservedField2 = bs.ReadUInt(endian),

ReservedField3 = bs.ReadUInt(endian),
};

}

// Validate Texture Format

public void ValidateTextureFormat()
{
int texFlags = (int)TextureFormat;

if(texFlags >= 10)
{
TextureFormat = (SexyTexFormat)(texFlags - 2);

CompressData = false;
}

else
CompressData = true;

}

// Write Info to BinaryStream

public void WriteBin(BinaryStream bs, FileVersionDetails<uint> verInfo, Endian endian = default)
{
ValidateTextureFormat();

SexyTexVersion.Write(bs, endian, verInfo.VersionNumber, verInfo.AdaptCompatibilityBetweenVersions);

bs.WriteInt(TextureWidth, endian);
bs.WriteInt(TextureHeight, endian);

bs.WriteInt( (int)TextureFormat, endian);
bs.WriteUInt( (uint)(CompressData ? 1 : 0), endian);

bs.WriteInt( (int)CompressionType, endian);
bs.WriteInt(SizeCompressed, endian);

bs.WriteUInt(ReservedField, endian);
bs.WriteUInt(ReservedField2, endian);
	
bs.WriteUInt(ReservedField3, endian);
}
 
}

}
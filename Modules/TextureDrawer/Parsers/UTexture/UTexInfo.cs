using ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.UTex;

namespace ZCore.Modules.TextureDrawer.Parsers.UTexture
{
// <summary> Represents Info for a UTexture. </summary>

public class UTexInfo : MetaModel<UTexInfo>
{
/** <summary> Gets or Sets the Texture Width. </summary>
<returns> The TextureWidth. </returns> */

public ushort TextureWidth{ get; set; }

/** <summary> Gets or Sets the Texture Height. </summary>
<returns> The TextureHeight. </returns> */

public ushort TextureHeight{ get; set; }

/** <summary> Gets or Sets the Texture Format. </summary>
<returns> The TextureFormat. </returns> */
		
public UTexFormat TextureFormat{ get; set; }

/// <summary> Creates a new Instance of the <c>UTexFormat/c>. </summary>

public UTexInfo()
{
}

/// <summary> Creates a new Instance of the <c>UTexFormat/c>. </summary>

public UTexInfo(int width, int height)
{
TextureWidth = (ushort)width;
TextureHeight = (ushort)height;

TextureFormat = UTexFormat.ABGR8888;
}

/// <summary> Creates a new Instance of the <c>UTexInfo</c>. </summary>

public UTexInfo(int width, int height, UTexFormat format)
{
TextureWidth = (ushort)width;
TextureHeight = (ushort)height;

TextureFormat = format;
}

// Read Info from BinaryStream

public static UTexInfo ReadBin(BinaryStream bs, Endian endian = default)
{

return new()
{
TextureWidth = bs.ReadUShort(endian),
TextureHeight = bs.ReadUShort(endian),
TextureFormat = (UTexFormat)bs.ReadUShort(endian)
};

}

// Write Info to BinaryStream

public void WriteBin(BinaryStream bs, Endian endian = default)
{
bs.WriteUShort(TextureWidth, endian);
bs.WriteUShort(TextureHeight, endian);

bs.WriteUShort( (ushort)TextureFormat, endian);
}

}

}
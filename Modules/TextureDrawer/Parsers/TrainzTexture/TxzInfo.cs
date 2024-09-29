using ZCore.Serializables.ArgumentsInfo.FileManager.Archive;
using ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.Txz;

namespace ZCore.Modules.TextureDrawer.Parsers.TrainzTexture
{
/// <summary> Represents Info for a TXZ Image. </summary>

public class TxzInfo : MetaModel<TxzInfo>
{
/** <summary> Gets or Sets the Texture Width. </summary>
<returns> The TextureWidth. </returns> */

public ushort TextureWidth{ get; set; }

/** <summary> Gets or Sets the Texture Height. </summary>
<returns> The TextureHeight. </returns> */

public ushort TextureHeight{ get; set; }

/** <summary> Gets or Sets the Texture Format. </summary>
<returns> The TextureFormat. </returns> */
		
public TxzFormat TextureFormat{ get; set; }

/// <summary> Creates a new Instance of the <c>TxzInfo</c>. </summary>

public TxzInfo()
{
}

/// <summary> Creates a new Instance of the <c>TxzInfo</c>. </summary>

public TxzInfo(int width, int height)
{
TextureWidth = (ushort)width;
TextureHeight = (ushort)height;

TextureFormat = TxzFormat.ABGR8888;
}

/// <summary> Creates a new Instance of the <c>TxzInfo</c>. </summary>

public TxzInfo(int width, int height, TxzFormat format)
{
TextureWidth = (ushort)width;
TextureHeight = (ushort)height;

TextureFormat = format;
}

// Read Info from BinaryStream

public static TxzInfo ReadBin(BinaryStream bs, Endian endian = default)
{

return new()
{
TextureWidth = bs.ReadUShort(endian),
TextureHeight = bs.ReadUShort(endian),
TextureFormat = (TxzFormat)bs.ReadUShort(endian)
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
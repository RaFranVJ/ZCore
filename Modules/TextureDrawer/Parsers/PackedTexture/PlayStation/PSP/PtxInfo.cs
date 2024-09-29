

using ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.Ptx.Format.PlayStation;

namespace ZCore.Modules.TextureDrawer.Parsers.PackedTexture.PlayStation.PSP
{
/// <summary> Represents Info for a Packed Texture (PTX). </summary>

public class PtxInfo : MetaModel<PtxInfo>
{
/** <summary> Gets or Sets the Size of the PTX when Parser. </summary>
<returns> The FileSize. </returns> */

public int FileSize{ get; set; }

/** <summary> Gets or Sets the Texture Width. </summary>
<returns> The TextureWidth. </returns> */
		
public ushort TextureWidth{ get; set; }

/** <summary> Gets or Sets the Texture Height. </summary>
<returns> The TextureHeight. </returns> */

public ushort TextureHeight{ get; set; }

/** <summary> Gets or Sets the Texture Format. </summary>
<returns> The TextureFormat. </returns> */

public PtxFormat_PSP TextureFormat{ get; set; }

/// <summary> Creates a new Instance of the <c>PtxInfo</c>. </summary>

public PtxInfo()
{
}

/// <summary> Creates a new Instance of the <c>PtxInfo</c>. </summary>

public PtxInfo(int height, int width)
{
TextureHeight = (ushort)height;
TextureWidth = (ushort)width;
}

/// <summary> Creates a new Instance of the <c>PtxInfo</c>. </summary>

public PtxInfo(int height, int width, PtxFormat_PSP format)
{
TextureHeight = (ushort)height;
TextureWidth = (ushort)width;

TextureFormat = format;
}

// Get Info from BinaryStream

public static PtxInfo ReadBin(BinaryStream bs, Endian endian = default)
{

return new()
{
FileSize = bs.ReadInt(endian),
TextureWidth = bs.ReadUShort(endian),
TextureHeight = bs.ReadUShort(endian),
TextureFormat = (PtxFormat_PSP)bs.ReadUInt(endian)
};

}

// Write Info to BinaryStream

public void WriteBin(BinaryStream bs, Endian endian = default)
{
FileSize = FileSize <= 0 ? (int)(bs.Length - 16) : FileSize;

bs.WriteInt(FileSize, endian);
bs.WriteUShort(TextureWidth, endian);
bs.WriteUShort(TextureHeight, endian);
bs.WriteUInt( (uint)TextureFormat, endian);
}

}

}
using ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.Ptx.Format.PlayStation;

namespace ZCore.Modules.TextureDrawer.Parsers.PackedTexture.PlayStation.PS4
{
/// <summary> Represents Info for a Packed Texture (PTX). </summary>

public class PtxInfo : MetaModel<PtxInfo>
{
/** <summary> Gets or Sets the Texture Height. </summary>
<returns> The TextureHeight. </returns> */

public int TextureHeight{ get; set; }

/** <summary> Gets or Sets the Texture Width. </summary>
<returns> The TextureWidth. </returns> */

public int TextureWidth{ get; set; }

/** <summary> Gets or Sets the Texture Size. </summary>
<returns> The TextureSize. </returns> */

public int TextureSize{ get; set; }

/** <summary> Gets or Sets the Texture Format. </summary>
<returns> The TextureFormat. </returns> */
	
public PtxFormat_PS4 TextureFormat{ get; set; }

/// <summary> Creates a new Instance of the <c>PtxInfo</c>. </summary>

public PtxInfo()
{
}

/// <summary> Creates a new Instance of the <c>PtxInfo</c>. </summary>

public PtxInfo(int height, int width)
{
TextureHeight = height;
TextureWidth = width;

TextureSize = TextureHelper.CalculateTextureSize(TextureWidth, TextureHeight);
}

/// <summary> Creates a new Instance of the <c>PtxInfo</c>. </summary>

public PtxInfo(int height, int width, PtxFormat_PS4 format)
{
TextureHeight = height;
TextureWidth = width;

TextureSize = TextureHelper.CalculateTextureSize(TextureWidth, TextureHeight);
TextureFormat = format;
}

// Get Info from BinaryStream

public static PtxInfo ReadBin(BinaryStream bs, Endian endian = default)
{

return new()
{
TextureHeight = bs.ReadInt(endian),
TextureWidth = bs.ReadInt(endian),
TextureSize = bs.ReadInt(endian),
TextureFormat = (PtxFormat_PS4)bs.ReadUInt(endian)
};

}

// Write Info to BinaryStream

public void WriteBin(BinaryStream bs, Endian endian = default)
{
TextureSize = (TextureSize <= 0) ? TextureHelper.CalculateTextureSize(TextureWidth, TextureHeight) : TextureSize;

bs.WriteInt(TextureHeight, endian);
bs.WriteInt(TextureWidth, endian);
bs.WriteInt(TextureSize, endian);
bs.WriteUInt( (uint)TextureFormat, endian);
}

}

}
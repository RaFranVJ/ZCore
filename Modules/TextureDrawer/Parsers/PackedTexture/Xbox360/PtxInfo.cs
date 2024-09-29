namespace ZCore.Modules.TextureDrawer.Parsers.PackedTexture.Xbox360
{
/// <summary> Represents Info for a Packed Texture (PTX). </summary>

public class PtxInfo : MetaModel<PtxInfo>
{
/** <summary> Gets or Sets the Texture Width. </summary>
<returns> The TextureWidth. </returns> */

public int TextureWidth{ get; set; }

/** <summary> Gets or Sets the Texture Height. </summary>
<returns> The TextureHeight. </returns> */

public int TextureHeight{ get; set; }

/** <summary> Gets or Sets the Block Size. </summary>
<returns> The BlockSize. </returns> */

public int BlockSize{ get; set; }

/// <summary> Creates a new Instance of the <c>PtxInfo</c>. </summary>

public PtxInfo()
{
}

/// <summary> Creates a new Instance of the <c>PtxInfo</c>. </summary>

public PtxInfo(int height, int width)
{
TextureHeight = height;
TextureWidth = width;
}

/// <summary> Creates a new Instance of the <c>PtxInfo</c>. </summary>

public PtxInfo(int height, int width, int blockSize)
{
TextureHeight = height;
TextureWidth = width;

BlockSize = blockSize;
}

// Get Info from BinaryStream

public static PtxInfo ReadBin(BinaryStream bs, Endian endian = default)
{
	
return new()
{
TextureWidth = bs.ReadInt(endian),
TextureHeight = bs.ReadInt(endian),
BlockSize = bs.ReadInt(endian)
};

}

// Write Info to BinaryStream

public void WriteBin(BinaryStream bs, Endian endian = default)
{
bs.WriteInt(TextureWidth, endian);
bs.WriteInt(TextureHeight, endian);

bs.WriteInt(BlockSize, endian);
}

}

}
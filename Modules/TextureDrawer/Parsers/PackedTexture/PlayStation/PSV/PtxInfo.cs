

using ZCore.Serializables.ArgumentsInfo.FileManager.Archive;

namespace ZCore.Modules.TextureDrawer.Parsers.PackedTexture.PlayStation.PSV
{
/// <summary> Represents Info for a Packed Texture (PTX). </summary>

public class PtxInfo : MetaModel<PtxInfo>
{
/** <summary> The Size of the <c>Padding</c> Field. </summary>

<remarks> Padding is Actually a Group of 3 Fields (int) ignored due to being Unknown  </remarks> */

private const int PaddingSizeX = 12;

/** <summary> The Size of the <c>Padding2</c> Field. </summary>

<remarks> Padding is Actually a Group of 2 Fields (int) ignored due to being Unknown  </remarks> */

private const int PaddingSizeY = 8;

/** <summary> Gets or Sets the File Version. </summary>
<returns> The FileVersion. </returns> */

public uint FileVersion{ get; set; }

/// <summary> Reserved Field for PTX, don't know what Stands for. </summary>

public uint ReservedField{ get; set; } = 1;

/** <summary> Gets or Sets the Block Size. </summary>

<remarks> This Field is Written twice in the Stream </remarks>

<returns> The BlockSize. </returns> */

public int BlockSize{ get; set; } = 0x40;

/** <summary> Gets or Sets the Size of the PTX when Parser. </summary>

<remarks> This Field is Written twice in the Stream </remarks>

<returns> The FileSize. </returns> */

public int FileSize{ get; set; }

/** <summary> Gets or Sets some Bytes used for Padding the File. </summary>
<returns> The Padding Bytes. </returns> */

public byte[] Padding{ get; set; } = new byte[PaddingSizeX];

/// <summary> Reserved Field for PTX, don't know what Stands for. </summary>

public int ReservedField2{ get; set; } = -1;

/** <summary> Gets or Sets some Bytes used for Padding the File. </summary>
<returns> The Padding Bytes. </returns> */

public byte[] Padding2{ get; set; } = new byte[PaddingSizeY];

/// <summary> Reserved Field for PTX, don't know what Stands for. </summary>

public int ReservedField3{ get; set; } = -2030043136;

/** <summary> Gets or Sets the Texture Width. </summary>
<returns> The TextureWidth. </returns> */
		
public ushort TextureWidth{ get; set; }

/** <summary> Gets or Sets the Texture Height. </summary>
<returns> The TextureHeight. </returns> */

public ushort TextureHeight{ get; set; }

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

// Get Info from BinaryStream

public static PtxInfo ReadBin(BinaryStream bs, Endian endian = default, bool adaptVer = true)
{

PtxInfo fileInfo = new()
{
FileVersion = PtxVersion.Read(bs, endian, adaptVer),
ReservedField = bs.ReadUInt(endian),
BlockSize = bs.ReadInt(endian),
FileSize = bs.ReadInt(endian),
Padding = bs.ReadBytes(PaddingSizeX)
};

// Second Block Info

bs.CompareInt<GenericValueMismatchException<int>>(fileInfo.BlockSize, endian);
bs.CompareInt<GenericValueMismatchException<int>>(fileInfo.FileSize, endian);

fileInfo.ReservedField2 = bs.ReadInt(endian);
fileInfo.Padding2 = bs.ReadBytes(PaddingSizeY);

fileInfo.ReservedField3 = bs.ReadInt(endian);
fileInfo.TextureWidth = bs.ReadUShort(endian);
fileInfo.TextureHeight = bs.ReadUShort(endian);

bs.CompareUInt<GenericValueMismatchException<uint>>(fileInfo.ReservedField, endian);

return fileInfo;
}

// Write Info to BinaryStream

public void WriteBin(BinaryStream bs, FileVersionDetails<uint> verInfo, Endian endian = default)
{
FileSize = FileSize <= 0 ? (int)(bs.Length - BlockSize) : FileSize;

PtxVersion.Write(bs, endian, verInfo.VersionNumber, verInfo.AdaptCompatibilityBetweenVersions); 

bs.WriteUInt(ReservedField, endian);
bs.WriteInt(BlockSize, endian);
bs.WriteInt(FileSize, endian);
bs.Write(Padding);

// Second Block Info

bs.WriteInt(BlockSize, endian);
bs.WriteInt(FileSize, endian);
bs.WriteInt(ReservedField2, endian);
bs.Write(Padding2);
bs.WriteInt(ReservedField3, endian);
bs.WriteUShort(TextureWidth, endian);
bs.WriteUShort(TextureHeight, endian);
bs.WriteUInt(ReservedField, endian);
}

}

}
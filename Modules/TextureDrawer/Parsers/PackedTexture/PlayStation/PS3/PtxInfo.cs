using System;
using ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.Ptx;
using ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.Ptx.Format.PlayStation;

namespace ZCore.Modules.TextureDrawer.Parsers.PackedTexture.PlayStation.PS3
{
/// <summary> Represents Info for a Packed Texture (PTX). </summary>

public class PtxInfo : MetaModel<PtxInfo>
{
/** <summary> The Size of the <c>Padding</c> Field. </summary>

<remarks> Padding is Actually a Group of 11 Fields (int) ignored due to being Unknown  </remarks> */

private const int PaddingSizeX = 44;

/** <summary> The Size of the <c>Padding2</c> Field. </summary>

<remarks> Padding is Actually a Group of 5 Fields (int) ignored due to being Unknown  </remarks> */

private const int PaddingSizeY = 20;

/** <summary> The Size of the <c>Padding3</c> Field. </summary>

<remarks> Padding is Actually a Group of 4 Fields (int) ignored due to being Unknown  </remarks> */

private const int PaddingSizeZ = 16;

/// <summary> Reserved Field for PTX, don't know what Stands for. </summary>

public uint ReservedField{ get; set; } = 0x7C;

/// <summary> Reserved Field for PTX, don't know what Stands for. </summary>

public uint ReservedField2{ get; set; } = 528391;

/** <summary> Gets or Sets the Texture Height. </summary>
<returns> The TextureHeight. </returns> */

public int TextureHeight{ get; set; }

/** <summary> Gets or Sets the Texture Width. </summary>
<returns> The TextureWidth. </returns> */

public int TextureWidth{ get; set; }

/** <summary> Gets or Sets the Texture Size. </summary>
<returns> The TextureSize. </returns> */

public int TextureSize{ get; set; }

/** <summary> Gets or Sets some Bytes used for Padding the File. </summary>
<returns> The Padding Bytes. </returns> */

public byte[] Padding{ get; set; } = new byte[PaddingSizeX];

/** <summary> Gets or Sets the Name of the Tool used for Parsing the Texture. </summary>

<remarks> PTX use NVidia Texture Tools (NVTT) by Default, on PS3 </remarks>

<returns> The ToolUsed. </returns> */

public string ToolUsed{ get; set; } = "NVTT";

/// <summary> Reserved Field for PTX, don't know what Stands for. </summary>

public uint ReservedField3{ get; set; } = 131080;

/// <summary> Reserved Field for PTX, don't know what Stands for. </summary>

public uint ReservedField4{ get; set; } = 32;

/// <summary> Reserved Field for PTX, don't know what Stands for. </summary>

public uint ReservedField5{ get; set; } = 4;

/** <summary> Gets or Sets the Texture Format. </summary>
<returns> The TextureFormat. </returns> */
	
public PtxFormat_PS3 TextureFormat{ get; set; }

/** <summary> Gets or Sets some Bytes used for Padding the File. </summary>
<returns> The Padding Bytes. </returns> */

public byte[] Padding2{ get; set; } = new byte[PaddingSizeY];

/// <summary> Reserved Field for PTX, don't know what Stands for. </summary>

public uint ReservedField6{ get; set; } = 4096;

/** <summary> Gets or Sets some Bytes used for Padding the File. </summary>
<returns> The Padding Bytes. </returns> */

public byte[] Padding3{ get; set; } = new byte[PaddingSizeZ];
	
/// <summary> Creates a new Instance of the <c>PtxInfo</c>. </summary>

public PtxInfo()
{
}

/// <summary> Creates a new Instance of the <c>PtxInfo</c>. </summary>

public PtxInfo(int height, int width)
{
TextureHeight = height;
TextureWidth = width;

TextureSize = TextureHelper.CalculateTextureSize(TextureHeight, TextureWidth);
}

/// <summary> Creates a new Instance of the <c>PtxInfo</c>. </summary>

public PtxInfo(int height, int width, PtxFormat_PS3 format)
{
TextureHeight = height;
TextureWidth = width;

TextureHeight = height;
TextureFormat = format;
}



// Read Info from BinaryStream

public static PtxInfo ReadBin(BinaryStream bs, Endian endian = default)
{

PtxInfo fileInfo = new()
{
ReservedField = bs.ReadUInt(endian),
ReservedField2 = bs.ReadUInt(endian),
TextureHeight = bs.ReadInt(endian),
TextureWidth = bs.ReadInt(endian),
TextureSize = bs.ReadInt(endian),
Padding = bs.ReadBytes(PaddingSizeX),
ToolUsed = bs.ReadString(4, default, endian),
ReservedField3 = bs.ReadUInt(endian),
ReservedField4 = bs.ReadUInt(endian),
ReservedField5 = bs.ReadUInt(endian)
};

_ = Enum.TryParse(bs.ReadString(4, default, endian), out PtxFormat_PS3 flags);
fileInfo.TextureFormat = flags;

fileInfo.Padding2 = bs.ReadBytes(PaddingSizeY);
fileInfo.ReservedField6 = bs.ReadUInt(endian);
fileInfo.Padding3 = bs.ReadBytes(PaddingSizeZ);

return fileInfo;
}

// Write Info to BinaryStream

public void WriteBin(BinaryStream bs, Endian endian = default)
{
TextureSize = (TextureSize <= 0) ? TextureHelper.CalculateTextureSize(TextureHeight, TextureWidth) : TextureSize;

bs.WriteUInt(ReservedField, endian);
bs.WriteUInt(ReservedField2, endian);

bs.WriteInt(TextureHeight, endian);
bs.WriteInt(TextureWidth, endian);

bs.WriteInt(TextureSize, endian);
bs.Write(Padding);

bs.WriteString(TextureFormat.ToString("D"), default, endian);
bs.WriteUInt(ReservedField3, endian);

bs.WriteUInt(ReservedField4, endian);
bs.WriteUInt(ReservedField5, endian);
    
bs.Write(Padding2);
bs.WriteUInt(ReservedField6, endian);

bs.Write(Padding3);
}

}

}
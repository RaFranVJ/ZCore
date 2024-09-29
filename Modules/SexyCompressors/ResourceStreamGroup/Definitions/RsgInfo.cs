namespace ZCore.Modules.SexyCompressors.ResourceStreamGroup.Definitions
{
/// <summary> Represents Info that Specifies how a RSG File was Packed </summary>

public class RsgInfo : SerializableClass<RsgInfo>
{
/** <summary> The Size of the <c>Padding</c> Field. </summary>

<remarks> Padding is Actually a Group of 5 Fields (int) ignored due to being Unknown  </remarks> */

private const int PaddingSize = 20;

/** <summary> Gets or Sets the RSG Version. </summary>
<returns> The File Version </returns> */

public uint FileVersion{ get; set; }

/// <summary> Reserved Field for RSG, don't know what Stands for. </summary>

public long ReservedField{ get; set; }

/** <summary> Gets or Sets the CompressionFlags that Determine how the RSB Packet was/should be Compressed. </summary>
<returns> true or false </returns> */

public uint CompressionFlags{ get; set; }

/** <summary> Gets or Sets the File Offset. </summary>
<returns> The File Offset </returns> */

public uint FileOffset{ get; set; }

/** <summary> Gets or Sets a Offset to the first Part of the RSG Stream. </summary>
<returns> The OffsetToPart0 </returns> */

public uint OffsetToPart0{ get; set; }

/** <summary> Gets or Sets the Size of Part0 after Compression. </summary>
<returns> The Size of Part0 (on Compression) </returns> */

public uint Part0_SizeCompressed{ get; set; }

/** <summary> Gets or Sets the Size of Part0 before Compression. </summary>
<returns> The Size of Part0 </returns> */

public uint Part0_Size{ get; set; }

/// <summary> Reserved Field for RSG, don't know what Stands for. </summary>

public int ReservedField2{ get; set; }

/** <summary> Gets or Sets a Offset to the second Part of the RSG Stream. </summary>
<returns> The OffsetToPart1 </returns> */

public uint OffsetToPart1{ get; set; }

/** <summary> Gets or Sets the Size of Part1 after Compression. </summary>
<returns> The Size of Part1 (on Compression) </returns> */

public uint Part1_SizeCompressed{ get; set; }

/** <summary> Gets or Sets the Size of Part1 before Compression. </summary>
<returns> The Size of Part1 </returns> */

public uint Part1_Size{ get; set; }

/** <summary> Gets or Sets some Bytes used for Padding the File. </summary>
<returns> The Padding Bytes. </returns> */

public byte[] Padding{ get; set; } = new byte[PaddingSize];

/** <summary> Gets or Sets the NumberOfFiles compressed in the Stream. </summary>
<returns> The NumberOfFiles </returns> */

public uint NumberOfFiles{ get; set; }

/** <summary> Gets or Sets a Offset to the FilesList in the Stream. </summary>

<returns> The OffsetToFilesList </returns> */

public uint OffsetToFilesList{ get; set; }

/// <summary> Creates a new Instance of the <c>RsgInfo</c> </summary>

public RsgInfo()
{
}

// Read Info from BinaryStream

public static RsgInfo Read(BinaryStream sourceStream, Endian endian, bool adaptVer)
{

RsgInfo rsgInfo = new()
{
FileVersion = RsgVersion.Read(sourceStream, endian, adaptVer),
ReservedField = sourceStream.ReadLong(endian),
CompressionFlags = sourceStream.ReadUInt(endian),
FileOffset = sourceStream.ReadUInt(endian),
OffsetToPart0 = sourceStream.ReadUInt(endian),
Part0_SizeCompressed = sourceStream.ReadUInt(endian),
Part0_Size = sourceStream.ReadUInt(endian),
ReservedField2 = sourceStream.ReadInt(endian),
OffsetToPart1 = sourceStream.ReadUInt(endian),
Part1_SizeCompressed = sourceStream.ReadUInt(endian),
Part1_Size = sourceStream.ReadUInt(endian),
Padding = sourceStream.ReadBytes(PaddingSize),
NumberOfFiles = sourceStream.ReadUInt(endian),
OffsetToFilesList = sourceStream.ReadUInt(endian)
};
	
return rsgInfo;
}

// Write Info to BinaryStream

}

}
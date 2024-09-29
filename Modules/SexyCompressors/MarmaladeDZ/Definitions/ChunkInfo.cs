using ZCore.Serializables.ArgumentsInfo.SexyCompressor.Dz;

namespace ZCore.Modules.SexyCompressors.MarmaladeDZ.Definitions
{
/// <summary> Represents Chunk Data inside a DTRZ File. </summary>

public class ChunkInfo
{
/** <summary> Gets or Sets the Index for a DirName inside the DTRZ Chunks. </summary>
<returns> The DirName Index. </returns> */

public ushort DirNameIndex{ get; set; }

/** <summary> Gets or Sets the Index for a FileName inside the DTRZ Chunks. </summary>
<returns> The FileName Index. </returns> */

public ushort FileNameIndex{ get; set; }

/** <summary> Gets or Sets the Index of a Chunk inside the Chunks List. </summary>
<returns> The Chunk Index. </returns> */

public ushort ChunkIndex{ get; set; }

/** <summary> Gets or Sets the Chunk Offset inside the DTRZ Stream. </summary>
<returns> The Chunk Offset. </returns> */

public int ChunkOffset{ get; set; }

/** <summary> Gets or Sets the Size used for Decompressing the DZ Stream with the ZLib algorithm. </summary>
<returns> The ZLib Size for Decompression. </returns> */

public int ZLibSizeForDecompression{ get; set; }

/** <summary> Gets or Sets the Chunk Size. </summary>
<returns> The Chunk Size. </returns> */

public int ChunkSize{ get; set; }

/** <summary> Gets or Sets the Type of Compression used in the DZ File. </summary>
<returns> The Compression Type. </returns> */

public CompressionFlags CompressionType{ get; set; }

/** <summary> Gets or Sets the Size used for Compressing the DZ Stream with the ZLib algorithm. </summary>
<returns> The ZLib Size for Compression. </returns> */

public int ZLibSizeForCompression{ get; set; }

/** <summary> Gets or Sets the Size used for Decompressing the DZ Stream with the ZLib algorithm. </summary>
<returns> The Multi Index. </returns> */

public int MultiIndex{ get; set; }

/** <summary> Gets or Sets the File Index inside the DTRZ Stream. </summary>

<remarks> This Field is 0 when there are more than one File in the Stream (FilesCount > 1) </remarks>

<returns> The File Index. </returns> */
		
public ushort FileIndex{ get; set; }

/// <summary> Creates a new Instance of the <c>ChunkInfo</c>. </summary>

public ChunkInfo()
{
CompressionType = CompressionFlags.ReadOnly;
}

/// <summary> Creates a new Instance of the <c>ChunkInfo</c>. </summary>

public ChunkInfo(ushort dirIndex, ushort fileIndex, ushort chunkIndex, int multiIndex = 0)
{
DirNameIndex = dirIndex;
FileNameIndex = fileIndex;

ChunkIndex = chunkIndex;
CompressionType = CompressionFlags.ReadOnly;

MultiIndex = multiIndex;
}

/** <summary> Reads the Chunk Info Stored on a Stream. </summary>

<param name = "sourceStream" > The Stream to be Read. </param>

<returns> The ChunkInfo. </returns> */

public static ChunkInfo Read(BinaryStream sourceStream, Endian endian)
{

return new()
{
ChunkOffset = sourceStream.ReadInt(endian),
ZLibSizeForDecompression = sourceStream.ReadInt(endian),
ChunkSize = sourceStream.ReadInt(endian),
CompressionType = (CompressionFlags)sourceStream.ReadUShort(endian),		
FileIndex = sourceStream.ReadUShort(endian)
};

}

/** <summary> Writes the Chunk Info of a DTRZ File into a Stream. </summary>

<param name = "targetStream" > The Stream where the ChunkInfo will be Written. </param> */

public void Write(BinaryStream targetStream, Endian endian)
{
targetStream.WriteInt(ChunkOffset, endian);
targetStream.WriteInt(ZLibSizeForDecompression, endian);
			
targetStream.WriteInt(ChunkIndex, endian);
targetStream.WriteUShort( (ushort)CompressionType, endian);

targetStream.WriteUShort(FileIndex, endian);
}
 
}

}
using System;

namespace ZCore.Modules.SexyCompressors.ArcVPackage.Definitions
{
/// <summary> Represents a Resource Contained on a Arc-V File. </summary>

public class ArcvResInfo
{
/** <summary> Gets or Sets the Resource Offset in the ARCV Stream. </summary>
<returns> The File Offset. </returns> */

public int FileOffset{ get; set; }

/** <summary> Sets a Value which Contains Info about the Size of an ARCV Resource. </summary>
<returns> The File Size Obtained. </returns> */

public int FileSize{ get; set; }

/** <summary> Gets or Sets a Value which Contains Info about the CRC32 Value of an ARCV Resource. </summary>
<returns> The CRC32 Value. </returns> */

public int CRC32{ get; set; }

/// <summary> Creates a new Instance of the <c>ArcvResInfo</c>. </summary>

public ArcvResInfo()
{
CRC32 = -1;
}

/** <summary> Creates a new Instance of the <c>ArcvResInfo</c> with the given Parameters. </summary>

<param name = "pos"> The File Position. </param>
<param name = "size"> The File Size. </param>
 */

public ArcvResInfo(long pos, long size, long checksum)
{
FileOffset = (int)pos;
FileSize = (int)size;

CRC32 = (int)checksum;
}

/** <summary> Reads the Resource Info Stored on a Stream. </summary>

<param name = "sourceStream" > The Stream to be Read. </param>

<returns> The ResInfo. </returns> */

public static ArcvResInfo Read(BinaryStream sourceStream, Endian endian)
{

return new()
{
FileOffset = (int)sourceStream.ReadUInt(endian),
FileSize = sourceStream.ReadInt(endian),
CRC32 = (int)sourceStream.ReadUInt(endian)
};

}

/** <summary> Writes the Resource Info of an ARCV File into a Stream. </summary>

<param name = "targetStream" > The Stream where the ResInfo will be Written. </param> */

public void Write(BinaryStream targetStream, Endian endian)
{
targetStream.WriteInt(FileOffset, endian);
targetStream.WriteInt(FileSize, endian);

targetStream.WriteInt(CRC32, endian);
}

}

}
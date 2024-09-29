using System.Collections.Generic;
using ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions.Resources;

namespace ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions
{
/// <summary> Represents Info related to a RSB Packet </summary>

public class RsbPacketInfo
{
/** <summary> Gets or Sets the Packet Version. </summary>
<returns> The Packet Version </returns> */

public uint PacketVersion{ get; set; }

/** <summary> Gets or Sets the CompressionFlags that Determine how the RSB Packet was/should be Compressed. </summary>
<returns> true or false </returns> */

public uint CompressionFlags{ get; set; }

/** <summary> Obtains or Creates a List of Entries for the RSB Packet. </summary>
<returns> The ResInfo </returns> */

public List<ResInfo> ResInfo{ get; set; }

/// <summary> Creates a new Instance of the <c>RsbPacketInfo</c> </summary>

public RsbPacketInfo()
{
ResInfo = new();
}

/// <summary> Creates a new Instance of the <c>RsbPacketInfo</c> </summary>

public RsbPacketInfo(uint version, uint flags, List<ResInfo> info)
{
PacketVersion = version;
CompressionFlags = flags;

ResInfo = info;
}

// Init PacketInfo

public static RsbPacketInfo Read(BinaryStream sourceStream, Endian endian, uint groupOffset, List<ResInfo> resInfo)
{
sourceStream.Position = groupOffset + 4;
uint packetVer = sourceStream.ReadUInt(endian);

sourceStream.Position = groupOffset + 16;
uint compressionFlags = sourceStream.ReadUInt(endian);

return new(packetVer, compressionFlags, resInfo);
}

}

}
using System.Collections.Generic;
using ZCore.Modules.SexyCompressors.ResourceStreamGroup.Definitions.Resources;

namespace ZCore.Modules.SexyCompressors.ResourceStreamGroup.Definitions
{
/// <summary> Represents Info related to a RSG Packet </summary>

public class RsgPacketInfo : SerializableClass<RsgPacketInfo>
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

/// <summary> Creates a new Instance of the <c>RsgPacketInfo</c> </summary>

public RsgPacketInfo()
{
ResInfo = new();
}

/// <summary> Creates a new Instance of the <c>RsgPacketInfo</c> </summary>

public RsgPacketInfo(uint version, uint flags, List<ResInfo> info)
{
PacketVersion = version;
CompressionFlags = flags;

ResInfo = info;
}

}

}
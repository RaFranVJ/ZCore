using System.Collections.Generic;

namespace ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions.Composite
{
/// <summary> Represents some Info for a CompositePacket </summary>

public class CompositePacketInfo
{
/** <summary> Gets or Sets the Index of this CompositePacket. </summary>
<returns> The PacketIndex </returns> */
		
public uint PacketIndex{ get; set; }

/** <summary> Gets or Sets the Category of this Packet. </summary>
<returns> The Packet Category </returns> */

public List<string> Category{ get; set; }

/// <summary> Creates a new Instance of the <c>CompositePacketInfo</c> </summary>

public CompositePacketInfo()
{
}

/// <summary> Creates a new Instance of the <c>CompositePacketInfo</c> </summary>

public CompositePacketInfo(uint index, List<string> category)
{
PacketIndex = index;
Category = category;
}

// Get Category Info

private static List<string> ReadCategory(BinaryStream sourceStream, Endian endian)
{
List<string> category = new();

uint categoryId = sourceStream.ReadUInt(endian);
category.Add($"{categoryId}");

string categoryName = sourceStream.ReadString(4, default, endian).Replace("\0", string.Empty);
category.Add(categoryName);

return category;
}

// Read CompositePacketInfo

public static List<CompositePacketInfo> ReadList(BinaryStream sourceStream, Endian endian, uint packetsCount)
{
List<CompositePacketInfo> packetInfo =  new();

for(int i = 0; i < packetsCount; i++)
{
uint packetIndex = sourceStream.ReadUInt(endian);
List<string> category = ReadCategory(sourceStream, endian);

packetInfo.Add( new(packetIndex, category) );

sourceStream.ReadBytes(4); // Skip 4 Bytes (don't know what they are)
}

return packetInfo;
}

}

}
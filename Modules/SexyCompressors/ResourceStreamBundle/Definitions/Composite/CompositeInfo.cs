using System;
using System.Collections.Generic;
using ZCore.Modules.SexyCompressors.ResourceStreamBundle.Exceptions;

namespace ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions.Composite
{
/// <summary> Represents some Info for a CompositeItem </summary>

public class CompositeInfo
{
/// <summary> The Composite Suffix. </summary>

public const string compositeSuffix = "_CompositeShell";

/** <summary> Gets or Sets a Name for the CompositeItem. </summary>
<returns> The ItemName </returns> */

public string ItemName{ get; set; }

/** <summary> Gets or Sets a Boolean that Determine if the CompositeItem is efectively a CompositeItem or not </summary>
<returns> true or false </returns> */

public bool IsComposite{ get; set; }

/** <summary> Gets or Sets the Number of Packets inide the CompositeItem. </summary>
<returns> The NumberOfPackets </returns> */

public uint NumberOfPackets{ get; set; }

/** <summary> Gets or Sets some Info related to this Packet. </summary>
<returns> The PacketInfo </returns> */
		
public List<CompositePacketInfo> PacketInfo{ get; set; }

/// <summary> Creates a new Instance of the <c>CompositeInfo</c> </summary>

public CompositeInfo()
{
ItemName = "COMPOSITE_";
IsComposite = true;

PacketInfo = new();
}

/// <summary> Creates a new Instance of the <c>CompositeInfo</c> </summary>

public CompositeInfo(string name, bool isComposite, uint packetsCount, List<CompositePacketInfo> info)
{
ItemName = name;
IsComposite = isComposite;

NumberOfPackets = packetsCount;
PacketInfo = info;
}

// Check if Group is Composite

public static bool CheckGroupID(string targetID) => !targetID.EndsWith(compositeSuffix);

// Remove suffix from CompositeName

public static void RemoveCompositeSuffix(ref string targetStr)
{
int suffixIndex = targetStr.IndexOf(compositeSuffix, StringComparison.OrdinalIgnoreCase);

if(suffixIndex >= 0) 
targetStr = targetStr.Remove(suffixIndex, compositeSuffix.Length);

}

// Check if CompositeName matches in CompositeInfo and CompositeEntry

public static void ValidateName(CompositeInfo info, RsbFileEntry entry)
{
string entryName = entry.FullName;
RemoveCompositeSuffix(ref entryName);

if(!info.ItemName.Equals(entryName, StringComparison.OrdinalIgnoreCase) )
throw new InvalidCompositeNameException(info.ItemName, entryName);
 
}

// Read Composite Info

public static List<CompositeInfo> ReadList(BinaryStream sourceStream, Endian endian, uint offset,
uint itemsCount, uint infoLength)
{
sourceStream.Position = offset;

List<CompositeInfo> compositeInfo = new();

for(var i = 0; i < itemsCount; i++)
{
long startOffset = sourceStream.Position;

string compositeName = sourceStream.ReadStringUntilZero(default, endian);

sourceStream.Position = startOffset + infoLength - 4;
uint packetsCount = sourceStream.ReadUInt(endian);

long backupOffset = sourceStream.Position;

sourceStream.Position = startOffset + 128;
				
List<CompositePacketInfo> packetInfo = CompositePacketInfo.ReadList(sourceStream, endian, packetsCount);
bool isComposite = CheckGroupID(compositeName);

RemoveCompositeSuffix(ref compositeName);
compositeInfo.Add( new(compositeName, isComposite, packetsCount, packetInfo) );
				
sourceStream.Position = backupOffset;
}

uint endOffset = infoLength * itemsCount + offset;
RsbHelper.CheckEndOffset(sourceStream, endOffset);

return compositeInfo;
}

}

}
using System.Collections.Generic;
using ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions.Composite;
using ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions.Resources;
using ZCore.Modules.SexyCompressors.ResourceStreamBundle.Exceptions;

namespace ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions.Description
{
/// <summary> Represents a Group Description </summary>

public class GroupDescription
{
/** <summary> Gets or Sets a Boolean that Determine if the Group should be threated as a Composite one or not. </summary>
<returns> true or false </returns> */

public bool IsComposite{ get; set; }

/** <summary> Gets or Sets a Map to the SubGroups inside the Group. </summary>
<returns> The SubGroupsMap </returns> */

public Dictionary<string, SubGroupDescription> SubGroupsMap{ get; set; } = new();

/// <summary> Creates a new Instance of the <c>GroupDescription</c> </summary>

public GroupDescription()
{
}

/// <summary> Creates a new Instance of the <c>GroupDescription</c> </summary>

public GroupDescription(bool isComposite, Dictionary<string, SubGroupDescription> subGroups)
{
IsComposite = isComposite;
SubGroupsMap = subGroups;
}

// Read Groups and SubGroups along with Composite

public static void Read(BinaryStream sourceStream, Endian endian, uint part3_Offset, 
List<CompositeResDescriptionInfo> compositeResInfo, Dictionary<string, GroupDescription> groupDesc,
uint expectedRsgNumber = 0x10, bool checkRsgNumber = true)
{
uint idOffset = sourceStream.ReadUInt(endian);

sourceStream.Position = part3_Offset + idOffset;
string groupId = sourceStream.ReadStringUntilZero(default, endian);

uint rsgCount = sourceStream.ReadUInt(endian);
Dictionary<string, SubGroupDescription> subGroups = new();

uint rsgNumber = sourceStream.ReadUInt(endian);

if(checkRsgNumber && rsgNumber != expectedRsgNumber)
throw new InvalidRsgNumberException(sourceStream.Position, rsgNumber, expectedRsgNumber);

var rsgInfo = ResInfoForRsb.ReadList(sourceStream, endian, rsgCount, part3_Offset, subGroups);

groupDesc.Add(groupId, new(CompositeInfo.CheckGroupID(groupId), subGroups) );
compositeResInfo.Add( new(groupId, rsgCount, rsgInfo) );
}

// Write Groups and SubGroups

public static void Write(BinaryStream part1_Res, BinaryStream part2_Res, BinaryStream part3_Res,
Endian endian, ResDescription resDesc, Dictionary<string, uint> stringPool)
{
// Process Groups

foreach(var key in resDesc.GroupsMap.Keys)
{
uint groupId = RsbHelper.ThrowInPool(part3_Res, stringPool, key, endian);
part1_Res.WriteUInt(groupId, endian);

var subGroupKeys = resDesc.GroupsMap[key].SubGroupsMap.Keys;

part1_Res.WriteUInt( (uint)subGroupKeys.Count, endian);
part1_Res.WriteUInt(0x10, endian);

// Process SubGroups

foreach(var subKey in subGroupKeys)
resDesc.GroupsMap[key].SubGroupsMap[subKey].Write(part1_Res, part2_Res, part3_Res, endian, stringPool, key);
      
}

}

}

}
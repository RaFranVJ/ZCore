using System.Collections.Generic;
using System.Linq;
using ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions.Resources;

namespace ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions.Description
{
/// <summary> Represents some Info for the Description of a Composite Resource </summary>

public class CompositeResDescriptionInfo
{
/** <summary> Gets or Sets a String used for Identifying the Composite Resource. </summary>
<returns> The CompositeID </returns> */

public string CompositeID{ get; set; }

/** <summary> Gets or Sets the Number of Groups inside the Composite Resource </summary>
<returns> The NumberOfGroups </returns> */

public uint NumberOfGroups{ get; set; }

/** <summary> Gets or Sets some Info related to the Groups of the Composite Resource. </summary>
<returns> The ResGroups </returns> */

public List<ResInfoForRsb> ResGroups{ get; set; }

/// <summary> Creates a new Instance of the <c>CompositeResDescriptionInfo</c> </summary>

public CompositeResDescriptionInfo()
{
CompositeID = "<SET A COMPOSITE ID HERE>";
ResGroups = new();
}

/// <summary> Creates a new Instance of the <c>CompositeInfo</c> </summary>

public CompositeResDescriptionInfo(string id, uint groupsCount, List<ResInfoForRsb> resGroups)
{
CompositeID = id;
NumberOfGroups = groupsCount;

ResGroups = resGroups;
}

// Process Composite Resources

public static void ProcessRes(BinaryStream sourceStream, Endian endian, uint part2_Offset, 
uint part3_Offset, List<CompositeResDescriptionInfo> compositeResInfo, 
Dictionary<string, GroupDescription> groupDesc, int groupIndex)
{
long backupOffset = sourceStream.Position;

uint groupsCount = compositeResInfo[groupIndex].NumberOfGroups;

for(int i = 0; i < groupsCount; i++)
{
uint resCount = compositeResInfo[groupIndex].ResGroups[i].NumberOfResources;

for(int j = 0; j < (int)resCount; j++)
{
sourceStream.Position = part2_Offset + compositeResInfo[groupIndex].ResGroups[i].ResInfo[j].OffsetToInfoInPart2;
var resDesc = FileDescription.Read(sourceStream, endian, part3_Offset, out string resId);

groupDesc.ElementAt(groupIndex).Value.SubGroupsMap.ElementAt(i).Value.ResourcesMap[resId] = resDesc;
}

}

sourceStream.Position = backupOffset;
}

}

}
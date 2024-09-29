using System.Collections.Generic;
using ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions.Description;

namespace ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions.Resources
{
/// <summary> Represents Info of a Resource which Points to a ResGroup inside a RSB Stream </summary>

public class ResInfoForRsb
{
/** <summary> Gets or Sets the ResolutionRatio of this File. </summary>
<returns> The ResolutionRatio </returns> */

public uint ResolutionRatio{ get; set; }

/** <summary> Gets or Sets the Language of this Group. </summary>
<returns> The Group Language </returns> */

public string GroupLanguage{ get; set; }

/** <summary> Gets or Sets a String used for Identifying the Group. </summary>
<returns> The GroupID </returns> */

public string GroupID{ get; set; }

/** <summary> Gets or Sets the Number of Resources inside the Group </summary>
<returns> The NumberOfGroups </returns> */

public uint NumberOfResources{ get; set; }

/** <summary> Gets or Sets some Info related to the Resources of this Group. </summary>
<returns> The ResInfo </returns> */

public List<ResInfoForRsg> ResInfo{ get; set; }

/// <summary> Creates a new Instance of the <c>ResInfo</c> (RSB Variation) </summary>

public ResInfoForRsb()
{
GroupID = "<SET A GROUP ID HERE>";

ResInfo = new();
}


/// <summary> Creates a new Instance of the <c>ResInfo</c> (RSB Variation) </summary>

public ResInfoForRsb(uint ratio, string lang, string id, uint resCount, List<ResInfoForRsg> info)
{
ResolutionRatio = ratio;
GroupLanguage = lang;

GroupID = id;
NumberOfResources = resCount;

ResInfo = info;
}
// Read a List of Info for RSB and its Groups

public static List<ResInfoForRsb> ReadList(BinaryStream sourceStream, Endian endian, uint groupsCount, 
uint part3_Offset, Dictionary<string, SubGroupDescription> subGroups)
{
List<ResInfoForRsb> rsgInfo = new();

for(uint i = 0; i < groupsCount; i++)
{
uint ratio = sourceStream.ReadUInt(endian);
string lang = sourceStream.ReadString(4, default, endian).Replace("\0", string.Empty);

uint rsgIdOffset = sourceStream.ReadUInt(endian);
uint resCount = sourceStream.ReadUInt(endian);

var resInfo = ResInfoForRsg.ReadList(sourceStream, endian, resCount);

sourceStream.Position = part3_Offset + rsgIdOffset;
string rsgId = sourceStream.ReadStringUntilZero(default, endian);

subGroups.Add(rsgId, new(ratio, lang) );
rsgInfo.Add( new(ratio, lang,rsgId, resCount, resInfo) );
}

return rsgInfo;
}

}

}
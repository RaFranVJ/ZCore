using System.Collections.Generic;
using ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions.Description;

namespace ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions.Resources
{
/// <summary> Represents Info of a Resource that is inside a ResGroup, that belongs to a RSB Stream </summary>

public class ResInfoForRsg
{
/** <summary> Gets or Sets to the Info of this Resource in the Part2 of the RSB Stream. </summary>
<returns> The OffsetToInfoInPart2 </returns> */

public uint OffsetToInfoInPart2{ get; set; }

/** <summary> Gets or Sets to the NumberOfProperties for this Resource. </summary>
<returns> The NumberOfResources </returns> */

public uint NumberOfProperties{ get; set; }

/** <summary> Gets or Sets a String used for Identifying the Resource. </summary>
<returns> The ResID </returns> */

public string ResID{ get; set; }

/** <summary> Gets or Sets a Path to the Resource. </summary>
<returns> The PathToResFile </returns> */

public string PathToResFile{ get; set; }

/** <summary> Gets or Sets some Info that Describes the Image. </summary>

<remarks> Must set this Field in case the Resource is a Image </remarks>

<returns> The ImageInfo </returns> */

public ImageInfoForResDescription ImageInfo{ get; set; }

/** <summary> Gets or Sets a List that Contains the Properties of this Resource. </summary>
<returns> The ResProperties </returns> */

public List<ResPropertiesInfo> ResProperties{ get; set; }

/// <summary> Creates a new Instance of the <c>ResInfo</c> (RSG Variation) </summary>

public ResInfoForRsg()
{
ResID = "<SET A RESOURCE ID HERE>";
PathToResFile = "<DEFINE A PATH FOR RESOURCE HERE>";

ResProperties = new();
}

/// <summary> Creates a new Instance of the <c>ResInfo</c> (RSG Variation) </summary>

public ResInfoForRsg(uint infoOffset)
{
OffsetToInfoInPart2 = infoOffset;
ResID = "<SET A RESOURCE ID HERE>";

PathToResFile = "<DEFINE A PATH FOR RESOURCE HERE>";
ResProperties = new();
}

/// <summary> Creates a new Instance of the <c>ResInfo</c> (RSG Variation) </summary>

public ResInfoForRsg(string id, string filePath)
{
ResID = id;
PathToResFile = filePath;

ResProperties = new();
}

// Read Info from RSG

public static List<ResInfoForRsg> ReadList(BinaryStream sourceStream, Endian endian, uint resCount)
{
List<ResInfoForRsg> resInfo = new();

for(uint i = 0; i < resCount; i++)
{
uint infoOffset = sourceStream.ReadUInt(endian);

resInfo.Add( new(infoOffset) );
}

return resInfo;
}

}

}
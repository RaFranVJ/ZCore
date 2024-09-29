using System.Collections.Generic;

namespace ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions.Description
{
/// <summary> Represents Info for each Resource inside a RSB Stream (used on the v3.0 of the Algorithm) </summary>

public class ResDescription : SerializableClass<ResDescription>
{
/** <summary> Gets or Sets a Map to the Groups inside the RSB Stream. </summary>
<returns> The SubGroupsMap </returns> */

public Dictionary<string, GroupDescription> GroupsMap{ get; set; } = new();

/// <summary> Creates a new Instance of the <c>FileDescription</c> </summary>

public ResDescription()
{
}

/// <summary> Creates a new Instance of the <c>FileDescription</c> </summary>

public ResDescription(Dictionary<string, GroupDescription> groupDesc)
{
GroupsMap = groupDesc;
}

// Read Description from BinaryStream

public static ResDescription Read(BinaryStream sourceStream, Endian endian, uint part1_Offset,
uint part2_Offset, uint part3_Offset)
{
sourceStream.Position = part1_Offset;

List<CompositeResDescriptionInfo> compositeResInfo = new();
ResDescription resDesc = new();

for(int i = 0; sourceStream.Position < part2_Offset; i++)
{
GroupDescription.Read(sourceStream, endian, part3_Offset, compositeResInfo, resDesc.GroupsMap);

CompositeResDescriptionInfo.ProcessRes(sourceStream, endian, part2_Offset, part3_Offset, 
compositeResInfo, resDesc.GroupsMap, i);
}

return resDesc;
}

// Write

public void Write(BinaryStream targetStream, Endian endian, int bufferSize, ref RsbInfo rsbInfo)
{
using BinaryStream part1_Res = new();
using BinaryStream part2_Res = new();

using BinaryStream part3_Res = new();
Dictionary<string, uint> stringPool = new();

part3_Res.WritePadding(1);
stringPool.Add(string.Empty, 0); // Root dir is Empty

GroupDescription.Write(part1_Res, part2_Res, part3_Res, endian, this, stringPool);

rsbInfo.OffsetToPart1 = (uint)targetStream.Position;

part1_Res.Position = 0;
FileManager.ProcessBuffer(part1_Res, targetStream, bufferSize);

rsbInfo.OffsetToPart2 = (uint)targetStream.Position;

part2_Res.Position = 0;
FileManager.ProcessBuffer(part2_Res, targetStream, bufferSize);

rsbInfo.OffsetToPart3 = (uint)targetStream.Position;

part3_Res.Position = 0;
FileManager.ProcessBuffer(part3_Res, targetStream, bufferSize);
}

}

}
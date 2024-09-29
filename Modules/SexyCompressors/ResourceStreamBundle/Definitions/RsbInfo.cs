using ZCore.Modules.SexyCompressors.ResourceStreamBundle.Exceptions;

namespace ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions
{
/// <summary> Represents Info that Specifies how a RSB File was Packed </summary>

public class RsbInfo : SerializableClass<RsbInfo>
{
/** <summary> Gets or Sets the RSB Version. </summary>
<returns> The File Version </returns> */

public uint FileVersion{ get; set; }

/// <summary> Reserved Field for RSB, don't know what Stands for. </summary>

public int ReservedField{ get; set; }

/** <summary> Gets or Sets the File Offset. </summary>
<returns> The File Offset </returns> */

public uint FileOffset{ get; set; }

/** <summary> Gets or Sets the NumberOfFiles compressed in the Stream. </summary>
<returns> The NumberOfFiles </returns> */

public uint NumberOfFiles{ get; set; }

/** <summary> Gets or Sets a Offset to the FilesList in the Stream. </summary>

<remarks> Offset expected in V3 is 0x6C, and 0x70 in V4 </remarks>

<returns> The OffsetToFilesList </returns> */

public uint OffsetToFilesList{ get; set; }

/// <summary> Reserved Field for RSB, don't know what Stands for. </summary>

public long ReservedField2{ get; set; }

/** <summary> Gets or Sets the Size of the RSG List in the Stream. </summary>
<returns> The GroupsListSize </returns> */

public uint GroupsListSize{ get; set; }

/** <summary> Gets or Sets a Offset to the GroupsList in the Stream. </summary>
<returns> The OffsetToGrousList </returns> */

public uint OffsetToGroupsList{ get; set; }

/** <summary> Gets or Sets the NumberOfGroups compressed in the Stream. </summary>
<returns> The NumberOfGroups </returns> */

public uint NumberOfGroups{ get; set; }

/** <summary> Gets or Sets a Offset to the GroupsInfo in the Stream. </summary>
<returns> The OffsetToGroupsInfo </returns> */

public uint OffsetToGroupsInfo{ get; set; }

/** <summary> Gets or Sets the Size in bytes for the GroupsInfo written in the Stream. </summary>
<returns> The SizeForGroupsInfo </returns> */

public uint SizeForGroupsInfo{ get; set; } = 204;

/** <summary> Gets or Sets the NumberOfCompositeFiles in the Stream. </summary>
<returns> The NumberOfCompositeFiles </returns> */

public uint NumberOfCompositeItems{ get; set; }

/** <summary> Gets or Sets a Offset to the CompositeInfo in the Stream. </summary>
<returns> The OffsetToCompositeInfo </returns> */

public uint OffsetToCompositeInfo{ get; set; } = 1156;

/** <summary> Gets or Sets the Size in bytes for the CompositeInfo written in the Stream. </summary>
<returns> The SizeForCompositeInfo </returns> */

public uint SizeForCompositeInfo{ get; set; }

/** <summary> Gets or Sets the Size of the CompositeList in the Stream. </summary>
<returns> The CompositeListSize </returns> */

public uint CompositeListSize{ get; set; }

/** <summary> Gets or Sets a Offset to the CompositeList in the Stream. </summary>
<returns> The OffsetToCompositeList </returns> */

public uint OffsetToCompositeList{ get; set; }

/** <summary> Gets or Sets the NumberOfAutoPools in the Stream. </summary>
<returns> The NumberOfAutoPools </returns> */

public uint NumberOfAutoPools{ get; set; }

/** <summary> Gets or Sets a Offset to the AutoPoolInfo in the Stream. </summary>
<returns> The OffsetToAutoPoolInfo </returns> */

public uint OffsetToAutoPoolInfo{ get; set; }

/** <summary> Gets or Sets the Size in bytes for the AutoPoolInfo written in the Stream. </summary>
<returns> The SizeForAutoPoolInfo </returns> */

public uint SizeForAutoPoolInfo{ get; set; }

/** <summary> Gets or Sets the NumberOfPtxFiles in the Stream. </summary>
<returns> The NumberOfPtxFiles </returns> */

public uint NumberOfPtxFiles{ get; set; }

/** <summary> Gets or Sets a Offset to the PtxInfo in the Stream. </summary>
<returns> The OffsetToPtxInfo </returns> */

public uint OffsetToPtxInfo{ get; set; }

/** <summary> Gets or Sets the Size in bytes for the PtxInfo written in the Stream. </summary>
<returns> The SizeForPtxInfo </returns> */

public uint SizeForPtxInfo{ get; set; }

/** <summary> Gets or Sets a Offset to the first Part of the RSB Stream. </summary>
<returns> The OffsetToPart1 </returns> */

public uint OffsetToPart1{ get; set; }

/** <summary> Gets or Sets a Offset to the second Part of the RSB Stream. </summary>
<returns> The OffsetToPart2 </returns> */

public uint OffsetToPart2{ get; set; }

/** <summary> Gets or Sets a Offset to the third Part of the RSB Stream. </summary>
<returns> The OffsetToPart3 </returns> */

public uint OffsetToPart3{ get; set; }

/// <summary> Creates a new Instance of the <c>RsbInfo</c> </summary>

public RsbInfo()
{
}

// Read Info from BinaryStream

public static RsbInfo Read(BinaryStream sourceStream, Endian endian, bool adaptVer)
{

RsbInfo rsbInfo = new()
{
FileVersion = RsbVersion.Read(sourceStream, endian, adaptVer),
ReservedField = sourceStream.ReadInt(endian),
FileOffset = sourceStream.ReadUInt(endian),
NumberOfFiles = sourceStream.ReadUInt(endian)
};

uint fileList_Offset = sourceStream.ReadUInt(endian);

if(rsbInfo.FileVersion == 3 && fileList_Offset != 0x6C)
throw new FileList_InvalidOffsetException(rsbInfo.FileVersion, fileList_Offset, 0x6C);

else if(rsbInfo.FileVersion == 4 && fileList_Offset != 0x70)
throw new FileList_InvalidOffsetException(rsbInfo.FileVersion, fileList_Offset, 0x70);

rsbInfo.OffsetToFilesList = fileList_Offset;
rsbInfo.ReservedField2 = sourceStream.ReadLong(endian);
rsbInfo.GroupsListSize = sourceStream.ReadUInt(endian);
rsbInfo.OffsetToGroupsList = sourceStream.ReadUInt(endian);
rsbInfo.NumberOfGroups = sourceStream.ReadUInt(endian);
rsbInfo.OffsetToGroupsInfo = sourceStream.ReadUInt(endian);
rsbInfo.SizeForGroupsInfo = sourceStream.ReadUInt(endian);
rsbInfo.NumberOfCompositeItems = sourceStream.ReadUInt(endian);
rsbInfo.OffsetToCompositeInfo = sourceStream.ReadUInt(endian);
rsbInfo.SizeForCompositeInfo = sourceStream.ReadUInt(endian);
rsbInfo.CompositeListSize = sourceStream.ReadUInt(endian);
rsbInfo.OffsetToCompositeList = sourceStream.ReadUInt(endian);
rsbInfo.NumberOfAutoPools = sourceStream.ReadUInt(endian);
rsbInfo.OffsetToAutoPoolInfo = sourceStream.ReadUInt(endian);
rsbInfo.SizeForAutoPoolInfo = sourceStream.ReadUInt(endian);
rsbInfo.NumberOfPtxFiles = sourceStream.ReadUInt(endian);
rsbInfo.OffsetToPtxInfo = sourceStream.ReadUInt(endian);
rsbInfo.SizeForPtxInfo = sourceStream.ReadUInt(endian);

uint part1_Offset = sourceStream.ReadUInt(endian);

if(rsbInfo.FileVersion == 3 && part1_Offset == 0)
throw new ResDesc_InvalidOffsetException();

rsbInfo.OffsetToPart1 = part1_Offset;

uint part2_Offset = sourceStream.ReadUInt(endian);

if(rsbInfo.FileVersion == 3 && part2_Offset == 0)
throw new ResDesc_InvalidOffsetException();

rsbInfo.OffsetToPart2 = part2_Offset;

uint part3_Offset = sourceStream.ReadUInt(endian);

if(rsbInfo.FileVersion == 3 && part3_Offset == 0)
throw new ResDesc_InvalidOffsetException();

rsbInfo.OffsetToPart3 = part3_Offset;

if(rsbInfo.FileVersion == 4)
rsbInfo.FileOffset = sourceStream.ReadUInt(endian);
		
return rsbInfo;
}

// Write Info to BinaryStream

public void Write(BinaryStream targetStream, Endian endian, bool adaptVer)
{
RsbVersion.Write(targetStream, endian, FileVersion, adaptVer);

targetStream.WriteInt(ReservedField, endian);
targetStream.WriteUInt(FileOffset, endian);
targetStream.WriteUInt(NumberOfFiles, endian);

if(FileVersion == 3 && OffsetToFilesList != 0x6C)
throw new FileList_InvalidOffsetException(FileVersion, OffsetToFilesList, 0x6C);

else if(FileVersion == 4 && OffsetToFilesList != 0x70)
throw new FileList_InvalidOffsetException(FileVersion, OffsetToFilesList, 0x70);

targetStream.WriteUInt(OffsetToFilesList, endian);
targetStream.WriteLong(ReservedField2, endian);
targetStream.WriteUInt(GroupsListSize, endian);
targetStream.WriteUInt(OffsetToGroupsList, endian);
targetStream.WriteUInt(NumberOfGroups, endian);
targetStream.WriteUInt(OffsetToGroupsInfo, endian);
targetStream.WriteUInt(SizeForGroupsInfo, endian);
targetStream.WriteUInt(NumberOfCompositeItems, endian);
targetStream.WriteUInt(OffsetToCompositeInfo, endian);
targetStream.WriteUInt(SizeForCompositeInfo, endian);
targetStream.WriteUInt(CompositeListSize, endian);
targetStream.WriteUInt(OffsetToCompositeList, endian);
targetStream.WriteUInt(NumberOfAutoPools, endian);
targetStream.WriteUInt(OffsetToAutoPoolInfo, endian);
targetStream.WriteUInt(SizeForAutoPoolInfo, endian);
targetStream.WriteUInt(NumberOfPtxFiles, endian);
targetStream.WriteUInt(OffsetToPtxInfo, endian);
targetStream.WriteUInt(SizeForPtxInfo, endian);

if(FileVersion == 3 && OffsetToPart1 == 0 && OffsetToPart2 == 0 && OffsetToPart3 == 0)
throw new ResDesc_InvalidOffsetException();

targetStream.WriteUInt(OffsetToPart1, endian);
targetStream.WriteUInt(OffsetToPart2, endian);
targetStream.WriteUInt(OffsetToPart3, endian);

if(FileVersion == 4)
targetStream.WriteUInt(FileOffset, endian);

}

}

}
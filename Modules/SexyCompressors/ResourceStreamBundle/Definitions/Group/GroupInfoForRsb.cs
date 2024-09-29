using System.Collections.Generic;

namespace ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions.Group
{
/// <summary> Represents some Info for a ResGroup inside a RSB Stream </summary>

public class GroupInfoForRsb
{
/** <summary> Gets or Sets the Group Name. </summary>
<returns> The GroupName </returns> */

public string GroupName{ get; set; }

/** <summary> Gets or Sets the Offset of the Group inside the RSB Stream </summary>
<returns> The GroupOffset </returns> */

public uint GroupOffset{ get; set; }

/** <summary> Gets or Sets the Size of the RSG File inside the Bundle Stream. </summary>
<returns> The FileLength </returns> */

public int FileLength{ get; set; }

/** <summary> Gets or Sets the Index of the Group inside the Pool. </summary>
<returns> The PoolIndex </returns> */

public uint PoolIndex{ get; set; }

/** <summary> Gets or Sets the NumberOfPtxFiles inside the Group. </summary>
<returns> The NumberOfPtxFiles </returns> */

public uint NumberOfPtxFiles{ get; set; }

/** <summary> Gets or Sets the NumberOfPtxFiles before the Group. </summary>
<returns> The NumberOfPtxFiles before Group </returns> */

public uint NumberOfPtxFilesBeforeGroup{ get; set; }

/** <summary> Gets or Sets some Bytes that represent the Header of this Group. </summary>
<returns> The PacketHeadInfo </returns> */

public byte[] PacketHeadInfo{ get; set; }

/// <summary> Creates a new Instance of the <c>GroupInfo</c> (RSB Variation) </summary>

public GroupInfoForRsb()
{
GroupName = "My Group Name";
}

/// <summary> Creates a new Instance of the <c>GroupInfo</c> (RSB Variation) </summary>

public GroupInfoForRsb(string name, uint offset, int length, uint index,
uint ptxCount, uint ptxCount_Before, byte[] packetHead = null)
{
GroupName = name;
GroupOffset = offset;

FileLength = length;
PoolIndex = index;

NumberOfPtxFiles = ptxCount;
NumberOfPtxFilesBeforeGroup = ptxCount_Before;

PacketHeadInfo = packetHead;
}

// Read Res Info for RSG

public static List<GroupInfoForRsb> ReadList(BinaryStream sourceStream, Endian endian, uint offset,
uint groupsCount, uint infoLength)
{
sourceStream.Position = offset;

List<GroupInfoForRsb> rsgInfo = new();
	
for(int i = 0; i < groupsCount; i++)
{
long startOffset = sourceStream.Position;

string packetName = sourceStream.ReadStringUntilZero(default, endian);

sourceStream.Position = startOffset + 128;

uint rsgOffset = sourceStream.ReadUInt(endian);
int rsgLength = sourceStream.ReadInt(endian);

uint rsgIndex = sourceStream.ReadUInt(endian);

sourceStream.Position = startOffset + infoLength - 8;
uint ptxCount = sourceStream.ReadUInt(endian);

uint ptxsBefore = sourceStream.ReadUInt(endian);

rsgInfo.Add( new(packetName, rsgOffset, rsgLength, rsgIndex, ptxCount, ptxsBefore) );
}

uint endOffset = infoLength * groupsCount + offset;
RsbHelper.CheckEndOffset(sourceStream, endOffset);

return rsgInfo;
}

// Write ResGroup Info

public static void Write(BinaryStream rsgInfo, BinaryStream rsgFile, BinaryStream rsgBank, Endian endian,
int bufferSize, string rsgName, ref int rsgPacketIndex, uint ptxsBefore, BinaryStream ptxInfo)
{
rsgInfo.WriteString(rsgName, default, endian);
rsgInfo.WritePadding(128 - rsgName.Length);

rsgInfo.WriteUInt( (uint)rsgBank.Position, endian);

rsgFile.Position = 0;
FileManager.ProcessBuffer(rsgFile, rsgBank, bufferSize);

rsgInfo.WriteUInt( (uint)rsgFile.Length, endian);
rsgInfo.WriteInt(rsgPacketIndex, endian);

rsgFile.Position = 16;
FileManager.ProcessBuffer(rsgFile, rsgInfo, bufferSize, 56);

long rsgOffset = rsgInfo.Position;

rsgFile.Position = 32;
rsgInfo.Position = rsgOffset - 36;

rsgInfo.WriteInt(rsgFile.ReadInt(endian), endian);

rsgInfo.Position = rsgOffset;
rsgInfo.WriteUInt(ptxsBefore, endian);
}

}

}
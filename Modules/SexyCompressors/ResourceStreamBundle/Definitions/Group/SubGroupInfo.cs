using System;
using System.Collections.Generic;
using System.IO;
using ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions.Composite;
using ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions.PopCapTexture;
using ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions.Resources;
using ZCore.Modules.SexyCompressors.ResourceStreamGroup;
using ZCore.Modules.SexyCompressors.ResourceStreamGroup.Methods;
using ZCore.Serializables.ArgumentsInfo.SexyCompressor.Rsg;

namespace ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions.Group
{
/// <summary> Represents some Info for a SubGroup that was Extracted from a RSB Stream </summary>

public class SubGroupInfo
{
/** <summary> Gets or Sets the SubGroup Name. </summary>
<returns> The SubGroupName </returns> */

public string SubGroupName{ get; set; }

/** <summary> Gets or Sets a Boolean that Determine if the Group should be threated as a Composite one or not. </summary>
<returns> true or false </returns> */

public List<string> Category{ get; set; }

/** <summary> Obtains or Creates a List of Entries for the SubGroups inside the Group. </summary>
<returns> The SubGroupEntries </returns> */

public RsbPacketInfo PacketInfo{ get; set; } = new();

/// <summary> Creates a new Instance of the <c>GroupInfo</c> </summary>

public SubGroupInfo()
{
SubGroupName = "My SubGroup Name";
}

public SubGroupInfo(string name, List<string> category, RsbPacketInfo packetInfo)
{
SubGroupName = name;
Category = category;

PacketInfo = packetInfo;
}

// Find Index for RsgInfo

private static int FindIndexForRsgInfo(List<GroupInfoForRsb> rsgInfo, uint packetIndex)
{
int index = 0;

while(rsgInfo[index].PoolIndex != packetIndex)
{

if(index >= rsgInfo.Count - 1)
throw new ArgumentOutOfRangeException(nameof(packetIndex) );
        
index++;
}

return index;
}

// Find Index for RSG Rntry

private static int FindIndexForRsgEntry(List<RsbFileEntry> rsgList, uint packetIndex)
{
int index = 0;

while(rsgList[index].PoolIndex != packetIndex)
{

if(index >= rsgList.Count - 1)
throw new ArgumentOutOfRangeException(nameof(packetIndex) );
        
index++;
}

return index;
}

// Init Category

private static List<string> InitCategory(List<string> baseList)
{
List<string> category = new();

category.Add(baseList[0] );
category.Add(baseList[1] );

return category;
}

// Extract Packets (also known as SubGroups)

public static List<SubGroupInfo> ExtractPackets(BinaryStream sourceStream, Endian endian, int bufferSize,
string outputDir, bool adaptVer, StringCase strCaseForFileNames, bool extractRsgContent,
List<RsbFileEntry> fileList, List<RsbFileEntry> rsgList, List<GroupInfoForRsb> rsgInfo,
List<PtxParamsForRsb> ptxInfo, CompositeInfo compositeInfo, RsgExtractParams extractParams)
{
List<SubGroupInfo> subGroupList = new();

for(int i = 0; i < compositeInfo.NumberOfPackets; i++)
{
uint packetIndex = compositeInfo.PacketInfo![i].PacketIndex;

int rsgInfo_Index = FindIndexForRsgInfo(rsgInfo, packetIndex);
int rsgEntry_Index = FindIndexForRsgEntry(rsgList, packetIndex);

GroupInfo.ValidateName(rsgInfo[rsgInfo_Index], rsgList[rsgEntry_Index], packetIndex);

string rsgName = RsgHelper.BuildFilePath(outputDir, rsgInfo[rsgInfo_Index].GroupName, strCaseForFileNames);
using BinaryStream rsgStream = extractRsgContent ? new() : BinaryStream.OpenWrite(rsgName);

sourceStream.Position = rsgInfo[rsgInfo_Index].GroupOffset;
FileManager.ProcessBuffer(sourceStream, rsgStream, bufferSize, rsgInfo[rsgInfo_Index].FileLength);

rsgStream.Position = 0;

string rsgPacketDir = extractRsgContent ? rsgName : string.Empty;
PathHelper.ChangeExtension(ref rsgPacketDir, ".packet");

var packetInfo = RsgCompressor.DecompressStream(rsgStream, bufferSize, strCaseForFileNames,
extractRsgContent, !extractRsgContent, endian, adaptVer, rsgPacketDir, extractParams);

if(extractRsgContent)
packetInfo.WriteObject(rsgPacketDir + Path.DirectorySeparatorChar + "PacketInfo.json");

var resInfo = ResInfo.Extract(fileList, rsgInfo, compositeInfo, packetInfo, ptxInfo, rsgInfo_Index, packetIndex);

var packetInfo_SubGroup = RsbPacketInfo.Read(sourceStream, endian, rsgInfo[rsgInfo_Index].GroupOffset, resInfo);
var category = InitCategory(compositeInfo.PacketInfo![i].Category);

subGroupList.Add( new(rsgInfo[rsgInfo_Index].GroupName, category, packetInfo_SubGroup) );
}

return subGroupList;
}

// Extract PacketInfo

public static void ExtractPacketInfo(BinaryStream sourceStream, Endian endian, int bufferSize,
string outputDir, bool adaptVer, StringCase strCaseForFileNames, List<RsbFileEntry> rsgList,
List<GroupInfoForRsb> rsgInfo, CompositeInfo compositeInfo)
{

for(int i = 0; i < compositeInfo.NumberOfPackets; i++)
{
uint packetIndex = compositeInfo.PacketInfo![i].PacketIndex;

int rsgInfo_Index = FindIndexForRsgInfo(rsgInfo, packetIndex);
int rsgEntry_Index = FindIndexForRsgEntry(rsgList, packetIndex);

GroupInfo.ValidateName(rsgInfo[rsgInfo_Index], rsgList[rsgEntry_Index], packetIndex);

using BinaryStream rsgStream = new();

sourceStream.Position = rsgInfo[rsgInfo_Index].GroupOffset;
FileManager.ProcessBuffer(sourceStream, rsgStream, bufferSize, rsgInfo[rsgInfo_Index].FileLength);

rsgStream.Position = 0;

var packetInfo = RsgCompressor.DecompressStream(rsgStream, bufferSize, strCaseForFileNames,
false, true, endian, adaptVer);

string pathToPacketInfo = RsgHelper.BuildInfoPath(outputDir, rsgInfo[rsgInfo_Index].GroupName, strCaseForFileNames);
packetInfo.WriteObject(pathToPacketInfo);
}

}

// Write SubGroup

public void Write(List<RsbFileEntry> fileList, List<RsbFileEntry> rsgFileList, BinaryStream compositeInfo, BinaryStream rsgInfo,
BinaryStream autoPoolInfo, BinaryStream ptxInfo, BinaryStream rsgFileBank, Endian endian, int bufferSize, bool adaptVer,
ref int rsgPacketIndex, ref uint ptxsBefore, string inputDir)
{
bool rsgComposite = false;

rsgFileList.Add( new(SubGroupName.ToUpper(), rsgPacketIndex) );

string rsgPath = RsgHelper.BuildFilePath(inputDir, SubGroupName, default);
using BinaryStream rsgFile = BinaryStream.OpenWrite(rsgPath);

RsgAnalisis.ComparePackets(PacketInfo, rsgPath, rsgFile, adaptVer);

uint ptxCount = 0;
// ProcessPacketInfo(PacketInfo, fileList, ptxInfo, ref rsgComposite, ref ptxsBefore);

// WriteCompositeInfo(compositeInfo, rsgFile, rsgName, ref rsgPacketIndex, kSecond);
GroupInfoForRsb.Write(rsgInfo, rsgFile, rsgFileBank, endian, bufferSize, SubGroupName, ref rsgPacketIndex, ptxsBefore, ptxInfo);

// WriteAutopoolInfo(autopoolInfo, rsgFile, rsgName, rsgComposite);   
}

}

}
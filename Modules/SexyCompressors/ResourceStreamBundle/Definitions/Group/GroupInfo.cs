using System;
using System.Collections.Generic;
using ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions.Composite;
using ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions.PopCapTexture;
using ZCore.Modules.SexyCompressors.ResourceStreamBundle.Exceptions;
using ZCore.Serializables.ArgumentsInfo.SexyCompressor.Rsg;

namespace ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions.Group
{
/// <summary> Represents some Info for a ResGroup that was Extracted from a RSB File </summary>

public class GroupInfo
{
/** <summary> Gets or Sets the Group Name. </summary>
<returns> The GroupName </returns> */

public string GroupName{ get; set; }

/** <summary> Gets or Sets a Boolean that Determine if the Group should be threated as a Composite one or not. </summary>
<returns> true or false </returns> */

public bool IsComposite{ get; set; }

/** <summary> Obtains or Creates a List of Entries for the SubGroups inside the Group. </summary>
<returns> The SubGroupEntries </returns> */

public List<SubGroupInfo> SubGroupEntries{ get; set; } = new();

/// <summary> Creates a new Instance of the <c>GroupInfo</c> </summary>

public GroupInfo()
{
GroupName = "My Res Group";
}

/// <summary> Creates a new Instance of the <c>GroupInfo</c> </summary>

public GroupInfo(string name, bool isComposite, List<SubGroupInfo> subGroups)
{
GroupName = name;
IsComposite = isComposite;

SubGroupEntries = subGroups;
}

// ValidateName

public static void ValidateName(GroupInfoForRsb info, RsbFileEntry entry, uint packetIndex)
{

if(!info.GroupName.Equals(entry.FullName, StringComparison.OrdinalIgnoreCase) )
throw new InvalidGroupNameException(info.GroupName, entry.FullName, packetIndex);

}

// Extract RSG from RSB

public static List<GroupInfo> ExtractRsg(BinaryStream sourceStream, Endian endian, int bufferSize,
string outputDir, bool adaptVer, StringCase strCaseForFileNames, bool extractRsgContent,
List<RsbFileEntry> fileList, List<RsbFileEntry> rsgList, List<CompositeInfo> compositeInfo,
List<RsbFileEntry> compositeList, List<GroupInfoForRsb> rsgInfo, List<PtxParamsForRsb> ptxInfo,
RsgExtractParams extractParams)
{
List<GroupInfo> groupList = new();

for(var i = 0; i < compositeInfo.Count; i++)
{
CompositeInfo.ValidateName(compositeInfo[i], compositeList[i] );

if(i == compositeInfo.Count - 1)
compositeList.Clear();

var subGroupList = SubGroupInfo.ExtractPackets(sourceStream, endian, bufferSize, outputDir, adaptVer,
strCaseForFileNames, extractRsgContent, fileList, rsgList, rsgInfo, ptxInfo, compositeInfo[i], extractParams);

groupList.Add( new(compositeInfo[i].ItemName, compositeInfo[i].IsComposite, subGroupList) );
}

rsgInfo.Clear();
rsgList.Clear();

compositeInfo.Clear();

return groupList;
}

// Extract RSG Info from RSB

public static void ExtractRsgInfo(BinaryStream sourceStream, Endian endian, int bufferSize,
string outputDir, bool adaptVer, StringCase strCaseForFileNames, List<RsbFileEntry> rsgList,
List<CompositeInfo> compositeInfo, List<RsbFileEntry> compositeList, List<GroupInfoForRsb> rsgInfo)
{

for(var i = 0; i < compositeInfo.Count; i++)
{
CompositeInfo.ValidateName(compositeInfo[i], compositeList[i] );

if(i == compositeInfo.Count - 1)
compositeList.Clear();

SubGroupInfo.ExtractPacketInfo(sourceStream, endian, bufferSize, outputDir, adaptVer,
strCaseForFileNames, rsgList, rsgInfo, compositeInfo[i] );

}

rsgInfo.Clear();
rsgList.Clear();

compositeInfo.Clear();
}

// Write GroupInfo

public void Write(List<RsbFileEntry> fileList, List<RsbFileEntry> rsgFileList, List<RsbFileEntry> compositeList,
BinaryStream compositeInfo, BinaryStream rsgInfo, BinaryStream autoPoolInfo, BinaryStream ptxInfo,
BinaryStream rsgFileBank, Endian endian, int bufferSize, bool adaptVer, ref int rsgPacketIndex,
ref uint ptxBeforeNumber, string inputDir)
{
string compositeName = IsComposite ? GroupName : GroupName + CompositeInfo.compositeSuffix;
compositeList.Add(new(compositeName.ToUpper(), compositeList.Count) );

compositeInfo.WriteString(compositeName, default, endian);
compositeInfo.WritePadding(128 - compositeName.Length);

foreach(var entry in SubGroupEntries)
entry.Write(fileList, rsgFileList, compositeInfo, rsgInfo, autoPoolInfo, ptxInfo, rsgFileBank,
endian, bufferSize, adaptVer, ref rsgPacketIndex, ref ptxBeforeNumber, inputDir);

compositeInfo.WritePadding(1024 - (SubGroupEntries.Count * 16) );
compositeInfo.WriteUInt( (uint)SubGroupEntries.Count, endian);
}

}

}
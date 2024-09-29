using System;
using System.Collections.Generic;
using ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions.Composite;
using ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions.Group;
using ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions.PopCapTexture;
using ZCore.Modules.SexyCompressors.ResourceStreamBundle.Exceptions;
using ZCore.Modules.SexyCompressors.ResourceStreamGroup.Definitions;
using ZCore.Modules.SexyCompressors.ResourceStreamGroup.Definitions.Resources;

namespace ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions.Resources
{
/// <summary> Represents Info of a Resource that was Extracted from a RSB Stream </summary>

public class ResInfo
{
/** <summary> Gets or Sets a Path to the Resource File. </summary>
<returns> The PathToResFile </returns> */

public string PathToResFile{ get; set; }

/** <summary> Gets or Sets some Info related to the PTX Image. </summary>
<returns> The ImageInfo </returns> */

public PtxInfoForRsg ImageInfo{ get; set; }

/** <summary> Gets or Sets a Set of Params that Determine how to Handle the PTX Images when Encoding/Decoding. </summary>
<returns> The ImageParams </returns> */

public PtxParams ImageParams{ get; set; }

/// <summary> Creates a new Instance of the <c>RsbResInfo</c> </summary>

public ResInfo()
{
PathToResFile = "<MUST DEFINE A PATH TO RES FILE>";
}

/// <summary> Creates a new Instance of the <c>RsbResInfo</c> </summary>

public ResInfo(string resPath)
{
PathToResFile = resPath;
}

/// <summary> Creates a new Instance of the <c>RsbResInfo</c> </summary>

public ResInfo(string resPath, PtxInfoForRsg ptxInfo, PtxParams ptxParams)
{
PathToResFile = resPath;

ImageInfo = ptxInfo;
ImageParams = ptxParams;
}

// Validate ResInfo

private static void Validate(RsgPacketInfo packetInfo, RsbFileEntry file, CompositeInfo compositeInfo,
List<PtxParamsForRsb> ptxInfo, int ptxsBefore, ref ResInfo resInfo)
{
bool packetExists = false;

for(int i = 0; i < packetInfo.ResInfo.Count; i++)
{
var res = packetInfo.ResInfo[i];

if(res.PathToResFile.Equals(file.FullName, StringComparison.OrdinalIgnoreCase) )
{
packetExists = true;

if(file.FullName.EndsWith(".ptx", StringComparison.OrdinalIgnoreCase) && compositeInfo.IsComposite)
{
PtxInfoForRsg.Validate(ptxInfo[ptxsBefore + res.ImageInfo!.TextureID], res.ImageInfo, file.FullName);
resInfo.ImageInfo = res.ImageInfo;

var format = ptxInfo[ptxsBefore + res.ImageInfo!.TextureID].TextureFormat;
var pitch = ptxInfo[ptxsBefore + res.ImageInfo!.TextureID].TexturePitch;

var aSize = ptxInfo[ptxsBefore + res.ImageInfo!.TextureID]?.AlphaSize;
var aChannel = ptxInfo[ptxsBefore + res.ImageInfo!.TextureID]?.AlphaChannel;

resInfo.ImageParams = new(format, pitch, aSize, aChannel);
}

break;
}

}

ptxInfo.Clear();
	
if(!packetExists)
throw new MissingPacketException(file.FullName);

}

// Extract ResInfo

public static List<ResInfo> Extract(List<RsbFileEntry> fileList, List<GroupInfoForRsb> rsgInfo,
CompositeInfo compositeInfo, RsgPacketInfo packetInfo, List<PtxParamsForRsb> ptxInfo,
int rsgIndex, uint packetIndex)
{
List<ResInfo> resInfo = new();

int ptxsBefore = (int)rsgInfo[rsgIndex].NumberOfPtxFilesBeforeGroup;

for(int i = 0; i < fileList.Count; i++)
{
RsbFileEntry file = fileList[i];

if(file.PoolIndex == packetIndex)
{
ResInfo singleInfo = new(file.FullName);

Validate(packetInfo, file, compositeInfo, ptxInfo, ptxsBefore, ref singleInfo);

resInfo.Add(singleInfo);
}

if(file.PoolIndex > packetIndex)
break;

}

fileList.Clear();

return resInfo;
}

}

}
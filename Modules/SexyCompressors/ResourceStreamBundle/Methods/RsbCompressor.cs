using System.Collections.Generic;
using System.IO;
using System.Linq;
using ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions;
using ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions.Composite;
using ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions.Description;
using ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions.Group;
using ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions.PopCapTexture;
using ZCore.Modules.SexyCompressors.ResourceStreamBundle.Exceptions;
using ZCore.Serializables.ArgumentsInfo.FileManager.Metadata;
using ZCore.Serializables.ArgumentsInfo.SexyCompressor.Rsb;
using ZCore.Serializables.ArgumentsInfo.SexyCompressor.Rsg;

namespace ZCore.Modules.SexyCompressors.ResourceStreamBundle.Methods
{
/// <summary> Initializes Compression Tasks for RSB Files which are used in some PvZ Games. </summary>

public static class RsbCompressor
{
/// <summary> The RSB Extension </summary>

private const string RsbExt = ".rsb";

/** <summary> The Header of a RSB File. </summary>

<remarks> It Occupies 4 Bytes in the Stream, which are: <c>0x31 0x62 0x73 0x72</c>, written in LittleEndian </remarks> */

private const string RsbHeader = "rsb1";

// Get RSB Stream 

// TO DO

/** <summary> Compresses the Content of a RSB Folder, such as Resources and Textures. </summary>

<param name = "inputPath"> The Path where the Folder to Compress is Located. </param>
<param name = "outputPath"> The Location where the Compressed File will be Saved. </param> */

// TO DO

// Decompress RSB Stream

public static ManifestInfo DecompressStream(BinaryStream input, string outputPath, int bufferSize,
RsgExtractParams extractParamsForRsg, Endian endian = default, bool adaptVer = true,
StringCase strCaseForFileNames = default, RsbExtractParams extractParams = null,
MetadataExportParams exportCfg = null)
{
extractParams ??= new();
exportCfg ??= new();	

PathHelper.ChangeExtension(ref outputPath, ".res_bundle");
DirManager.CheckMissingFolder(outputPath);

input.CompareString<InvalidRsbException>(RsbHeader, default, Endian.LittleEndian);

RsbInfo fileInfo = RsbInfo.Read(input, endian, adaptVer);
string auxPath = string.Empty;

if(exportCfg.ExportMetadataFromFiles)
{
auxPath = outputPath + Path.DirectorySeparatorChar + "RsbInfo.json";
fileInfo.WriteObject(auxPath);
}

#region ======= Read RSB Entries =======

var fileList = RsbFileEntry.GetListForUnpacking(input, endian, fileInfo.OffsetToFilesList, fileInfo.NumberOfFiles);

if(extractParams.ExtractFileList && fileList.Count > 0)
{
auxPath = outputPath + Path.DirectorySeparatorChar + "FileList.json";
File.WriteAllText(auxPath, JsonSerializer.SerializeObject(fileList) );
}

var rsgList = RsbFileEntry.GetListForUnpacking(input, endian, fileInfo.OffsetToGroupsList, fileInfo.GroupsListSize);

if(extractParams.ExtractRsgList && rsgList.Count > 0)
{
auxPath = outputPath + Path.DirectorySeparatorChar + "RsgList.json";
File.WriteAllText(auxPath, JsonSerializer.SerializeObject(rsgList) );
}

var compositeInfo = CompositeInfo.ReadList(input, endian, fileInfo.OffsetToCompositeInfo,
fileInfo.NumberOfCompositeItems, fileInfo.SizeForCompositeInfo);

if(extractParams.ExtractCompositeInfo && compositeInfo.Count > 0)
{
auxPath = outputPath + Path.DirectorySeparatorChar + "CompositeInfo" + Path.DirectorySeparatorChar;
DirManager.CheckMissingFolder(auxPath);

foreach(var info in compositeInfo)
File.WriteAllText(auxPath + $"{info.ItemName}.json", JsonSerializer.SerializeObject(info) );

}

var compositeList = RsbFileEntry.GetListForUnpacking(input, endian, fileInfo.OffsetToCompositeList, fileInfo.CompositeListSize);

if(extractParams.ExtractCompositeList && compositeList.Count > 0)
{
auxPath = outputPath + Path.DirectorySeparatorChar + "CompositeList.json";
File.WriteAllText(auxPath, JsonSerializer.SerializeObject(compositeList) );
}

var rsgInfo = GroupInfoForRsb.ReadList(input, endian, fileInfo.OffsetToGroupsInfo, 
fileInfo.NumberOfGroups, fileInfo.SizeForGroupsInfo);

if(extractParams.ExtractRsgInfo && rsgInfo.Count > 0)
{
auxPath = outputPath + Path.DirectorySeparatorChar + "GroupInfo" + Path.DirectorySeparatorChar;
DirManager.CheckMissingFolder(auxPath);

foreach(var info in rsgInfo)
File.WriteAllText(auxPath + $"{info.GroupName}.json", JsonSerializer.SerializeObject(info) );

}

var autoPoolInfo = AutoPoolInfo.Read(input, endian, fileInfo.OffsetToAutoPoolInfo,
fileInfo.NumberOfAutoPools, fileInfo.SizeForAutoPoolInfo);

if(extractParams.ExtractAutoPoolInfo && autoPoolInfo.Count > 0)
{
auxPath = outputPath + Path.DirectorySeparatorChar + "AutoPoolInfo" + Path.DirectorySeparatorChar;
DirManager.CheckMissingFolder(auxPath);

foreach(var info in autoPoolInfo)
File.WriteAllText(auxPath + $"{info.PoolName}.json", JsonSerializer.SerializeObject(info) );

}

autoPoolInfo.Clear();

var ptxInfo = PtxParamsForRsb.Read(input, endian, fileInfo.OffsetToPtxInfo,
fileInfo.SizeForPtxInfo, fileInfo.NumberOfPtxFiles);

if(extractParams.ExtractPtxInfo && ptxInfo.Count > 0)
{
auxPath = outputPath + Path.DirectorySeparatorChar + "PtxInfo.json";
File.WriteAllText(auxPath, JsonSerializer.SerializeObject(ptxInfo) );
}

#endregion

if(fileInfo.FileVersion == 3)
{
var resDesc = ResDescription.Read(input, endian, fileInfo.OffsetToPart1, fileInfo.OffsetToPart2, fileInfo.OffsetToPart3);

resDesc.WriteObject(outputPath + Path.DirectorySeparatorChar + "ResDescription.json");
}

List<GroupInfo> groupList = null;

if(extractParams.ExtractRsgFiles)
groupList = GroupInfo.ExtractRsg(input, endian, bufferSize, outputPath, adaptVer, strCaseForFileNames,
extractParams.ExtractContentFromRsgFiles, fileList, rsgList, compositeInfo, compositeList, rsgInfo, ptxInfo,
extractParamsForRsg);

else if(!extractParams.ExtractRsgFiles && extractParams.ExtractInfoFromRsgPackets)
{
fileList.Clear();
ptxInfo.Clear();

GroupInfo.ExtractRsgInfo(input, endian, bufferSize, outputPath, adaptVer, strCaseForFileNames,
rsgList, compositeInfo, compositeList, rsgInfo);
}

else
{
fileList.Clear();
rsgList.Clear();

compositeInfo.Clear();
compositeList.Clear();

ptxInfo.Clear();
}

var rsgNames = rsgInfo.Select(rsg => rsg.GroupName).ToList();
rsgInfo.Clear();

ManifestInfo manifestInfo = new(fileInfo.FileVersion, fileInfo.NumberOfPtxFiles,
new(rsgNames, $"{outputPath}\\Packets"), groupList);

return manifestInfo;
}

/** <summary> Decompresses the Content of an RSB File, such as Resources and Textures. </summary>

<param name = "inputPath" > The Path to the RSB File to Decompress. </param>
<param name = "outputPath" > The Path where the Decompressed Contents will be Saved (this must be a Folder). </param> */

public static void UnpackFile(string inputPath, string outputPath, int bufferSize,
RsgExtractParams extractParamsForRsg, Endian endian = default, bool adaptVer = true,
StringCase strCaseForFileNames = default, RsbExtractParams extractParams = null,
MetadataExportParams exportCfg = null)
{
using BinaryStream inputFile = BinaryStream.Open(inputPath);

PathHelper.ChangeExtension(ref outputPath, ".res_bundle");

var manifestInfo = DecompressStream(inputFile, outputPath, bufferSize, extractParamsForRsg,
endian, adaptVer, strCaseForFileNames, extractParams, exportCfg);

manifestInfo.WriteObject(outputPath + Path.DirectorySeparatorChar + "ManifestInfo.json");
}

}

}
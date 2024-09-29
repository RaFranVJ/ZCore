using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using ZCore.Modules.FileCompressors;
using ZCore.Modules.SexyCompressors.ResourceStreamGroup.Definitions;
using ZCore.Modules.SexyCompressors.ResourceStreamGroup.Definitions.FilePath;
using ZCore.Modules.SexyCompressors.ResourceStreamGroup.Definitions.Part;
using ZCore.Modules.SexyCompressors.ResourceStreamGroup.Definitions.Resources;
using ZCore.Modules.SexyCompressors.ResourceStreamGroup.Exceptions;
using ZCore.Serializables.ArgumentsInfo.SexyCompressor.Rsg;

namespace ZCore.Modules.SexyCompressors.ResourceStreamGroup.Methods
{
/// <summary> Initializes Compression Tasks for RSG Files which are used in some PvZ Games. </summary>

public static class RsgCompressor
{
/** <summary> The Header of a RSG File. </summary>

<remarks> It Occupies 4 Bytes in the Stream, which are: <c>0x70 0x67 0x73 0x72</c>, written in LittleEndian </remarks> */

private const string RsgHeader = "rsgp";

// Part0 Entries

public static List<Part0_Info> Part0_Entries = new();

// Part1 Entries

public static List<Part1_Info> Part1_Entries = new();

// Write RSG Data

private static void WriteResData(BinaryStream sourceStream, Stream dataStream, Endian endian,
int bufferSize, uint flags, bool isAtlas)
{
uint part0_Offset = (uint)sourceStream.Position;
uint part0_Size = (uint)dataStream.Length;

long backupOffset;
     
if(flags < 2)
{
FileManager.ProcessBuffer(dataStream, sourceStream, bufferSize);

backupOffset = sourceStream.Position;

sourceStream.Position = 0x18;
sourceStream.WriteUInt(part0_Offset, endian);
			
sourceStream.WriteUInt(part0_Size, endian);
			
if(isAtlas)
sourceStream.WriteUInt(0);

else
sourceStream.WriteUInt(part0_Size, endian);
			
sourceStream.Position = backupOffset;
}

else
{
using MemoryStream compressedStream = new();

int compressionLevel = flags == 3 ? Deflater.BEST_COMPRESSION : Deflater.DEFAULT_COMPRESSION;

using DeflaterOutputStream deflaterStream = new(compressedStream, new(compressionLevel), bufferSize);
deflaterStream.IsStreamOwner = false; // Don't Close baseStream

FileManager.ProcessBuffer(dataStream, deflaterStream, bufferSize);
int compressedSize = (int)compressedStream.Length;

compressedStream.Position = 0;
FileManager.ProcessBuffer(compressedStream, sourceStream, bufferSize);

int bytesToAppend = RsgHelper.BeautifyLength(compressedSize);
sourceStream.WritePadding(bytesToAppend);

uint part0_SizeCompressed = (uint)(compressedSize + bytesToAppend);

backupOffset = sourceStream.Position;

sourceStream.Position = 0x18;
sourceStream.WriteUInt(part0_Offset, endian);

sourceStream.WriteUInt(part0_SizeCompressed, endian);
sourceStream.WriteUInt(part0_Size, endian);

sourceStream.Position = backupOffset;
}

}
	
// Compress RSG Stream

private static void CompressData(BinaryStream sourceStream, BinaryStream dataGroup, BinaryStream atlasGroup,
Endian endian, int bufferSize, uint compressionFlags)
{

if(dataGroup.Length != 0)
{
dataGroup.Position = 0; 
WriteResData(sourceStream, dataGroup, endian, bufferSize, compressionFlags, false);

dataGroup.Close(); // Close Stream
}
 
if(atlasGroup.Length != 0)
{
uint part1_Offset;
uint part1_Size = (uint)atlasGroup.Length;

using BinaryStream emptyStream = new();
emptyStream.WriteUInt(252536, endian);

emptyStream.WriteUInt(1, Endian.BigEndian);
emptyStream.WritePadding(4088);

if(compressionFlags == 0 || compressionFlags == 2)
{

if(compressionFlags == 2 && dataGroup.Length == 0)
WriteResData(sourceStream, emptyStream, endian, bufferSize, 1, true);
      
else
WriteResData(sourceStream, Stream.Null, endian, bufferSize, 1, true);
                
part1_Offset = (uint)sourceStream.Position;

atlasGroup.Position = 0;
FileManager.ProcessBuffer(atlasGroup, sourceStream, bufferSize);

atlasGroup.Close(); // Close Stream
long backupOffset = sourceStream.Position;

sourceStream.Position = 0x28;
sourceStream.WriteUInt(part1_Offset, endian);

sourceStream.WriteUInt(part1_Size, endian);
sourceStream.WriteUInt(part1_Size, endian);

sourceStream.Position = backupOffset;
}

else
{

if(compressionFlags == 3 && dataGroup.Length == 0)
WriteResData(sourceStream, emptyStream, endian, bufferSize, 1, true);
                    
else
WriteResData(sourceStream, Stream.Null, endian, bufferSize, 1, true);

part1_Offset = (uint)sourceStream.Position;

using MemoryStream compressedStream = new();
       
var compressionLevel = compressionFlags == 3 ? CompressionLevel.Optimal : CompressionLevel.Fastest;
atlasGroup.Position = 0;

using DeflateStream deflateStream = DeflateCompressor.CompressStream(atlasGroup, compressedStream,
compressionLevel, bufferSize);

atlasGroup.Close(); // Close Stream
int compressedSize = (int)compressedStream.Length;

compressedStream.Position = 0;
FileManager.ProcessBuffer(compressedStream, sourceStream, bufferSize);

int bytesToAppend = RsgHelper.BeautifyLength(compressedSize);
sourceStream.WritePadding(bytesToAppend);
                    
uint part1_SizeCompressed = (uint)(compressedSize + bytesToAppend);

long backupOffset2 = sourceStream.Position;

sourceStream.Position = 0x28;
sourceStream.WriteUInt(part1_Offset, endian);

sourceStream.WriteUInt(part1_SizeCompressed, endian);
sourceStream.WriteUInt(part1_Size, endian);

sourceStream.Position = backupOffset2;
}  

}

else
{
sourceStream.Position = 0x28;

sourceStream.WriteUInt( (uint)sourceStream.Length);
}

}

// Write RSG File

private static void WriteFile(BinaryStream sourceStream, List<TempRsgPath> temporalPaths, uint compressionFlags,
string inputPath, int bufferSize, Endian endian = default, bool useResDir = true)
{
long fileListBeginOffset = sourceStream.Position;
	
if(fileListBeginOffset != 92) 
throw new FileList_InvalidOffsetException(fileListBeginOffset, 92);

using BinaryStream dataGroup = new();
using BinaryStream atlasGroup = new();

uint dataPos = 0;
uint atlasPos = 0;

for(int i = 0; i < temporalPaths.Count; i++)
{
long beginOffset = sourceStream.Position;
var resInfo = temporalPaths[i].FileInfo;

sourceStream.WriteStringAsFourBytes(temporalPaths[i].PathToSlice);

long backupOffset = sourceStream.Position;

for (var h = 0; h < temporalPaths[i].PosInfo.Count; h++)
{ 
sourceStream.Position = beginOffset + temporalPaths[i].PosInfo[h].PathOffset * 4 + 1;

sourceStream.WriteUTripleByte(temporalPaths[i].PosInfo[h].PathPosition, endian);
}

string resPath = RsgHelper.BuildResPath(inputPath, resInfo.PathToResFile, useResDir, default);

using BinaryStream resStream = BinaryStream.Open(resPath);

int bytesToAppend = RsgHelper.BeautifyLength( (int)resStream.Length, true);

if(temporalPaths[i].IsAtlasImage)
{
FileManager.ProcessBuffer(resStream, atlasGroup, bufferSize);

atlasGroup.WritePadding(bytesToAppend);

sourceStream.Position = backupOffset;
sourceStream.WriteUInt(1, endian);

sourceStream.WriteUInt(atlasPos, endian);
sourceStream.WriteUInt( (uint)resStream.Length, endian);

sourceStream.WriteInt(resInfo.ImageInfo!.TextureID);
sourceStream.WritePadding(8);

sourceStream.WriteInt(resInfo.ImageInfo!.TextureWidth);
sourceStream.WriteInt(resInfo.ImageInfo!.TextureHeight);
 
atlasPos += (uint)(resStream.Length + bytesToAppend);
}

else
{
FileManager.ProcessBuffer(resStream, dataGroup, bufferSize);

dataGroup.WritePadding(bytesToAppend);

sourceStream.Position = backupOffset;
sourceStream.WriteUInt(0);

sourceStream.WriteUInt(dataPos, endian);
sourceStream.WriteUInt( (uint)resStream.Length, endian);
					
dataPos += (uint)(resStream.Length + bytesToAppend);
}

}

var fileListLength = sourceStream.Position - fileListBeginOffset;

int padding = RsgHelper.BeautifyLength( (int)sourceStream.Position);
sourceStream.WritePadding(padding);

long backupOffset2 = sourceStream.Position;

sourceStream.Position = 0x14;
sourceStream.WriteUInt( (uint)backupOffset2, endian);

sourceStream.Position = 0x48;
sourceStream.WriteUInt( (uint)fileListLength, endian);

sourceStream.WriteUInt( (uint)fileListBeginOffset, endian);

sourceStream.Position = backupOffset2;

CompressData(sourceStream, dataGroup, atlasGroup, endian, bufferSize, compressionFlags);
}
		
// Get RSG Stream

public static BinaryStream CompressStream(string inputPath, RsgPacketInfo packetInfo, int bufferSize,
bool useResDir, Endian endian = default, string outputPath = null)
{
PathHelper.ChangeExtension(ref outputPath, RsgHelper.FileExt);

if(packetInfo == null)
throw new MissingPacketInfoException(outputPath);

BinaryStream rsgStream = string.IsNullOrEmpty(outputPath) ? new() : BinaryStream.OpenWrite(outputPath);

rsgStream.WriteString(RsgHeader, default, Endian.LittleEndian);
rsgStream.WriteUInt(packetInfo.PacketVersion, endian);

rsgStream.WritePadding(8);
rsgStream.WriteUInt(packetInfo.CompressionFlags, endian);

rsgStream.WritePadding(72);

var temporalPaths = TempRsgPath.GetListForPacking(packetInfo.ResInfo);
WriteFile(rsgStream, temporalPaths, packetInfo.CompressionFlags, inputPath, bufferSize, endian, useResDir);

return rsgStream;
}

/** <summary> Compresses the Content of an RSG Folder, such as Resources and Textures. </summary>

<param name = "inputPath"> The Path where the Folder to Compress is Located. </param>
<param name = "outputPath"> The Location where the Compressed File will be Saved. </param> */

public static void PackDir(string inputPath, string outputPath, int bufferSize, Endian endian = default)
{
RsgPacketInfo packetInfo = new RsgPacketInfo().ReadObject(inputPath + Path.DirectorySeparatorChar + "PacketInfo.json");

using BinaryStream outputFile = CompressStream(inputPath, packetInfo, bufferSize, true, endian, outputPath);
}

// Decompress Res from RSG and Get its Info
		
public static RsgPacketInfo DecompressStream(BinaryStream targetStream, int bufferSize, StringCase strCaseForResName,
bool useResDir, bool getPacketInfoOnly, Endian endian = default, bool adaptVer = true, string outputPath = "",
RsgExtractParams extractParams = null)
{
extractParams ??= new();

PathHelper.ChangeExtension(ref outputPath, ".packet");

targetStream.CompareString<InvalidRsgException>(RsgHeader, default, Endian.LittleEndian);

Part0_Entries.Clear();
Part1_Entries.Clear();

RsgInfo fileInfo = RsgInfo.Read(targetStream, endian, adaptVer);
TempRsgPath.InitListForUnpacking(targetStream, endian, fileInfo.OffsetToFilesList, fileInfo.NumberOfFiles);

List<ResInfo> resInfo = new();
BinaryStream fileData;

BinaryStream part0_RawData;
BinaryStream part1_RawData;

string auxPath;

if(!string.IsNullOrEmpty(outputPath) )
DirManager.CheckMissingFolder(outputPath);

if(Part0_Entries.Count > 0)
{

if(extractParams.ExtractEntriesFromPart0 && useResDir)
{
auxPath = outputPath + Path.DirectorySeparatorChar + "Part0.json";
File.WriteAllText(auxPath, JsonSerializer.SerializeObject(Part0_Entries) );
}

part0_RawData = getPacketInfoOnly ? new() : RsgAnalisis.GetStream(targetStream, fileInfo, false, bufferSize);

for(int i = 0; i < Part0_Entries.Count; i++)
{

if(!getPacketInfoOnly)
{
string resPath = RsgHelper.BuildResPath(outputPath, Part0_Entries[i].FilePath, useResDir, strCaseForResName);
fileData = BinaryStream.OpenWrite(resPath);

part0_RawData.Position = Part0_Entries[i].FileOffset;
FileManager.ProcessBuffer(part0_RawData, fileData, bufferSize, (int)Part0_Entries[i].FileSize);			
}

resInfo.Add( new(Part0_Entries[i].FilePath) );
}

part0_RawData.Close();
}

if(Part1_Entries.Count > 0)
{

if(extractParams.ExtractEntriesFromPart1 && useResDir)
{
auxPath = outputPath + Path.DirectorySeparatorChar + "Part1.json";
File.WriteAllText(auxPath, JsonSerializer.SerializeObject(Part1_Entries) );
}

part1_RawData = getPacketInfoOnly ? new() : RsgAnalisis.GetStream(targetStream, fileInfo, true, bufferSize);

for(int i = 0; i < Part1_Entries.Count; i++)
{

if(!getPacketInfoOnly)
{
string resPath = RsgHelper.BuildResPath(outputPath, Part1_Entries[i].FilePath, useResDir, strCaseForResName);
fileData = BinaryStream.OpenWrite(resPath);

part1_RawData.Position = Part1_Entries[i].FileOffset;
FileManager.ProcessBuffer(part1_RawData, fileData, bufferSize, (int)Part1_Entries[i].FileSize);
}

PtxInfoForRsg ptxInfo = new(Part1_Entries[i].TextureID, Part1_Entries[i].TextureWidth, Part1_Entries[i].TextureHeight);
			
resInfo.Add( new(Part1_Entries[i].FilePath, ptxInfo) );
}

part1_RawData.Close();
}

if(!getPacketInfoOnly)
targetStream.Close();

return new(fileInfo.FileVersion, fileInfo.CompressionFlags, resInfo);
}

// Unpack File

public static void UnpackFile(string inputPath, string outputPath, int bufferSize,
RsgExtractParams extractParams, StringCase strCaseForResNames = default, Endian endian = default, 
bool adaptVer = true)
{
using BinaryStream inputFile = BinaryStream.Open(inputPath);
PathHelper.ChangeExtension(ref outputPath, ".packet");

var packetInfo = DecompressStream(inputFile, bufferSize, strCaseForResNames, true,
false, endian, adaptVer, outputPath, extractParams);

packetInfo.WriteObject(outputPath + Path.DirectorySeparatorChar + "PacketInfo.json");
}

}

}
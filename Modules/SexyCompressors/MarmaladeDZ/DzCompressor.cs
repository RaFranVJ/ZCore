using System.Collections.Generic;
using System.IO;
using System.Text;
using ZCore.Modules.SexyCompressors.MarmaladeDZ.Definitions;
using ZCore.Modules.SexyCompressors.MarmaladeDZ.Exceptions;
using ZCore.Serializables.ArgumentsInfo.FileManager;
using ZCore.Serializables.ArgumentsInfo.FileManager.Archive;
using ZCore.Serializables.ArgumentsInfo.FileManager.Metadata;
using ZCore.Serializables.ArgumentsInfo.SexyCompressor.Dz;

namespace ZCore.Modules.SexyCompressors.MarmaladeDZ
{
/// <summary> Initializes Compression Tasks for DTRZ Files. </summary>

public static class DzCompressor
{
/// <summary> The Dz Extension </summary>

private const string DzExt = ".dz";

/** <summary> The Header of a DTRZ File. </summary>

<remarks> It Occupies 4 Bytes in the Stream, which are: <c>0x44 0x54 0x52 0x5A</c> </remarks> */

private const string DtrzHeader = "DTRZ";

// Get DTRZ Stream

public static BinaryStream CompressStream(string dirName, Endian endian = default, Encoding encoding = default,
ushort maxChunkIndex = 0xFFFF, Dictionary<string, CompressionFlags> mappedExts = null,
FileVersionDetails<byte> versionInfo = null, DzEntryParams entryParams = null, string outputPath = null, 
string pathToDzInfo = null, params string[] entryNames)
{

if(entryNames == null || entryNames.Length == 0)
throw new EmptyDzDirException();

mappedExts ??= new();
versionInfo ??= new();

entryParams ??= new();

PathHelper.AddExtension(ref outputPath, DzExt);

BinaryStream dzStream = string.IsNullOrEmpty(outputPath) ? new() : BinaryStream.OpenWrite(outputPath);

string[] fileNames = new string[entryNames.Length];
string[] pathNames = new string[entryNames.Length];

var dirPool = DzHelper.GetDirPool(dirName, fileNames, pathNames, entryNames);
				
DtrzInfo dzInfo; 

if(string.IsNullOrEmpty(pathToDzInfo) )
{
dzInfo = new(entryNames.Length, dirPool.Length, fileNames);

dzInfo.UpdateAll(dirPool, pathNames, entryNames);
}

else
dzInfo = new DtrzInfo().ReadObject(pathToDzInfo);

dzStream.WriteString(DtrzHeader, default, endian);
dzInfo.WritePart1(dzStream, endian, encoding, versionInfo, maxChunkIndex);

long backupOffset = dzStream.Position;
dzStream.Position += entryNames.Length << 4;

// Add all the Entries into the Stream

for(int i = 0; i < entryNames.Length; i++)
DzHelper.AddDzEntry(dzStream, mappedExts, entryNames[i], dzInfo.Chunks[i], entryParams);
  		
dzStream.Position = backupOffset;
dzInfo.WritePart2(dzStream, endian);

return dzStream;
}

/** <summary> Compresses the Content of a DTRZ Folder, such as Resources and Textures. </summary>

<param name = "inputPath"> The Path where the Folder to Compress is Located. </param>
<param name = "outputPath"> The Location where the Compressed File will be Saved. </param> */

public static void PackDir(string inputPath, string outputPath, string encodingStr,
Endian endian = default, ushort maxChunkIndex = 0xFFFF, FileSystemSearchParams resFilter = null,
Dictionary<string, CompressionFlags> mappedExts = null, FileVersionDetails<byte> versionInfo = null, 
DzEntryParams entryParams = null, MetadataImportParams importCfg = null)
{
resFilter ??= new();
importCfg ??= new();

var encoding = EncodeHelper.GetEncodingType(encodingStr);
string[] filesList = DirManager.GetEntryNames(inputPath, resFilter);

string pathToDzInfo = importCfg.ImportMetadataToFiles ? DtrzInfo.ResolvePath(inputPath, importCfg) : null;

using BinaryStream outputFile = CompressStream(inputPath, endian, encoding, maxChunkIndex,
mappedExts, versionInfo, entryParams, outputPath, pathToDzInfo, filesList); 
     
}

/** <summary> Decompresses the Content of a DTRZ File, such as Resources and Textures. </summary>

<param name = "inputPath"> The Path to the File to Decompress. </param>
<param name = "outputPath"> The Location where the Compressed Content will be Saved (must be a Dir). </param> */

public static void UnpackFile(string inputPath, string outputPath, string encodingStr, Endian endian = default,
bool adaptVer = true, ushort maxChunkIndex = 0xFFFF, DzEntryParams entryParams = null, MetadataExportParams exportCfg = null)
{
entryParams ??= new();
exportCfg ??= new();

var encoding = EncodeHelper.GetEncodingType(encodingStr);
using IDisposablePool pool = new();

using BinaryStream dzStream = BinaryStream.Open(inputPath);
dzStream.CompareString<InvalidDzException>(DtrzHeader, default, endian);

DtrzInfo dzInfo = DtrzInfo.ReadAllParts(dzStream, endian, encoding, adaptVer, maxChunkIndex);
BinaryStream[] streamsList = new BinaryStream[dzInfo.FilesCount];

for(int i = 0; i < dzInfo.FilesCount; i++)
{
streamsList[i] = dzInfo.ChunkNames[i] == null ? dzStream : 
pool.Add( BinaryStream.OpenWrite(Path.GetDirectoryName(inputPath) + Path.DirectorySeparatorChar + dzInfo.ChunkNames[i]) );
}

string entryName;

for(int i = 0; i < dzInfo.ChunksCount; i++)
{
entryName = outputPath + Path.DirectorySeparatorChar + dzInfo.DirNames[dzInfo.Chunks[i].DirNameIndex];
entryName = entryName + Path.DirectorySeparatorChar + dzInfo.FileNames[dzInfo.Chunks[i].FileNameIndex];

if(dzInfo.Chunks[i].MultiIndex != 0)
{
string fileExt = Path.GetExtension(entryName);
entryName = $"{entryName[..^fileExt.Length]}_multi_{dzInfo.Chunks[i].MultiIndex}{fileExt}";
}

DzHelper.ExtractDzEntry(streamsList[dzInfo.Chunks[i].FileIndex], entryName, dzInfo.Chunks[i], entryParams);
}

if(exportCfg.ExportMetadataFromFiles)
dzInfo.WriteObject(DtrzInfo.ResolvePath(inputPath, exportCfg) );

}

}

}
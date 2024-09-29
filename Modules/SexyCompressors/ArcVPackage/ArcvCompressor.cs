using System.IO;
using ZCore.Modules.SexyCompressors.ArcVPackage.Definitions;
using ZCore.Modules.SexyCompressors.ArcVPackage.Exceptions;
using ZCore.Serializables.ArgumentsInfo.FileManager;
using ZCore.Serializables.ArgumentsInfo.FileManager.Metadata;
using ZCore.Serializables.ArgumentsInfo.SexyCompressor.ArcV;

namespace ZCore.Modules.SexyCompressors.ArcVPackage
{
/// <summary> Initializes Compression Tasks for ARCV Files which are used on NDS ROMS. </summary>

public static class ArcvCompressor
{
/// <summary> The ARCV Extension </summary>

private const string ArcvExt = ".arcv";

/** <summary> The Header of an ARCV File. </summary>

<remarks> It Occupies 4 Bytes in the Stream, which are: <c>0x41 0x52 0x43 0x56</c> </remarks> */

private const string ArcvHeader = "ARCV";

// Apply Padding to the ARCV Stresm

public static void PadStream(BinaryStream targetStream, int identation = 4, byte padding = 0xAC)
{
int offsetDiff = (int)(targetStream.Position % identation);

// Apply Padding if the Offset is not Aliggned
			
if(offsetDiff != 0)
{

for(int i = identation - offsetDiff; i > 0; i--)
targetStream.WriteByte(padding);

}

}

// Get ARCV Stream

public static BinaryStream CompressStream(int bufferSize, Endian endian = default, ArcvStyleInfo styleInfo = null,
string outputPath = null, string pathToResInfo = null, params string[] entryNames)
{
	
if(entryNames == null || entryNames.Length == 0)
throw new EmptyArcvDirException();
	
styleInfo ??= new();

PathHelper.AddExtension(ref outputPath, ArcvExt);
BinaryStream arcvStream = string.IsNullOrEmpty(outputPath) ? new() : BinaryStream.OpenWrite(outputPath);

arcvStream.WriteString(ArcvHeader, default, endian);
arcvStream.WriteInt(entryNames.Length, endian);

arcvStream.Position += styleInfo.BytesIdentation; // Bytes Identation is 4
arcvStream.Position += entryNames.Length * styleInfo.SizeDiffBetweenFiles; // SizeDiff is 12

var resInfo = string.IsNullOrEmpty(pathToResInfo) ?
ArcvInfoContainer.GetResInfoFromJson(arcvStream, bufferSize, styleInfo, pathToResInfo, entryNames) :
ArcvInfoContainer.GetResInfoFromDir(arcvStream, bufferSize, styleInfo, entryNames);

arcvStream.Position = styleInfo.BytesIdentation * 2; // Skip 8 Bytes
arcvStream.WriteInt( (int)arcvStream.Length, endian);

// Iterate bewteen each ResInfo and Write the Details into the Stream

for(int i = 0; i < resInfo.Count; i++)
resInfo[i].Write(arcvStream, endian);

return arcvStream;
}

/** <summary> Compresses the Content of an ARCV Folder, such as Resources and Textures. </summary>

<param name = "inputPath"> The Path where the Folder to Compress is Located. </param>
<param name = "outputPath"> The Location where the Compressed File will be Saved. </param> */

public static void PackDir(string inputPath, string outputPath, int bufferSize, Endian endian = default,
FileSystemSearchParams resFilter = null, ArcvStyleInfo styleInfo = null, MetadataImportParams importCfg = null)
{
resFilter ??= new();
importCfg ??= new();

string[] filesList = DirManager.GetEntryNames(inputPath, resFilter);
string pathToResInfo = importCfg.ImportMetadataToFiles ? ArcvInfoContainer.ResolvePath(inputPath, importCfg) : null;

using BinaryStream outputFile = CompressStream(bufferSize, endian, styleInfo, outputPath, pathToResInfo, filesList);
}

/** <summary> Decompresses the Content of an ARCV File, such as Resources and Textures. </summary>

<param name = "inputPath" > The Access Path where the ARCV File to be Decompressed is Located. </param>
<param name = "outputPath" > The Path where the Decompressed Contents will be Saved (this must be a Folder). </param> */

public static void UnpackFile(string inputPath, string outputPath, int bufferSize, Endian endian = default,
bool formatBinNames = true, string workspaceName = "NDS", ArcvStyleInfo styleInfo = null, 
MetadataExportParams exportCfg = null)
{
styleInfo ??= new();
exportCfg ??= new();	

DirManager.CheckMissingFolder(outputPath);
outputPath = outputPath + Path.DirectorySeparatorChar + workspaceName;

using BinaryStream inputFile = BinaryStream.Open(inputPath);
inputFile.CompareString<InvalidArcvException>(ArcvHeader, default, endian);

int resCount = inputFile.ReadInt(endian);
inputFile.Position += styleInfo.BytesIdentation; // Skip 4 Bytes

ArcvInfoContainer infoContainer = new();

// Extract all the Resources found

for(int i = 0; i < resCount; i++)
{
ArcvResInfo fileInfo = ArcvResInfo.Read(inputFile, endian);

if(exportCfg.ExportMetadataFromFiles)
infoContainer.ResInfo.Add(fileInfo);

string innerExt = inputFile.PeekString( (int)styleInfo.BytesIdentation); // Read 4 Bytes for the Extension

string resPath = formatBinNames ?
outputPath + ArcvHelper.BuildResNameAsBin(fileInfo.CRC32, innerExt) :
outputPath + ArcvHelper.BuildResName(fileInfo.CRC32, innerExt);

ArcvInfoContainer.ExtractResource(inputFile, resPath, fileInfo.FileOffset, fileInfo.FileSize, bufferSize);
}

if(exportCfg.ExportMetadataFromFiles)
infoContainer.WriteObject(ArcvInfoContainer.ResolvePath(inputPath, exportCfg) );

}

}

}
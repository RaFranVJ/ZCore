using ZCore.Serializables.ArgumentsInfo.FileManager.Archive;
using ZCore.Serializables.ArgumentsInfo.FileManager.Metadata;

namespace ZCore.Modules.TextureDrawer.Parsers.PackedTexture.PlayStation.PSV
{
/// <summary> Initializes Parsing Tasks for PTX Files (PSV). </summary>

public static class PtxParser
{
/** <summary> The Header of a PackedTexture File. </summary>

<remarks> It Occupies 4 Bytes in the Stream, which are: <c>0x47 0x58 0x54 0x00</c> </remarks> */

private const string PtxHeader = "GXT\0";

// Get PTX Stream

public static BinaryStream EncodeStream(SexyBitmap input, Endian endian = default, FileVersionDetails<uint> verInfo = null,
string outputPath = null, string pathToPtxInfo = null)
{
verInfo ??= new(PtxVersion.ExpectedVersions.MaxValue);

PtxInfo fileInfo = string.IsNullOrEmpty(pathToPtxInfo) ? 
new(input.Height, input.Width) :
new PtxInfo().ReadObject(pathToPtxInfo);

BinaryStream ptxStream = string.IsNullOrEmpty(outputPath) ? new() : BinaryStream.OpenWrite(outputPath);
ptxStream.WriteString(PtxHeader, default, endian);

fileInfo.WriteBin(ptxStream, verInfo, endian);
DXT5_RGBA_Morton.Write(ptxStream, input, endian);

ptxStream.Position = PtxHeader.Length - 1;
fileInfo.WriteBin(ptxStream, verInfo, endian); // Update Info (FileSize)

return ptxStream;
}

/** <summary> Encodes an Image as a PTX File. </summary>

<param name = "inputPath"> The Path where the Image to Encode is Located. </param>
<param name = "outputPath"> The Location where the Encoded PTX File will be Saved. </param>

<exception cref = "ArgumentNullException"></exception>
<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception>
<exception cref = "NotSupportedException"></exception>
<exception cref = "UnauthorizedAccessException"></exception> */

public static void EncodeFile(string inputPath, string outputPath, Endian endian = default,
FileVersionDetails<uint> verInfo = null, MetadataImportParams importCfg = null)
{
importCfg ??= new();

PathHelper.ChangeExtension(ref outputPath, PtxUtils.FileExt);

using SexyBitmap inputFile = SexyBitmap.CreateNew(inputPath);
string pathToPtxInfo = importCfg.ImportMetadataToFiles ? PtxInfo.ResolvePath(inputPath, importCfg) : null;

using BinaryStream outputFile = EncodeStream(inputFile, endian, verInfo, outputPath, pathToPtxInfo);
}

// Get Plain Image

public static SexyBitmap DecodeStream(BinaryStream input, Endian endian = default, bool adaptVer = true,
bool compareFileSize = true, string pathToPtxInfo = null)
{
input.CompareString<InvalidPtxException>(PtxHeader, default, endian);

PtxInfo fileInfo = PtxInfo.ReadBin(input, endian, adaptVer);

if(compareFileSize)
TextureHelper.CompareFileSize(input, fileInfo.FileSize + fileInfo.BlockSize);

SexyBitmap output = DXT5_RGBA_Morton.Read(input, fileInfo.TextureWidth, fileInfo.TextureHeight, endian);

if(!string.IsNullOrEmpty(pathToPtxInfo) )
fileInfo.WriteObject(pathToPtxInfo);

return output;
}

/** <summary> Decodes a PTX File as an Image. </summary>

<param name = "inputPath"> The Path where the PTX File to Decode is Located. </param>
<param name = "outputPath"> The Location where the Decoded Image will be Saved. </param>

<exception cref = "ArgumentNullException"></exception>
<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception>
<exception cref = "NotSupportedException"></exception>
<exception cref = "UnauthorizedAccessException"></exception> */

public static void DecodeFile(string inputPath, string outputPath, Endian endian = default,
bool adaptVer = true, bool compareFileSize = true, MetadataExportParams exportCfg = null)
{
exportCfg ??= new();

PathHelper.ChangeExtension(ref outputPath, ".png");

using BinaryStream inputFile = BinaryStream.Open(inputPath);
string pathToPtxInfo = exportCfg.ExportMetadataFromFiles ? PtxInfo.ResolvePath(inputPath, exportCfg) : null;

using SexyBitmap image = DecodeStream(inputFile, endian, adaptVer, compareFileSize, pathToPtxInfo);
image.Save(outputPath);        
}

/** <summary> Checks the Integrity of a PTX File. </summary>
<param name = "targetPath"> The Path to the File to Analize. </param> */

public static void CheckFileSize(string targetPath, Endian endian = default, bool adaptVer = true)
{
using BinaryStream inputFile = BinaryStream.Open(targetPath);

inputFile.CompareString<InvalidPtxException>(PtxHeader, default, endian);
PtxInfo fileInfo = PtxInfo.ReadBin(inputFile, endian, adaptVer);

TextureHelper.CompareFileSize(inputFile, fileInfo.FileSize + fileInfo.BlockSize);
}

}

}
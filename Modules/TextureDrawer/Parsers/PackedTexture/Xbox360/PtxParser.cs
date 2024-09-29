using ZCore.Modules.TextureDrawer.Parsers.PackedTexture.Xbox360.Exceptions;
using ZCore.Serializables.ArgumentsInfo.FileManager.Metadata;

namespace ZCore.Modules.TextureDrawer.Parsers.PackedTexture.Xbox360
{
/// <summary> Initializes Parsing Tasks for PTX Files (PS4). </summary>

public static class PtxParser
{
/** <summary> The Header of a PTX File. </summary>
<remarks> It Occupies 4 Bytes in the Stream, which are: <c>0x54 0x69 0xA4 0x72</c> </remarks> */

private const uint PtxMagic = 1409294362;

// Get PTX Stream

public static BinaryStream EncodeStream(SexyBitmap input, Endian endian = default, string outputPath = null, string pathToPtxInfo = null)
{
int paddedWidth = TextureHelper.CalculatePaddedDimension(input.Width, 128);

PtxInfo fileInfo = string.IsNullOrEmpty(pathToPtxInfo) ? 
new(input.Height, input.Width, paddedWidth << 2) :
new PtxInfo().ReadObject(pathToPtxInfo);

PathHelper.ChangeExtension(ref outputPath, PtxUtils.FileExt);
BinaryStream ptxStream = string.IsNullOrEmpty(outputPath) ? new() : BinaryStream.OpenWrite(outputPath);

fileInfo.WriteBin(ptxStream, endian);
ptxStream.WriteUInt(PtxMagic, endian);

DXT5_RGBA_Padding.Write(ptxStream, input, fileInfo.BlockSize, endian);

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
MetadataImportParams importCfg = null)
{
importCfg ??= new();

using SexyBitmap inputFile = SexyBitmap.CreateNew(inputPath);
string pathToPtxInfo = importCfg.ImportMetadataToFiles ? PtxInfo.ResolvePath(inputPath, importCfg) : null;

using BinaryStream outputFile = EncodeStream(inputFile, endian, outputPath, pathToPtxInfo);
}

// Get Plain Image

public static SexyBitmap DecodeStream(BinaryStream input, Endian endian = default, string pathToPtxInfo = null)
{
PtxInfo fileInfo = PtxInfo.ReadBin(input, endian);

input.CompareUInt<Xbox_InvalidPtxException>(PtxMagic, endian);

SexyBitmap output = DXT5_RGBA_Padding.Read(input, fileInfo.TextureWidth, fileInfo.TextureHeight, fileInfo.BlockSize);

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
MetadataExportParams exportCfg = null)
{
exportCfg ??= new();

PathHelper.ChangeExtension(ref outputPath, ".png");

using BinaryStream inputFile = BinaryStream.Open(inputPath);
string pathToPtxInfo = exportCfg.ExportMetadataFromFiles ? PtxInfo.ResolvePath(inputPath, exportCfg) : null;

using SexyBitmap image = DecodeStream(inputFile, endian, pathToPtxInfo);
image.Save(outputPath); 
}

}

}
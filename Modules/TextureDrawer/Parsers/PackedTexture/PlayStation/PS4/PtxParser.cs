using ZCore.Serializables.ArgumentsInfo.FileManager.Metadata;
using ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.Ptx.Format.PlayStation;

namespace ZCore.Modules.TextureDrawer.Parsers.PackedTexture.PlayStation.PS4
{
/// <summary> Initializes Parsing Tasks for PTX Files (PS4). </summary>

public static class PtxParser
{
// Get PTX Stream

public static BinaryStream EncodeStream(SexyBitmap input, PtxFormat_PS4 format, Endian endian = default,
string outputPath = null, string pathToPtxInfo = null)
{

PtxInfo fileInfo = string.IsNullOrEmpty(pathToPtxInfo) ? 
new(input.Height, input.Width, format) :
new PtxInfo().ReadObject(pathToPtxInfo);

PathHelper.ChangeExtension(ref outputPath, PtxUtils.FileExt);
BinaryStream ptxStream = string.IsNullOrEmpty(outputPath) ? new() : BinaryStream.OpenWrite(outputPath);

ptxStream.WriteString(PtxUtils.FileHeader, default, endian);
fileInfo.WriteBin(ptxStream, endian);

// Encode Image

switch(format)
{
case PtxFormat_PS4.RGB565:
RGB565.Write(ptxStream, input, endian);
break;

case PtxFormat_PS4.RGBA5551:
RGBA5551.Write(ptxStream, input, endian);
break;

case PtxFormat_PS4.ABGR8888:
ABGR8888.Write(ptxStream, input, endian);
break;

case PtxFormat_PS4.DXT5_RGBA_MortonBlock:
DXT5_RGBA_MortonBlock.Write(ptxStream, input, endian);
break;

case PtxFormat_PS4.L8:
L8.Write(ptxStream, input);
break;

case PtxFormat_PS4.LA88:
LA88.Write(ptxStream, input, endian);
break;

case PtxFormat_PS4.DXT1_RGB:
DXT1_RGB.Write(ptxStream, input, endian);
break;

case PtxFormat_PS4.DXT3_RGBA:
DXT3_RGBA.Write(ptxStream, input, endian);
break;

case PtxFormat_PS4.DXT5_RGBA:
DXT5_RGBA.Write(ptxStream, input, endian);
break;

default:
RGBA4444.Write(ptxStream, input, endian);
break;
}

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

public static void EncodeFile(string inputPath, string outputPath, PtxFormat_PS4 format,
Endian endian = default, MetadataImportParams importCfg = null)
{
importCfg ??= new();

using SexyBitmap inputFile = SexyBitmap.CreateNew(inputPath);
string pathToPtxInfo = importCfg.ImportMetadataToFiles ? PtxInfo.ResolvePath(inputPath, importCfg) : null;

using BinaryStream outputFile = EncodeStream(inputFile, format, endian, outputPath, pathToPtxInfo);
}

// Get Plain Image

public static SexyBitmap DecodeStream(BinaryStream input, Endian endian = default,
bool compareTextureSize = true, string pathToPtxInfo = null)
{
input.CompareString<InvalidPtxException>(PtxUtils.FileHeader, default, endian);

PtxInfo fileInfo = PtxInfo.ReadBin(input, endian);

if(compareTextureSize)
TextureHelper.CompareTextureSize(fileInfo.TextureWidth, fileInfo.TextureHeight, fileInfo.TextureSize);

// Decode Image

SexyBitmap output = fileInfo.TextureFormat switch
{
PtxFormat_PS4.RGB565 => RGB565.Read(input, fileInfo.TextureWidth, fileInfo.TextureHeight, endian),
PtxFormat_PS4.RGBA5551 => RGBA5551.Read(input, fileInfo.TextureWidth, fileInfo.TextureHeight, endian),
PtxFormat_PS4.ABGR8888 => ABGR8888.Read(input, fileInfo.TextureWidth, fileInfo.TextureHeight, endian),
PtxFormat_PS4.DXT5_RGBA_MortonBlock => DXT5_RGBA_MortonBlock.Read(input, fileInfo.TextureWidth, fileInfo.TextureHeight, endian),
PtxFormat_PS4.L8 => L8.Read(input, fileInfo.TextureWidth, fileInfo.TextureHeight),
PtxFormat_PS4.LA88 => LA88.Read(input, fileInfo.TextureWidth, fileInfo.TextureHeight, endian),
PtxFormat_PS4.DXT1_RGB => DXT1_RGB.Read(input, fileInfo.TextureWidth, fileInfo.TextureHeight, endian),
PtxFormat_PS4.DXT3_RGBA => DXT3_RGBA.Read(input, fileInfo.TextureWidth, fileInfo.TextureHeight, endian),
PtxFormat_PS4.DXT5_RGBA => DXT5_RGBA.Read(input, fileInfo.TextureWidth, fileInfo.TextureHeight, endian),
_ => RGBA4444.Read(input, fileInfo.TextureWidth, fileInfo.TextureHeight, endian)
};


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
bool compareTextureSize = true, MetadataExportParams exportCfg = null)
{
exportCfg ??= new();

PathHelper.ChangeExtension(ref outputPath, ".png");

using BinaryStream inputFile = BinaryStream.Open(inputPath);
string pathToPtxInfo = exportCfg.ExportMetadataFromFiles ? PtxInfo.ResolvePath(inputPath, exportCfg) : null;

using SexyBitmap image = DecodeStream(inputFile, endian, compareTextureSize, pathToPtxInfo);
image.Save(outputPath);        
}

/** <summary> Checks the Integrity of a PTX File. </summary>
<param name = "targetPath"> The Path to the File to Analize. </param> */

public static void CheckTextureSize(string targetPath, Endian endian = default)
{
using BinaryStream inputFile = BinaryStream.Open(targetPath);

inputFile.CompareString<InvalidPtxException>(PtxUtils.FileHeader, default, endian);
PtxInfo fileInfo = PtxInfo.ReadBin(inputFile, endian);

TextureHelper.CompareTextureSize(fileInfo.TextureWidth, fileInfo.TextureHeight, fileInfo.TextureSize);
}

}

}
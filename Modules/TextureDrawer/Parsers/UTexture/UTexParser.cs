using System;
using ZCore.Modules.TextureDrawer.Parsers.UTexture.Exceptions;
using ZCore.Serializables.ArgumentsInfo.FileManager.Metadata;
using ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.UTex;

namespace ZCore.Modules.TextureDrawer.Parsers.UTexture
{
/// <summary> Initializes Parsing Tasks for UTex Files. </summary>

public static class UTexParser
{
/** <summary> The Header of a UTex File. </summary>

<remarks> It Occupies 2 Bytes in the Stream, which are: <c>0x75 0x0A</c> </remarks> */

private const ushort UTexMagic = 2677;

// Get TEX Stream

public static BinaryStream EncodeStream(SexyBitmap input, UTexFormat format, Endian endian = default,
string outputPath = null, string pathToTexInfo = null)
{

UTexInfo fileInfo = string.IsNullOrEmpty(pathToTexInfo) ? 
new(input.Width, input.Height, format) :
new UTexInfo().ReadObject(pathToTexInfo);

PathHelper.ChangeExtension(ref outputPath, ".tex");
BinaryStream texStream = string.IsNullOrEmpty(outputPath) ? new() : BinaryStream.OpenWrite(outputPath);

texStream.WriteUShort(UTexMagic, endian);
fileInfo.WriteBin(texStream, endian);

// Encode Image
                
switch(format)
{
case UTexFormat.ABGR8888:
ABGR8888.Write(texStream, input, endian);
break;

case UTexFormat.RGBA4444:
RGBA4444.Write(texStream, input, endian);
break;

case UTexFormat.RGBA5551:
RGBA5551.Write(texStream, input, endian);
break;

case UTexFormat.RGB565:
RGB565.Write(texStream, input, endian);
break;

default:
throw new NotSupportedException($"\"{format}\" is an Unknown Format or is not Supported yet");
}
    
return texStream;
}

/** <summary> Encodes an Image as a TXZ File. </summary>

<param name = "inputPath"> The Path where the Image to Encode is Located. </param>
<param name = "outputPath"> The Location where the Encoded TXZ File will be Saved. </param>

<exception cref = "ArgumentNullException"></exception>
<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception>
<exception cref = "NotSupportedException"></exception>
<exception cref = "UnauthorizedAccessException"></exception> */

public static void EncodeFile(string inputPath, string outputPath, UTexFormat format,
Endian endian = default, MetadataImportParams importCfg = null)
{
importCfg ??= new();	

using SexyBitmap inputFile = SexyBitmap.CreateNew(inputPath);
string pathToTexInfo = importCfg.ImportMetadataToFiles ? UTexInfo.ResolvePath(inputPath, importCfg) : null;

using BinaryStream outputFile = EncodeStream(inputFile, format, endian, outputPath, pathToTexInfo);
}

// Get Plain Image

public static SexyBitmap DecodeStream(BinaryStream input, Endian endian = default, string pathToTexInfo = null)
{
input.CompareUShort<InvalidUTexException>(UTexMagic, endian);

UTexInfo fileInfo = UTexInfo.ReadBin(input, endian);

// Decode Image

SexyBitmap output = fileInfo.TextureFormat switch
{
UTexFormat.ABGR8888 => ABGR8888.Read(input, fileInfo.TextureWidth, fileInfo.TextureHeight, endian),
UTexFormat.RGBA4444 => RGBA4444.Read(input, fileInfo.TextureWidth, fileInfo.TextureHeight, endian),
UTexFormat.RGBA5551 => RGBA5551.Read(input, fileInfo.TextureWidth, fileInfo.TextureHeight, endian),
UTexFormat.RGB565 => RGB565.Read(input, fileInfo.TextureWidth, fileInfo.TextureHeight, endian),       
_ => throw new NotSupportedException($"\"{fileInfo.TextureFormat}\" is an Unknown Format or is not Supported yet")
};

if(!string.IsNullOrEmpty(pathToTexInfo) )
fileInfo.WriteObject(pathToTexInfo);

return output;
}

/** <summary> Decodes a TEX File as an Image. </summary>

<param name = "inputPath"> The Path where the TEX File to be Decoded is Located. </param>
<param name = "outputPath"> The Location where the Decoded Image File will be Saved. </param>

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
string pathToTexInfo = exportCfg.ExportMetadataFromFiles ? UTexInfo.ResolvePath(inputPath, exportCfg) : null;

using SexyBitmap image = DecodeStream(inputFile, endian, pathToTexInfo);
image.Save(outputPath);
}

}

}
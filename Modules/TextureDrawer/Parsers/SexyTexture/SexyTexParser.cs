using System;
using System.IO;
using System.IO.Compression;
using ZCore.Modules.FileCompressors;
using ZCore.Modules.TextureDrawer.Parsers.SexyTexture.Definitions;
using ZCore.Modules.TextureDrawer.Parsers.SexyTexture.Exceptions;
using ZCore.Serializables.ArgumentsInfo.FileManager.Archive;
using ZCore.Serializables.ArgumentsInfo.FileManager.Metadata;
using ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.SexyTex;

namespace ZCore.Modules.TextureDrawer.Parsers.SexyTexture
{
/// <summary> Initializes Parsing Tasks for SexyTex Files. </summary>

public static class SexyTexParser
{
/** <summary> The Header of a SexyTex File. </summary>

<remarks> It Occupies 8 Bytes in the Stream, which are: <c>0x53 0x45 0x58 0x59 0x54 0x45 0x58 0x00</c> </remarks> */

private const string SexyTexHeader = "SEXYTEX\0";

// Get TEX Stream

public static BinaryStream EncodeStream(SexyBitmap input, CompressionLevel level, int bufferSize, SexyTexFormat format,
Endian endian = default, FileVersionDetails<uint> verInfo = null, string outputPath = null, string pathToTexInfo = null)
{
verInfo ??= new();

SexyTexInfo fileInfo = string.IsNullOrEmpty(pathToTexInfo) ? 
new(input.Width, input.Height, format) :
new SexyTexInfo().ReadObject(pathToTexInfo);

PathHelper.ChangeExtension(ref outputPath, ".tex");
BinaryStream texStream = string.IsNullOrEmpty(outputPath) ? new() : BinaryStream.OpenWrite(outputPath);

texStream.WriteString(SexyTexHeader, default, endian);
fileInfo.WriteBin(texStream, verInfo, endian);

// Encode Image

using BinaryStream imageData = new();
                
switch(format)
{
case SexyTexFormat.ARGB8888:
ARGB8888.Write(imageData, input, endian);
break;

case SexyTexFormat.ARGB4444:
ARGB4444.Write(imageData, input, endian);
break;

case SexyTexFormat.ARGB1555:
ARGB1555.Write(imageData, input, endian);
break;

case SexyTexFormat.RGB565:
RGB565.Write(imageData, input, endian);
break;

case SexyTexFormat.ABGR8888:
ABGR8888.Write(imageData, input, endian);
break;

case SexyTexFormat.RGBA4444:
RGBA4444.Write(imageData, input, endian);
break;

case SexyTexFormat.RGBA5551:
RGBA5551.Write(imageData, input, endian);
break;

case SexyTexFormat.XRGB8888:
XRGB8888.Write(imageData, input, endian);
break;

case SexyTexFormat.LA88:
LA88.Write(imageData, input, endian);
break;

default:
throw new NotSupportedException($"\"{format}\" is an Unknown Format or is not Supported yet");
}

imageData.Position = 0;

if(fileInfo.CompressData)
{
using ZLibStream zLibStream = ZLibCompressor.CompressStream(imageData, texStream, level, bufferSize);

fileInfo.SizeCompressed = (int)(texStream.Length - 0x30);
}

else
{
FileManager.ProcessBuffer(imageData, texStream, bufferSize);

fileInfo.SizeCompressed = 0;
}

texStream.Position = SexyTexHeader.Length - 1;
fileInfo.WriteBin(texStream, verInfo, endian); // Update Info (SizeCompressed)

return texStream;
}

/** <summary> Encodes an Image as a SexyTexture. </summary>

<param name = "inputPath"> The Path where the Image to Encode is Located. </param>
<param name = "outputPath"> The Location where the Encoded TEX File will be Saved. </param>

<exception cref = "ArgumentNullException"></exception>
<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception>
<exception cref = "NotSupportedException"></exception>
<exception cref = "UnauthorizedAccessException"></exception> */

public static void EncodeFile(string inputPath, string outputPath, CompressionLevel level, int bufferSize, 
SexyTexFormat format, Endian endian = default, FileVersionDetails<uint> verInfo = null,
MetadataImportParams importCfg = null)
{
importCfg ??= new();	

using SexyBitmap inputFile = SexyBitmap.CreateNew(inputPath);
string pathToTexInfo = importCfg.ImportMetadataToFiles ? SexyTexInfo.ResolvePath(inputPath, importCfg) : null;

using BinaryStream outputFile = EncodeStream(inputFile, level, bufferSize, format,
endian, verInfo, outputPath, pathToTexInfo);

}

// Get Plain Image

public static SexyBitmap DecodeStream(BinaryStream input, int bufferSize, Endian endian = default, 
bool adaptVer = true, string pathToTexInfo = null)
{
input.CompareString<InvalidSexyTexException>(SexyTexHeader, default, endian);

SexyTexInfo fileInfo = SexyTexInfo.ReadBin(input, adaptVer, endian);

BinaryStream imageData;

if(fileInfo.CompressData)
{
using ZLibStream zLibStream = new(input, CompressionMode.Decompress);

imageData = new( ZLibCompressor.DecompressStream(zLibStream, bufferSize) );
}

else
{
imageData = new();

FileManager.ProcessBuffer(input, imageData);
}

imageData.Position = 0;

// Decode Image

SexyBitmap output = fileInfo.TextureFormat switch
{
SexyTexFormat.ARGB8888 => ARGB8888.Read(imageData, fileInfo.TextureWidth, fileInfo.TextureHeight, endian),
SexyTexFormat.ARGB4444 => ARGB4444.Read(imageData, fileInfo.TextureWidth, fileInfo.TextureHeight, endian),
SexyTexFormat.ARGB1555 => ARGB1555.Read(imageData, fileInfo.TextureWidth, fileInfo.TextureHeight, endian),
SexyTexFormat.RGB565 => RGB565.Read(imageData, fileInfo.TextureWidth, fileInfo.TextureHeight, endian),
SexyTexFormat.ABGR8888 => ABGR8888.Read(imageData, fileInfo.TextureWidth, fileInfo.TextureHeight, endian),
SexyTexFormat.RGBA4444 => RGBA4444.Read(imageData, fileInfo.TextureWidth, fileInfo.TextureHeight, endian),
SexyTexFormat.RGBA5551 => RGB565.Read(imageData, fileInfo.TextureWidth, fileInfo.TextureHeight, endian),
SexyTexFormat.XRGB8888 => XRGB8888.Read(imageData, fileInfo.TextureWidth, fileInfo.TextureHeight, endian),
SexyTexFormat.LA88 => LA88.Read(imageData, fileInfo.TextureWidth, fileInfo.TextureHeight, endian),
_ => throw new NotSupportedException($"\"{fileInfo.TextureFormat}\" is an Unknown Format or is not Supported yet")
};

if(!string.IsNullOrEmpty(pathToTexInfo) )
fileInfo.WriteObject(pathToTexInfo);

return output;
}

/** <summary> Decodes a SexyTexture as an Image. </summary>

<param name = "inputPath"> The Path where the SexyTex to Decode is Located. </param>
<param name = "outputPath"> The Location where the Decoded Image will be Saved. </param>

<exception cref = "ArgumentNullException"></exception>
<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception>
<exception cref = "NotSupportedException"></exception>
<exception cref = "UnauthorizedAccessException"></exception> */

public static void DecodeFile(string inputPath, string outputPath, int bufferSize,
Endian endian = default, bool adaptVer = true, MetadataExportParams exportCfg = null)
{
exportCfg ??= new();

PathHelper.ChangeExtension(ref outputPath, ".png");

using BinaryStream inputFile = BinaryStream.Open(inputPath);
string pathToTexInfo = exportCfg.ExportMetadataFromFiles ? SexyTexInfo.ResolvePath(inputPath, exportCfg) : null;

using SexyBitmap image = DecodeStream(inputFile, bufferSize, endian, adaptVer, pathToTexInfo);
image.Save(outputPath);
}

}

}
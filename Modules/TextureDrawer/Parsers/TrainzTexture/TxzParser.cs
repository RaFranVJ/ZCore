using System;
using System.IO.Compression;
using ZCore.Modules.FileCompressors;
using ZCore.Modules.TextureDrawer.Parsers.TrainzTexture.Exceptions;
using ZCore.Serializables.ArgumentsInfo.FileManager.Metadata;
using ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.Txz;

namespace ZCore.Modules.TextureDrawer.Parsers.TrainzTexture
{
/// <summary> Initializes Parsing Tasks for TXZ Files. </summary>

public static class TxzParser
{
/** <summary> The Header of a TXZ File. </summary>

<remarks> It Occupies 2 Bytes in the Stream, which are: <c>0x75 0x0A</c> </remarks> */

private const ushort TxzMagic = 2677;

// Get TXZ Stream

public static BinaryStream EncodeStream(SexyBitmap input, CompressionLevel level, int bufferSize, TxzFormat format,
Endian endian = default, string outputPath = null, string pathToTxzInfo = null)
{

TxzInfo fileInfo = string.IsNullOrEmpty(pathToTxzInfo) ? 
new(input.Width, input.Height, format) :
new TxzInfo().ReadObject(pathToTxzInfo);

PathHelper.ChangeExtension(ref outputPath, ".txz");
BinaryStream txzStream = string.IsNullOrEmpty(outputPath) ? new() : BinaryStream.OpenWrite(outputPath);

txzStream.WriteUShort(TxzMagic, endian);
fileInfo.WriteBin(txzStream, endian);

// Encode Image

using BinaryStream imageData = new();
                    
switch(format)
{
case TxzFormat.ABGR8888:
ABGR8888.Write(imageData, input, endian);
break;

case TxzFormat.RGBA4444:
RGBA4444.Write(imageData, input, endian);
break;

case TxzFormat.RGBA5551:
RGBA5551.Write(imageData, input, endian);
break;

case TxzFormat.RGB565:
RGB565.Write(imageData, input, endian);
break;

default:
throw new NotSupportedException($"\"{format}\" is an Unknown Format or is not Supported yet");
}

imageData.Position = 0;

using ZLibStream zLibStream = ZLibCompressor.CompressStream(imageData, txzStream, level, bufferSize);
          
return txzStream;
}

/** <summary> Encodes an Image as a TXZ File. </summary>

<param name = "inputPath"> The Path where the Image to Encode is Located. </param>
<param name = "outputPath"> The Location where the Encoded TXZ File will be Saved. </param>

<exception cref = "ArgumentNullException"></exception>
<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception>
<exception cref = "NotSupportedException"></exception>
<exception cref = "UnauthorizedAccessException"></exception> */

public static void EncodeFile(string inputPath, string outputPath, CompressionLevel level, int bufferSize, 
TxzFormat format, Endian endian = default, MetadataImportParams importCfg = null)
{
importCfg ??= new();	

using SexyBitmap inputFile = SexyBitmap.CreateNew(inputPath);
string pathToTxzInfo = importCfg.ImportMetadataToFiles ? TxzInfo.ResolvePath(inputPath, importCfg) : null;

using BinaryStream outputFile = EncodeStream(inputFile, level, bufferSize, format, endian, outputPath, pathToTxzInfo);
}

// Get Plain Image

public static SexyBitmap DecodeStream(BinaryStream input, int bufferSize, Endian endian = default, 
string pathToTxzInfo = null)
{
input.CompareUShort<InvalidTxzException>(TxzMagic, endian);

TxzInfo fileInfo = TxzInfo.ReadBin(input, endian);

using ZLibStream zLibStream = new(input, CompressionMode.Decompress);
using BinaryStream imageData = new( ZLibCompressor.DecompressStream(zLibStream, bufferSize) );
  
imageData.Position = 0;

// Decode Image

SexyBitmap output = fileInfo.TextureFormat switch
{
TxzFormat.ABGR8888 => ABGR8888.Read(imageData, fileInfo.TextureWidth, fileInfo.TextureHeight, endian),
TxzFormat.RGBA4444 => RGBA4444.Read(imageData, fileInfo.TextureWidth, fileInfo.TextureHeight, endian),
TxzFormat.RGBA5551 => RGBA5551.Read(imageData, fileInfo.TextureWidth, fileInfo.TextureHeight, endian),
TxzFormat.RGB565 => RGB565.Read(imageData, fileInfo.TextureWidth, fileInfo.TextureHeight, endian),       
_ => throw new NotSupportedException($"\"{fileInfo.TextureFormat}\" is an Unknown Format or is not Supported yet")
};

if(!string.IsNullOrEmpty(pathToTxzInfo) )
fileInfo.WriteObject(pathToTxzInfo);

return output;
}

/** <summary> Decodes a TXZ File as an Image. </summary>

<param name = "inputPath"> The Path where the RTON File to be Decoded is Located. </param>
<param name = "outputPath"> The Location where the Decoded JSON File will be Saved. </param>

<exception cref = "ArgumentNullException"></exception>
<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception>
<exception cref = "NotSupportedException"></exception>
<exception cref = "UnauthorizedAccessException"></exception> */

public static void DecodeFile(string inputPath, string outputPath, int bufferSize,
Endian endian = default, MetadataExportParams exportCfg = null)
{
exportCfg ??= new();

PathHelper.ChangeExtension(ref outputPath, ".png");

using BinaryStream inputFile = BinaryStream.Open(inputPath);
string pathToTxzInfo = exportCfg.ExportMetadataFromFiles ? TxzInfo.ResolvePath(inputPath, exportCfg) : null;

using SexyBitmap image = DecodeStream(inputFile, bufferSize, endian, pathToTxzInfo);
image.Save(outputPath);
}
   
}

}
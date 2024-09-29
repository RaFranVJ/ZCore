using System;
using System.IO;
using System.IO.Compression;
using ZCore.Modules.FileCompressors;
using ZCore.Modules.SexyCompressors.StandarMediaFile.Exceptions;
using ZCore.Serializables.ArgumentsInfo.FileManager.Metadata;
using ZCore.Serializables.ArgumentsInfo.FileSecurity.Integrity;
using ZCore.Serializables.ArgumentsInfo.SexyCompressor.Smf;
using ZCore.Serializables.ArgumentsInfo.SexyCompressor.Smf.Integrity;

namespace ZCore.Modules.SexyCompressors.StandarMediaFile
{
/// <summary> Initializes Compression Tasks for Standar Media Files (SMF). </summary>

public static class SmfCompressor
{
/// <summary> The SMF Extension </summary>

private const string SmfExt = ".smf";

/** <summary> The Header of a SMF File. </summary>

<remarks> It Occupies 4 Bytes in the Stream, which are: <c>0xD4 0xFE 0xAD 0xDE</c> </remarks> */

private const uint SmfMagic = 3735944916;

// Get SMF Stream

public static BinaryStream CompressStream(BinaryStream input, CompressionLevel level, int bufferSize,
Endian endian = default, int hexBytesCount = 4, StringCase? strCaseForTags = null,
Adler32BytesInfo adler32Cfg = null, string outputPath = null, string pathToSmfInfo = null, string pathToSmfTag = null)
{
adler32Cfg ??= new();

StandarMediaInfo fileInfo = string.IsNullOrEmpty(pathToSmfInfo) ? 
new(input, hexBytesCount, level, adler32Cfg) :
new StandarMediaInfo().ReadObject(pathToSmfInfo);

PathHelper.AddExtension(ref outputPath, SmfExt);
BinaryStream smfStream = string.IsNullOrEmpty(outputPath) ? new() : BinaryStream.OpenWrite(outputPath);

smfStream.WriteUInt(SmfMagic, endian);
smfStream.Write(fileInfo.HexSizeBeforeCompression);

smfStream.WriteUShort(fileInfo.CompressionFlags, endian);

using MemoryStream rsbStream = new();
using DeflateStream compressionStream = DeflateCompressor.CompressStream(input, rsbStream, level, bufferSize);

rsbStream.Position = 0;
FileManager.ProcessBuffer(rsbStream, smfStream, bufferSize);

smfStream.Write(fileInfo.Adler32Bytes);

if(!string.IsNullOrEmpty(pathToSmfTag) )
SmfTagCreator.SaveTag(input, (StringCase)strCaseForTags, pathToSmfTag);

return smfStream;
}

/** <summary> Compresses the Contents of a RSB File as a SMF File by using Deflate Compression. </summary>

<param name = "inputPath"> The Path where the File to be Compressed is Located. </param>
<param name = "outputPath"> The Location where the Compressed File will be Saved. </param>
<param name = "compressionLvl"> The Compression Level to be Used. </param>

<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception>
<exception cref = "NotSupportedException"></exception>
<exception cref = "SecurityException"></exception>
<exception cref = "UnauthorizedAccessException"></exception> */

public static void CompressFile(string inputPath, string outputPath, CompressionLevel level, int bufferSize,
Endian endian = default, int hexBytesCount = 4, Adler32BytesInfo adler32Cfg = null, SmfTagInfo tagInfo = null,
MetadataImportParams importCfg = null)
{
tagInfo ??= new();
importCfg ??= new();	

using BinaryStream inputFile = BinaryStream.Open(inputPath);

string pathToSmfInfo = importCfg.ImportMetadataToFiles ? StandarMediaInfo.ResolvePath(inputPath, importCfg) : null;
string pathToSmfTag = tagInfo.GenerateTagsOnCompression ? SmfTagCreator.ResolvePath(inputPath, tagInfo) : null;

using BinaryStream outputFile = CompressStream(inputFile, level, bufferSize, endian, hexBytesCount,
tagInfo.HashedStringCase, adler32Cfg, outputPath, pathToSmfInfo, pathToSmfTag);

}

// Get RSB Stream

public static Stream DecompressStream(BinaryStream input, int bufferSize, Endian endian = default, int hexBytesCount = 4,
SmfIntegrityInfo integrityCfg = null, Adler32BytesInfo adler32Cfg = null, string outputPath = null,
StringCase? strCaseForTags = null, string pathToSmfTag = null, string pathToSmfInfo = null)
{
integrityCfg ??= new();
adler32Cfg ??= new();

input.CompareUInt<InvalidSmfException>(SmfMagic, endian);

StandarMediaInfo fileInfo = new()
{
HexSizeBeforeCompression = input.ReadBytes(hexBytesCount),
CompressionFlags = input.ReadUShort(endian)
};

long plainSize = StandarMediaInfo.GetPlainSize(fileInfo.HexSizeBeforeCompression);
using MemoryStream smfStream = new();

FileManager.ProcessBuffer(input, smfStream, bufferSize, plainSize);

input.Position = input.Position - adler32Cfg.MaxAdler32Bytes - 1;
fileInfo.Adler32Bytes = input.ReadBytes(adler32Cfg.MaxAdler32Bytes);

smfStream.Position = 0;
using DeflateStream decompressionStream = new(smfStream, CompressionMode.Decompress);

Stream rsbStream = DeflateCompressor.DecompressStream(decompressionStream, bufferSize, outputPath);

if(integrityCfg.CheckIntegrityOnDecompression)
SmfAnalisis.IntegrityCheck(rsbStream, fileInfo, integrityCfg.AnalisisType, strCaseForTags, pathToSmfTag, adler32Cfg);

if(!string.IsNullOrEmpty(pathToSmfInfo) )
fileInfo.WriteObject(pathToSmfInfo);

return rsbStream;
}

/** <summary> Decompresses the Contents of a SMF File as a RSB File by using Deflate Compression. </summary>

<param name = "inputPath" > The Path where the File to be Decompressed is Located. </param>
<param name = "outputPath" > The Location where the Decompressed File will be Saved. </param>

<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception>
<exception cref = "NotSupportedException"></exception>
<exception cref = "UnauthorizedAccessException"></exception> */

public static void DecompressFile(string inputPath, string outputPath, bool removeSmfExt, int bufferSize,
Endian endian = default, int hexBytesCount = 4, SmfIntegrityInfo integrityCfg = null,
Adler32BytesInfo adler32Cfg = null, SmfTagInfo tagInfo = null, MetadataExportParams exportCfg = null)
{
integrityCfg ??= new();
exportCfg ??= new();

tagInfo ??= new();

if(removeSmfExt)
PathHelper.RemoveExtension(ref outputPath, SmfExt);

using BinaryStream inputFile = BinaryStream.Open(inputPath);
string pathToSmfTag = integrityCfg.CheckIntegrityOnDecompression ? SmfTagCreator.ResolvePath(inputPath, tagInfo) : null;

string pathToSmfInfo = exportCfg.ExportMetadataFromFiles ? StandarMediaInfo.ResolvePath(inputPath, exportCfg) : null;

using Stream outputFile = DecompressStream(inputFile, bufferSize, endian, hexBytesCount, integrityCfg,
adler32Cfg, outputPath, tagInfo.HashedStringCase, pathToSmfTag, pathToSmfInfo);

}

/** <summary> Generates a SMF Tag File in the Specfied Location. </summary>

<param name = "sourcePath"> The Path to the RSB file from which the Tag will be Created. </param>
<param name = "targetPath"> The Path where to Save the SMF Tag. </param> */

public static void CreateTagFile(string sourcePath, string targetPath, StringCase strCase)
{
using BinaryStream inputFile = BinaryStream.Open(sourcePath);

SmfTagCreator.SaveTag(inputFile, strCase, targetPath);
}

/** <summary> Checks the Integrity of a SMF File before Compression. </summary>
<param name = "targetPath"> The Path to the File to Analize. </param> */

public static void CheckFileIntegrity(string targetPath, IntegrityCheckType analisisType, 
MetadataImportParams importCfg = null, SmfTagInfo tagInfo = null, Adler32BytesInfo adler32Cfg = null)
{
importCfg ??= new();

using BinaryStream inputFile = BinaryStream.Open(targetPath);

string pathToSmfInfo = StandarMediaInfo.ResolvePath(targetPath, importCfg);
var fileInfo = new StandarMediaInfo().ReadObject(pathToSmfInfo);

string pathToSmfTag = SmfTagCreator.ResolvePath(targetPath, tagInfo);

SmfAnalisis.IntegrityCheck(inputFile, fileInfo, analisisType, tagInfo.HashedStringCase, pathToSmfTag, adler32Cfg);
}

}

}
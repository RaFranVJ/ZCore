using SevenZip;
using SevenZip.Compression.LZMA;
using System;
using System.IO;
using ZCore.Modules;

namespace ZCore.Modules.FileCompressors
{
/// <summary> Initializes Compression Tasks for Files by using the LZMA algorithm. </summary>

public static class LzmaCompressor
{
/// <summary> The LZMA Extension </summary>

private const string LZMAExt = ".lzma";

/** <summary> An Interface used for Displaying the Progress of a Task from the Compressor. </summary>
<remarks> I'm keeping this as <c>null</c> for Console Projects. </remarks> */

private static readonly ICodeProgress processReporter = null;

// Compress LZMA Stream

public static BinaryStream CompressStream(Stream input, bool useSizeInfo, long inputSize, long outputSize,
CoderPropID[] propIDs = null, object[] props = null, Endian? endian = null, string outputPath = null)
{
using BinaryStream output = string.IsNullOrEmpty(outputPath) ? new() : BinaryStream.OpenWrite(outputPath);
Encoder fileCompressor = new();

if(propIDs != null && props != null)
fileCompressor.SetCoderProperties(propIDs, props);

fileCompressor.WriteCoderProperties(output);
long plainDataSize = inputSize;

long compressedDataSize = outputSize;

if(useSizeInfo)
{
plainDataSize = input.Length;

output.WriteLong(plainDataSize, (Endian)endian);
}

fileCompressor.Code(input, output, plainDataSize, compressedDataSize, processReporter);

return output;
}

/** <summary> Compresses the Contents of a File by using LZMA Compression. </summary>

<param name = "inputPath"> The Path where the File to be Compressed is Located. </param>
<param name = "outputPath"> The Location where the Compressed File will be Saved. </param>

<exception cref = "FileNotFoundException"></exception>
<exception cref = "IndexOutOfRangeException"></exception>
<exception cref = "IOException"></exception>
<exception cref = "NotSupportedException"></exception>
<exception cref = "NullReferenceException"></exception>
<exception cref = "UnauthorizedAccessException"></exception> */

public static void CompressFile(string inputPath, string outputPath, bool useSizeInfo, long inputSize,
long outputSize, CoderPropID[] propIDs = null, object[] props = null, Endian? endian = null)
{
PathHelper.AddExtension(ref outputPath, LZMAExt);

using FileStream inputFile = File.OpenRead(inputPath);

using BinaryStream outputFile = CompressStream(inputFile, useSizeInfo, inputSize, outputSize, propIDs, props, endian, outputPath);
}

/** <summary> Decompresses the Contents of a File by using LZMA Compression. </summary>

<param name = "inputPath"> The Path where the File to be Decompressed is Located. </param>
<param name = "outputPath"> The Location where the Decompressed File will be Saved. </param>

<exception cref = "FileNotFoundException"></exception>
<exception cref = "IndexOutOfRangeException"></exception>
<exception cref = "IOException"></exception>
<exception cref = "NotSupportedException"></exception>
<exception cref = "NullReferenceException"></exception>
<exception cref = "UnauthorizedAccessException"></exception> */

public static Stream DecompressStream(BinaryStream input, bool useSizeInfo, long inputSize,
long outputSize, int propsCount = 5, Endian? endian = null, string outputPath = null)
{
using Stream output =  string.IsNullOrEmpty(outputPath) ? new MemoryStream() : File.OpenWrite(outputPath);
Decoder fileDecompressor = new();

byte[] coderPropsInfo = input.ReadBytes(propsCount);
fileDecompressor.SetDecoderProperties(coderPropsInfo);

long compressedDataSize = inputSize;
long plainDataSize = useSizeInfo ? input.ReadLong( (Endian)endian) : outputSize;

fileDecompressor.Code(input, output, compressedDataSize, plainDataSize, processReporter);

return output;
}

/** <summary> Compresses the Contents of a File by using LZMA Compression. </summary>

<param name = "inputPath"> The Path where the File to be Compressed is Located. </param>
<param name = "outputPath"> The Location where the Compressed File will be Saved. </param>

<exception cref = "FileNotFoundException"></exception>
<exception cref = "IndexOutOfRangeException"></exception>
<exception cref = "IOException"></exception>
<exception cref = "NotSupportedException"></exception>
<exception cref = "NullReferenceException"></exception>
<exception cref = "UnauthorizedAccessException"></exception> */

public static void DecompressFile(string inputPath, string outputPath, bool useSizeInfo, long inputSize,
long outputSize, int propsCount = 5, Endian? endian = null)
{
PathHelper.RemoveExtension(ref outputPath, LZMAExt);

using BinaryStream inputFile = BinaryStream.Open(inputPath);

using Stream outputFile = DecompressStream(inputFile, useSizeInfo, inputSize, outputSize, propsCount, endian, outputPath);
}

}

}
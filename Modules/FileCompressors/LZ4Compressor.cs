using K4os.Compression.LZ4.Streams;
using System;
using System.IO;
using ZCore.Modules;

namespace ZCore.Modules.FileCompressors
{
/// <summary> Initializes Compression Tasks for Files by using the LZ4 algorithm. </summary>

public static class LZ4Compressor
{
/// <summary> The LZ4 Extension </summary>

private const string LZ4Ext = ".lz4";

// Get LZ4 Stream

public static LZ4EncoderStream CompressStream(Stream input, Stream output, LZ4EncoderSettings settings, int bufferSize)
{
LZ4EncoderStream lz4Stream = LZ4Stream.Encode(output, settings);

FileManager.ProcessBuffer(input, lz4Stream, bufferSize);

return lz4Stream;
}

/** <summary> Compresses the Contents of a File by using LZ4 Compression. </summary>

<param name = "inputPath"> The Access Path where the File to be Compressed is Located. </param>
<param name = "outputPath"> The Location where the Compressed File will be Saved. </param>

<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception>
<exception cref = "NotSupportedException"></exception>
<exception cref = "UnauthorizedAccessException"></exception> */

public static void CompressFile(string inputPath, string outputPath, LZ4EncoderSettings encoderSettings, int bufferSize)
{
PathHelper.AddExtension(ref outputPath, LZ4Ext);

using FileStream inputFile = File.OpenRead(inputPath);
using FileStream outputFile = File.OpenWrite(outputPath);

using LZ4EncoderStream compressionStream = CompressStream(inputFile, outputFile, encoderSettings, bufferSize);
}

// Get Plain Stream

public static Stream DecompressStream(LZ4DecoderStream input, int bufferSize, string outputPath = default)
{
Stream outputStream = string.IsNullOrEmpty(outputPath) ? new MemoryStream() : File.OpenWrite(outputPath);

FileManager.ProcessBuffer(input, outputStream, bufferSize);

return outputStream;
}

/** <summary> Decompresses the Contents of a File by using LZ4 Compression. </summary>

<param name = "inputPath"> The Path where the File to be Decompressed is Located. </param>
<param name = "outputPath"> The Location where the Decompressed File will be Saved. </param>

<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception>
<exception cref = "NotSupportedException"></exception>
<exception cref = "UnauthorizedAccessException"></exception> */

public static void DecompressFile(string inputPath, string outputPath, LZ4DecoderSettings decoderSettings, int bufferSize)
{
PathHelper.RemoveExtension(ref outputPath, LZ4Ext);

using FileStream inputFile = File.OpenRead(inputPath);
using LZ4DecoderStream decompressionStream = LZ4Stream.Decode(inputFile, decoderSettings);

using Stream outputFile = DecompressStream(decompressionStream, bufferSize, outputPath);
}

}

}
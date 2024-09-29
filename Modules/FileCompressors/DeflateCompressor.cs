using System;
using System.IO;
using System.IO.Compression;

namespace ZCore.Modules.FileCompressors
{
/// <summary> Initializes Compressing and Decompressing Functions for Files by using the Deflate algorithm. </summary>

public static class DeflateCompressor
{
/// <summary> The Deflate Extension </summary>

private const string DeflateExt = ".dfl";

// Get Deflate Stream

public static DeflateStream CompressStream(Stream input, Stream output, CompressionLevel level, int bufferSize)
{
DeflateStream dflStream = new(output, level);

FileManager.ProcessBuffer(input, dflStream, bufferSize);

return dflStream;
}

/** <summary> Compresses the Contents of a File by using the Deflate Algorithm. </summary>

<param name = "inputPath"> The Path where the File to be Compressed is Located. </param>
<param name = "outputPath"> The Location where the Compressed File will be Saved. </param>

<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception>
<exception cref = "NotSupportedException"></exception>
<exception cref = "UnauthorizedAccessException"></exception> */

public static void CompressFile(string inputPath, string outputPath, CompressionLevel compressLevel, int bufferSize)
{
PathHelper.AddExtension(ref outputPath, DeflateExt);

using FileStream inputFile = File.OpenRead(inputPath);
using FileStream outputFile = File.OpenWrite(outputPath);

using DeflateStream compressionStream = CompressStream(inputFile, outputFile, compressLevel, bufferSize);
}

// Get Plain Stream

public static Stream DecompressStream(DeflateStream input, int bufferSize, string outputPath = default)
{
Stream outputStream = string.IsNullOrEmpty(outputPath) ? new MemoryStream() : File.OpenWrite(outputPath);

FileManager.ProcessBuffer(input, outputStream, bufferSize);

return outputStream;
}

/** <summary> Decompresses the Contents of a File by using the Brotli Algorithm. </summary>

<param name = "inputPath"> The Path where the File to be Decompressed is Located. </param>
<param name = "outputPath"> The Location where the Decompressed File will be Saved. </param>

<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception>
<exception cref = "NotSupportedException"></exception>
<exception cref = "SecurityException"></exception>
<exception cref = "UnauthorizedAccessException"></exception> */

public static void DecompressFile(string inputPath, string outputPath, int bufferSize)
{
PathHelper.RemoveExtension(ref outputPath, DeflateExt);

using FileStream inputFile = File.OpenRead(inputPath);
using DeflateStream decompressionStream = new(inputFile, CompressionMode.Decompress);

using Stream outputFile = DecompressStream(decompressionStream, bufferSize, outputPath);
}

}

}
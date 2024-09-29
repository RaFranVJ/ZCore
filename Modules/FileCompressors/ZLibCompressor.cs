using System;
using System.IO;
using System.IO.Compression;
using ZCore.Modules;

namespace ZCore.Modules.FileCompressors
{
/// <summary> Initializes Compression Tasks for Files by using the ZLib algorithm. </summary>

public static class ZLibCompressor
{
/// <summary> The ZLib Extension </summary>

private const string ZLibExt = ".zlib";

// Get ZLIB Stream

public static ZLibStream CompressStream(Stream input, Stream output, CompressionLevel level, int bufferSize)
{
ZLibStream zlStream = new(output, level);

FileManager.ProcessBuffer(input, zlStream, bufferSize);

return zlStream;
}

/** <summary> Compresses the Contents of a File by using the ZLib Algorithm. </summary>

<param name = "inputPath"> The Path where the File to be Compressed is Located. </param>
<param name = "outputPath"> The Location where the Compressed File will be Saved. </param>

<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception>
<exception cref = "NotSupportedException"></exception>
<exception cref = "UnauthorizedAccessException"></exception> */

public static void CompressFile(string inputPath, string outputPath, CompressionLevel Level, int bufferSize)
{
PathHelper.AddExtension(ref outputPath, ZLibExt);

using FileStream inputFile = File.OpenRead(inputPath);
using FileStream outputFile = File.OpenWrite(outputPath);

using ZLibStream compressionStream = CompressStream(inputFile, outputFile, Level, bufferSize);
}

// Get Plain Stream

public static Stream DecompressStream(ZLibStream input, int bufferSize, string outputPath = default)
{
Stream outputStream = string.IsNullOrEmpty(outputPath) ? new MemoryStream() : File.OpenWrite(outputPath);

FileManager.ProcessBuffer(input, outputStream, bufferSize);

return outputStream;
}

/** <summary> Decompresses the Contents of a File by using the ZLib Algorithm. </summary>

<param name = "inputPath"> The Path where the File to be Decompressed is Located. </param>
<param name = "outputPath"> The Location where the Decompressed File will be Saved. </param>

<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception>
<exception cref = "NotSupportedException"></exception>
<exception cref = "SecurityException"></exception>
<exception cref = "UnauthorizedAccessException"></exception> */

public static void DecompressFile(string inputPath, string outputPath, int bufferSize)
{
PathHelper.RemoveExtension(ref outputPath, ZLibExt);

using FileStream inputFile = File.OpenRead(inputPath);
using ZLibStream decompressionStream = new(inputFile, CompressionMode.Decompress);

using Stream outputFile = DecompressStream(decompressionStream, bufferSize, outputPath);
}

}

}
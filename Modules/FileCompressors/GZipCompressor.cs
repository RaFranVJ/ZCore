using System;
using System.IO;
using System.IO.Compression;
using ZCore.Modules;

namespace ZCore.Modules.FileCompressors
{
/// <summary> Initializes Compression Tasks for Files by using the GZip algorithm. </summary>

public static class GZipCompressor
{
/// <summary> The GZip Extension </summary>

private const string GZipExt = ".gz";

// Get GZip Stream

public static GZipStream CompressStream(Stream input, Stream output, CompressionLevel level, int bufferSize)
{
GZipStream gzStream = new(output, level);

FileManager.ProcessBuffer(input, gzStream, bufferSize);

return gzStream;
}

/** <summary> Compresses the Contents of a File by using the GZip Algorithm. </summary>

<param name = "inputPath"> The Path where the File to be Compressed is Located. </param>
<param name = "outputPath"> The Location where the Compressed File will be Saved. </param>

<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception>
<exception cref = "NotSupportedException"></exception>
<exception cref = "UnauthorizedAccessException"></exception> */

public static void CompressFile(string inputPath, string outputPath, CompressionLevel compressLevel, int bufferSize)
{
PathHelper.AddExtension(ref outputPath, GZipExt);

using FileStream inputFile = File.OpenRead(inputPath);
using FileStream outputFile = File.OpenWrite(outputPath);

using GZipStream compressionStream = CompressStream(inputFile, outputFile, compressLevel, bufferSize);
}

// Get Plain Stream

public static Stream DecompressStream(GZipStream input, int bufferSize, string outputPath = default)
{
Stream outputStream = string.IsNullOrEmpty(outputPath) ? new MemoryStream() : File.OpenWrite(outputPath);

FileManager.ProcessBuffer(input, outputStream, bufferSize);

return outputStream;
}

/** <summary> Decompresses the Contents of a File by using the GZip Algorithm. </summary>

<param name = "inputPath"> The Path where the File to be Decompressed is Located. </param>
<param name = "outputPath"> The Location where the Decompressed File will be Saved. </param>

<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception>
<exception cref = "NotSupportedException"></exception><
<exception cref = "UnauthorizedAccessException"></exception> */

public static void DecompressFile(string inputPath, string outputPath, int bufferSize)
{
PathHelper.RemoveExtension(ref outputPath, GZipExt);

using FileStream inputFile = File.OpenRead(inputPath);
using GZipStream decompressionStream = new(inputFile, CompressionMode.Decompress);

using Stream outputFile = DecompressStream(decompressionStream, bufferSize, outputPath);
}

}

}
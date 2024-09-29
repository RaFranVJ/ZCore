using System.IO;
using System.IO.Compression;

namespace ZCore.Modules.FileCompressors
{
/// <summary> Initializes Compression Tasks for Files by using the Brotli algorithm. </summary>

public static class BrotliCompressor
{
/// <summary> The Brotli Extension </summary>

private const string BrotliExt = ".br";

// Get Brotli Stream

public static BrotliStream CompressStream(Stream input, Stream output, CompressionLevel level, int bufferSize)
{
BrotliStream brStream = new(output, level);

FileManager.ProcessBuffer(input, brStream, bufferSize);

return brStream;
}

/** <summary> Compresses the Contents of a File by using the Brotli Algorithm. </summary>

<param name = "inputPath"> The Path where the File to be Compressed is Located. </param>
<param name = "outputPath"> The Location where the Compressed File will be Saved. </param>

<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception> */

public static void CompressFile(string inputPath, string outputPath, CompressionLevel compressLevel, int bufferSize)
{
PathHelper.AddExtension(ref outputPath, BrotliExt);

using FileStream inputFile = File.OpenRead(inputPath);
using FileStream outputFile = File.OpenWrite(outputPath);

using BrotliStream compressionStream = CompressStream(inputFile, outputFile, compressLevel, bufferSize);
}

// Get Plain Stream

public static Stream DecompressStream(BrotliStream input, int bufferSize, string outputPath = default)
{
Stream outputStream = string.IsNullOrEmpty(outputPath) ? new MemoryStream() : File.OpenWrite(outputPath);

FileManager.ProcessBuffer(input, outputStream, bufferSize);

return outputStream;
}

/** <summary> Decompresses the Contents of a File by using the Brotli Algorithm. </summary>

<param name = "inputPath"> The Path where the File to be Decompressed is Located. </param>
<param name = "outputPath"> The Location where the Decompressed File will be Saved. </param>

<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception> */

public static void DecompressFile(string inputPath, string outputPath, int bufferSize)
{
PathHelper.RemoveExtension(ref outputPath, BrotliExt);

using FileStream inputFile = File.OpenRead(inputPath);
using BrotliStream decompressionStream = new(inputFile, CompressionMode.Decompress);

using Stream outputFile = DecompressStream(decompressionStream, bufferSize, outputPath);
}

}

}
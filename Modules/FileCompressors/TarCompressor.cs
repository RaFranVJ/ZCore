using ICSharpCode.SharpZipLib.Tar;
using System;
using System.IO;
using ZCore.Serializables.ArgumentsInfo.FileManager;

namespace ZCore.Modules.FileCompressors
{
/// <summary> Initializes Compression Tasks for Files by using the Tar algorithm. </summary>

public static class TarCompressor
{
/// <summary> The LZMA Extension </summary>

private const string TarExt = ".tar";

/** <summary> Adds a new Entry to the Specified <c>TarOutputStream</c>. </summary>

<param name = "sourcePath"> The Path where the Entry to Add is Located. </param>
<param name = "targetStream"> The Stream where the Tar Entry will be Added. </param> */

public static void AddTarEntry(string sourcePath, TarOutputStream targetStream, int bufferSize)
{
using FileStream inputFile = File.OpenRead(sourcePath);

TarEntry fileEntry = TarEntry.CreateEntryFromFile(sourcePath);
targetStream.PutNextEntry(fileEntry);

FileManager.ProcessBuffer(inputFile, targetStream, bufferSize);
}

// Get Tar Stream

public static TarOutputStream CompressStream(Stream target, int blockFactor, int bufferSize, params string[] entryNames)
{
TarOutputStream tarStream = new(target, blockFactor);

foreach(string name in entryNames)
AddTarEntry(name, tarStream, bufferSize);

return tarStream;
}

/** <summary> Compresses the Contents of a File by using Tar Compression. </summary>

<param name = "inputPath"> The Path where the File to be Compressed is Located. </param>
<param name = "outputPath"> The Location where the Compressed File will be Saved. </param>

<exception cref = "ArgumentException"></exception>
<exception cref = "ArgumentNullException"></exception>
<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception>
<exception cref = "NotSupportedException"></exception>
<exception cref = "PathTooLongException"></exception>
<exception cref = "UnauthorizedAccessException"></exception> */

public static void CompressFile(string inputPath, string outputPath, int blockFactor, 
int bufferSize, FileSystemSearchParams searchParams)
{
PathHelper.AddExtension(ref outputPath, TarExt);

string[] filesList = DirManager.GetEntryNames(inputPath, searchParams);

using FileStream outputFile = File.OpenWrite(outputPath);
using TarOutputStream compressionStream = CompressStream(outputFile, blockFactor, bufferSize, filesList);
}

/** <summary> Extracts a Entry from the Specified <c>TarInputStream</c>. </summary>

<param name = "sourceStream"> The Stream that Contains the Tar Entry to Extract. </param>
<param name = "targetPath"> The Path where the Extracted Entry should be Saved. </param> */

public static void ExtractTarEntry(TarInputStream sourceStream, string targetPath, TarEntry fileEntry, int bufferSize)
{

if(fileEntry.IsDirectory)
{
string parentPath = Path.GetDirectoryName(targetPath);
DirManager.CheckMissingFolder(parentPath);
}

else
{
using FileStream outputFile = File.OpenWrite(targetPath);
FileManager.ProcessBuffer(sourceStream, outputFile, bufferSize);
}

}

/** <summary> Decompresses the Contents of a File by using Tar Compression. </summary>

<param name = "inputPath"> The Path where the File to be Decompressed is Located. </param>
<param name = "outputPath"> The Location where the Decompressed contents will be Saved. </param>

<exception cref = "ArgumentException"></exception>
<exception cref = "ArgumentNullException"></exception>
<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception>
<exception cref = "NotSupportedException"></exception>
<exception cref = "PathTooLongException"></exception>
<exception cref = "SecurityException"></exception>
<exception cref = "UnauthorizedAccessException"></exception> */

public static void DecompressFile(string inputPath, string outputPath, int bufferSize)
{
using FileStream inputFile = File.OpenRead(inputPath);
using TarInputStream decompressionStream = new(inputFile);

TarEntry fileEntry;

while( (fileEntry = decompressionStream.GetNextEntry() ) != null)
{
string filePath = outputPath + Path.DirectorySeparatorChar + fileEntry.Name;

ExtractTarEntry(decompressionStream, filePath, fileEntry, bufferSize);
}

}

}

}
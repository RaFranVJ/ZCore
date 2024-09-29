using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using ZCore.Serializables.ArgumentsInfo.Compressor.Zip;
using ZCore.Serializables.ArgumentsInfo.FileManager;


namespace ZCore.Modules.FileCompressors
{
/// <summary> Initializes Compressing Tasks for Files by using the Zip algorithm. </summary>

public static class ZipCompressor
{
/// <summary> The ZIP Extension </summary>

private const string ZipExt = ".zip";

/** <summary> Adds a new Entry to the Specified <c>ZipArchive</c>. </summary>

<param name = "sourcePath"> The Path where the Entry to Add is Located. </param>
<param name = "targetStream"> The Stream where the Zip Entry will be Added. </param> */

public static void AddZipEntry(string pathToEntry, ZipArchive zipFile, Stream targetStream,
ZipEntriesInfo entryCfg = default)
{
entryCfg ??= new();

var fileEntry = zipFile.CreateEntry(pathToEntry, entryCfg.CompressionLvl);

if(!string.IsNullOrEmpty(entryCfg.OptionalEntryComment) )
fileEntry.Comment = entryCfg.OptionalEntryComment;

fileEntry.ExternalAttributes = entryCfg.ExternalOSAttributes;

if(entryCfg.LastWriteTime != null)
fileEntry.LastWriteTime = (DateTime)entryCfg.LastWriteTime;

using Stream entryStream = fileEntry.Open();

FileManager.ProcessBuffer(entryStream, targetStream, entryCfg.BufferSizeForIOTasks);
}

// Get ZIP Archive

public static ZipArchive CompressStream(Stream target, ZipArchiveMode zipMode, Encoding encoding, 
ZipEntriesInfo entriesInfo, string comment = default, params string[] entryNames)
{
ZipArchive zipFile = new(target, zipMode, true, encoding);

if(!string.IsNullOrEmpty(comment) )
zipFile.Comment = comment;

foreach(string name in entryNames)
AddZipEntry(name, zipFile, target, entriesInfo);

return zipFile;
}

/** <summary> Compresses the Contents of a File by using Zip Compression. </summary>

<param name = "inputPath"> The Path where the File to be Compressed is Located. </param>
<param name = "outputPath"> The Location where the Compressed File will be Saved. </param>

<exception cref = "ArgumentException"></exception>
<exception cref = "ArgumentNullException"></exception>
<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception>
<exception cref = "NotSupportedException"></exception>
<exception cref = "PathTooLongException"></exception>
<exception cref = "SecurityException"></exception>
<exception cref = "UnauthorizedAccessException"></exception> */

public static void CompressFile(string inputPath, string outputPath, ZipArchiveMode zipMode, string encodingStr,
string comment, ZipEntriesInfo entriesInfo, FileSystemSearchParams filesFilter = null)
{
PathHelper.AddExtension(ref outputPath, ZipExt);

var encoding = EncodeHelper.GetEncodingType(encodingStr);
string[] filesList = DirManager.GetEntryNames(inputPath, filesFilter);

using FileStream outputFile = File.OpenWrite(outputPath);
using ZipArchive compressionStream = CompressStream(outputFile, zipMode, encoding, entriesInfo, comment, filesList);
}

/** <summary> Extracts the specified <c>ZipArchiveEntry</c> to a new Location. </summary>

<param name = "sourceEntry"> The Entry to Extract. </param>
<param name = "targetPath"> The Path where the Extracted Entry should be Saved. </param> */

private static void ExtractZipEntry(ZipArchiveEntry sourceEntry, string targetPath, int bufferSize)
{
DirManager.CheckMissingFolder(Path.GetDirectoryName(targetPath) );

using FileStream outputFile = File.OpenWrite(targetPath);
using Stream entryStream = sourceEntry.Open();

FileManager.ProcessBuffer(entryStream, outputFile, bufferSize);
}

/** <summary> Decompresses the Contents of a File by using Zip Compression. </summary>

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

public static void DecompressFile(string inputPath, string outputPath, ZipArchiveMode zipMode, string encodingStr,
int bufferSize)
{
zipMode = (zipMode != ZipArchiveMode.Create) ? zipMode : ZipArchiveMode.Read;

using FileStream inputFile = File.OpenRead(inputPath);
var encoding = EncodeHelper.GetEncodingType(encodingStr);

using ZipArchive decompressionStream = new(inputFile, zipMode, false, encoding);

foreach(ZipArchiveEntry entry in decompressionStream.Entries)
{
string filePath = outputPath + Path.DirectorySeparatorChar + entry.Name;

ExtractZipEntry(entry, filePath, bufferSize);
}

}

}

}
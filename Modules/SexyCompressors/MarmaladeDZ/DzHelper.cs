using ICSharpCode.SharpZipLib.BZip2;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using ZCore.Modules.FileCompressors;
using ZCore.Modules.SexyCompressors.MarmaladeDZ.Definitions;
using ZCore.Serializables.ArgumentsInfo.Compressor;
using ZCore.Serializables.ArgumentsInfo.Compressor.BZip2;
using ZCore.Serializables.ArgumentsInfo.Compressor.Lzma;
using ZCore.Serializables.ArgumentsInfo.SexyCompressor.Dz;

namespace ZCore.Modules.SexyCompressors.MarmaladeDZ
{
/// <summary> A useful Class used for Handling the Data inside DTRZ Streams. </summary>

public static class DzHelper
{
/// <summary> Gets the Compression Method related to the given FileName. </summary>

private static CompressionFlags GetCompressionMethod(Dictionary<string, CompressionFlags> mappedExts, string fileName)
{
string fileExt = Path.GetExtension(fileName).ToLower();

if(mappedExts.ContainsKey(fileExt))
return mappedExts[fileExt];
	
return default;
}

// Gets a Pool of Strings related to DirNames

public static StringPool GetDirPool(string dirName, string[] fileNames, string[] pathNames, params string[] entryNames)
{
StringPool dirPool = new();
int dirNameIndex = dirName.Length + 1;

dirPool.ThrowInPool(string.Empty);
				
for(int i = 0; i < entryNames.Length; i++)
{
fileNames[i] = Path.GetFileName(entryNames[i] );
string baseDir = Path.GetDirectoryName(entryNames[i] ) ?? string.Empty;

pathNames[i] = dirNameIndex > baseDir.Length ? string.Empty : PathHelper.FormatWindowsPath(baseDir[dirNameIndex..] );
dirPool.ThrowInPool(pathNames[i] );
}

return dirPool;
}

// Compress or Decompress a Dz Entry for a DZ Stream (not fully Supported)

private static void ProcessDzEntry(BinaryStream sourceStream, string entryName, int bufferSize, 
ChunkInfo chunkInfo, bool operationMode)
{
// Add the DZ Entry to the Stream

if(operationMode)
{
using BinaryStream targetStream = BinaryStream.Open(entryName);
                        
chunkInfo.ChunkSize = (int)targetStream.Length;
chunkInfo.ZLibSizeForDecompression = (int)targetStream.Length;
							
FileManager.ProcessBuffer(targetStream, sourceStream, bufferSize);               
}

// Extract the DZ Entry from the Stream

else
{
using BinaryStream targetStream = BinaryStream.OpenWrite(entryName);

FileManager.ProcessBuffer(sourceStream, targetStream, bufferSize, chunkInfo.ZLibSizeForDecompression);                          
}

}

// Compress or Decompress a ZLib Entry for a DZ Stream

private static void ProcessZLibEntry(BinaryStream sourceStream, string entryName, GenericCompressorInfo zlibCfg, 
ChunkInfo chunkInfo, bool operationMode)
{
// Add the ZLib Entry to the Stream

if(operationMode)
{     
using BinaryStream targetStream = new();  
using BinaryStream entryStream = BinaryStream.Open(entryName);

chunkInfo.ChunkSize = (int)entryStream.Length;
chunkInfo.ZLibSizeForDecompression = (int)entryStream.Length;
		
using GZipStream gzStream = GZipCompressor.CompressStream(entryStream, targetStream, zlibCfg.CompressionLvl,
zlibCfg.BufferSizeForIOTasks);			

targetStream.Position = 0;

FileManager.ProcessBuffer(targetStream, sourceStream, zlibCfg.BufferSizeForIOTasks);     
}

// Extract the ZLib Entry from the Stream

else
{
using BinaryStream targetStream = new();
FileManager.ProcessBuffer(sourceStream, targetStream, zlibCfg.BufferSizeForIOTasks, chunkInfo.ZLibSizeForCompression);

targetStream.Position = 0;

using GZipStream gzStream = new(targetStream, CompressionMode.Decompress);
using Stream entryStream = GZipCompressor.DecompressStream(gzStream, zlibCfg.BufferSizeForIOTasks, entryName);                               
}

}

// Compress or Decompress a BZip2 Entry for a DZ Stream

private static void ProcessBZip2Entry(BinaryStream sourceStream, string entryName, BZip2Settings bz2Cfg,
ChunkInfo chunkInfo, bool operationMode)
{
// Add the BZip2 Entry to the Stream

if(operationMode)
{     
using BinaryStream targetStream = new();       
using BinaryStream entryStream = BinaryStream.Open(entryName);

chunkInfo.ChunkSize = (int)entryStream.Length;
chunkInfo.ZLibSizeForDecompression = (int)entryStream.Length;

using BZip2OutputStream bz2Stream = BZip2Compressor.CompressStream(entryStream, targetStream,
bz2Cfg.BlockSizeForCompression, bz2Cfg.BufferSizeForIOTasks);

targetStream.Position = 0;

FileManager.ProcessBuffer(targetStream, sourceStream, bz2Cfg.BufferSizeForIOTasks);     
}

// Extract the BZip2 Entry from the Stream

else
{
using BinaryStream targetStream = new();
FileManager.ProcessBuffer(sourceStream, targetStream, bz2Cfg.BufferSizeForIOTasks, chunkInfo.ZLibSizeForCompression);

targetStream.Position = 0;
using BZip2InputStream bz2Stream = new(targetStream);

BZip2Compressor.DecompressStream(bz2Stream, bz2Cfg.BufferSizeForIOTasks, entryName);                                             
}

}

// Compress or Decompress a Entry that only has Zeroes for a DZ Stream

private static void ProcessEmptyEntry(BinaryStream sourceStream, string entryName, ChunkInfo chunkInfo, bool operationMode, uint? zeroBytes = null)
{
// Add the Entry to the Stream

if(operationMode)
{     
using BinaryStream targetStream = BinaryStream.Open(entryName);

chunkInfo.ChunkSize = (int)targetStream.Length;
chunkInfo.ZLibSizeForDecompression = 0;                      
}

// Extract the Entry from the Stream

else
{
int bytesToWrite = chunkInfo.ChunkSize / (int)zeroBytes;
int additionalBytes = chunkInfo.ChunkSize % (int)zeroBytes;

byte[] paddingBytes = new byte[ (int)zeroBytes];
using BinaryStream targetStream = BinaryStream.OpenWrite(entryName);
                            
for(int i = 0; i < bytesToWrite; i++)
targetStream.Write(paddingBytes, 0, (int)zeroBytes);

if(additionalBytes != 0)
targetStream.Write(paddingBytes, 0, additionalBytes);
                                                    
}

}

// Compress or Decompress a Entry that only has ReadOnly permissions for a DZ Stream

private static void ProcessStoreEntry(BinaryStream sourceStream, string entryName, int bufferSize, ChunkInfo chunkInfo, bool operationMode)
{
// Add the Entry to the Stream

if(operationMode)
{     
using BinaryStream targetStream = BinaryStream.Open(entryName);
                        
chunkInfo.ChunkSize = (int)targetStream.Length;
chunkInfo.ZLibSizeForDecompression = (int)targetStream.Length;

FileManager.ProcessBuffer(targetStream, sourceStream, bufferSize);                                     
}

// Extract the Entry from the Stream

else
{
using BinaryStream targetStream = BinaryStream.OpenWrite(entryName);

FileManager.ProcessBuffer(sourceStream, targetStream, bufferSize, chunkInfo.ChunkSize);                                                                     
}

}

// Compress or Decompress a LZMA Entry for a DZ Stream

private static void ProcessLzmaEntry(BinaryStream sourceStream, string entryName, int bufferSize,
LzmaSettings lzmaCfg, ChunkInfo chunkInfo, bool operationMode)
{
// Add the LZMA Entry to the Stream

if(operationMode)
{
using BinaryStream entryStream = BinaryStream.Open(entryName);

using BinaryStream targetStream = LzmaCompressor.CompressStream(entryStream, lzmaCfg.UseSizeInfo, lzmaCfg.InputDataSize,
lzmaCfg.OutputDataSize, lzmaCfg.IDSForCoderProps, lzmaCfg.CoderProperties, lzmaCfg.BytesOrderForSizeInfo, entryName);

chunkInfo.ChunkSize = (int)entryStream.Length;
chunkInfo.ZLibSizeForDecompression = (int)entryStream.Length;
                         
targetStream.Position = 0;
FileManager.ProcessBuffer(targetStream, sourceStream, bufferSize);                                   
}

// Extract the LZMA Entry from the Stream

else
{

using Stream targetStream = LzmaCompressor.DecompressStream(sourceStream, lzmaCfg.UseSizeInfo, chunkInfo.ZLibSizeForCompression - 13,
lzmaCfg.OutputDataSize, lzmaCfg.PropsCountForDecompression, lzmaCfg.BytesOrderForSizeInfo, entryName);
                                                   
}

}

/// <summary> Adds a Entry to the given DZ Stream by evaluating its Compression Method. </summary>

public static void AddDzEntry(BinaryStream sourceStream, Dictionary<string, CompressionFlags> mappedExts, string entryName,
ChunkInfo chunkInfo, DzEntryParams entryParams)
{
var compressionMethod = GetCompressionMethod(mappedExts, entryName);

chunkInfo.CompressionType = compressionMethod;
chunkInfo.ChunkOffset = (int)sourceStream.Position;

if( (compressionMethod & CompressionFlags.Dz) != 0)
{
chunkInfo.CompressionType = (chunkInfo.CompressionType & ~CompressionFlags.Dz) | CompressionFlags.ReadOnly;

ProcessDzEntry(sourceStream, entryName, entryParams.BufferSizeForIOTasks, chunkInfo, true);
}

else if( (compressionMethod & CompressionFlags.ZLib) != 0)
ProcessZLibEntry(sourceStream, entryName, entryParams.ConfigForZLibEntries, chunkInfo, true);

else if( (compressionMethod & CompressionFlags.BZip2) != 0)
ProcessBZip2Entry(sourceStream, entryName, entryParams.ConfigForBZip2Entries, chunkInfo, true);
					
else if( (compressionMethod & CompressionFlags.ZeroBytes) != 0)
ProcessEmptyEntry(sourceStream, entryName, chunkInfo, true);

else if( (compressionMethod & CompressionFlags.ReadOnly) != 0)
ProcessStoreEntry(sourceStream, entryName, entryParams.BufferSizeForIOTasks, chunkInfo, true);

else if( (compressionMethod & CompressionFlags.Lzma) != 0)
ProcessLzmaEntry(sourceStream, entryName, entryParams.BufferSizeForIOTasks, entryParams.ConfigForLzmaEntries, chunkInfo, true);

else
{
chunkInfo.CompressionType = compressionMethod | CompressionFlags.ReadOnly;

ProcessStoreEntry(sourceStream, entryName, entryParams.BufferSizeForIOTasks, chunkInfo, true);
}

}

/// <summary> Extracts a Entry from the given DZ Stream by evaluating its Compression Method. </summary>

public static void ExtractDzEntry(BinaryStream sourceStream, string entryName, ChunkInfo chunkInfo, DzEntryParams entryParams)
{
DirManager.CheckMissingFolder(Path.GetDirectoryName(entryName) );
                        
sourceStream.Position = chunkInfo.ChunkOffset;

if(chunkInfo.ZLibSizeForCompression == -1)
chunkInfo.ZLibSizeForCompression = (int)(sourceStream.Length - sourceStream.Position);
                   					
CompressionFlags compressionMethod = chunkInfo.CompressionType;

if( (compressionMethod & CompressionFlags.Dz) != 0)
ProcessDzEntry(sourceStream, entryName, entryParams.BufferSizeForIOTasks, chunkInfo, false);

else if( (compressionMethod & CompressionFlags.ZLib) != 0)
ProcessZLibEntry(sourceStream, entryName, entryParams.ConfigForZLibEntries, chunkInfo, false);

else if( (compressionMethod & CompressionFlags.BZip2) != 0)
ProcessBZip2Entry(sourceStream, entryName, entryParams.ConfigForBZip2Entries, chunkInfo, false);
					
else if( (compressionMethod & CompressionFlags.ZeroBytes) != 0)
ProcessEmptyEntry(sourceStream, entryName, chunkInfo, false);

else if( (compressionMethod & CompressionFlags.ReadOnly) != 0)
ProcessStoreEntry(sourceStream, entryName, entryParams.BufferSizeForIOTasks, chunkInfo, false);

else if( (compressionMethod & CompressionFlags.Lzma) != 0)
ProcessLzmaEntry(sourceStream, entryName, entryParams.BufferSizeForIOTasks, entryParams.ConfigForLzmaEntries, chunkInfo, false);

else
ProcessStoreEntry(sourceStream, entryName, entryParams.BufferSizeForIOTasks, chunkInfo, false);

}

}

}
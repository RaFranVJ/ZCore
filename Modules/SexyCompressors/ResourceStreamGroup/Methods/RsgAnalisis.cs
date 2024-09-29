using System.IO;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using ZCore.Modules.FileCompressors;
using ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions;
using ZCore.Modules.SexyCompressors.ResourceStreamGroup.Definitions;
using ZCore.Modules.SexyCompressors.ResourceStreamGroup.Exceptions;

namespace ZCore.Modules.SexyCompressors.ResourceStreamGroup.Methods
{
/// <summary> Initializes Analisis on RSG Files. </summary>

public static class RsgAnalisis
{
// ZLib Headers in RSG

private static readonly byte[,] ZLibHeaders =
{

{ 0x78, 0x1 },
{ 0x78, 0x5E },
{ 0x78, 0x9C },
{ 0x78, 0xDA }

};

// Compare PacketInfo in RSB and in its Group

public static void ComparePackets(RsbPacketInfo sourceInfo, string filePath, BinaryStream rsgStream, bool adaptVer)
{		
var groupInfo = RsgCompressor.DecompressStream(rsgStream, 0, default, false, true, adaptVer: adaptVer);

if(groupInfo.PacketVersion != sourceInfo.PacketVersion)
throw new PacketInfo_VersionMismatchException(sourceInfo.PacketVersion, groupInfo.PacketVersion, filePath);

if(groupInfo.CompressionFlags != sourceInfo.CompressionFlags)
throw new PacketInfo_CompressionFlagsMismatchException(sourceInfo.CompressionFlags, groupInfo.CompressionFlags, filePath);

if(groupInfo.ResInfo.Count != sourceInfo.ResInfo.Count)
throw new PacketInfo_ResInfoMismatchException(sourceInfo.ResInfo.Count, groupInfo.ResInfo.Count, filePath);

groupInfo.ResInfo.Sort( (a, b) => a.PathToResFile.CompareTo(b.PathToResFile) );
sourceInfo.ResInfo.Sort( (a, b) => a.PathToResFile.CompareTo(b.PathToResFile) );

for(int i = 0; i < sourceInfo.ResInfo.Count; i++)
{

if(groupInfo.ResInfo[i].PathToResFile != sourceInfo.ResInfo[i].PathToResFile)
throw new ResInfo_PathMismatchException(sourceInfo.ResInfo[i].PathToResFile, groupInfo.ResInfo[i].PathToResFile, filePath);

if(groupInfo.ResInfo[i].ImageInfo != null && sourceInfo.ResInfo[i].ImageInfo != null)
{

if(groupInfo.ResInfo[i].ImageInfo.TextureID != sourceInfo.ResInfo[i].ImageInfo.TextureID)
throw new PtxInfo_IdMismatchException(sourceInfo.ResInfo[i].ImageInfo.TextureID,
groupInfo.ResInfo[i].ImageInfo.TextureID, filePath);

if(groupInfo.ResInfo[i].ImageInfo.TextureWidth != sourceInfo.ResInfo[i].ImageInfo.TextureWidth)
throw new InvalidPacketWidthException(groupInfo.ResInfo[i].ImageInfo.TextureWidth, 
sourceInfo.ResInfo[i].ImageInfo.TextureWidth, filePath);

if(groupInfo.ResInfo[i].ImageInfo.TextureHeight != sourceInfo.ResInfo[i].ImageInfo.TextureHeight)
throw new InvalidPacketHeightException(groupInfo.ResInfo[i].ImageInfo.TextureHeight,
sourceInfo.ResInfo[i].ImageInfo.TextureHeight, filePath);

}

}

}

// Check if Header read match the expected ones

private static bool IsZLibHeader(byte[] headerBytes)
{
	
for(int i = 0; i < ZLibHeaders.GetLength(0); i++)
{
	
if(headerBytes[0] == ZLibHeaders[i, 0] && headerBytes[1] == ZLibHeaders[i, 1] )
return false;
            
}

return true;
}

// Decompress Stream with Deflate or get Plain Stream instead

public static BinaryStream GetStream(BinaryStream sourceStream, RsgInfo fileInfo, bool hasAtlasInfo, int bufferSize)
{
BinaryStream targetStream = new();

if(hasAtlasInfo)
{
sourceStream.Position = fileInfo.OffsetToPart1;

byte[] headerBytes = sourceStream.ReadBytes(2);

if(fileInfo.CompressionFlags == 0 || fileInfo.CompressionFlags == 2 || IsZLibHeader(headerBytes) )
{
sourceStream.Position = fileInfo.OffsetToPart1;

FileManager.ProcessBuffer(sourceStream, targetStream, bufferSize, (int)fileInfo.Part1_Size);
}
		
else
targetStream = DecompressStream(sourceStream, (int)fileInfo.Part1_SizeCompressed, fileInfo.OffsetToPart1, bufferSize);

}

else
{
sourceStream.Position = fileInfo.OffsetToPart0;

byte[] headerBytes = sourceStream.ReadBytes(2);	

if(fileInfo.CompressionFlags < 2 || IsZLibHeader(headerBytes) )
{
sourceStream.Position = fileInfo.OffsetToPart0;

FileManager.ProcessBuffer(sourceStream, targetStream, bufferSize, (int)fileInfo.Part0_Size);
}

else
targetStream = DecompressStream(sourceStream, (int)fileInfo.Part0_SizeCompressed, fileInfo.OffsetToPart0, bufferSize);

}

targetStream.Position = 0;

return targetStream;
}

// Get Decompressed Stream

private static BinaryStream DecompressStream(BinaryStream targetStream, int sizeCompressed, uint offset, int bufferSize)
{
targetStream.Position = offset;

using MemoryStream compressedStream = new();
FileManager.ProcessBuffer(targetStream, compressedStream, bufferSize, sizeCompressed);

compressedStream.Position = 0;

using InflaterInputStream inflaterStream = new(compressedStream, new(), bufferSize);
inflaterStream.IsStreamOwner = false; // Don't Close baseStream

using MemoryStream decompressedStream = new();
FileManager.ProcessBuffer(inflaterStream, decompressedStream, bufferSize);

return new(decompressedStream);
}

}

}
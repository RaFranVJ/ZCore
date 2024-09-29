using System;
using System.Collections.Generic;
using System.Text;
using ZCore.Serializables.ArgumentsInfo.FileManager.Archive;
using ZCore.Serializables.ArgumentsInfo.SexyCompressor.Dz;

namespace ZCore.Modules.SexyCompressors.MarmaladeDZ.Definitions
{
/// <summary> Represents Info for DTRZ Packages. </summary>

public class DtrzInfo : MetaModel<DtrzInfo>
{
/** <summary> Gets or Sets a Value that Determines the amount of FileNames available in the Stream. </summary>
<returns> The Number of FileNames. </returns> */

public ushort NumberOfFileNames{ get; set; }

/** <summary> Gets or Sets a Value that Determines the amount of DirNames available in the Stream. </summary>
<returns> The Number of DirNames. </returns> */

public ushort NumberOfDirNames{ get; set; }

/** <summary> Gets or Sets the Version of a DTRZ File. </summary>
<returns> The DZ Version. </returns> */

public byte FileVersion{ get; set; }

/** <summary> Gets or Sets a Collection of Names related to Files Stored in the Stream. </summary>
<returns> The File Names. </returns> */

public string[] FileNames{ get; set; }

/** <summary> Gets or Sets a Collection of Names related to Dirs Stored in the Stream. </summary>
<returns> The Dir Names. </returns> */

public string[] DirNames{ get; set; }

/** <summary> Gets or Sets some Info related to the Chunks of the Files Compressed in the DZ Stream. </summary>
<returns> The DZ Chunks. </returns> */

public ChunkInfo[] Chunks{ get; set; }

/** <summary> Gets or Sets a Value that Determines the amount of Archives available in the Stream. </summary>
<returns> The Files Count. </returns> */

public ushort FilesCount{ get; set; }

/** <summary> Gets or Sets a Value that Determines the amount of Chunks available in the Stream. </summary>
<returns> The Chunks Count. </returns> */
		
public ushort ChunksCount{ get; set; }

/** <summary> Gets or Sets a Collection of Names related to Chunks Stored in the Stream. </summary>
<returns> The Chunk Names. </returns> */
		
public string[] ChunkNames{ get; set; }

/// <summary> Creates a new Instance of the <c>DtrzInfo</c>. </summary>

public DtrzInfo()
{
}

/// <summary> Creates a new Instance of the <c>DtrzInfo</c>. </summary>

public DtrzInfo(int filesCount, int dirsCount, params string[] fileNames)
{
NumberOfFileNames = (ushort)filesCount;
NumberOfDirNames = (ushort)dirsCount;

FileNames = fileNames;
DirNames = new string[dirsCount];
}

/// <summary> Updates the File Names inside the given DtrzInfo. </summary>

private static void UpdateFileNames(BinaryStream sourceStream, DtrzInfo targetInfo, Encoding encoding, Endian endian)
{
targetInfo.FileNames = new string[targetInfo.NumberOfFileNames];

for(ushort i = 0; i < targetInfo.NumberOfFileNames; i++)        
targetInfo.FileNames[i] = sourceStream.ReadStringUntilZero(encoding, endian);

}

/// <summary> Updates the Dir Names inside the given DtrzInfo. </summary>

private static void UpdateDirNames(BinaryStream sourceStream, DtrzInfo targetInfo, Encoding encoding, Endian endian)
{
targetInfo.DirNames = new string[targetInfo.NumberOfDirNames];

targetInfo.DirNames[0] = string.Empty; // Root Dir has no Name (also its Ignored)
			
for(ushort i = 1; i < targetInfo.NumberOfDirNames; i++)
targetInfo.DirNames[i] = sourceStream.ReadStringUntilZero(encoding, endian);

}

/// <summary> Creates a Collection of Chunks inside the given DtrzInfo. </summary>

private static void CreateChunks(BinaryStream sourceStream, DtrzInfo targetInfo, Endian endian, ushort maxChunkIndex, ref int chunksCount)
{
List<ChunkInfo> tempChunks = new();

// Add Chunks to a Temporal List

for(ushort i = 0; i < targetInfo.NumberOfFileNames; i++)
{
ushort dirIndex = sourceStream.ReadUShort(endian);
ushort chunkIndex;

int multiIndex = 0;

// Keep Adding Chunks until End of ChunkList is Reached

while( (chunkIndex = sourceStream.ReadUShort(endian) ) != maxChunkIndex) // Max Index is ushort.MaValue
{
tempChunks.Add( new(dirIndex, i, chunkIndex, multiIndex++) );

if(chunkIndex > chunksCount)
chunksCount = chunkIndex;

}

}

chunksCount++;

targetInfo.Chunks = new ChunkInfo[chunksCount];

// Now, Add the Temp Chunks inside the ones from the DtrzInfo

foreach(ChunkInfo item in tempChunks)
targetInfo.Chunks[item.ChunkIndex] = item;

}

/** <summary> Obtains a Collection of Chunks used further for Updating the ones inside DtrzInfo. </summary>

<remarks> This is Useful for Adding Support for Unpacking Multiple DZ Streams, one inside another. </remarks> */

private static List<ChunkInfo>[] GetChunksCollection(BinaryStream sourceStream, DtrzInfo targetInfo, Endian endian, int chunksCount)
{
targetInfo.FilesCount = sourceStream.ReadUShort(endian);

List<ChunkInfo>[] chunksCollection = new List<ChunkInfo>[targetInfo.FilesCount];

// Init the Collection with Default Values

for(int i = 0; i < targetInfo.FilesCount; i++)
chunksCollection[i] = new();

targetInfo.ChunksCount = sourceStream.ReadUShort(endian);

// Now, Update eah Element of the Collection

for(int i = 0; i < chunksCount; i++)
{
targetInfo.Chunks[i] = ChunkInfo.Read(sourceStream, endian);
				
chunksCollection[targetInfo.Chunks[i].FileIndex].Add(targetInfo.Chunks[i] );
}

return chunksCollection;
}

/// <summary> Calculates the Size of each Chunk obtained in the Collection. </summary>

private static void CalculateChunksSize(BinaryStream sourceStream, DtrzInfo targetInfo, Endian endian, int chunksCount)
{
var chunksCollection = GetChunksCollection(sourceStream, targetInfo, endian, chunksCount);
		
foreach(List<ChunkInfo> chunksList in chunksCollection)
{
int listSize = chunksList.Count - 1;
chunksList.Sort( (a, b) => a.ChunkOffset - b.ChunkOffset);
 
for(int i = 0; i < listSize; i++)         
chunksList[i].ZLibSizeForCompression = chunksList[i + 1].ChunkOffset - chunksList[i].ChunkOffset;
              
if( (chunksList[listSize].CompressionType & CompressionFlags.ReadOnly) != 0)
chunksList[listSize].ZLibSizeForCompression = chunksList[listSize].ChunkSize;

else
chunksList[listSize].ZLibSizeForCompression = -1; // will be set soon (Extracted from PopStudio)

// Now, Update the Chunks with the one with Calculated Size

foreach(ChunkInfo chunk in chunksList)
targetInfo.Chunks[chunk.ChunkIndex] = chunk;

}

}

/// <summary> Updates the ChunkNames inside the given DtrzInfo. </summary>

private static void UpdateChunkNames(BinaryStream sourceStream, DtrzInfo targetInfo, Encoding encoding, Endian endian)
{
targetInfo.ChunkNames = new string[targetInfo.FilesCount];
targetInfo.ChunkNames[0] = null; // First Chunk has no Name (also its Ignored)

for(int i = 1; i < targetInfo.FilesCount; i++)
targetInfo.ChunkNames[i] = sourceStream.ReadStringUntilZero(encoding, endian);

}

// Updates the DzInfo inside the Stream (including DirNames and Chunks)

public void UpdateAll(StringPool dirPool, string[] pathNames, params string[] entryNames)
{

for(int i = 0; i < dirPool.Length; i++)
DirNames[i] = dirPool[i].Value;
         
Chunks = new ChunkInfo[entryNames.Length];

for(ushort i = 0; i < entryNames.Length; i++)
Chunks[i] = new( (ushort)dirPool[pathNames[i] ].Index, i, i);

}

/** <summary> Reads all the Parts of DTRZ Info from a Stream. </summary>

<param name = "sourceStream" > The Stream to be Read. </param>

<returns> The DtrzInfo. </returns> */
		
public static DtrzInfo ReadAllParts(BinaryStream sourceStream, Endian endian, Encoding encoding,
bool adaptVer, ushort maxChunkIndex)
{

DtrzInfo dzInfo = new()
{
NumberOfFileNames = sourceStream.ReadUShort(endian),
NumberOfDirNames = sourceStream.ReadUShort(endian),
FileVersion = DtrzVersion.Read(sourceStream, adaptVer)
};
	
UpdateFileNames(sourceStream, dzInfo, encoding, endian);
UpdateDirNames(sourceStream, dzInfo, encoding, endian);

int chunksCount = 0;
CreateChunks(sourceStream, dzInfo, endian, maxChunkIndex, ref chunksCount);

CalculateChunksSize(sourceStream, dzInfo, endian, chunksCount);
UpdateChunkNames(sourceStream, dzInfo, encoding, endian);

return dzInfo;
}

/** <summary> Writes the first Part of DTRZ Info to a Stream. </summary>

<param name = "sourceStream" > The Stream to be Read. </param> */

public void WritePart1(BinaryStream targetStream, Endian endian, Encoding encoding,
FileVersionDetails<byte> versionInfo, ushort maxChunkIndex)
{
NumberOfFileNames = (ushort)FileNames.Length;
NumberOfDirNames = (ushort)DirNames.Length;

FileVersion = versionInfo.VersionNumber;

targetStream.WriteUShort(NumberOfFileNames, endian);
targetStream.WriteUShort(NumberOfDirNames, endian);

DtrzVersion.Write(targetStream, (byte)versionInfo.VersionNumber, versionInfo.AdaptCompatibilityBetweenVersions);
	
for(int i = 0; i < NumberOfFileNames; i++)     
targetStream.WriteStringUntilZero(FileNames[i], encoding, endian);
            
for(int i = 1; i < NumberOfDirNames; i++) 
targetStream.WriteStringUntilZero(DirNames[i], encoding, endian);
     
int matchCount = Chunks.Length;
Array.Sort(Chunks, (a, b) => a.FileNameIndex - b.FileNameIndex);

for(int i = 0; i < matchCount; i++)
{
targetStream.WriteUShort(Chunks[i].DirNameIndex, endian);
targetStream.WriteUShort(Chunks[i].ChunkIndex, endian);

targetStream.WriteUShort(maxChunkIndex, endian);
}
          
targetStream.WriteUShort(1, endian);
targetStream.WriteUShort( (ushort)matchCount, endian);
}

/** <summary> Writes the second Part of DTRZ Info to a Stream. </summary>

<param name = "sourceStream" > The Stream to be Read. </param> */

public void WritePart2(BinaryStream targetStream, Endian endian)
{
int matchCount = Chunks.Length;

for(int i = 0; i < matchCount; i++)        
Chunks[i].Write(targetStream, endian);
          
}

}

}
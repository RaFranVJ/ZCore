using System.Collections.Generic;
using System.IO;
using ZCore.Serializables.ArgumentsInfo.SexyCompressor.ArcV;

namespace ZCore.Modules.SexyCompressors.ArcVPackage.Definitions
{
/// <summary> Groups all the Info related to Resources inside the ARCV Stream. </summary>

public class ArcvInfoContainer : MetaModel<ArcvInfoContainer>
{
/** <summary> Gets or Sets some Info Related to th ARCV Resources. </summary>
<returns> The Res Info. </returns> */

public List<ArcvResInfo> ResInfo{ get; set; }

/// <summary> Creates a new Instance of the <c>ArcvInfoContainer</c>. </summary>

public ArcvInfoContainer()
{
ResInfo = new();
}

/** <summary> Creates a new Instance of the <c>ArcvInfoContainer</c> with the given Parameters. </summary>

<param name = "pos"> The File Position. </param>
<param name = "size"> The File Size. </param>
 */

public ArcvInfoContainer(params ArcvResInfo[] info)
{
ResInfo = new(info);
}

/** <summary> Loads the Data from the given <c>ArcvResInfo</c>. </summary>

<param name = "filePath"> The Path to the File where the ARCV Info should be Saved. </param>
<param name = "fileInfo"> The Data to Save. </param> */

// Add a Resource into an ARCV Stream

public static void AddResource(string sourcePath, BinaryStream targetStream, int bufferSize)
{
using BinaryStream entryStream = BinaryStream.Open(sourcePath);

FileManager.ProcessBuffer(entryStream, targetStream, bufferSize);
}

// Add a Resource into an ARCV Stream and returns the Info related to it

public static void AddResource(ArcvResEntry sourceEntry, List<ArcvResInfo> resInfo, BinaryStream targetStream, int bufferSize)
{
using BinaryStream entryStream = BinaryStream.Open(sourceEntry.PathToResource);

resInfo.Add( new(targetStream.Position, entryStream.Length, sourceEntry.CRC32) );

FileManager.ProcessBuffer(entryStream, targetStream, bufferSize);
}

// Extract a Resource from a ARCV Stream

public static void ExtractResource(BinaryStream sourceStream, string targetPath, long resOffset, int resSize, int bufferSize)
{
long posBackup = sourceStream.Position;
sourceStream.Position = resOffset;

using MemoryStream bufferStream = new( sourceStream.ReadBytes(resSize) );
using BinaryStream resStream = BinaryStream.OpenWrite(targetPath);

FileManager.ProcessBuffer(bufferStream, resStream, bufferSize);
sourceStream.Position = posBackup;
}

// Get Res Info from Dir

public static List<ArcvResInfo> GetResInfoFromDir(BinaryStream targetStream, int bufferSize, ArcvStyleInfo styleInfo, params string[] entryNames)
{
List<ArcvResInfo> resInfo = new();

var resEntries = ArcvResEntry.BuildEntries(entryNames);

// Write Bytes of each Res inside the ARCV Stream (and Apply Padding if Necessary)

for(int i = 0; i < resEntries.Count; i++)
{
ArcvCompressor.PadStream(targetStream, (int)styleInfo.BytesIdentation, styleInfo.PaddingByte);

AddResource(resEntries[i], resInfo, targetStream, bufferSize);
}

return resInfo;
}

// Get Res Info from JSON

public static List<ArcvResInfo> GetResInfoFromJson(BinaryStream targetStream, int bufferSize, ArcvStyleInfo styleInfo, 
string pathToResInfo, params string[] entryNames)
{
ArcvInfoContainer infoContainer = new ArcvInfoContainer().ReadObject(pathToResInfo);

// Write Bytes of each Res inside the ARCV Stream (and Apply Padding if Necessary)

for(int i = 0; i < entryNames.Length; i++)
{
ArcvCompressor.PadStream(targetStream, (int)styleInfo.BytesIdentation, styleInfo.PaddingByte);

AddResource(entryNames[i], targetStream, bufferSize);
}

return infoContainer.ResInfo;
}

}

}
using System;
using System.Collections.Generic;
using ZCore.Modules.SexyCompressors.ResourceStreamGroup.Methods;
using ZCore.Modules.SexyCompressors.ResourceStreamGroup.Definitions.Part;
using ZCore.Modules.SexyCompressors.ResourceStreamGroup.Definitions.Resources;

namespace ZCore.Modules.SexyCompressors.ResourceStreamGroup.Definitions.FilePath
{
/// <summary> Represents a Temporal RSG Path </summary>

public class TempRsgPath
{
/** <summary> Gets or Sets a Temporal Path that will be Split into an Array of Strings. </summary>
<returns> The PathToSlice </returns> */

public string PathToSlice{ get; set; }

/** <summary> Gets or Sets the Path Key </summary>
<returns> The Path Key </returns> */

public int Key{ get; set; }

/** <summary> Gets or Sets some Info related to the ResFile </summary>
<returns> The Info  </returns> */

public ResInfo FileInfo{ get; set; }

/** <summary> Gets or Sets a Boolean that Determines if the File is an Atlas Image </summary>
<returns> true or false </returns> */

public bool IsAtlasImage{ get; set; }

/** <summary> Obtains or Creates a List that Contains Info related to the Path Position. </summary>
<returns> The PosInfo </returns> */

public List<PathPositionInfo> PosInfo{ get; set; } = new();

/// <summary> Creates a new Instance of the <c>TempRsgPath</c> </summary>

public TempRsgPath()
{
FileInfo = new();
}

/// <summary> Creates a new Instance of the <c>TempRsgPath</c> </summary>

public TempRsgPath(string path, int key, ResInfo info, bool isAtlas)
{
PathToSlice = path;
Key = key;

FileInfo = info;
IsAtlasImage = isAtlas;
}

// Process Temp RSG Paths for Packing

public static List<TempRsgPath> GetListForPacking(List<ResInfo> resInfo)
{
resInfo.Sort( (a, b) => string.Compare(a.PathToResFile, b.PathToResFile, StringComparison.OrdinalIgnoreCase) );

resInfo.Insert(0, new(string.Empty) ); // Root Dir is Empty

List<TempRsgPath> temporalPaths = new();
int currentPos = 0;

for(int i = 0; i < resInfo.Count - 1; i++)
{
string currentPath = resInfo[i].PathToResFile.ToUpper();
string nextPath = resInfo[i + 1].PathToResFile.ToUpper();

if(!EncodeHelper.IsASCII(nextPath) )
throw new Exception($"Path must be Encoded with ASCII: \"{nextPath}\"");

int maxLength = Math.Max(currentPath.Length, nextPath.Length);

for(int k = 0; k < maxLength; k++)
{

if(k >= currentPath.Length || k >= nextPath.Length || currentPath[k] != nextPath[k] )
{

for(int h = temporalPaths.Count - 1; h >= 0; h--)
{

if(k >= temporalPaths[h].Key)
{
temporalPaths[h].PosInfo.Add( new( (uint)currentPos, (uint)(k - temporalPaths[h].Key) ) );
break;
}

}

currentPos += nextPath.EndsWith(".PTX") ? nextPath.Length - k + 9 : nextPath.Length - k + 4;

temporalPaths.Add( new(nextPath[k..], k, resInfo[i + 1], nextPath.EndsWith(".PTX") ) );
break;
}

}

}

return temporalPaths;
}

// Process Entries for Unpacking

public static void InitListForUnpacking(BinaryStream sourceStream, Endian endian, uint listOffset, uint listSize)
{
List<NameDict> nameDict = new();
string fullName = string.Empty;

sourceStream.Position = listOffset;

uint offsetLimit = listOffset + listSize;

while(sourceStream.Position < offsetLimit)
{
string singleChar = sourceStream.ReadString(1, default, endian);
int byteOffset = sourceStream.ReadTripleByte(endian) * 4;

if(singleChar == "\0")
{
	
if(byteOffset != 0)
nameDict.Add( new(fullName, byteOffset) );

bool typeFlags = sourceStream.ReadUInt(endian) == 1;

if(typeFlags)
RsgCompressor.Part1_Entries.Add( Part1_Info.Read(sourceStream, endian, fullName) );

else
RsgCompressor.Part0_Entries.Add( Part0_Info.Read(sourceStream, endian, fullName) );

for(int i = 0; i < nameDict.Count; i++)
{

if(nameDict[i].ByteOffset + listOffset == sourceStream.Position)
{
fullName = nameDict[i].PathName;

nameDict.RemoveAt(i);
break;
}

}

}

else
{

if(byteOffset != 0)
{
nameDict.Add( new(fullName, byteOffset) );

fullName += singleChar;
}

else
fullName += singleChar;
                    
}
 
}

}

}

}
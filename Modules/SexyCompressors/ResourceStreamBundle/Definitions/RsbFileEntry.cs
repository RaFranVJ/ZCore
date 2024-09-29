using System;
using System.Collections.Generic;
using ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions.FilePath;
using ZCore.Modules.SexyCompressors.ResourceStreamGroup.Definitions.FilePath;

namespace ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions
{
/// <summary> Represents a File Entry inside a RSB Directory </summary>

public class RsbFileEntry
{
/** <summary> Gets or Sets the FullName of the File. </summary>
<returns> The File Name </returns> */

public string FullName{ get; set; }

/** <summary> Gets or Sets the Index of the File inside the Pool. </summary>
<returns> The PoolIndex </returns> */
		
public int PoolIndex{ get; set; }

/// <summary> Creates a new Instance of the <c>RsbFileEntry</c> </summary>

public RsbFileEntry()
{
FullName = "<Must be a Path>";
}

/// <summary> Creates a new Instance of the <c>RsbFileEntry</c> </summary>

public RsbFileEntry(string name, int index)
{
FullName = name;
PoolIndex = index;
}

// Split Path for a RSB Entry

public static List<RsbFileEntry> GetListForUnpacking(BinaryStream sourceStream, Endian endian, uint tempOffset, uint tempLength)
{
sourceStream.Position = tempOffset;

List<RsbFileEntry> fileList = new();
List<NameDict> nameDict = new();

string pathName = string.Empty;
uint offsetLimit = tempOffset + tempLength;
		
while(sourceStream.Position < offsetLimit)
{
string singleChar = sourceStream.ReadString(1, default, endian);
int byteOffset = sourceStream.ReadTripleByte(endian) * 4;

if(singleChar == "\0")
{

if(byteOffset != 0)
nameDict.Add( new(pathName, byteOffset) );

int poolOffset = sourceStream.ReadInt(endian);
fileList.Add( new(pathName, poolOffset) );

for(int i = 0; i < nameDict.Count; i++)
{

if(nameDict[i].ByteOffset + tempOffset == sourceStream.Position)
{
pathName = nameDict[i].PathName;

nameDict.RemoveAt(i);
break;
}

}

}

else
{

if(byteOffset != 0)
{
nameDict.Add( new(pathName, byteOffset) );

pathName += singleChar;
}

else
pathName += singleChar;

}
 
}

RsbHelper.CheckEndOffset(sourceStream, offsetLimit);

fileList.Sort( (a, b) => a.PoolIndex.CompareTo(b.PoolIndex) );

return fileList;
}

// Init List for Packing

public static List<TempRsbPath> GetListForPacking(List<RsbFileEntry> fileEntries)
{
fileEntries.Sort( (a, b) => string.Compare(a.FullName.ToUpper(), b.FullName.ToUpper(), StringComparison.OrdinalIgnoreCase) );

fileEntries.Insert(0, new(string.Empty, -1) ); // Root Dir is Empty

List<TempRsbPath> temporalPaths = new();
int currentPos = 0;

for(int i = 0; i < fileEntries.Count - 1; i++)
{
string currentPath = fileEntries[i].FullName.ToUpper();
string nextPath = fileEntries[i + 1].FullName.ToUpper();

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

currentPos += nextPath.Length - k + 2;

temporalPaths.Add( new(nextPath[k..], k, fileEntries[i + 1].PoolIndex) );
break;
}

}

}

return temporalPaths;
}

}

}
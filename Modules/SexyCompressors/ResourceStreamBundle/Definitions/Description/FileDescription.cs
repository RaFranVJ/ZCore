using System.Collections.Generic;
using ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions.Resources;
using ZCore.Modules.SexyCompressors.ResourceStreamBundle.Exceptions;

namespace ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions.Description
{
/// <summary> Represents a File Description for a Resource </summary>

public class FileDescription
{
/** <summary> Gets or Sets a Number that Indicates the File Type. </summary>
<returns> The FileType </returns> */

public uint FileType{ get; set; }

/** <summary> Gets or Sets the File Path. </summary>
<returns> The FilePath </returns> */

public string FilePath{ get; set; }

/** <summary> Gets or Sets some Info that Describes the Image. </summary>
<returns> The FilePath </returns> */

public ImageInfoForResDescription ImageInfo{ get; set; }

/** <summary> Gets or Sets a Map to the SubGroups inside the Group. </summary>
<returns> The SubGroupsMap </returns> */

public Dictionary<string, string> PropertiesMap{ get; set; }

/// <summary> Creates a new Instance of the <c>FileDescription</c> </summary>

public FileDescription()
{
PropertiesMap = new();
}

/// <summary> Creates a new Instance of the <c>FileDescription</c> </summary>

public FileDescription(uint type, string filePath, ImageInfoForResDescription imageInfo,
Dictionary<string, string> properties)
{
FileType = type;
FilePath = filePath;

ImageInfo = imageInfo;
PropertiesMap = properties;
}

// Read Properties for ResFile

private static Dictionary<string, string> ReadProperties(BinaryStream sourceStream, Endian endian, uint part3_Offset,
uint integrityFactor = 0, bool checkForCorruptedRsb = true)
{
Dictionary<string, string> properties = new();

uint propsCount = sourceStream.ReadUInt(endian);

for(uint i = 0; i < propsCount; i++)
{
uint keyOffset = sourceStream.ReadUInt(endian);
uint integral = sourceStream.ReadUInt(endian);

if(checkForCorruptedRsb && integral != integrityFactor)
throw new CorruptedRsbException(sourceStream.Position);

uint valueOffset = sourceStream.ReadUInt(endian);

sourceStream.Position = part3_Offset + keyOffset;
string key = sourceStream.ReadStringUntilZero(default, endian);

sourceStream.Position = part3_Offset + valueOffset;
string value = sourceStream.ReadStringUntilZero(default, endian);

properties.Add(key, value);
}

return properties;
}

// Read Description for Resource File

public static FileDescription Read(BinaryStream sourceStream, Endian endian, uint part3_Offset, out string resId, 
uint expectedOffsetForPart2 = 0, bool checkOffsetInPart2 = true, ushort expectedHeadLength = 0x1C, bool checkHeadLength = true)
{
uint offsetForPt2 = sourceStream.ReadUInt(endian);

if(checkOffsetInPart2 && offsetForPt2 != expectedOffsetForPart2)
throw new Part2_InvalidOffsetException(sourceStream.Position, offsetForPt2, expectedOffsetForPart2);

uint type = sourceStream.ReadUShort(endian);
ushort headLength = sourceStream.ReadUShort(endian);

if(checkHeadLength && headLength != expectedHeadLength)
throw new FileDesc_InvalidHeadLengthException(sourceStream.Position, headLength, expectedHeadLength);

uint ptxInfo_EndOffset = sourceStream.ReadUInt(endian);
uint ptxInfo_StartOffset = sourceStream.ReadUInt(endian);

uint resIdOffset = sourceStream.ReadUInt(endian);
uint pathOffset = sourceStream.ReadUInt(endian);

sourceStream.Position = part3_Offset + resIdOffset;
resId = sourceStream.ReadStringUntilZero(default, endian);

sourceStream.Position = part3_Offset + pathOffset;
string resPath = sourceStream.ReadStringUntilZero(default, endian);

var ptxInfo = ImageInfoForResDescription.Read(sourceStream, endian, ptxInfo_StartOffset, ptxInfo_EndOffset, part3_Offset);
var properties = ReadProperties(sourceStream, endian, part3_Offset);

return new(type, resPath, ptxInfo, properties);
}

// Write Properties

private void WriteProperties(BinaryStream part2_Res, BinaryStream part3_Res, Endian endian, Dictionary<string, uint> stringPool)
{

foreach(var prop in PropertiesMap)
{
uint keyOffset = RsbHelper.ThrowInPool(part3_Res, stringPool, prop.Key, endian);
part2_Res.WriteUInt(keyOffset, endian);

part2_Res.WriteInt(0);

uint valueOffset = RsbHelper.ThrowInPool(part3_Res, stringPool, prop.Value, endian);
part2_Res.WriteUInt(valueOffset, endian);
}

}

// Write Description for Resource File

public void Write(BinaryStream part1_Res, BinaryStream part2_Res, BinaryStream part3_Res,
Endian endian, Dictionary<string, uint> stringPool, string resKey)
{
part1_Res.WriteUInt( (uint)part2_Res.Position, endian);

part2_Res.WriteInt(0);
part2_Res.WriteUShort( (ushort)FileType, endian);
part2_Res.WriteUShort(0x1C, endian);

long backupOffsetPt2 = part2_Res.Position;
part2_Res.Position += 0x8;

uint offsetForPt3 = RsbHelper.ThrowInPool(part3_Res, stringPool, resKey, endian);
part2_Res.WriteUInt(offsetForPt3, endian);

uint pathOffsetPt3 = RsbHelper.ThrowInPool(part3_Res, stringPool, FilePath, endian);
part2_Res.WriteUInt(pathOffsetPt3, endian);

part2_Res.WriteUInt( (uint)PropertiesMap.Count, endian);

if(FileType == 0)
ImageInfo.Write(part2_Res, part3_Res, endian, stringPool, backupOffsetPt2);
    
WriteProperties(part2_Res, part3_Res, endian, stringPool);
}

}

}
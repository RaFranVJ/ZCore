using System.Collections.Generic;

namespace ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions.Description
{
/// <summary> Represents Info for an Image that can be a Texture or an Atlas </summary>

public class ImageInfoForResDescription
{
/** <summary> Gets or Sets a Value that Indicates the Image Type. </summary>
<returns> The ImageType </returns> */

public ushort ImageType{ get; set; }

/** <summary> Gets or Sets a Value that Indicates the Atlas Flags. </summary>
<returns> The AtlasFlags </returns> */

public ushort AtlasFlags{ get; set; }

/** <summary> Gets or Sets the X Coordinate for this Image. </summary>
<returns> The X Coordinate </returns> */

public ushort X{ get; set; }

/** <summary> Gets or Sets the Y Coordinate for this Image. </summary>
<returns> The Y Coordinate </returns> */

public ushort Y{ get; set; }

/** <summary> Gets or Sets the X Coordinate for the Atlas of this Image. </summary>
<returns> The X Coordinate for Atlas </returns> */

public ushort AtlasX{ get; set; }

/** <summary> Gets or Sets the Y Coordinate for the Atlas of this Image. </summary>
<returns> The Y Coordinate for Atlas </returns> */

public ushort AtlasY{ get; set; }

/** <summary> Gets or Sets the Atlas Width of this Image. </summary>
<returns> The Atlas </returns> */

public ushort AtlasWidth{ get; set; }

/** <summary> Gets or Sets the Atlas Height of this Image. </summary>
<returns> The AtlasHeight </returns> */

public ushort AtlasHeight{ get; set; }

/** <summary> Gets or Sets the Number of Rows in the Image. </summary>
<returns> The Number of Rows </returns> */

public ushort NumberOfRows{ get; set; }

/** <summary> Gets or Sets the Number of Columns in the Image. </summary>
<returns> The Y Coordinate for Atlas </returns> */

public ushort NumberOfColumns{ get; set; }

/** <summary> Gets or Sets the Parent Image. </summary>
<returns> The ParentImage </returns> */

public string ParentImage{ get; set; }

/// <summary> Creates a new Instance of the <c>ImageInfoForResDescription</c> </summary>

public ImageInfoForResDescription()
{
}

// Read ImageInfo used for ResDesc

public static ImageInfoForResDescription Read(BinaryStream sourceStream, Endian endian, uint startOffset,
uint endOffset, uint part3_Offset)
{
ImageInfoForResDescription imageInfo = null;

if(endOffset * startOffset != 0)
{

imageInfo = new()
{
ImageType = sourceStream.ReadUShort(endian),
AtlasFlags = sourceStream.ReadUShort(endian),
X = sourceStream.ReadUShort(endian),
Y = sourceStream.ReadUShort(endian),
AtlasX = sourceStream.ReadUShort(endian),
AtlasY = sourceStream.ReadUShort(endian),
AtlasWidth = sourceStream.ReadUShort(endian),
AtlasHeight = sourceStream.ReadUShort(endian),
NumberOfRows = sourceStream.ReadUShort(endian),
NumberOfColumns = sourceStream.ReadUShort(endian)
};

sourceStream.Position = part3_Offset + sourceStream.ReadUInt(endian);
imageInfo.ParentImage = sourceStream.ReadStringUntilZero(default, endian);
}

return imageInfo;
}

// Write ImageInfo

public void Write(BinaryStream part2_Res, BinaryStream part3_Res, Endian endian,
Dictionary<string, uint> stringPool, long backupOffset)
{
uint startOffsetPt2 = (uint)part2_Res.Position;

part2_Res.WriteUInt(ImageType, endian);
part2_Res.WriteUInt(AtlasFlags, endian);
part2_Res.WriteUInt(X, endian);
part2_Res.WriteUInt(Y, endian);
part2_Res.WriteUInt(AtlasX, endian);
part2_Res.WriteUInt(AtlasY, endian);
part2_Res.WriteUInt(AtlasWidth, endian);
part2_Res.WriteUInt(AtlasHeight, endian);
part2_Res.WriteUInt(NumberOfRows, endian);
part2_Res.WriteUInt(NumberOfColumns, endian);

uint parentOffsetPt3 = RsbHelper.ThrowInPool(part3_Res, stringPool, ParentImage, endian);
part2_Res.WriteUInt(parentOffsetPt3, endian);

uint endOffsetPt2 = (uint)part2_Res.Position;
part2_Res.Position = backupOffset;

part2_Res.WriteUInt(endOffsetPt2, endian);
part2_Res.WriteUInt(startOffsetPt2, endian);

part2_Res.Position = endOffsetPt2;
}

}

}
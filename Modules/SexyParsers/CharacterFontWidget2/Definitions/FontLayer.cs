using System.Collections.Generic;

namespace ZCore.Modules.SexyParsers.CharacterFontWidget2.Definitions
{
/// <summary> Represents a Font Layer. </summary>

public class FontLayer 
{
/** <summary> Gets or Sets the Name of the FontLayer. </summary>
<returns> The Layer Name. </returns> */

public string LayerName{ get; set; }

/** <summary> Gets or Sets the Number of Required Tags for this Layer. </summary>
<returns> The Number of RequiredTags. </returns> */

public uint NumberOfRequiredTags{ get; set; }

/** <summary> Gets or Sets a List of Tags Required for this Layer. </summary>
<returns> The RequiredTags. </returns> */

public List<string> RequiredTags{ get; set; }

/** <summary> Gets or Sets the Number of Tags to Exclude from this Layer. </summary>
<returns> The Number of TagsToExclude. </returns> */

public uint NumberOfTagsToExclude{ get; set; }

/** <summary> Gets or Sets the Tags to Exclude from this Layer. </summary>
<returns> The TagsToExclude. </returns> */

public List<string> TagsToExclude{ get; set; }

/** <summary> Gets or Sets the Number of Kernings for this Layer. </summary>
<returns> The Number of Kernings. </returns> */

public uint NumberOfKernings{ get; set; }

/** <summary> Gets or Sets the FontKernings for this Layer. </summary>
<returns> The FontKernings. </returns> */

public List<FontKerning> FontKernings{ get; set; }

/** <summary> Gets or Sets the Number of FontCharacters for this Layer. </summary>
<returns> The Number of FontCharacters. </returns> */

public uint NumberOfFontChars{ get; set; }

/** <summary> Gets or Sets the FontCharacters for this FontLayer. </summary>
<returns> The FontCharacters. </returns> */

public List<FontCharacter> FontCharacters{ get; set; }

/** <summary> Gets or Sets the Color Multiples for this Layer </summary>
<returns> The ColorMultiples. </returns> */

public PvrColor_RGBA ColorMultiples{ get; set; }

/** <summary> Gets or Sets the Color Addends for this Layer </summary>
<returns> The ColorAddends. </returns> */

public PvrColor_RGBA ColorAddends{ get; set; }

/** <summary> Gets or Sets the Image File for this Layer </summary>
<returns> The ImageFile. </returns> */

public string ImageFile{ get; set; }

/** <summary> Gets or Sets the Draw Mode for this Layer </summary>
<returns> The DrawMode. </returns> */

public int DrawMode{ get; set; }

/** <summary> Gets or Sets the Layer Offset. </summary>
<returns> The LayerOffset. </returns> */

public SexyPoint LayerOffset{ get; set; }

/** <summary> Gets or Sets the Layer Spacing. </summary>
<returns> The Layer Spacing. </returns> */

public int LayerSpacing{ get; set; }

/** <summary> Gets or Sets a Range that PointSize must follow on this Layer. </summary>
<returns> The PointSize Range. </returns> */
		
public Limit<int> PointSizeRange{ get; set; }

/** <summary> Gets or Sets the Point Size of this Layer. </summary>
<returns> The PointSize. </returns> */

public int PointSize{ get; set; }

/** <summary> Gets or Sets Ascent of this FontLayer. </summary>
<returns> The Font Ascent. </returns> */

public int FontAscent{ get; set; }

/** <summary> Gets or Sets Ascent Padding of this Layer. </summary>
<returns> The AscentPadding. </returns> */

public int AscentPadding{ get; set; }

/** <summary> Gets or Sets Height of this FontLayer. </summary>
<returns> The Layer Height. </returns> */

public int LayerHeight{ get; set; }

/** <summary> Gets or Sets default Height of this Layer. </summary>
<returns> The DefaultHeight. </returns> */

public int DefaultHeight{ get; set; }

/** <summary> Gets or Sets LineSpacing Offset of this Layer. </summary>
<returns> The LineSpacingOffset. </returns> */

public int LineSpacingOffset{ get; set; }
		
/** <summary> Gets or Sets base Order of this Layer. </summary>
<returns> The BaseOrder. </returns> */

public int BaseOrder{ get; set; }

/// <summary> Creates a new <c>FontLayer</c> </summary>

public FontLayer()
{
}

// Get Kernings

private static List<FontKerning> LoadKernings(BinaryStream bs, uint count, Endian endian = default)
{
List<FontKerning> kernings = new();

for(uint i = 0; i < count; i++)
kernings.Add(FontKerning.Read(bs, endian) );	

return kernings;
}

// Get FontLayer from BinaryStream

public static FontLayer Read(BinaryStream bs, Endian endian = default)
{

FontLayer layer = new()
{
LayerName = bs.ReadStringByIntLength(default, endian),
NumberOfRequiredTags = bs.ReadUInt(endian)
};

layer.RequiredTags = TagHelper.LoadTags(bs, layer.NumberOfRequiredTags, endian);

layer.NumberOfTagsToExclude = bs.ReadUInt(endian);
layer.TagsToExclude = TagHelper.LoadTags(bs, layer.NumberOfTagsToExclude, endian);

layer.NumberOfKernings = bs.ReadUInt(endian);
layer.FontKernings = LoadKernings(bs, layer.NumberOfKernings, endian);

layer.ColorMultiples = new()
{
Red = bs.ReadInt(endian),
Green = bs.ReadInt(endian),
Blue = bs.ReadInt(endian),
Alpha = bs.ReadInt(endian)
};

layer.ColorAddends = new()
{
Red = bs.ReadInt(endian),
Green = bs.ReadInt(endian),
Blue = bs.ReadInt(endian),
Alpha = bs.ReadInt(endian)
};

layer.ImageFile = bs.ReadStringByIntLength(default, endian);
layer.DrawMode = bs.ReadInt(endian);

layer.LayerOffset = new()
{
X = bs.ReadInt(endian),
Y = bs.ReadInt(endian),
};

layer.LayerSpacing = bs.ReadInt(endian);

layer.PointSizeRange = new()
{
MinValue = bs.ReadInt(endian),
MaxValue = bs.ReadInt(endian)
};

layer.PointSize = bs.ReadInt(endian);
layer.FontAscent = bs.ReadInt(endian);
layer.AscentPadding = bs.ReadInt(endian);
layer.LayerHeight = bs.ReadInt(endian);
layer.DefaultHeight = bs.ReadInt(endian);
layer.LineSpacingOffset = bs.ReadInt(endian);
layer.BaseOrder = bs.ReadInt(endian);

return layer;
}

// Save Kernings

private void SaveKernings(BinaryStream bs, List<FontKerning> kernings, uint count, Endian endian = default)
{

if(kernings == null)
return;

count = count > kernings.Count ? (uint)kernings.Count : count;

for(int i = 0; i < count; i++)
kernings[i].Write(bs, endian);	

}

// Write FontLayer to BinaryStream

public void Write(BinaryStream bs, Endian endian = default)
{
bs.WriteStringByIntLength(LayerName, default, endian);

bs.WriteUInt(NumberOfRequiredTags, endian);
TagHelper.SaveTags(bs, RequiredTags, NumberOfRequiredTags, endian);

bs.WriteUInt(NumberOfTagsToExclude, endian);
TagHelper.SaveTags(bs, TagsToExclude, NumberOfTagsToExclude, endian);

bs.WriteUInt(NumberOfKernings, endian);
SaveKernings(bs, FontKernings, NumberOfKernings, endian);

bs.WriteInt(ColorMultiples.Red, endian);
bs.WriteInt(ColorMultiples.Green, endian);
bs.WriteInt(ColorMultiples.Blue, endian);
bs.WriteInt(ColorMultiples.Alpha, endian);

bs.WriteInt(ColorAddends.Red, endian);
bs.WriteInt(ColorAddends.Green, endian);
bs.WriteInt(ColorAddends.Blue, endian);
bs.WriteInt(ColorAddends.Alpha, endian);

bs.WriteStringByIntLength(ImageFile, default, endian);
bs.WriteInt(DrawMode, endian);

bs.WriteInt(LayerOffset.X, endian);
bs.WriteInt(LayerOffset.Y, endian);

bs.WriteInt(LayerSpacing, endian);

bs.WriteInt(PointSizeRange.MinValue, endian);
bs.WriteInt(PointSizeRange.MaxValue, endian);

bs.WriteInt(PointSize, endian);
bs.WriteInt(FontAscent, endian);
bs.WriteInt(AscentPadding, endian);
bs.WriteInt(LayerHeight, endian);
bs.WriteInt(DefaultHeight, endian);
bs.WriteInt(LineSpacingOffset, endian);
bs.WriteInt(BaseOrder, endian);
}

}

}
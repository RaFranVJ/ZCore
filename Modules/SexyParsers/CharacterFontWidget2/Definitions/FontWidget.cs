using System;
using System.Collections.Generic;
using ZCore.Modules.SexyParsers.CharacterFontWidget2.Parser;
using ZCore.Serializables.ArgumentsInfo.FileManager.Archive;

namespace ZCore.Modules.SexyParsers.CharacterFontWidget2.Definitions
{
/// <summary> Represents a Font Widget. </summary>

public class FontWidget : MetaModel<FontWidget>
{
/** <summary> Gets or Sets the Version of the Widget. </summary>
<returns> The Widget Version. </returns> */

public Int128 Version{ get; set; }	

/** <summary> Gets or Sets Ascent of this FontWidget. </summary>
<returns> The Font Ascent. </returns> */

public int FontAscent{ get; set; }

/** <summary> Gets or Sets Ascent Padding of this Widget. </summary>
<returns> The AscentPadding. </returns> */

public int AscentPadding{ get; set; }

/** <summary> Gets or Sets Height of this FontWidget. </summary>
<returns> The Widget Height. </returns> */

public int WidgetHeight{ get; set; }

/** <summary> Gets or Sets LineSpacing Offset of this Widget. </summary>
<returns> The LineSpacingOffset. </returns> */

public int LineSpacingOffset{ get; set; }

/** <summary> Gets or Sets a Boolean that Determines if this Widget was Initialized or not. </summary>
<returns> <b>true</b> if the Widget is Initialized; <b>false</b> otherwise. </returns> */

public bool IsInitialized{ get; set; }

/** <summary> Gets or Sets default PointSize of this Widget. </summary>
<returns> The DefaultPointSize. </returns> */

public int DefaultPointSize{ get; set; }

/** <summary> Gets or Sets the Number of Chars for this FontWidget. </summary>
<returns> The Number of Chars. </returns> */

public uint NumberOfChars{ get; set; }

/** <summary> Gets or Sets the Characters for this FontWidget. </summary>
<returns> The CharacterItems. </returns> */

public List<CharacterItem> Chars{ get; set; }

/** <summary> Gets or Sets the Number of Layers for this FontWidget. </summary>
<returns> The Number of Layers. </returns> */

public uint NumberOfLayers{ get; set; }

/** <summary> Gets or Sets the Layers of this Widget. </summary>
<returns> The FontLayers. </returns> */

public List<FontLayer> Layers{ get; set; }

/** <summary> Gets or Sets the source File for this FontWidget. </summary>
<returns> The SourceFile. </returns> */

public string SourceFile{ get; set; }

/// <summary> Unknown Field, don't know its Purpose. </summary>

public string ErrorHeader{ get; set; }

/** <summary> Gets or Sets the Point Size of this Widget. </summary>
<returns> The PointSize. </returns> */

public int PointSize{ get; set; }

/** <summary> Gets or Sets the Number of Tags for this FontWidget. </summary>
<returns> The Number of Tags. </returns> */

public uint NumberOfTags{ get; set; }

/** <summary> Gets or Sets a List of Tags for this Widget. </summary>
<returns> The Tags. </returns> */

public List<string> Tags{ get; set; }

/** <summary> Gets or Sets the Scale of this Widget. </summary>
<returns> The Widget Scale. </returns> */

public double Scale{ get; set; }

/** <summary> Gets or Sets a Boolean that Determines if ScaledImages should be Forced to be White. </summary>
<returns> <b>true</b> if ScaledImages should be Forced to White; <b>false</b> otherwise. </returns> */

public bool ForceScaledImageToWhite{ get; set; }

/** <summary> Gets or Sets a Boolean that Determines if all Layers of this Widget should be Activated or not. </summary>
<returns> <b>true</b> if all Layers should be Activated; <b>false</b> otherwise. </returns> */

public bool ActivateAllLayers{ get; set; }

/// <summary> Creates a new <c>FontWidget</c> </summary>

public FontWidget()
{
}

// Get CharItems

private static List<CharacterItem> LoadCharItems(BinaryStream bs, uint count, Endian endian = default)
{
List<CharacterItem> charItems = new();

for(uint i = 0; i < count; i++)
charItems.Add(CharacterItem.Read(bs, endian) );	

return charItems;
}

// Get FontLayers

private static List<FontLayer> LoadLayers(BinaryStream bs, uint count, Endian endian = default)
{
List<FontLayer> layers = new();

for(uint i = 0; i < count; i++)
layers.Add(FontLayer.Read(bs, endian) );	

return layers;
}

// Get FontWidget from BinaryStream

public static FontWidget ReadBin(BinaryStream bs, Endian endian = default, bool adaptVer = true)
{

FontWidget widget = new()
{
Version = Cfw2Version.Read(bs, endian, adaptVer),

FontAscent = bs.ReadInt(endian),
AscentPadding = bs.ReadInt(endian),
WidgetHeight = bs.ReadInt(endian),
LineSpacingOffset = bs.ReadInt(endian),
IsInitialized = bs.ReadBool(),
DefaultPointSize = bs.ReadInt(endian),

NumberOfChars = bs.ReadUInt(endian)
};

widget.Chars = LoadCharItems(bs, widget.NumberOfChars, endian);

widget.NumberOfLayers = bs.ReadUInt(endian);
widget.Layers = LoadLayers(bs, widget.NumberOfLayers, endian);

widget.SourceFile = bs.ReadStringByIntLength(default, endian);
widget.ErrorHeader = bs.ReadStringByIntLength(default, endian);
widget.PointSize = bs.ReadInt(endian);

widget.NumberOfTags = bs.ReadUInt(endian);
widget.Tags = TagHelper.LoadTags(bs, widget.NumberOfTags, endian);

widget.Scale = bs.ReadDouble(endian);
widget.ForceScaledImageToWhite = bs.ReadBool();
widget.ActivateAllLayers = bs.ReadBool();

return widget;
}

// Save CharacterItems

private static void SaveCharItems(BinaryStream bs, List<CharacterItem> charItems, uint count, Endian endian = default)
{

if(charItems == null)
return;

count = count > charItems.Count ? (uint)charItems.Count : count;

for(int i = 0; i < count; i++)
charItems[i].Write(bs, endian);	

}

// Save FontLayers

private static void SaveLayers(BinaryStream bs, List<FontLayer> layers, uint count, Endian endian = default)
{

if(layers == null)
return;

count = count > layers.Count ? (uint)layers.Count : count;

for(int i = 0; i < count; i++)
layers[i].Write(bs, endian);	

}

// Save FontWidget to BinaryStream

public void WriteBin(BinaryStream bs, FileVersionDetails<Int128> verInfo, Endian endian = default)
{
Version = verInfo.VersionNumber;
Cfw2Version.Write(bs, endian, verInfo.VersionNumber, verInfo.AdaptCompatibilityBetweenVersions);  

bs.WriteInt(FontAscent, endian);
bs.WriteInt(AscentPadding, endian);
bs.WriteInt(WidgetHeight, endian);
bs.WriteInt(LineSpacingOffset, endian);
bs.WriteBool(IsInitialized);
bs.WriteInt(DefaultPointSize, endian);

bs.WriteUInt(NumberOfChars, endian);
SaveCharItems(bs, Chars, NumberOfChars, endian);

bs.WriteUInt(NumberOfLayers, endian);
SaveLayers(bs, Layers, NumberOfLayers, endian);

bs.WriteStringByIntLength(SourceFile, default, endian);
bs.WriteStringByIntLength(ErrorHeader, default, endian);
bs.WriteInt(PointSize, endian);

bs.WriteUInt(NumberOfTags, endian);
TagHelper.SaveTags(bs, Tags, NumberOfTags, endian);

bs.WriteDouble(Scale, endian);
bs.WriteBool(ForceScaledImageToWhite);
bs.WriteBool(ActivateAllLayers);
}

}

}
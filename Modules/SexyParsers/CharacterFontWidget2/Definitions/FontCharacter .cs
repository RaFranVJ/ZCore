namespace ZCore.Modules.SexyParsers.CharacterFontWidget2.Definitions
{
/// <summary> Represents a Font Character. </summary>

public class FontCharacter 
{
/** <summary> Gets or Sets the Char index in the Font. </summary>
<returns> The Char Index. </returns> */

public char Index{ get; set; }

/** <summary> Gets or Sets a Rectangle for the Font as a Image. </summary>
<returns> The ImageRect. </returns> */
		
public SexyRect ImageRect{ get; set; }

/** <summary> Gets or Sets the Image Offset. </summary>
<returns> The ImageOffset. </returns> */
		
public SexyPoint ImageOffset{ get; set; }

/** <summary> Gets or Sets the Offset of the First Kerning. </summary>
<returns> The FirstKerning. </returns> */

public ushort FirstKerning{ get; set; }

/** <summary> Gets or Sets the Numbers of Kernings in the Font. </summary>
<returns> The KerningsCount. </returns> */

public ushort KerningsCount{ get; set; }

/** <summary> Gets or Sets the Width of the Font Character. </summary>
<returns> The Font Width. </returns> */

public int Width{ get; set; }

/** <summary> Gets or Sets the Order of the Font Character. </summary>
<returns> The Char Order </returns> */
		
public int Order{ get; set; }

/// <summary> Creates a new <c>FontCharacter</c> </summary>

public FontCharacter()
{
}

// Get FontCharacter from BinaryStream

public static FontCharacter Read(BinaryStream bs, Endian endian = default)
{

return new()
{
Index = bs.ReadChar(endian),

ImageRect = new()
{
X = bs.ReadInt(endian),
Y = bs.ReadInt(endian),
Width = bs.ReadInt(endian),
Height = bs.ReadInt(endian)
},

ImageOffset = new()
{
X = bs.ReadInt(endian),
Y = bs.ReadInt(endian)
},

FirstKerning = bs.ReadUShort(endian),
KerningsCount = bs.ReadUShort(endian),
Width = bs.ReadInt(endian),
Order = bs.ReadInt(endian)
};

}

// Write FontCharacter to BinaryStream

public void Write(BinaryStream bs, Endian endian = default)
{
bs.WriteChar(Index, endian);

bs.WriteInt(ImageRect.X, endian);
bs.WriteInt(ImageRect.Y, endian);
bs.WriteInt(ImageRect.Width, endian);
bs.WriteInt(ImageRect.Height, endian);

bs.WriteInt(ImageOffset.X, endian);
bs.WriteInt(ImageOffset.Y, endian);

bs.WriteUShort(FirstKerning, endian);
bs.WriteUShort(KerningsCount, endian);
bs.WriteInt(Width, endian);
bs.WriteInt(Order, endian);
}

}

}
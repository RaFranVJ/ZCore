namespace ZCore.Modules.SexyParsers.CharacterFontWidget2.Definitions
{
/// <summary> Represents a Font Kerning. </summary>

public class FontKerning
{
/** <summary> Gets or Sets the Kerning offset in the Font. </summary>
<returns> The Char Offset. </returns> */

public ushort Offset{ get; set; }

/** <summary> Gets or Sets the Kerning index in the Font. </summary>
<returns> The Char Index. </returns> */

public char Index{ get; set; }

/// <summary> Creates a new <c>FontKerning</c> </summary>

public FontKerning()
{
}

// Get FontKerning from BinaryStream

public static FontKerning Read(BinaryStream bs, Endian endian = default)
{

return new()
{
Offset = bs.ReadUShort(endian),
Index = bs.ReadChar(endian)
};

}

// Write FontKerning to BinaryStream

public void Write(BinaryStream bs, Endian endian = default)
{
bs.WriteUShort(Offset, endian);
bs.WriteChar(Index, endian);
}

}

}
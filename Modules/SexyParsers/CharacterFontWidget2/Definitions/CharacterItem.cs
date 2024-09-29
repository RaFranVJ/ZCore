namespace ZCore.Modules.SexyParsers.CharacterFontWidget2.Definitions
{
/// <summary> Represents a Character. </summary>

public class CharacterItem
{
/** <summary> Gets or Sets the Char index in the Font. </summary>
<returns> The Char Index. </returns> */

public char Index{ get; set; }

/** <summary> Gets or Sets the Char representation of this <c>CharacterItem</c>. </summary>
<returns> The Char itself. </returns> */

public char Value{ get; set; }

/// <summary> Creates a new <c>CharacterItem</c> </summary>

public CharacterItem()
{
}

// Get CharacterItem from BinaryStream

public static CharacterItem Read(BinaryStream bs, Endian endian = default)
{

return new()
{
Index = bs.ReadChar(endian),
Value = bs.ReadChar(endian)
};

}

// Write CharacterItem to BinaryStream

public void Write(BinaryStream bs, Endian endian = default)
{
bs.WriteChar(Index, endian);
bs.WriteChar(Value, endian);
}

}

}
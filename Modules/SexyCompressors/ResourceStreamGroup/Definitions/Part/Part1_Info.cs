namespace ZCore.Modules.SexyCompressors.ResourceStreamGroup.Definitions.Part
{
/// <summary> Stores Info related to the Part1 of a RSG, which represents Textures </summary>
	
public class Part1_Info : Part0_Info
{
/** <summary> Gets or Sets the TextureID. </summary>
<returns> The TextureID </returns> */

public int TextureID{ get; set; }

/** <summary> Gets or Sets the TextureWidth. </summary>
<returns> The TextureWidth </returns> */

public int TextureWidth{ get; set; }

/** <summary> Gets or Sets the TextureHeight. </summary>
<returns> The TextureHeight </returns> */

public int TextureHeight{ get; set; }

/// <summary> Creates a new Instance of the <c>Part1_Info</c> </summary>

public Part1_Info()
{
}

/// <summary> Creates a new Instance of the <c>Part1_Info</c> </summary>

public Part1_Info(string path, uint offset, uint size, int id, int width, int height)
{
FilePath = path;
FileOffset = offset;

FileSize = size;
TextureID = id;

TextureWidth = width;
TextureHeight = height;
}

// Read Info for Part1

public static new Part1_Info Read(BinaryStream sourceStream, Endian endian, string path)
{
uint offset = sourceStream.ReadUInt(endian);
uint size = sourceStream.ReadUInt(endian);

int id = sourceStream.ReadInt(endian);

sourceStream.Position += 8;
int width = sourceStream.ReadInt(endian);

int height = sourceStream.ReadInt(endian);

return new(path, offset, size, id, width, height);
}

}

}
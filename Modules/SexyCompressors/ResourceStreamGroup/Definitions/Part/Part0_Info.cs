namespace ZCore.Modules.SexyCompressors.ResourceStreamGroup.Definitions.Part
{
/// <summary> Stores Info related to the Part0 of a RSG </summary>
	
public class Part0_Info
{
/** <summary> Gets or Sets the FilePath. </summary>
<returns> The FilePath </returns> */

public string FilePath{ get; set; }

/** <summary> Gets or Sets the FileOffset. </summary>
<returns> The FileOffset </returns> */
		
public uint FileOffset{ get; set; }

/** <summary> Gets or Sets the FileSize. </summary>
<returns> The FileSize </returns> */
		
public uint FileSize{ get; set; }

/// <summary> Creates a new Instance of the <c>Part0_Info</c> </summary>

public Part0_Info()
{
}

/// <summary> Creates a new Instance of the <c>Part0_Info</c> </summary>

public Part0_Info(string path, uint offset, uint size)
{
FilePath = path;
FileOffset = offset;

FileSize = size;
}

// Read Info for Part0

public static Part0_Info Read(BinaryStream sourceStream, Endian endian, string path)
{
uint offset = sourceStream.ReadUInt(endian);
uint size = sourceStream.ReadUInt(endian);

return new(path, offset, size);
}

}

}
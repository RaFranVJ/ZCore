namespace ZCore.Modules.TextureDrawer.Parsers.SexyTexure
{
/// <summary> Stores Info about the Version of a SexyTexure. </summary>

public static class SexyTexVersion
{
/// <summary> The Expected Version Number </summary>

private const uint ExpectedVerNumber = 0;

/** <summary> Reads the Version of a SexyTexure. </summary>

<param name = "targetStream"> The Stream to be Read. </param>
<param name = "endian"> The endian Order. </param> */

public static uint Read(BinaryStream targetStream, Endian endian, bool adaptCompatibility)
{
uint version = targetStream.ReadUInt(endian);

if( (version != ExpectedVerNumber) && adaptCompatibility)
throw new InvalidFileVersionException<uint>(version, null, ExpectedVerNumber);

return version;
}

/** <summary> Writes the Version to a XNB File. </summary>

<param name = "targetStream"> The Stream where the Data will be Written. </param>
<param name = "endian"> The endian Order. </param> */

public static void Write(BinaryStream targetStream, Endian endian, uint version, bool adaptCompatibility)
{
uint safeVersion = adaptCompatibility && (version != ExpectedVerNumber) ? ExpectedVerNumber : version;

targetStream.WriteUInt(safeVersion, endian);
}

}

}
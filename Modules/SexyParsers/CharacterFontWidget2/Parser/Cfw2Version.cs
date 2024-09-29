using System;

namespace ZCore.Modules.SexyParsers.CharacterFontWidget2.Parser
{
/// <summary> Stores Info about the Version of a CFW2 File. </summary>

public static class Cfw2Version
{
/// <summary> The Expected Version Number </summary>

public static readonly Int128 ExpectedVerNumber = new(0, 0);

/** <summary> Reads the Version of a CFW2 File. </summary>

<param name = "targetStream"> The Stream to be Read. </param>
<param name = "endian"> The endian Order of the Data. </param> */

public static Int128 Read(BinaryStream targetStream, Endian endian, bool adaptCompatibility)
{
Int128 version = targetStream.ReadInt128(endian);

if( (version != ExpectedVerNumber) && adaptCompatibility)
throw new NotSupportedException($"This File is Encoded with an unknown Version (v{version}.0)"); //

return version;
}

/** <summary> Writes the Version to a CFW2 File. </summary>

<param name = "targetStream"> The Stream where the Data will be Written. </param>
<param name = "endian"> The endian Order of the Data. </param> */

public static void Write(BinaryStream targetStream, Endian endian, Int128 version, bool adaptCompatibility)
{
Int128 safeVersion = adaptCompatibility && (version != ExpectedVerNumber) ? ExpectedVerNumber : version;

targetStream.WriteInt128(safeVersion, endian);
}

}

}
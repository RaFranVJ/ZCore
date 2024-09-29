using System;
using System.Linq;
using ZCore.Modules.SexyCompressors.MarmaladeDZ.Exceptions;

namespace ZCore.Modules.SexyCompressors.MarmaladeDZ
{
/// <summary> Stores Info about the Version of a DTRZ File. </summary>

public static class DtrzVersion
{
/// <summary> The Expected Version Number </summary>

private const byte ExpectedVerNumber = 0;

/** <summary> Reads the Version of a DTRZ File. </summary>

<param name = "targetStream"> The Stream to be Read. </param> */

public static byte Read(BinaryStream sourceStream, bool adaptCompatibility)
{
byte version = sourceStream.ReadByte();

if(version != ExpectedVerNumber && adaptCompatibility)
throw new InvalidDzVersionException(version, ExpectedVerNumber);

return version;
}

/** <summary> Writes the Version to a DTRZ File. </summary>

<param name = "targetStream"> The Stream where the Data will be Written. </param> */

public static void Write(BinaryStream targetStream, byte version, bool adaptCompatibility)
{
byte safeVersion = (adaptCompatibility && version != ExpectedVerNumber) ? ExpectedVerNumber : version;

targetStream.WriteByte(safeVersion);
}

}

}
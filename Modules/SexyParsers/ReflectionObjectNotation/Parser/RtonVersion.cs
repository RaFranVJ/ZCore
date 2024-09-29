using System;
using System.Linq;
using ZCore.Modules.SexyParsers.ReflectionObjectNotation.Exceptions;

namespace ZCore.Modules.SexyParsers.ReflectionObjectNotation.Parser
{
/// <summary> Stores Info about the Version of a RTON File. </summary>

public static class RtonVersion
{
/// <summary> The Expected Version Number </summary>

public const uint ExpectedVerNumber = 1;

/** <summary> Reads the Version of a RTON File. </summary>

<param name = "targetStream"> The Stream to be Read. </param>
<param name = "endian"> The endian Order of the RTON Data. </param> */

public static void Read(BinaryStream targetStream, Endian endian, bool adaptCompatibility)
{
uint version = targetStream.ReadUInt(endian);

if( (version != ExpectedVerNumber) && adaptCompatibility)
throw new InvalidRtonVersionException(version, ExpectedVerNumber);

}

/** <summary> Writes the Version to a RTON File. </summary>

<param name = "targetStream"> The Stream where the Data will be Written. </param>
<param name = "endian"> The endian Order of the RTON Data. </param> */

public static void Write(BinaryStream targetStream, Endian endian, uint version, bool adaptCompatibility)
{
uint safeVersion = adaptCompatibility && (version != ExpectedVerNumber) ? ExpectedVerNumber : version;

targetStream.WriteUInt(safeVersion , endian);
}

}

}
using System;
using System.Linq;
using ZCore.Modules.TextureDrawer.Parsers.XnaGameStudio.Exceptions;

namespace ZCore.Modules.TextureDrawer.Parsers.XnaGameStudio.VersionCheck
{
/// <summary> Stores Info about the Version of the DDS frok a XNB File. </summary>

public static class DdsVersion
{
/// <summary> The Expected Version Number </summary>

private const uint ExpectedVerNumber = 0;

/** <summary> Reads the DDS Version of a XNB File. </summary>

<param name = "targetStream"> The Stream to be Read. </param>
<param name = "endian"> The endian Order of the XNB Data. </param> */

public static uint Read(BinaryStream targetStream, Endian endian, bool adaptCompatibility)
{
uint version = targetStream.ReadUInt(endian);

if( (version != ExpectedVerNumber) && adaptCompatibility)
throw new InvalidDdsVersionException(version, ExpectedVerNumber);

return version;
}

/** <summary> Writes the DDS Version to a XNB File. </summary>

<param name = "targetStream"> The Stream where the Data will be Written. </param>
<param name = "endian"> The endian Order of the XNB Data. </param> */

public static void Write(BinaryStream targetStream, Endian endian, uint version, bool adaptCompatibility)
{
uint safeVersion = adaptCompatibility && (version != ExpectedVerNumber) ? ExpectedVerNumber : version;

targetStream.WriteUInt(safeVersion, endian);
}

}

}
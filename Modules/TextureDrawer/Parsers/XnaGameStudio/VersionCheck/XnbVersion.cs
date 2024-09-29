using System;
using ZCore.Modules.TextureDrawer.Parsers.XnaGameStudio.Exceptions;

namespace ZCore.Modules.TextureDrawer.Parsers.XnaGameStudio.VersionCheck
{
/// <summary> Stores Info about the Version of a XNB File. </summary>

public static class XnbVersion
{
/// <summary> The Expected Versions </summary>

public static readonly Limit<ushort> ExpectedVersions = new(1, 5);

/** <summary> Reads the Version of a XNB File. </summary>

<param name = "targetStream"> The Stream to be Read. </param>
<param name = "endian"> The endian Order of the XNB Data. </param> */

public static ushort Read(BinaryStream targetStream, Endian endian, bool adaptCompatibility)
{
ushort version = targetStream.ReadUShort(endian);

if(!ExpectedVersions.IsParamInRange(version) && adaptCompatibility)
throw new InvalidXnbVersionException(version, ExpectedVersions);

return version;
}

/** <summary> Writes the Version to a XNB File. </summary>

<param name = "targetStream"> The Stream where the Data will be Written. </param>
<param name = "endian"> The endian Order of the XNB Data. </param> */

public static void Write(BinaryStream targetStream, Endian endian, ushort version, bool adaptCompatibility)
{
ushort safeVersion = ExpectedVersions.CheckParamRange(version);

targetStream.WriteUShort(safeVersion , endian);
}

}

}
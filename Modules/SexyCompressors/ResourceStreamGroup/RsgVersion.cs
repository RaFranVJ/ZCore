using System.Collections.Generic;
using ZCore.Modules.SexyCompressors.ResourceStreamGroup.Exceptions;

namespace ZCore.Modules.SexyCompressors.ResourceStreamGroup
{
/// <summary> Stores Info about the Version of a RSG File. </summary>

public static class RsgVersion
{
/// <summary> The Expected Versions </summary>

public static readonly List<uint> ExpectedVersions = new() { 1, 3, 4 };

/** <summary> Reads the Version of a RSG File. </summary>

<param name = "targetStream"> The Stream to be Read. </param>
<param name = "endian"> The endian Order of the RSG Data. </param> */

public static uint Read(BinaryStream targetStream, Endian endian, bool adaptCompatibility)
{
uint version = targetStream.ReadUInt(endian);

if(!ExpectedVersions.Contains(version) && adaptCompatibility)
throw new InvalidRsgVersionException(version, ExpectedVersions);

return version;
}

/** <summary> Writes the Version to a RSG File. </summary>

<param name = "targetStream"> The Stream where the Data will be Written. </param>
<param name = "endian"> The endian Order of the RSG Data. </param> */

public static void Write(BinaryStream targetStream, Endian endian, uint version, bool adaptCompatibility)
{
uint safeVersion = adaptCompatibility && ExpectedVersions.Contains(version) ? version : ExpectedVersions[2];

targetStream.WriteUInt(safeVersion , endian);
}

}

}
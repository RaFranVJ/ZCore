using ZCore.Modules.TextureDrawer.Parsers.PackedTexture.PlayStation.PSV.Exceptions;

namespace ZCore.Modules.TextureDrawer.Parsers.PackedTexture.PlayStation.PSV
{
/// <summary> Stores Info about the Version of a PTX File. </summary>

public static class PtxVersion
{
/// <summary> The Expected Versions </summary>

public static readonly Limit<uint> ExpectedVersions = new(1, 0x10000003);

/** <summary> Reads the Version of a PTX File. </summary>

<param name = "targetStream"> The Stream to be Read. </param>
<param name = "endian"> The endian Order of the PTX Data. </param> */

public static uint Read(BinaryStream targetStream, Endian endian, bool adaptCompatibility)
{
uint version = targetStream.ReadUInt(endian);

if(!ExpectedVersions.IsParamInRange(version) && adaptCompatibility)
throw new Psv_InvalidPtxVersionException(version, ExpectedVersions);

return version;
}

/** <summary> Writes the Version to a PTX File. </summary>

<param name = "targetStream"> The Stream where the Data will be Written. </param>
<param name = "endian"> The endian Order of the PTX Data. </param> */

public static void Write(BinaryStream targetStream, Endian endian, uint version, bool adaptCompatibility)
{
uint safeVersion = ExpectedVersions.CheckParamRange(version);

targetStream.WriteUInt(safeVersion , endian);
}

}

}
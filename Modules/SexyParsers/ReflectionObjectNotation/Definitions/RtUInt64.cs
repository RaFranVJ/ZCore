using System.Text.Json;

namespace ZCore.Modules.SexyParsers.ReflectionObjectNotation.Definitions
{
/// <summary> Represents a 64-bits unsigned Integer in the RtSystem. </summary>

public static class RtUInt64
{
/** <summary> Reads a 64-bits unsigned Integer from a RTON File and Writes its Representation to a JSON File. </summary>

<param name = "inputStream"> The Stream which Contains the RTON Data to be Read. </param>
<param name = "outputStream"> The Stream where the JSON Data will be Written. </param>
<param name = "sourceID"> The Identifier of the RTON Value. </param>
<param name = "endian"> The endian Order of the RTON Data. </param> */

public static void Read(BinaryStream inputStream, Utf8JsonWriter outputStream, RtTypeIdentifier sourceID,
Endian endian = default)
{
ulong value = (sourceID == RtTypeIdentifier.ULong) ? inputStream.ReadULong(endian) : 0uL;

outputStream.WriteNumberValue(value);
}

/** <summary> Reads a 64-bits unsigned Integer from a JSON File and Writes its Representation to a RTON File. </summary>

<param name = "outputStream"> The Stream where the RTON Data will be Written. </param>
<param name = "targetValue"> The UInt64 to be Written. </param>
<param name = "endian"> The endian Order of the RTON Data. </param> */

public static void Write(BinaryStream outputStream, ulong targetValue, Endian endian = default)
{

if(targetValue == 0uL)
outputStream.WriteByte( (byte)RtTypeIdentifier.ULong_0);

else
{
outputStream.WriteByte( (byte)RtTypeIdentifier.ULong);

outputStream.WriteULong(targetValue, endian);
}

}

}

}
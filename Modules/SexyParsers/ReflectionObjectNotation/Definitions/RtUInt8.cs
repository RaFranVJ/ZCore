using System.Text.Json;

namespace ZCore.Modules.SexyParsers.ReflectionObjectNotation.Definitions
{
/// <summary> Represents an 8-bits unsigned Integer in the RtSystem. </summary>

public static class RtUInt8
{
/** <summary> Reads an 8-bits unsigned Integer from a RTON File and Writes its Representation to a JSON File. </summary>

<param name = "inputStream"> The Stream which Contains the RTON Data to be Read. </param>
<param name = "outputStream"> The Stream where the JSON Data will be Written. </param>
<param name = "sourceID"> The Identifier of the RTON Value. </param> */

public static void Read(BinaryStream inputStream, Utf8JsonWriter outputStream, RtTypeIdentifier sourceID)
{
byte value = (sourceID == RtTypeIdentifier.Byte) ? inputStream.ReadByte() : (byte)0;

outputStream.WriteNumberValue(value);
}

/** <summary> Reads an 8-bits unsigned Integer from a JSON File and Writes its Representation to a RTON File. </summary>

<param name = "outputStream"> The Stream where the RTON Data will be Written. </param>
<param name = "targetValue"> The UInt8 to be Written. </param> */

public static void Write(BinaryStream outputStream, byte targetValue)
{

if(targetValue == 0)
outputStream.WriteByte( (byte)RtTypeIdentifier.Byte_0);

else
{
outputStream.WriteByte( (byte)RtTypeIdentifier.Byte);

outputStream.WriteByte(targetValue);
}

}

}

}
using System.Text.Json;

namespace ZCore.Modules.SexyParsers.ReflectionObjectNotation.Definitions
{
/// <summary> Represents a 32-bits signed Integer in the RtSystem. </summary>

public static class RtInt32
{
/** <summary> Reads a 32-bits signed Integer from a RTON File and Writes its Representation to a JSON File. </summary>

<param name = "inputStream"> The Stream which Contains the RTON Data to be Read. </param>
<param name = "outputStream"> The Stream where the JSON Data will be Written. </param>
<param name = "sourceID"> The Identifier of the RTON Value. </param>
<param name = "endian"> The endian Order of the RTON Data. </param> */

public static void Read(BinaryStream inputStream, Utf8JsonWriter outputStream, RtTypeIdentifier sourceID,
Endian endian = default)
{
int value = (sourceID == RtTypeIdentifier.Int) ? inputStream.ReadInt(endian) : 0;

outputStream.WriteNumberValue(value);
}

/** <summary> Reads a 32-bits signed Integer from a JSON File and Writes its Representation to a RTON File. </summary>

<param name = "outputStream"> The Stream where the RTON Data will be Written. </param>
<param name = "targetValue"> The Int32 to be Written. </param>
<param name = "endian"> The endian Order of the RTON Data. </param> */

public static void Write(BinaryStream outputStream, int targetValue, Endian endian = default)
{

if(targetValue == 0)
outputStream.WriteByte( (byte)RtTypeIdentifier.Int_0);

else
{
outputStream.WriteByte( (byte)RtTypeIdentifier.Int);

outputStream.WriteInt(targetValue, endian);
}

}

}

}
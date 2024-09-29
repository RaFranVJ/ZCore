using System.Text.Json;

namespace ZCore.Modules.SexyParsers.ReflectionObjectNotation.Definitions
{
/// <summary> Represents an 8-bits signed Integer in the RtSystem. </summary>

public static class RtInt8
{
/** <summary> Reads an 8-bits signed Interger from a RTON File and Writes its Representation to a JSON File. </summary>

<param name = "inputStream"> The Stream which Contains the RTON Data to be Read. </param>
<param name = "outputStream"> The Stream where the JSON Data will be Written. </param>
<param name = "sourceID"> The Identifier of the RTON Value. </param> */

public static void Read(BinaryStream inputStream, Utf8JsonWriter outputStream, RtTypeIdentifier sourceID)
{
sbyte value = (sourceID == RtTypeIdentifier.SByte) ? inputStream.ReadSByte() : (sbyte)0;

outputStream.WriteNumberValue(value);
}

/** <summary> Reads an 8-bits signed from a JSON File and Writes its Representation to a RTON File. </summary>

<param name = "outputStream"> The Stream where the RTON Data will be Written. </param>
<param name = "targetValue"> The Int8 to be Written. </param> */

public static void Write(BinaryStream outputStream, sbyte targetValue)
{

if(targetValue == 0)
outputStream.WriteByte( (byte)RtTypeIdentifier.SByte_0);

else
{
outputStream.WriteByte( (byte)RtTypeIdentifier.SByte);

outputStream.WriteSByte(targetValue);
}

}

}

}
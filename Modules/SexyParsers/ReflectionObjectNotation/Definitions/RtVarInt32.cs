using System.Text.Json;

namespace ZCore.Modules.SexyParsers.ReflectionObjectNotation.Definitions
{
/// <summary> Represents a 32-bits variant Integer in the RtSystem. </summary>

public static class RtVarInt32
{
/** <summary> Reads a 32-bits variant Integer from a RTON File and Writes its Representation to a JSON File. </summary>

<param name = "inputStream"> The Stream which Contains the RTON Data to be Read. </param>
<param name = "outputStream"> The Stream where the JSON Data will be Written. </param> */

public static void Read(BinaryStream inputStream, Utf8JsonWriter outputStream)
{
int value = inputStream.ReadVarInt();

outputStream.WriteNumberValue(value);
}

/** <summary> Reads a 32-bits variant Integer from a JSON File and Writes its Representation to a RTON File. </summary>

<param name = "outputStream"> The Stream where the RTON Data will be Written. </param>
<param name = "targetValue"> The VarInt32 to be Written. </param> */

public static void Write(BinaryStream outputStream, int targetValue)
{
outputStream.WriteByte( (byte)RtTypeIdentifier.VarInt);

outputStream.WriteVarInt(targetValue);
}

}

}
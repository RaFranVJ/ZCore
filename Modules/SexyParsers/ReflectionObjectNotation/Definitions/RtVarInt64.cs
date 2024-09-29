using System.Text.Json;

namespace ZCore.Modules.SexyParsers.ReflectionObjectNotation.Definitions
{
/// <summary> Represents a 64-bits variant Integer in the RtSystem. </summary>

public static class RtVarInt64
{
/** <summary> Reads a 64-bits variant Integer from a RTON File and Writes its Representation to a JSON File. </summary>

<param name = "inputStream"> The Stream which Contains the RTON Data to be Read. </param>
<param name = "outputStream"> The Stream where the JSON Data will be Written. </param> */

public static void Read(BinaryStream inputStream, Utf8JsonWriter outputStream)
{
long value = inputStream.ReadVarLong();

outputStream.WriteNumberValue(value);
}

/** <summary> Reads a 64-bits variant Integer from a JSON File and Writes its Representation to a RTON File. </summary>

<param name = "inputStream"> The Stream which Contains the JSON Data to be Read. </param>
<param name = "outputStream"> The Stream where the RTON Data will be Written. </param> */

public static void Write(Utf8JsonReader inputStream, BinaryStream outputStream)
{
outputStream.WriteByte( (byte)RtTypeIdentifier.VarLong);

long value = inputStream.GetInt64();
outputStream.WriteVarLong(value);
}

}

}
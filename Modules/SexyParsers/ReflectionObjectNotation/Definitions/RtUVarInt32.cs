using System.Text.Json;

namespace ZCore.Modules.SexyParsers.ReflectionObjectNotation.Definitions
{
/// <summary> Represents an unsigned 32-bits variant Integer in the RtSystem. </summary>

public static class RtUVarInt32
{
/** <summary> Reads an unsigned 32-bits variant Integer from a RTON File and Writes its Representation to a JSON File. </summary>

<param name = "inputStream"> The Stream which Contains the RTON Data to be Read. </param>
<param name = "outputStream"> The Stream where the JSON Data will be Written. </param> */

public static void Read(BinaryStream inputStream, Utf8JsonWriter outputStream)
{
uint value = inputStream.ReadUVarInt();

outputStream.WriteNumberValue(value);
}

/** <summary> Reads an unsigned 32-bits variant Integer from a JSON File and Writes its Representation to a RTON File. </summary>

<param name = "inputStream"> The Stream which Contains the JSON Data to be Read. </param>
<param name = "outputStream"> The Stream where the RTON Data will be Written. </param> */

public static void Write(Utf8JsonReader inputStream, BinaryStream outputStream)
{
outputStream.WriteByte( (byte)RtTypeIdentifier.UVarInt);

uint value = inputStream.GetUInt32();
outputStream.WriteUVarInt(value);
}

}

}
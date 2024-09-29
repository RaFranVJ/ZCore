using System.Text.Json;
using ZCore.Modules.SexyParsers.ReflectionObjectNotation.Exceptions;

namespace ZCore.Modules.SexyParsers.ReflectionObjectNotation.Definitions
{
/// <summary> Represents an Array in the RtSystem. </summary>

public static class RtArray
{
/** <summary> Reads an Array from a RTON File and Writes its Representation to a JSON File. </summary>

<param name = "inputStream"> The Stream which Contains the RTON Data to be Read. </param>
<param name = "outputStream"> The Stream where the JSON Data will be Written. </param>
<param name = "endian"> The endian Order of the RTON Data. </param> */

public static void Read(BinaryStream rtonStream, Utf8JsonWriter jsonWriter,
Endian endian = default, bool ignoreStrLengthMismatch = false)
{
rtonStream.CompareByte<RtArray_InvalidStartIdException>( (byte)RtTypeIdentifier.Array_Start);
jsonWriter.WriteStartArray();

uint elementsCount = rtonStream.ReadUVarInt();

for(int i = 0; i < elementsCount; i++)
{
var elementType = (RtTypeIdentifier)rtonStream.ReadByte();

RtObject.Decode(rtonStream, jsonWriter, elementType, endian, ignoreStrLengthMismatch);
}

rtonStream.CompareByte<RtArray_InvalidEndIdException>( (byte)RtTypeIdentifier.Array_End);

jsonWriter.WriteEndArray();
}

/** <summary> Reads an Array from a JSON File and Writes its Representation to a RTON File. </summary>

<param name = "outputStream"> The Stream where the RTON Data will be Written. </param>
<param name = "sourceData"> The JSON Array to be Read. </param>
<param name = "endian"> The endian Order of the RTON Data. </param> 

<returns> The Array Size </returns> */

public static void Write(BinaryStream rtonStream, JsonElement jsonArray, bool encodeStrWithUtf, Endian endian = default)
{
rtonStream.WriteByte( (byte)RtTypeIdentifier.Array_Start);

uint elementsCount = (uint)jsonArray.GetArrayLength();
rtonStream.WriteUVarInt(elementsCount);

for(int i = 0; i < elementsCount; i++)
RtObject.Encode(rtonStream, jsonArray[i], encodeStrWithUtf, endian);

rtonStream.WriteByte( (byte)RtTypeIdentifier.Array_End);
}

}

}
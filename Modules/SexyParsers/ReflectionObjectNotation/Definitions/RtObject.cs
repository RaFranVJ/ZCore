using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using ZCore.Modules.SexyParsers.ReflectionObjectNotation.Exceptions;

namespace ZCore.Modules.SexyParsers.ReflectionObjectNotation.Definitions
{
/// <summary> Represents an Object in the RtSystem. </summary>

public static class RtObject
{
/** <summary> Evaluates the Current JSON Token and writes its RTON Equivalent. </summary>

<param name = "jsonReader"> The JSON Reader. </param>
<param name = "outputStream"> The RTON Stream. </param>
<param name = "endian"> The endian Order of the RTON Data. </param> */

public static void Encode(BinaryStream outputStream, JsonElement jsonValue,
bool encodeStrWithUtf, Endian endian = default)
{

switch(jsonValue.ValueKind)
{
case JsonValueKind.Null:

case JsonValueKind.Undefined:
outputStream.WriteByte( (byte)RtTypeIdentifier.Null);
break;

case JsonValueKind.Object:
outputStream.WriteByte( (byte)RtTypeIdentifier.Object_Start);

Write(outputStream, jsonValue, encodeStrWithUtf, endian);
break;

case JsonValueKind.Array:
outputStream.WriteByte( (byte)RtTypeIdentifier.Array);

RtArray.Write(outputStream, jsonValue, encodeStrWithUtf, endian);
break;

case JsonValueKind.Number:
RtNumber.EvaluateNumericValue(outputStream, jsonValue.GetDouble(), endian);
break;

case JsonValueKind.True:

case JsonValueKind.False:
RtBoolean.Write(outputStream, jsonValue.GetBoolean() );
break;

default:
RtString.EvaluateStringType(outputStream, jsonValue.GetString(), encodeStrWithUtf, endian);
break;
}

jsonValue = default; // Release Obj Memory
}

/** <summary> Reads an Object from a JSON File and Writes its Representation to a RTON File. </summary>

<param name = "outputStream"> The Stream where the RTON Data will be Written. </param>
<param name = "jsonData"> The JSON Data. </param>
<param name = "endian"> The endian Order of the RTON Data. </param> */

public static void Write(BinaryStream outputStream, JsonElement jsonData, bool encodeStrWithUtf, Endian endian = default)
{
var jsonMap = jsonData.EnumerateObject();
jsonData = default; // Release Obj Memory

for(int i = 0; i < jsonMap.Count(); i++)
{
RtString.EvaluateStringType(outputStream, jsonMap.ElementAt(i).Name, encodeStrWithUtf, endian);

Encode(outputStream, jsonMap.ElementAt(i).Value, encodeStrWithUtf, endian);
}

jsonMap = default; // Release Obj Memory
outputStream.WriteByte( (byte)RtTypeIdentifier.Object_End);
}

/** <summary> Decodes a Object from a RTON File and writes its Contents to a JSON File. </summary>

<param name = "inputStream"> The Stream which Contains the RTON Data to be Read. </param>
<param name = "outputStream"> The Stream where the JSON Data will be Written. </param>
<param name = "sourceID"> The Identifier of the RTON Value. </param>
<param name = "endian"> The endian Order of the RTON Data. </param> */

public static void Decode(BinaryStream inputStream, Utf8JsonWriter outputStream, RtTypeIdentifier sourceID, 
Endian endian = default, bool ignoreStrLengthMismatch = false)
{

switch(sourceID)
{
case RtTypeIdentifier.Bool_false:

case RtTypeIdentifier.Bool_true:
RtBoolean.Read(outputStream, sourceID);
break;

case RtTypeIdentifier.Null:
outputStream.WriteStringValue("\\*");
break;

case RtTypeIdentifier.Byte:

case RtTypeIdentifier.Byte_0:
RtUInt8.Read(inputStream, outputStream, sourceID);
break;

case RtTypeIdentifier.SByte:

case RtTypeIdentifier.SByte_0:
RtInt8.Read(inputStream, outputStream, sourceID);
break;

case RtTypeIdentifier.Short:

case RtTypeIdentifier.Short_0:
RtInt16.Read(inputStream, outputStream, sourceID, endian);
break;

case RtTypeIdentifier.UShort:

case RtTypeIdentifier.UShort_0:
RtUInt16.Read(inputStream, outputStream, sourceID, endian);
break;

case RtTypeIdentifier.Int:

case RtTypeIdentifier.Int_0:
RtInt32.Read(inputStream, outputStream, sourceID, endian);
break;

case RtTypeIdentifier.Float:

case RtTypeIdentifier.Float_0:
RtFloat32.Read(inputStream, outputStream, sourceID, endian);
break;

case RtTypeIdentifier.VarInt:
RtVarInt32.Read(inputStream, outputStream);
break;

case RtTypeIdentifier.ZigZagInt:
RtZigZagInt32.Read(inputStream, outputStream);;
break;

case RtTypeIdentifier.UInt:

case RtTypeIdentifier.UInt_0:
RtUInt32.Read(inputStream, outputStream, sourceID, endian);
break;

case RtTypeIdentifier.UVarInt:
RtUVarInt32.Read(inputStream, outputStream);
break;

case RtTypeIdentifier.Long:

case RtTypeIdentifier.Long_0:
RtInt64.Read(inputStream, outputStream, sourceID, endian);
break;

case RtTypeIdentifier.Double:

case RtTypeIdentifier.Double_0:
RtFloat64.Read(inputStream, outputStream, sourceID, endian);
break;

case RtTypeIdentifier.VarLong:
RtVarInt64.Read(inputStream, outputStream);
break;

case RtTypeIdentifier.ZigZagLong:
RtZigZagInt64.Read(inputStream, outputStream);
break;

case RtTypeIdentifier.ULong:

case RtTypeIdentifier.ULong_0:
RtUInt64.Read(inputStream, outputStream, sourceID, endian);
break;

case RtTypeIdentifier.UVarLong:
RtUVarInt64.Read(inputStream, outputStream);
break;

case RtTypeIdentifier.NativeString:
RtNativeString.Read(inputStream, outputStream, false, endian);
break;

case RtTypeIdentifier.UnicodeString:
RtUnicodeString.Read(inputStream, outputStream, false, endian, ignoreStrLengthMismatch);
break;

case RtTypeIdentifier.IDString:

case RtTypeIdentifier.IDString_Null:
RtIDString.Read(inputStream, outputStream, false, sourceID, ignoreStrLengthMismatch);
break;

case RtTypeIdentifier.Object_Start:
Read(inputStream, outputStream, endian, ignoreStrLengthMismatch);
break;

case RtTypeIdentifier.Array:
RtArray.Read(inputStream, outputStream, endian, ignoreStrLengthMismatch);
break;

case RtTypeIdentifier.BinaryString:
RtBinaryString.Read(inputStream, outputStream, false, ignoreStrLengthMismatch);
break;

case RtTypeIdentifier.NativeString_Value:
RtNativeString.Read(inputStream, outputStream, false, false, endian);
break;

case RtTypeIdentifier.NativeString_Index:
RtNativeString.Read(inputStream, outputStream, true, false);
break;

case RtTypeIdentifier.UnicodeString_Value:
RtUnicodeString.Read(inputStream, outputStream, false, false, endian, ignoreStrLengthMismatch);
break;

case RtTypeIdentifier.UnicodeString_Index:
RtUnicodeString.Read(inputStream, outputStream, true, false, ignoreLengthMitsmach: ignoreStrLengthMismatch);
break;

case RtTypeIdentifier.ObjectWithBoolean:
ReadBoolean(inputStream, outputStream);
break;

default:
throw new RtObject_InvalidValueIdException(inputStream.Position, sourceID);
}

}

/** <summary> Reads an Object from a RTON File and Writes its Representation to a JSON File. </summary>

<param name = "inputStream"> The Stream which Contains the RTON Data to be Read. </param>
<param name = "outputStream"> The Stream where the JSON Data will be Written. </param>
<param name = "endian"> The endian Order of the RTON Data. </param> */

public static void Read(BinaryStream inputStream, Utf8JsonWriter outputStream,
Endian endian = default, bool ignoreStrLengthMismatch = false)
{
outputStream.WriteStartObject();

while(true)
{
var objIdentifier = (RtTypeIdentifier)inputStream.ReadByte();

if(objIdentifier == RtTypeIdentifier.Object_End)
break;

switch(objIdentifier)
{
case RtTypeIdentifier.Null:
outputStream.WritePropertyName("\\*");
break;

case RtTypeIdentifier.NativeString:
RtNativeString.Read(inputStream, outputStream, true, endian);
break;

case RtTypeIdentifier.UnicodeString:
RtUnicodeString.Read(inputStream, outputStream, true, endian, ignoreStrLengthMismatch);
break;

case RtTypeIdentifier.IDString:

case RtTypeIdentifier.IDString_Null:
RtIDString.Read(inputStream, outputStream, true, objIdentifier, ignoreStrLengthMismatch);
break;

case RtTypeIdentifier.BinaryString:
RtBinaryString.Read(inputStream, outputStream, true, ignoreStrLengthMismatch);
break;

case RtTypeIdentifier.NativeString_Value:
RtNativeString.Read(inputStream, outputStream, false, true, endian);
break;

case RtTypeIdentifier.NativeString_Index:
RtNativeString.Read(inputStream, outputStream, true, true, endian);
break;

case RtTypeIdentifier.UnicodeString_Value:
RtUnicodeString.Read(inputStream, outputStream, false, true, endian, ignoreStrLengthMismatch);
break;

case RtTypeIdentifier.UnicodeString_Index:
RtUnicodeString.Read(inputStream, outputStream, true, true, endian, ignoreStrLengthMismatch);
break;

default:
throw new RtObject_InvalidPropertyIdException(inputStream.Position, objIdentifier);
}

objIdentifier = (RtTypeIdentifier)inputStream.ReadByte();

Decode(inputStream, outputStream, objIdentifier, endian, ignoreStrLengthMismatch);
}

outputStream.WriteEndObject();
}

/** <summary> Reads a Boolean from a RtObject and Writes its Representation to a JsonObject. </summary>

<param name = "inputStream"> The Stream which Contains the RTON Data to be Read. </param>
<param name = "outputStream"> The Stream where the JSON Data will be Written. </param> */

private static void ReadBoolean(BinaryStream inputStream, Utf8JsonWriter jsonWriter)
{
bool value = inputStream.ReadBool();

jsonWriter.WriteBooleanValue(value);
}

}

}
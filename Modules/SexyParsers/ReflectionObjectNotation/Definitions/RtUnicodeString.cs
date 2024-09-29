using System;
using System.Text;
using System.Text.Json;
using ZCore.Modules.SexyParsers.ReflectionObjectNotation.Exceptions;

namespace ZCore.Modules.SexyParsers.ReflectionObjectNotation.Definitions
{
/// <summary> Represents a Unicode String in the RtSystem. </summary>

public static class RtUnicodeString
{
/// <summary> The Encoding of a Unicode String. </summary>

private static readonly Encoding UnicodeStrEncoding = Encoding.UTF32;

/** <summary> Decodes a Unicode String from a RTON File. </summary>

<param name = "sourceStream"> The Stream which Contains the Data to be Decoded. </param>
<param name = "endian"> The endian Order of the RTON Data. </param>

<returns> The Decoded String. </returns> */

private static string DecodeString(BinaryStream sourceStream, Endian endian, bool ignoreLengthMitsmach,
RtTypeIdentifier strFlags = RtTypeIdentifier.UnicodeString)
{
int expectedLen = sourceStream.ReadVarInt();
string unicodeStr = sourceStream.ReadStringByVarIntLength(UnicodeStrEncoding, endian);

if( (unicodeStr.Length < expectedLen) && !ignoreLengthMitsmach)
throw new RtString_LengthMismatchException(sourceStream.Position, strFlags, unicodeStr, expectedLen);

return unicodeStr;
}

/** <summary> Reads a Unicode String from a RTON File and Writes its Representation to a JSON File. </summary>

<param name = "inputStream"> The Stream which Contains the RTON Data to be Read. </param>
<param name = "outputStream"> The Stream where the JSON Data will be Written. </param>
<param name = "isPropertyName"> Determines if the UnicodeString should be Written as a PropertyName or not. </param>
<param name = "endian"> The endian Order of the RTON Data. </param> */

public static void Read(BinaryStream inputStream, Utf8JsonWriter outputStream, bool isPropertyName,
Endian endian = default, bool ignoreLengthMitsmach = false)
{
string unicodeString = DecodeString(inputStream, endian, ignoreLengthMitsmach);

RtString.WriteJsonString(outputStream, unicodeString, isPropertyName);
}

/** <summary> Reads a Unicode String from a RTON File and Writes its Representation to a JSON File. </summary>

<param name = "inputStream"> The Stream which Contains the RTON Data to be Read. </param>
<param name = "outputStream"> The Stream where the JSON Data will be Written. </param>
<param name = "isIndexed"> A boolean that Determines if the UnicodeString is in the Reference List or not. </param>
<param name = "isPropertyName"> Determines if the UnicodeString should be Written as a PropertyName or not. </param> */

public static void Read(BinaryStream inputStream, Utf8JsonWriter outputStream, bool isIndexed,
bool isPropertyName, Endian endian = default, bool ignoreLengthMitsmach = false)
{
string unicodeString;

if(isIndexed)
{
int unicodeStrIndex = inputStream.ReadVarInt();

unicodeString = ReferenceStringsHandler.GetStringFromUnicodeList(unicodeStrIndex);
}

else
{
unicodeString = DecodeString(inputStream, endian, ignoreLengthMitsmach, RtTypeIdentifier.UnicodeString_Value);

ReferenceStringsHandler.AddStringToUnicodeList(unicodeString);
}

RtString.WriteJsonString(outputStream, unicodeString, isPropertyName);
}

/** <summary> Reads a Unicode String from a JSON File and Writes its Representation to a RTON File. </summary>

<param name = "outputStream"> The Stream where the RTON Data will be Written. </param>
<param name = "targetStr"> The String to be Written. </param>
<param name = "endian"> The endian Order of the RTON Data. </param> */

public static void Write(BinaryStream outputStream, string targetStr, Endian endian = default)
{

if(ReferenceStringsHandler.ListHasUnicodeString(targetStr) )
{
outputStream.WriteByte( (byte)RtTypeIdentifier.UnicodeString_Index);

int unicodeStrIndex = ReferenceStringsHandler.GetUnicodeStringIndex(targetStr);

outputStream.WriteVarInt(unicodeStrIndex);
}

else
{
outputStream.WriteByte( (byte)RtTypeIdentifier.UnicodeString_Value);

outputStream.WriteVarInt(targetStr.Length);
outputStream.WriteStringByVarIntLength(targetStr, UnicodeStrEncoding, endian);

ReferenceStringsHandler.AddStringToUnicodeList(targetStr);
}

}

}

}
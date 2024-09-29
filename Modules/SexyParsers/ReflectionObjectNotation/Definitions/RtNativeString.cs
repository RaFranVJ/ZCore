using System.Text;
using System.Text.Json;

namespace ZCore.Modules.SexyParsers.ReflectionObjectNotation.Definitions
{
/// <summary> Represents a Native String in the RtSystem. </summary>

public static class RtNativeString
{
/// <summary> The Encoding of a Native String. </summary>

private static readonly Encoding NativeStrEncoding = Encoding.Default;

/** <summary> Reads a Native String from a RTON File and Writes its Representation to a JSON File. </summary>

<param name = "inputStream"> The Stream which Contains the RTON Data to be Read. </param>
<param name = "outputStream"> The Stream where the JSON Data will be Written. </param>
<param name = "isPropertyName"> Determines if the NativeString should be Written as a PropertyName or not. </param>
<param name = "endian"> The endian Order of the RTON Data. </param> */

public static void Read(BinaryStream inputStream, Utf8JsonWriter outputStream, bool isPropertyName, 
Endian endian = default)
{
string nativeString = inputStream.ReadStringByVarIntLength(NativeStrEncoding, endian);

RtString.WriteJsonString(outputStream, nativeString, isPropertyName);
}

/** <summary> Reads a Native String from a RTON File and Writes its Representation to a JSON File. </summary>

<param name = "inputStream"> The Stream which Contains the RTON Data to be Read. </param>
<param name = "outputStream"> The Stream where the JSON Data will be Written. </param>
<param name = "isIndexed"> A boolean that Determines if the NativeString is in the Reference List or not. </param>
<param name = "isPropertyName"> Determines if the NativeString should be Written as a PropertyName or not. </param>
<param name = "endian"> The endian Order of the RTON Data. </param> */

public static void Read(BinaryStream inputStream, Utf8JsonWriter outputStream, bool isIndexed, bool isPropertyName,
Endian endian = default)
{
string nativeString;

if(isIndexed)
{
int nativeStrIndex = inputStream.ReadVarInt();

nativeString = ReferenceStringsHandler.GetStringFromNativeList(nativeStrIndex);
}

else
{
nativeString = inputStream.ReadStringByVarIntLength(NativeStrEncoding, endian);

ReferenceStringsHandler.AddStringToNativeList(nativeString);
}

RtString.WriteJsonString(outputStream, nativeString, isPropertyName);
}

/** <summary> Reads a Native String from a JSON File and Writes its Representation to a RTON File. </summary>

<param name = "outputStream"> The Stream where the RTON Data will be Written. </param>
<param name = "targetStr"> The String to be Written. </param>
<param name = "endian"> The endian Order of the RTON Data. </param> */

public static void Write(BinaryStream outputStream, string targetStr, Endian endian = default)
{

if(ReferenceStringsHandler.ListHasNativeString(targetStr) )
{
outputStream.WriteByte( (byte)RtTypeIdentifier.NativeString_Index);

int nativeStrIndex = ReferenceStringsHandler.GetNativeStringIndex(targetStr);

outputStream.WriteVarInt(nativeStrIndex);
}

else
{
outputStream.WriteByte( (byte)RtTypeIdentifier.NativeString_Value);

outputStream.WriteStringByVarIntLength(targetStr, NativeStrEncoding, endian);

ReferenceStringsHandler.AddStringToNativeList(targetStr);
}

}

}

}
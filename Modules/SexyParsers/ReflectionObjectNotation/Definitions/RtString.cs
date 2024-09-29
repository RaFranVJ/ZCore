using System.Text.Json;

namespace ZCore.Modules.SexyParsers.ReflectionObjectNotation.Definitions
{
/// <summary> Represents a String in the RtSystem. </summary>

public static class RtString
{
/** <summary> Evaluates the Type of a JSON String and writes the String to a RTON File according to its Type. </summary>

<param name = "outputStream"> The Stream where the RTON Data will be Written. </param>
<param name = "sourceStr"> The JSON String to Evaluate. </param>
<param name = "endian"> The endian Order of the RTON Data. </param> */

public static void EvaluateStringType(BinaryStream outputStream, string sourceStr,
bool encodeStrWithUtf, Endian endian = default)
{
var binaryStrMatch = RtBinaryString.PerformRegex(sourceStr);
var rtIDStrMatch = RtIDString.PerformRegex(sourceStr);

if(sourceStr == null)
outputStream.WriteByte( (byte)RtTypeIdentifier.Null);

else if(sourceStr == "\u0065")
RtFloat32.Write(outputStream, float.E, endian);

else if(sourceStr == "\u03B5")
RtFloat32.Write(outputStream, float.Epsilon, endian);

else if(sourceStr == "\u03C0")
RtFloat32.Write(outputStream, float.Pi, endian);

else if(sourceStr == "\u03C4")
RtFloat32.Write(outputStream, float.Tau, endian);

else if(sourceStr == "-Infinity")
RtFloat32.Write(outputStream, float.NegativeInfinity, endian);

else if(sourceStr == "Infinity")
RtFloat32.Write(outputStream, float.PositiveInfinity, endian);

else if(sourceStr == "NaN")
RtFloat32.Write(outputStream, float.NaN, endian);

else if(encodeStrWithUtf)
RtUnicodeString.Write(outputStream, sourceStr, endian);

else if(rtIDStrMatch.Success)
RtIDString.Write(outputStream, rtIDStrMatch);

else if(binaryStrMatch.Success)
RtBinaryString.Write(outputStream, binaryStrMatch);

else
RtNativeString.Write(outputStream, sourceStr, endian);

}

/** <summary> Writes a String to a JSON File with the Specified Options. </summary>

<param name = "outputStream"> The Stream where the JSON Data will be Written. </param>
<param name = "targetStr"> The String to be Written. </param>
<param name = "isPropertyName"> Determines if the JsonString should be Written as a PropertyName or not. </param> */

public static void WriteJsonString(Utf8JsonWriter outputStream, string targetStr, bool isPropertyName)
{

if(isPropertyName)
outputStream.WritePropertyName(targetStr);

else
outputStream.WriteStringValue(targetStr);

}

}

}
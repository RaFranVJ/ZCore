using System.Text.Json;

namespace ZCore.Modules.SexyParsers.ReflectionObjectNotation.Definitions
{
/// <summary> Represents a 32-bits Float-point in the RtSystem. </summary>

public static class RtFloat32
{
/** <summary> Evaluates the Type of a RTON Float and writes the Value to a JSON File according to its Type. </summary>

<param name = "outputStream"> The Stream where the RTON Data will be Written. </param>
<param name = "sourceStr"> The Float to Evaluate. </param>
<param name = "endian"> The endian Order of the RTON Data. </param> */

private static void EvaluateValue(BinaryStream outputStream, float sourceValue, Endian endian)
{

if(sourceValue == 0.0f)
outputStream.WriteByte( (byte)RtTypeIdentifier.Float_0);

else if(sourceValue == float.Epsilon)
RtUnicodeString.Write(outputStream, "\u03B5", endian);

else if(sourceValue == float.E)
RtUnicodeString.Write(outputStream, "\u0065", endian);

else if(sourceValue == float.Pi)
RtUnicodeString.Write(outputStream, "\u03C0", endian);

else if(sourceValue == float.Tau)
RtUnicodeString.Write(outputStream, "\u03C4", endian);

else if(sourceValue == float.NegativeInfinity)
RtNativeString.Write(outputStream, "-Infinity", endian);

else if(sourceValue == float.PositiveInfinity)
RtNativeString.Write(outputStream, "Infinity", endian);

else if(sourceValue == float.NaN)
RtNativeString.Write(outputStream, "NaN", endian);

else
{
outputStream.WriteByte( (byte)RtTypeIdentifier.Float);

outputStream.WriteFloat(sourceValue, endian);
}

}

/** <summary> Reads a 32-bits Float-point from a RTON File and Writes its Representation to a JSON File. </summary>

<param name = "inputStream"> The Stream which Contains the RTON Data to be Read. </param>
<param name = "outputStream"> The Stream where the JSON Data will be Written. </param>
<param name = "sourceID"> The Identifier of the RTON Value. </param>
<param name = "endian"> The endian Order of the RTON Data. </param> */

public static void Read(BinaryStream inputStream, Utf8JsonWriter outputStream, RtTypeIdentifier sourceID, Endian endian = default)
{
float value = (sourceID == RtTypeIdentifier.Float) ? inputStream.ReadFloat(endian) : 0.0f;

outputStream.WriteNumberValue(value);
}

/** <summary> Reads a 32-bits Float-point from a JSON File and Writes its Representation to a RTON File. </summary>

<param name = "outputStream"> The Stream where the RTON Data will be Written. </param>
<param name = "targetValue"> The Float32 to be Written. </param>
<param name = "endian"> The endian Order of the RTON Data. </param> */

public static void Write(BinaryStream outputStream, float targetValue, Endian endian = default) => EvaluateValue(outputStream, targetValue, endian);
}

}
using System.Text.Json;

namespace ZCore.Modules.SexyParsers.ReflectionObjectNotation.Definitions
{
/// <summary> Represents a 64-bits Float-point in the RtSystem. </summary>

public static class RtFloat64
{
/** <summary> Evaluates the Type of a RTON Double and writes the Value to a JSON File according to its Type. </summary>

<param name = "outputStream"> The Stream where the RTON Data will be Written. </param>
<param name = "sourceStr"> The Double to Evaluate. </param>
<param name = "endian"> The endian Order of the RTON Data. </param> */

private static void EvaluateValue(BinaryStream outputStream, double sourceValue, Endian endian)
{

if(sourceValue == 0.0d)
outputStream.WriteByte( (byte)RtTypeIdentifier.Double_0);

else if(sourceValue == double.Epsilon)
RtUnicodeString.Write(outputStream, "\u03B5", endian);

else if(sourceValue == double.E)
RtUnicodeString.Write(outputStream, "\u0065", endian);

else if(sourceValue == double.Pi)
RtUnicodeString.Write(outputStream, "\u03C0", endian);

else if(sourceValue == double.Tau)
RtUnicodeString.Write(outputStream, "\u03C4", endian);

else if(sourceValue == double.NegativeInfinity)
RtNativeString.Write(outputStream, "-Infinity", endian);

else if(sourceValue == double.PositiveInfinity)
RtNativeString.Write(outputStream, "Infinity", endian);

else if(sourceValue == double.NaN)
RtNativeString.Write(outputStream, "NaN", endian);

else
{
outputStream.WriteByte( (byte)RtTypeIdentifier.Double);

outputStream.WriteDouble(sourceValue, endian);
}

}

/** <summary> Reads a 64-bits Float-point from a RTON File and Writes its Representation to a JSON File. </summary>

<param name = "inputStream"> The Stream which Contains the RTON Data to be Read. </param>
<param name = "outputStream"> The Stream where the JSON Data will be Written. </param>
<param name = "sourceID"> The Identifier of the RTON Value. </param>
<param name = "endian"> The endian Order of the RTON Data. </param> */

public static void Read(BinaryStream inputStream, Utf8JsonWriter outputStream, RtTypeIdentifier sourceID, Endian endian = default)
{
double value = (sourceID == RtTypeIdentifier.Double) ? inputStream.ReadDouble(endian) : 0.0d;

outputStream.WriteNumberValue(value);
}

/** <summary> Reads a 64-bits Float-point from a JSON File and Writes its Representation to a RTON File. </summary>

<param name = "outputStream"> The Stream where the RTON Data will be Written. </param>
<param name = "targetValue"> The Float64 to be Written. </param>
<param name = "endian"> The endian Order of the RTON Data. </param> */

public static void Write(BinaryStream outputStream, double targetValue, Endian endian = default) => EvaluateValue(outputStream, targetValue, endian);
}

}
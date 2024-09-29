using System.Text.Json;

namespace ZCore.Modules.SexyParsers.ReflectionObjectNotation.Definitions
{
/// <summary> Represents a Nunber in the RtSystem. </summary>

public static class RtNumber
{
/** <summary> Checks if a Value is a Single-Precision or a Double-Precision Point 
and writes the Value to a RTON File. </summary>

<param name = "outputStream"> The Stream where the RTON Data will be Written. </param>
<param name = "targetValue"> The Value to be Written. </param>
<param name = "endian"> The endian Order of the RTON Data. </param> */

private static void CheckFloatPoint(BinaryStream outputStream, double targetValue, Endian endian = default)
{

if(targetValue <= float.MaxValue)
RtFloat32.Write(outputStream, (float)targetValue, endian);

else
RtFloat64.Write(outputStream, targetValue, endian);

}

/** <summary> Checks the Type of a Integer (either Signed or Unsigned) and writes the Value to a RTON File. </summary>

<param name = "outputStream"> The Stream where the RTON Data will be Written. </param>
<param name = "targetValue"> The Value to be Written. </param>
<param name = "endian"> The endian Order of the RTON Data. </param> */

private static void CheckUnsignedInteger(BinaryStream outputStream, double targetValue, Endian endian = default)
{

if(targetValue >= 16384 && targetValue <= short.MaxValue)
RtInt16.Write(outputStream, (short)targetValue, endian);

else if(targetValue > short.MaxValue && targetValue <= ushort.MaxValue)
RtUInt16.Write(outputStream, (ushort)targetValue, endian);

else if(targetValue > ushort.MaxValue && targetValue <= int.MaxValue)
RtInt32.Write(outputStream, (int)targetValue, endian);

else if(targetValue > int.MaxValue && targetValue <= uint.MaxValue)
RtUInt32.Write(outputStream, (uint)targetValue, endian);

else if(targetValue > uint.MaxValue && targetValue <= long.MaxValue)
RtInt64.Write(outputStream, (long)targetValue, endian);

else if(targetValue > long.MaxValue && targetValue <= ulong.MaxValue)
RtUInt64.Write(outputStream, (ulong)targetValue, endian);

else
RtVarInt32.Write(outputStream, (int)targetValue);

}

/** <summary> Checks if the Type of a signed Integer and writes the Value to a RTON File. </summary>

<param name = "outputStream"> The Stream where the RTON Data will be Written. </param>
<param name = "targetValue"> The Value to be Written. </param>
<param name = "endian"> The endian Order of the RTON Data. </param> */

private static void CheckSignedInteger(BinaryStream outputStream, double targetValue, Endian endian = default)
{

if(targetValue < short.MinValue && targetValue >= short.MinValue)
RtInt16.Write(outputStream, (short)targetValue, endian);

else if(targetValue < short.MinValue && targetValue >= int.MinValue)
RtInt32.Write(outputStream, (int)targetValue, endian);

else if(targetValue < int.MinValue && targetValue >= long.MinValue)
RtInt64.Write(outputStream, (long)targetValue, endian);

else
RtZigZagInt32.Write(outputStream, (int)targetValue);

}

/** <summary> Evaluates the Type of a JSON Number and writes the Value to a RTON File according to its Type. </summary>

<param name = "outputStream"> The Stream where the RTON Data will be Written. </param>
<param name = "jsonReader"> The JSON Reader. </param>
<param name = "endian"> The endian Order of the RTON Data. </param> */

public static void EvaluateNumericValue(BinaryStream outputStream, double targetValue, Endian endian = default)
{

if(targetValue.ToString().IndexOf('.') > -1)
CheckFloatPoint(outputStream, targetValue, endian);

else if(targetValue <= ulong.MaxValue)
CheckUnsignedInteger(outputStream, targetValue, endian);

else
CheckSignedInteger(outputStream, targetValue, endian);

}

}

}
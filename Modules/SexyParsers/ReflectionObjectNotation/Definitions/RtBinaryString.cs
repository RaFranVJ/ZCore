using System;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using ZCore.Modules.SexyParsers.ReflectionObjectNotation.Exceptions;

namespace ZCore.Modules.SexyParsers.ReflectionObjectNotation.Definitions
{
/// <summary> Represents a Binary String in the RtSystem. </summary>

public static partial class RtBinaryString
{
/// <summary> The Encoding of a Binary String. </summary>

private static readonly Encoding BinaryStrEncoding = Encoding.Latin1;

/// <summary> The Format a Binary String should Follow. </summary>

private const string BinaryStrFormat = "$BINARY(\"{0}\", {1})";

/** <summary> Checks if the providen String is in the expected Pattern (BINARY). </summary>

<param name = "targetStr"> The String to be Analized. </param>

<returns> The result from the expression Match. </returns> */

public static Match PerformRegex(string targetStr) => BinRegex().Match(targetStr);

/** <summary> Reads a Binary String from a RTON File and Writes its Representation to a JSON File. </summary>

<param name = "inputStream"> The Stream which Contains the RTON Data to be Read. </param>
<param name = "outputStream"> The Stream where the JSON Data will be Written. </param>
<param name = "isPropertyName"> Determines if the BinaryString should be Written as a PropertyName or not. </param> */

public static void Read(BinaryStream inputStream, Utf8JsonWriter outputStream, bool isPropertyName,
bool ignoreLengthMitsmach = false)
{
byte expectedLength = inputStream.ReadByte();
string refString = inputStream.ReadStringByVarIntLength(BinaryStrEncoding);

if( (refString.Length < expectedLength) && !ignoreLengthMitsmach)
throw new RtString_LengthMismatchException(inputStream.Position, RtTypeIdentifier.BinaryString, refString, expectedLength);

int binaryOffset = inputStream.ReadVarInt();
string binaryString = string.Format(BinaryStrFormat, refString, binaryOffset);

RtString.WriteJsonString(outputStream, binaryString, isPropertyName);
}

/** <summary> Reads a Binary String from a JSON File and Writes its Representation to a RTON File. </summary>

<param name = "outputStream"> The Stream where the RTON Data will be Written. </param>
<param name = "sourceRegex"> The Regex of the Binary String. </param> */

public static void Write(BinaryStream outputStream, Match sourceRegex)
{
outputStream.WriteByte( (byte)RtTypeIdentifier.BinaryString);

string refString = sourceRegex.Groups[1].ToString();
byte refStringLength = (byte)(refString.Length / 4);

outputStream.WriteByte(refStringLength);
outputStream.WriteStringByVarIntLength(refString, BinaryStrEncoding);

int binaryOffset = int.Parse(sourceRegex.Groups[2].ToString() );

outputStream.WriteVarInt(binaryOffset);
}

// Binary

[GeneratedRegex(@"\$BINARY\(""(.*?)"", (\d+)\)")]

private static partial Regex BinRegex();
}

}
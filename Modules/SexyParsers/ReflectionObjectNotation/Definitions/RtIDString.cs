using System;
using System.Text.Json;
using System.Text.RegularExpressions;
using ZCore.Modules.SexyParsers.ReflectionObjectNotation.Exceptions;

namespace ZCore.Modules.SexyParsers.ReflectionObjectNotation.Definitions
{
/// <summary> Represents a ID reference in the RtSystem. </summary>

public static partial class RtIDString
{
/** <summary> Checks if the providen String is in the expected Pattern (RTID). </summary>

<param name = "targetStr"> The String to be Analized. </param>

<returns> The result from the expression Match. </returns> */

public static Match PerformRegex(string targetStr) => RtIDRegex().Match(targetStr);

// Get Uid String for the RTID Reference

private static string ReadUidString(BinaryStream targetStream, RtIDType idFlags, bool ignoreLengthMitsmach)
{
int expectedUidLength = targetStream.ReadVarInt();
string refString = targetStream.ReadStringByVarIntLength();

if( (refString.Length < expectedUidLength) && !ignoreLengthMitsmach)
throw new RtIdString_LengthMismatchException(targetStream.Position, idFlags, refString, expectedUidLength);

int uidSubType = targetStream.ReadVarInt();
int uidType = targetStream.ReadVarInt();
uint uidHash = targetStream.ReadUInt();

return string.Format(RtIDFormat.UidReference, uidType, uidSubType, uidHash, refString);
}

// Get Alias String for the RTID Reference

private static string ReadAliasString(BinaryStream targetStream, RtIDType idFlags, bool ignoreLengthMitsmach)
{
int expectedRefLength = targetStream.ReadVarInt();
string refString = targetStream.ReadStringByVarIntLength();

if( (refString.Length < expectedRefLength) && !ignoreLengthMitsmach)
throw new RtIdString_LengthMismatchException(targetStream.Position, idFlags, refString, expectedRefLength);

int expectedAliasLength = targetStream.ReadVarInt();
string aliasString = targetStream.ReadStringByVarIntLength();

if( (aliasString.Length < expectedAliasLength) && !ignoreLengthMitsmach)
throw new RtIdString_LengthMismatchException(targetStream.Position, idFlags, aliasString, expectedAliasLength);

return string.Format(RtIDFormat.AliasReference, aliasString, refString);
}

/** <summary> Reads a RtID String from a RTON File and Writes its Representation to a JSON File. </summary>

<param name = "inputStream"> The Stream which Contains the RTON Data to be Read. </param>
<param name = "outputStream"> The Stream where the JSON Data will be Written. </param>
<param name = "isPropertyName"> Determines if the UnicodeString should be Written as a PropertyName or not. </param>
<param name = "sourceID"> The Identifier of the RTON Value. </param> */

public static void Read(BinaryStream inputStream, Utf8JsonWriter outputStream, bool isPropertyName,
RtTypeIdentifier sourceID, bool ignoreLengthMitsmach = false)
{
string idString;

if(sourceID == RtTypeIdentifier.IDString)
{
RtIDType idFlags = (RtIDType)inputStream.ReadByte();

switch(idFlags)
{
case RtIDType.NullReference:
idString = RtIDFormat.NullReference;
break;

case RtIDType.UidReference:
idString = ReadUidString(inputStream, idFlags, ignoreLengthMitsmach);
break;

case RtIDType.AliasReference:
idString = ReadAliasString(inputStream, idFlags, ignoreLengthMitsmach);
break;

default:
throw new Exception(string.Format("Unknown RtID Type: \"{0}\" at {1:x8}", (byte)idFlags, inputStream.Position) );
}

}

else
idString = RtIDFormat.NullReference;

RtString.WriteJsonString(outputStream, idString, isPropertyName);
}

/** <summary> Reads a RtID String from a JSON File and Writes its Representation to a RTON File. </summary>

<param name = "outputStream"> The Stream where the RTON Data will be Written. </param>
<param name = "sourceRegex"> The Regex of the RtID String. </param>
<param name = "targetStr"> The RtID String to be Written. </param> */

public static void Write(BinaryStream outputStream, Match sourceRegex)
{
outputStream.WriteByte( (byte)RtTypeIdentifier.IDString);

string aliasString = sourceRegex.Groups[1].ToString();
string uidString = sourceRegex.Groups[2].ToString();

if(aliasString == string.Empty && uidString == string.Empty)
{
outputStream.WriteByte( (byte)RtIDType.NullReference);
return;
}

Match uidMatch = UidRegex().Match(aliasString);

if(uidMatch.Success)
{
outputStream.WriteByte( (byte)RtIDType.UidReference);

outputStream.WriteVarInt(uidString.Length);
outputStream.WriteStringByVarIntLength(uidString);

int firstUidDigits = Convert.ToInt32(uidMatch.Groups[1].ToString(), 16);
outputStream.WriteVarInt(firstUidDigits);

int secondUidDigits = Convert.ToInt32(uidMatch.Groups[2].ToString(), 16);
outputStream.WriteVarInt(secondUidDigits);

uint thirdUidDigits = Convert.ToUInt32(uidMatch.Groups[3].ToString(), 16);
outputStream.WriteUInt(thirdUidDigits);
}

else
{
outputStream.WriteByte( (byte)RtIDType.AliasReference);

outputStream.WriteVarInt(uidString.Length);
outputStream.WriteStringByVarIntLength(uidString);

outputStream.WriteVarInt(aliasString.Length);
outputStream.WriteStringByVarIntLength(aliasString);
}

}

// rtid

[GeneratedRegex("^RTID\\((.*)@(.*)\\)$")]

private static partial Regex RtIDRegex();

// uid

[GeneratedRegex("^([0-9|a-f]+)\\.([0-9|a-f]+)\\.([0-9|a-f]{8})$")]

private static partial Regex UidRegex();
}

/// <summary> Defines the expected Reference Type in a RtIDString. </summary>

public enum RtIDType : byte
{
/// <summary> The Reference pointed is <b>null</b>. </summary>
NullReference = 0x00,

/// <summary> The Reference pointed is a <b>UID</b>. </summary>
UidReference = 0x02,

/// <summary> The Reference pointed is an <b>Alias</b>. </summary>
AliasReference = 0x03
}

/// <summary> The formats used for Parsing RtIDStrings. </summary>

public readonly struct RtIDFormat
{
/// <summary> Used when the Reference pointed is <b>null</b>. </summary>

public const string NullReference = "RTID()";

/// <summary> Used when the Reference pointed is a <b>UID</b>. </summary>

public const string UidReference = "RTID({0:d}.{1:d}.{2:X8}@{3})";

/// <summary> Used when the Reference pointed is an <b>Alias</b>. </summary>

public const string AliasReference = "RTID({0}@{1})";
}

}
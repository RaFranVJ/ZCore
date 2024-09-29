using System.Text.Json;

namespace ZCore.Modules.SexyParsers.ReflectionObjectNotation.Definitions
{
/// <summary> Represents a Boolean in the RtSystem. </summary>

public static class RtBoolean
{
/** <summary> Reads a Boolean from a RTON File and Writes its Representation to a JSON File. </summary>

<param name = "outputStream"> The Stream where the JSON Data will be Written. </param>
<param name = "sourceID"> The Identifier of the RTON Value. </param> */

public static void Read(Utf8JsonWriter outputStream, RtTypeIdentifier sourceID)
{
bool value = sourceID.Equals(RtTypeIdentifier.Bool_true);

outputStream.WriteBooleanValue(value);
}

/** <summary> Reads a Boolean from a JSON File and writes its Representation to a RTON File. </summary>

<param name = "outputStream"> The Stream where the RTON Data will be Written. </param>
<param name = "sourceType"> The Type of the JSON Value. </param> */

public static void Write(BinaryStream outputStream, bool targetValue)
{
var identifier = targetValue ? RtTypeIdentifier.Bool_true : RtTypeIdentifier.Bool_false;

outputStream.WriteByte( (byte)identifier);
}

}

}
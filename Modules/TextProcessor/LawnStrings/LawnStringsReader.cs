using LawnStrs = ZCore.Modules.TextProcessor.LawnStrings.Definitions.LawnStrings;

using System.IO;
using System.Text;
using ZCore.Modules.TextProcessor.LawnStrings.Definitions.Map;

namespace ZCore.Modules.TextProcessor.LawnStrings
{
/// <summary> Allows Reading the Content of a LawnStrings File. </summary>

public static class LawnStringsReader
{
/** <summary> Reads a LawnStrings File as PlainText. </summary>

<remarks> This method may be Slow with Large Files, be careful </remarks>

<returns> The LawnStrings Text </returns> */

public static string ReadText(Stream sourceStream, Encoding encoding = null)
{
encoding ??= Encoding.UTF8;

using StreamReader textReader = new(sourceStream, encoding);

return textReader.ReadToEnd();
}

/** <summary> Reads a LawnStrings File as PlainText. </summary>

<returns> The LawnStrings Text </returns> */

public static string ReadText(string filePath, Encoding encoding = null)
{
using FileStream inputFile = File.OpenRead(filePath);

return ReadText(inputFile, encoding);
}

/**<summary> Reads a LawnStrings JSON content from a stream. </summary>

<param name = "sourceStream"> The stream containing the LawnStrings JSON data. </param>

<returns>The LawnStrings content deserialized from the JSON.</returns> */

public static LawnStrs ReadJson(Stream sourceStream)
{
return JsonSerializer.DeserializeObject<LawnStrs>(ReadText(sourceStream) ) ?? new();
}

/** <summary> Reads a LawnStrings File as Json. </summary>

<returns> The LawnStrings Content </returns> */

public static LawnStrs ReadJson(string filePath) => JsonSerializer.DeserializeObject<LawnStrs>(ReadText(filePath) ) ?? new();

/** <summary> Reads a LawnStrings File as a JsonMap. </summary>

<returns> The LawnStrings Content </returns> */

public static LawnStringsMap ReadJsonMap(Stream sourceStream) 
{
return JsonSerializer.DeserializeObject<LawnStringsMap>(ReadText(sourceStream) ) ?? new();
}

/** <summary> Reads a LawnStrings File as a JsonMap. </summary>

<returns> The LawnStrings Content </returns> */

public static LawnStringsMap ReadJsonMap(string filePath)
{
return JsonSerializer.DeserializeObject<LawnStringsMap>(ReadText(filePath) ) ?? new();
}

}

}
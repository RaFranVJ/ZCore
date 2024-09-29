using LawnStrs = ZCore.Modules.TextProcessor.LawnStrings.Definitions.LawnStrings;

using System.IO;
using System.Text;
using ZCore.Modules.TextProcessor.LawnStrings.Definitions.Map;

namespace ZCore.Modules.TextProcessor.LawnStrings
{
/// <summary> Allows Writting Content to a LawnStrings File. </summary>

public static class LawnStringsWriter
{
/** <summary> Writes PlainText to a LawnStrings File </summary> */

public static void WriteText(Stream sourceStream, string text, Encoding encoding = null)
{
encoding ??= Encoding.UTF8;

using StreamWriter textWriter = new(sourceStream, encoding);

textWriter.Write(text);
}

/** <summary> Writes PlainText to a LawnStrings File </summary> */

public static void WriteText(string filePath, string text, Encoding encoding = null)
{
using FileStream inputFile = File.OpenWrite(filePath);

WriteText(inputFile, text, encoding);
}

/** <summary> Writes a LawnStrings File as Json. </summary> */

public static void WriteJson(Stream sourceStream, LawnStrs lawnStrs) 
{
WriteText(sourceStream, JsonSerializer.SerializeObject(lawnStrs) );
}

/** <summary> Writes a LawnStrings File as Json. </summary> */

public static void WriteJson(string filePath, LawnStrs lawnStrs)
{
WriteText(filePath, JsonSerializer.SerializeObject(lawnStrs) );
}

/** <summary> Writes a LawnStrings File as a JsonMap. </summary> */

public static void WriteJsonMap(Stream sourceStream, LawnStringsMap lawnStrs)
{
WriteText(sourceStream, JsonSerializer.SerializeObject(lawnStrs) );
}

/** <summary> Writes a LawnStrings File as a JsonMap. </summary> */

public static void WriteJsonMap(string filePath, LawnStringsMap lawnStrs)
{
WriteText(filePath, JsonSerializer.SerializeObject(lawnStrs) );
}

}

}
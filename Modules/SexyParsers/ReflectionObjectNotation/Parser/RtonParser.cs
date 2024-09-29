using System;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using ZCore.Modules.SexyParsers.ReflectionObjectNotation.Definitions;
using ZCore.Modules.SexyParsers.ReflectionObjectNotation.Exceptions;
using ZCore.Serializables.ArgumentsInfo.FileManager.Archive;
using ZCore.Serializables.ArgumentsInfo.SexyParser.Rton;

namespace ZCore.Modules.SexyParsers.ReflectionObjectNotation.Parser
{
/// <summary> Initializes Parsing Tasks for RTON Files. </summary>

public static class RtonParser
{
/** <summary> The Header of a RTON File. </summary>

<remarks> It Occupies 4 Bytes in the Stream, which are: <c>0x52 0x54 0x4F 0x4E</c> </remarks> */

private const string RtonHeader = "RTON";

/** <summary> The Tail of a RTON File. </summary>

<remarks> It Occupies 4 Bytes in the Stream, which are: <c>0x44 0x4F 0x4E 0x45</c> </remarks> */

private const string RtonTail = "DONE";

/** <summary> Gets a new Instance of the <c>JavaScriptEncoder</c> based on the given TypeName. </summary>

<param name = "encoderTypeName"> The TypeName of the JSON Encoder. </param>

<returns> The JSON Encoder. </returns> */

private static JavaScriptEncoder GetJsonEncoder(string encoderTypeName)
{

return encoderTypeName switch
{
"UnsafeRelaxedJsonEscaping" => JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
_ => JavaScriptEncoder.Default
};

}

// Get RTON Stream

public static BinaryStream EncodeStream(Stream input, Endian endian = default,
FileVersionDetails<uint> versionInfo = default, RtonEncoderInfo encoderInfo = null, 
string outputPath = default, string pathToRefStrings = default)
{
versionInfo ??= new(RtonVersion.ExpectedVerNumber);
encoderInfo ??= new();

PathHelper.ChangeExtension(ref outputPath, ".rton");
BinaryStream rtonStream = string.IsNullOrEmpty(outputPath) ? new() : BinaryStream.OpenWrite(outputPath);

rtonStream.WriteString(RtonHeader, default, endian);
RtonVersion.Write(rtonStream, endian, versionInfo.VersionNumber, versionInfo.AdaptCompatibilityBetweenVersions);

JsonDocumentOptions jsonOptions = new()
{
AllowTrailingCommas = encoderInfo.IgnoreExtraCommas,
CommentHandling = encoderInfo.CommentsHandling,
MaxDepth = encoderInfo.MaxDataDepth
};

using JsonDocument jsonDoc = JsonDocument.Parse(input, jsonOptions);
input.Dispose(); // Release memory

RtObject.Write(rtonStream, jsonDoc.RootElement, encoderInfo.EncodeStringsWithUtf8, endian);
rtonStream.WriteString(RtonTail, default, endian);

if(!string.IsNullOrEmpty(pathToRefStrings) )
ReferenceStringsHandler.WriteStrings(pathToRefStrings);

ReferenceStringsHandler.ClearStrings();

return rtonStream;
}

/** <summary> Encodes a JSON File as a RTON File. </summary>

<param name = "inputPath"> The Path where the JSON File to be Encoded is Located. </param>
<param name = "outputPath"> The Location where the Encoded RTON File will be Saved. </param>

<exception cref = "ArgumentNullException"></exception>
<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception>
<exception cref = "NotSupportedException"></exception>
<exception cref = "UnauthorizedAccessException"></exception> */

public static void EncodeFile(string inputPath, string outputPath, bool useRefStrings,
Endian endian = default, FileVersionDetails<uint> versionInfo = null, RtonEncoderInfo encoderInfo = null)
{
using FileStream inputFile = File.OpenRead(inputPath);

string pathToRefStrings = useRefStrings ? ReferenceStringsHandler.GetRefFilePath(inputPath) : null;

using BinaryStream outputFile = EncodeStream(inputFile, endian, versionInfo, encoderInfo, outputPath, pathToRefStrings);
}

// Get JSON Stream

public static Stream DecodeStream(BinaryStream input, Endian endian = default, bool adaptVer = true,
RtonDecoderInfo decoderInfo = null, string outputPath = null, string pathToRefStrings = null)
{
decoderInfo ??= new();

input.CompareString<InvalidRtonException>(RtonHeader, default, endian);
RtonVersion.Read(input, endian, adaptVer);

PathHelper.ChangeExtension(ref outputPath, ".json");
Stream output = string.IsNullOrEmpty(outputPath) ? new MemoryStream() : File.OpenWrite(outputPath);

JsonWriterOptions writerConfig = new()
{
Encoder = GetJsonEncoder(decoderInfo.JsonEncoder),
Indented = decoderInfo.FormattingIndented,
MaxDepth = decoderInfo.MaxDataDepth,
SkipValidation = decoderInfo.SkipJsonValidation
};

using Utf8JsonWriter jsonWriter = new(output, writerConfig);

if(!string.IsNullOrEmpty(pathToRefStrings) )
ReferenceStringsHandler.ReadStrings(pathToRefStrings);

RtObject.Read(input, jsonWriter, endian, decoderInfo.IgnoreStrLengthMismatch);
input.CompareString<InvalidRtonTailException>(RtonTail, default, endian);

ReferenceStringsHandler.ClearStrings();

return output;
}

/** <summary> Decodes a RTON File as a JSON File. </summary>

<param name = "inputPath"> The Path where the RTON File to be Decoded is Located. </param>
<param name = "outputPath"> The Location where the Decoded JSON File will be Saved. </param>

<exception cref = "ArgumentNullException"></exception>
<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception>
<exception cref = "NotSupportedException"></exception>
<exception cref = "UnauthorizedAccessException"></exception> */

public static void DecodeFile(string inputPath, string outputPath, bool useRefStrings,
Endian endian = default, bool adaptVer = true, RtonDecoderInfo decoderInfo = null)
{
using BinaryStream inputFile = BinaryStream.Open(inputPath);

string pathToRefStrings = useRefStrings ? ReferenceStringsHandler.GetRefFilePath(inputPath) : null;

using Stream outputFile = DecodeStream(inputFile, endian, adaptVer, decoderInfo, outputPath, pathToRefStrings);
}

}

}
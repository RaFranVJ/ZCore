using System;
using ZCore.Modules.SexyParsers.CharacterFontWidget2.Definitions;
using ZCore.Serializables.ArgumentsInfo.FileManager.Archive;

namespace ZCore.Modules.SexyParsers.CharacterFontWidget2.Parser
{
/// <summary> Initializes Parsing Tasks for CFW2 Files. </summary>

public static class Cfw2Parser
{
// Get CFW2 Stream

public static BinaryStream EncodeStream(FontWidget widget, Endian endian = default,
FileVersionDetails<Int128> versionInfo = default, string outputPath = null)
{
versionInfo ??= new(Cfw2Version.ExpectedVerNumber);

PathHelper.ChangeExtension(ref outputPath, ".cfw2");
BinaryStream cfw2Stream = string.IsNullOrEmpty(outputPath) ? new() : BinaryStream.OpenWrite(outputPath);

widget.WriteBin(cfw2Stream, versionInfo, endian);		

return cfw2Stream;
}

/** <summary> Encodes a FontWidget as a CFW2 File. </summary>

<param name = "inputPath"> The Path to the FontWidget (must be a JSON File). </param>
<param name = "outputPath"> The Location where the Encoded File will be Saved. </param>

<exception cref = "ArgumentNullException"></exception>
<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception>
<exception cref = "NotSupportedException"></exception>
<exception cref = "UnauthorizedAccessException"></exception> */

public static void EncodeFile(string inputPath, string outputPath, Endian endian = default,
FileVersionDetails<Int128> versionInfo = default)
{
FontWidget widget = new FontWidget().ReadObject(inputPath);

using BinaryStream outputFile = EncodeStream(widget, endian, versionInfo, outputPath);
}
	
// Get FontWidget from BinaryStream

public static FontWidget DecodeStream(BinaryStream input, Endian endian = default, bool adaptVer = true) 
{
return FontWidget.ReadBin(input, endian, adaptVer);
}

/** <summary> Decodes a CFW2 File as a FontWidget Instance. </summary>

<param name = "inputPath"> The Path to the CFW2 File. </param>
<param name = "outputPath"> The Location where the Decoded File will be Saved. </param>

<exception cref = "ArgumentNullException"></exception>
<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception>
<exception cref = "NotSupportedException"></exception>
<exception cref = "UnauthorizedAccessException"></exception> */

public static void DecodeFile(string inputPath, string outputPath, Endian endian = default, bool adaptVer = true)
{
PathHelper.ChangeExtension(ref outputPath, ".json");
using BinaryStream inputFile = BinaryStream.Open(outputPath);

FontWidget widget = DecodeStream(inputFile, endian, adaptVer);
widget.WriteObject(outputPath);
}    

}

}
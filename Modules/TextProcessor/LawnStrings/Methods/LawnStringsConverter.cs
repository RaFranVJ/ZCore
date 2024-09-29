using LawnStrs = ZCore.Modules.TextProcessor.LawnStrings.Definitions.LawnStrings;

using System.IO;
using System.Text;
using ZCore.Modules.TextProcessor.LawnStrings.Definitions.Map;
using ZCore.Modules.SexyParsers.ReflectionObjectNotation.Parser;
using ZCore.Serializables.ArgumentsInfo.TextProcessor.LawnStrings.Converter;

namespace ZCore.Modules.TextProcessor.LawnStrings.Methods
{
/// <summary> Allows the Conversion of Different LawnStrings Files between PlainText,
/// Json (List and Map Style) or Rton. </summary>

public static class LawnStringsConverter
{
/** <summary> Converts plain text to a JSON list. </summary> 

<param name="sourceStream">Input stream containing plain text.</param> 
<param name="encoding">Encoding used for the input text.</param> 

<returns> The resulting <c>LawnStrings</c> object. </returns> */

public static LawnStrs ConvertTextToJson(Stream sourceStream, bool ignoreDuplicatedKeys, Encoding encoding)
{
return new(sourceStream, ignoreDuplicatedKeys, encoding);
}

/** <summary> Converts plain text to a JSON map. </summary> 

<param name = "sourceStream"> Input stream containing plain text .</param> 
<param name = "encoding"> Encoding used for the input text. </param> 

<returns> The resulting <c>LawnStrings_Map</c> object. </returns> */

public static LawnStringsMap ConvertTextToJsonMap(Stream sourceStream, bool ignoreDuplicatedKeys, Encoding encoding) 
{
return new(sourceStream, ignoreDuplicatedKeys, encoding);
}

/** <summary> Converts plain text to an RTON list. </summary>

<param name="sourceStream">Input stream containing plain text.</param> 
<param name="encoding">Encoding used for the input text.</param> 
<param name="outputPath">Optional output path for the resulting RTON data.</param> 

<returns>A BinaryStream containing the RTON encoded data.</returns> */

public static BinaryStream ConvertTextToRton(Stream sourceStream, bool ignoreDuplicatedKeys, Encoding encoding, 
string outputPath = null)
{
using MemoryStream jsonData = new();

LawnStringsWriter.WriteJson(jsonData, ConvertTextToJson(sourceStream, ignoreDuplicatedKeys, encoding) );
jsonData.Position = 0;

return RtonParser.EncodeStream(jsonData, outputPath: outputPath);
}

/** <summary> Converts plain text to an RTON map. </summary> 

<param name="sourceStream">Input stream containing plain text.</param> 
<param name="outputPath">Optional output path for the resulting RTON data.</param>

<returns>A BinaryStream containing the RTON encoded data.</returns> */

public static BinaryStream ConvertTextToRtonMap(Stream sourceStream, bool ignoreDuplicatedKeys, Encoding encoding,
string outputPath = null)
{
using MemoryStream jsonData = new();

LawnStringsWriter.WriteJsonMap(jsonData, ConvertTextToJsonMap(sourceStream, ignoreDuplicatedKeys, encoding) );
jsonData.Position = 0;

return RtonParser.EncodeStream(jsonData, outputPath: outputPath);
}

// PlainText Conversion

private static void PlainTextConversion(string inputPath, string outputPath, bool ignoreDuplicatedKeys,
LawnStringsFormat destFormat, Encoding encoding)
{
using FileStream inputFile = File.OpenRead(inputPath);

switch(destFormat)
{
case LawnStringsFormat.JsonMap:
LawnStringsWriter.WriteJsonMap(outputPath, ConvertTextToJsonMap(inputFile, ignoreDuplicatedKeys, encoding) );
break;

case LawnStringsFormat.RtonList:

using(BinaryStream rtonList = ConvertTextToRton(inputFile, ignoreDuplicatedKeys, encoding, outputPath) )
{
};

break;

case LawnStringsFormat.RtonMap:

using(BinaryStream rtonMap = ConvertTextToRtonMap(inputFile, ignoreDuplicatedKeys, encoding, outputPath) )
{
};

break;

default:
LawnStringsWriter.WriteJson(outputPath, ConvertTextToJson(inputFile, ignoreDuplicatedKeys, encoding) );
break;
}

}

/** <summary> Converts a JSON list to a JSON map. </summary>

<param name="sourceStream">Input stream containing the JSON data.</param> 

<returns> The resulting <c>LawnStrings_Map</c> object. </returns> */

public static LawnStringsMap ConvertJsonListToJsonMap(Stream sourceStream, bool ignoreDuplicatedKeys)
{
return LawnStringsReader.ReadJson(sourceStream).ToMap(ignoreDuplicatedKeys);
}

/** <summary> Converts a JSON list to an RTON map. </summary> 

<param name="sourceStream">Input stream containing the JSON data.</param> 
<param name="outputPath">Optional output path for the resulting RTON data.</param> 

<returns>A BinaryStream containing the RTON encoded data.</returns> */

public static BinaryStream ConvertJsonToRtonMap(Stream sourceStream, bool ignoreDuplicatedKeys, string outputPath = null)
{
using MemoryStream jsonData = new();

LawnStringsWriter.WriteJsonMap(jsonData, ConvertJsonListToJsonMap(sourceStream, ignoreDuplicatedKeys) );
jsonData.Position = 0;

return RtonParser.EncodeStream(jsonData, outputPath: outputPath);
}

// JsonList Conversion

private static void JsonConversion(string inputPath, string outputPath, bool ignoreDuplicatedKeys,
LawnStringsFormat destFormat, Encoding encoding)
{
using FileStream inputFile = File.OpenRead(inputPath);

switch(destFormat)
{
case LawnStringsFormat.JsonMap:
LawnStringsWriter.WriteJsonMap(outputPath, ConvertJsonListToJsonMap(inputFile, ignoreDuplicatedKeys) );
break;

case LawnStringsFormat.RtonList:

using(BinaryStream rtonList = RtonParser.EncodeStream(inputFile, outputPath: outputPath) )
{
};

break;

case LawnStringsFormat.RtonMap:

using(BinaryStream rtonMap = ConvertJsonToRtonMap(inputFile, ignoreDuplicatedKeys, outputPath) )
{
};

break;

default:
LawnStringsReader.ReadJson(inputFile).WriteAsPlainText(outputPath, ignoreDuplicatedKeys, encoding);
break;
}

}

/** <summary>  Converts a JSON map to a JSON list. </summary> 

<param name="sourceStream">Input stream containing the JSON map.</param> 

<returns>The resulting <c>LawnStrings</c> object.</returns> */

public static LawnStrs ConvertJsonMapToJsonList(Stream sourceStream)
{
return LawnStringsReader.ReadJsonMap(sourceStream).ToList();
}

/** <summary> Converts a JSON map to an RTON list. </summary> 

<param name="sourceStream"> Input stream containing the JSON map. </param> 
<param name="outputPath"> Optional output path for the resulting RTON data. </param> 

<returns>A BinaryStream containing the RTON encoded data.</returns> */

public static BinaryStream ConvertJsonMapToRton(Stream sourceStream, string outputPath = null)
{
using MemoryStream jsonData = new();

LawnStringsWriter.WriteJson(jsonData, ConvertJsonMapToJsonList(sourceStream) );
jsonData.Position = 0;
	
return RtonParser.EncodeStream(jsonData, outputPath: outputPath);
}

// JsonMap Conversion

private static void JsonMapConversion(string inputPath, string outputPath, LawnStringsFormat destFormat, Encoding encoding)
{
using FileStream inputFile = File.OpenRead(inputPath);

switch(destFormat)
{
case LawnStringsFormat.JsonList:
LawnStringsWriter.WriteJson(outputPath, ConvertJsonMapToJsonList(inputFile) );
break;

case LawnStringsFormat.RtonList:

using(BinaryStream rtonList = ConvertJsonMapToRton(inputFile, outputPath) )
{
};

break;

case LawnStringsFormat.RtonMap:

using(BinaryStream rtonMap = RtonParser.EncodeStream(inputFile, outputPath: outputPath) )
{
};

break;

default:
LawnStringsReader.ReadJsonMap(inputFile).WriteAsPlainText(outputPath, encoding);
break;
}

}

/** <summary> Converts an RTON list to a JSON map. </summary> 

<param name="sourceStream">Input BinaryStream containing the RTON data.</param> 

<returns>The resulting <c>LawnStrings_Map</c> object.</returns> */

public static LawnStringsMap ConvertRtonToJsonMap(BinaryStream sourceStream, bool ignoreDuplicatedKeys)
{
using Stream jsonData = RtonParser.DecodeStream(sourceStream);

jsonData.Position = 0;

return ConvertJsonListToJsonMap(jsonData, ignoreDuplicatedKeys);
}

/** <summary> Converts an RTON list to an RTON map. </summary>

<param name="sourceStream"> Input BinaryStream containing the RTON data. </param>
 
<returns>A BinaryStream containing the RTON encoded map.</returns> */

public static BinaryStream ConvertRtonListToRtonMap(BinaryStream sourceStream, bool ignoreDuplicatedKeys,
string outputPath = null)
{
using Stream jsonData = RtonParser.DecodeStream(sourceStream);

jsonData.Position = 0;

return ConvertJsonToRtonMap(jsonData, ignoreDuplicatedKeys, outputPath);
}

// RtonList Conversion

private static void RtonConversion(string inputPath, string outputPath, bool ignoreDuplicatedKeys,
LawnStringsFormat destFormat, Encoding encoding)
{
using BinaryStream inputFile = BinaryStream.Open(inputPath);

switch(destFormat)
{
case LawnStringsFormat.JsonList:

using(Stream jsonList = RtonParser.DecodeStream(inputFile, outputPath: outputPath) )
{
};

break;

case LawnStringsFormat.JsonMap:
LawnStringsWriter.WriteJsonMap(outputPath, ConvertRtonToJsonMap(inputFile, ignoreDuplicatedKeys) );
break;

case LawnStringsFormat.RtonMap:

using(BinaryStream rtonMap = ConvertRtonListToRtonMap(inputFile, ignoreDuplicatedKeys, outputPath) )
{
};

break;

default:

using(Stream jsonData = RtonParser.DecodeStream(inputFile) )
{
jsonData.Position = 0;

LawnStringsReader.ReadJson(jsonData).WriteAsPlainText(outputPath, ignoreDuplicatedKeys, encoding);
}

break;
}

}

/** <summary> Converts an RTON map to a JSON list. </summary> 

<param name="sourceStream"> Input BinaryStream containing the RTON map data. </param> 

<returns> The resulting <c>LawnStrings</c> object. </returns> */

public static LawnStrs ConvertRtonMapToJson(BinaryStream sourceStream)
{
using Stream jsonData = RtonParser.DecodeStream(sourceStream);

jsonData.Position = 0;

return ConvertJsonMapToJsonList(jsonData);
}

/** <summary> Converts an RTON map to an RTON list. </summary> 

<param name="sourceStream"> Input BinaryStream containing the RTON map data. </param> 

<returns>A BinaryStream containing the RTON encoded list.</returns> */

public static BinaryStream ConvertRtonMapToRtonList(BinaryStream sourceStream, string outputPath = null)
{
using Stream jsonData = RtonParser.DecodeStream(sourceStream);

jsonData.Position = 0;

return ConvertJsonMapToRton(jsonData, outputPath);
}

// RtonMap Conversion

private static void RtonMapConversion(string inputPath, string outputPath, LawnStringsFormat destFormat, Encoding encoding)
{
using BinaryStream inputFile = BinaryStream.Open(inputPath);

switch(destFormat)
{
case LawnStringsFormat.JsonList:
LawnStringsWriter.WriteJson(outputPath, ConvertRtonMapToJson(inputFile) );
break;

case LawnStringsFormat.JsonMap:

using(Stream jsonMap = RtonParser.DecodeStream(inputFile, outputPath: outputPath) )
{
};

break;

case LawnStringsFormat.RtonList:

using(BinaryStream rtonList = ConvertRtonMapToRtonList(inputFile, outputPath) )
{
};

break;

default:

using(Stream jsonData = RtonParser.DecodeStream(inputFile) )
{
jsonData.Position = 0;

LawnStringsReader.ReadJsonMap(jsonData).WriteAsPlainText(outputPath, encoding);
}

break;
}

}

// Convert LawnStrings File

public static void ConvertFile(string inputPath, LawnStringsConvertInfo config = null)
{

if(config.InputFormat == config.OutputFormat)
return;

config ??= new();

string outputPath = LawnStringsHelper.BuildPath(inputPath, config.OutputFormat, "Converted");
var encoding = EncodeHelper.GetEncodingType(config.EncodingForPlainText);

switch(config.InputFormat)
{
case LawnStringsFormat.JsonList:
JsonConversion(inputPath, outputPath, config.IgnoreDuplicatedKeys, config.OutputFormat, encoding);
break;

case LawnStringsFormat.JsonMap:
JsonMapConversion(inputPath, outputPath, config.OutputFormat, encoding);
break;

case LawnStringsFormat.RtonList:
RtonConversion(inputPath, outputPath, config.IgnoreDuplicatedKeys, config.OutputFormat, encoding);
break;

case LawnStringsFormat.RtonMap:
RtonMapConversion(inputPath, outputPath, config.OutputFormat, encoding);
break;

default:
PlainTextConversion(inputPath, outputPath, config.IgnoreDuplicatedKeys, config.OutputFormat, encoding);
break;
}

}

}

}
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using ZCore.Modules.SexyParsers.ReflectionObjectNotation.Parser;
using ZCore.Modules.TextProcessor.LawnStrings.Definitions.Map;
using ZCore.Modules.TextProcessor.LawnStrings.Methods;
using ZCore.Serializables.ArgumentsInfo.TextProcessor.LawnStrings.Converter;

namespace ZCore.Modules.TextProcessor.LawnStrings
{
/// <summary> Performs some Helpful Tasks used on Handling LawnStrings. </summary>

public static partial class LawnStringsHelper
{
// FileName
	
public const string BaseFileName = "LawnStrings";

/** <summary> Determines if a Line is a Section header based on the presence of Brackets: [ and ]. </summary>

// <param name = "line"> The line to check. </param>

<returns> True if the line is a section header; otherwise, false. </returns> */
 
private static bool IsSectionHeader(string line) => SectionRegex().IsMatch(line);

// Read Lines from a TextReader and add them to a List

public static List<string> GetListFromPlainText(Stream sourceStream, bool ignoreDuplicatedKeys, Encoding encoding = null)
{
encoding ??= Encoding.UTF8;

List<string> entries = new();
using StreamReader textReader = new(sourceStream, encoding);

string line;
StringBuilder currentBlock = new();

while( (line = textReader.ReadLine()) != null)
{
line = line.Trim();

if(string.IsNullOrEmpty(line) )
continue;

if(IsSectionHeader(line) )
{

if(currentBlock.Length > 0)
{
entries.Add(currentBlock.ToString().TrimEnd() );

currentBlock.Clear();
}

line = line.Substring(1, line.Length - 2);

if(entries.Contains(line) && ignoreDuplicatedKeys)
{
string nextLine = textReader.ReadLine()?.Trim();

if(string.IsNullOrEmpty(nextLine) || !IsSectionHeader(nextLine))
continue;

line = nextLine;
}

entries.Add(line);
}
		
else
{

if(currentBlock.Length > 0)
currentBlock.Append("\\n");
            
currentBlock.Append(line);
}

}

if(currentBlock.Length > 0)
entries.Add(currentBlock.ToString().TrimEnd() ); 

return entries;
}

// Read Lines from a Stream and add them to a Dictionary

public static Dictionary<string, string> GetMapFromPlainText(Stream sourceStream, bool ignoreDuplicatedKeys,
Encoding encoding = null)
{
encoding ??= Encoding.UTF8;

Dictionary<string, string> entries = new();
using StreamReader textReader = new(sourceStream, encoding);

string line;
string currentKey = null;

StringBuilder currentValue = new();

while( (line = textReader.ReadLine() ) != null)
{
line = line.Trim();

if(string.IsNullOrEmpty(line) )
continue;

if(IsSectionHeader(line) )
{

if(!string.IsNullOrEmpty(currentKey) && currentValue.Length > 0)
{
entries[currentKey] = currentValue.ToString();

currentValue.Clear();
}

currentKey = line.Substring(1, line.Length - 2);

if(entries.ContainsKey(currentKey) && !ignoreDuplicatedKeys)
{
int duplicateCount = 1;
string newKey;

do
{
newKey = $"{currentKey}_{duplicateCount}";
duplicateCount++;
}
		
while(entries.ContainsKey(newKey) );

currentKey = newKey;
}

if(entries.ContainsKey(currentKey) && ignoreDuplicatedKeys)
{
string nextLine = textReader.ReadLine()?.Trim();

if(string.IsNullOrEmpty(nextLine) || !IsSectionHeader(nextLine) )
continue;

currentKey = nextLine;
}

}

else
{

if(currentValue.Length > 0)
currentValue.Append("\\n");

currentValue.Append(line);
}

}

if(!string.IsNullOrEmpty(currentKey) && currentValue.Length > 0)  
entries[currentKey] = currentValue.ToString();

return entries;
}


// Get LawnStrings Ext

public static string GetExtension(LawnStringsFormat sourceFormat)
{

return sourceFormat switch
{
LawnStringsFormat.JsonList or LawnStringsFormat.JsonMap => ".json",
LawnStringsFormat.RtonList or LawnStringsFormat.RtonMap => ".rton",
_ => ".txt",
};

}

// Get new Path for LawnStrings

public static string BuildPath(LawnStringsFormat sourceFormat)
{
string fileExt = GetExtension(sourceFormat);

string inputPath = LibInfo.CurrentDllDirectory + Path.DirectorySeparatorChar + BaseFileName + fileExt;
PathHelper.CheckDuplicatedPath(ref inputPath);

return inputPath;
}

// Get new Path for LawnStrings

public static string BuildPath(CultureInfo culture, LawnStringsFormat sourceFormat)
{
string fileExt = GetExtension(sourceFormat);
string fileName = BaseFileName + $"-{culture.Name}";

string inputPath = LibInfo.CurrentDllDirectory + Path.DirectorySeparatorChar + fileName + fileExt;
PathHelper.CheckDuplicatedPath(ref inputPath);

return inputPath;
}

// Get new Path for LawnStrings

public static string BuildPath(string sourcePath, LawnStringsFormat targetFormat, string suffix = "Output")
{
string baseDir = Path.GetDirectoryName(sourcePath);
string fileName = Path.GetFileNameWithoutExtension(sourcePath);

string fileExt = GetExtension(targetFormat);
string outputPath = baseDir + Path.DirectorySeparatorChar + fileName + $"_{suffix}" + fileExt;

PathHelper.CheckDuplicatedPath(ref outputPath);

return outputPath;
}

// Get LawnStringsMap from Input or a Conversion

public static LawnStringsMap GetMap(string inputPath, bool ignoreDuplicatedKeys, Encoding encoding,
LawnStringsFormat sourceFormat)
{
using BinaryStream inputFile = BinaryStream.Open(inputPath);

LawnStringsMap result;

switch(sourceFormat)
{
case LawnStringsFormat.JsonList:
result = LawnStringsConverter.ConvertJsonListToJsonMap(inputFile, ignoreDuplicatedKeys);
break;

case LawnStringsFormat.JsonMap:
result = LawnStringsReader.ReadJsonMap(inputFile);
break;

case LawnStringsFormat.RtonList:
result = LawnStringsConverter.ConvertRtonToJsonMap(inputFile, ignoreDuplicatedKeys);
break;

case LawnStringsFormat.RtonMap:

using(Stream jsonData = RtonParser.DecodeStream(inputFile) )
{
jsonData.Position = 0;

result = LawnStringsReader.ReadJsonMap(jsonData); 
}

break;

default:
result = LawnStringsConverter.ConvertTextToJsonMap(inputFile, ignoreDuplicatedKeys, encoding);
break;
}

return result;
}

// Save LawnStringsMap to Output

public static void SaveMap(string outputPath, LawnStringsMap targetStrs, Encoding encoding, LawnStringsFormat targetFormat)
{

switch(targetFormat)
{
case LawnStringsFormat.JsonList:
LawnStringsWriter.WriteJson(outputPath, targetStrs.ToList() );
break;

case LawnStringsFormat.JsonMap:
LawnStringsWriter.WriteJsonMap(outputPath, targetStrs);
break;

case LawnStringsFormat.RtonList:

using(MemoryStream jsonList = new() )
{
LawnStringsWriter.WriteJson(jsonList, targetStrs.ToList() );
jsonList.Position = 0;

using BinaryStream rtonList = RtonParser.EncodeStream(jsonList, outputPath: outputPath);
};

break;

case LawnStringsFormat.RtonMap:

using(MemoryStream jsonMap = new() )
{
LawnStringsWriter.WriteJsonMap(jsonMap, targetStrs);
jsonMap.Position = 0;

using BinaryStream rtonMap = RtonParser.EncodeStream(jsonMap, outputPath: outputPath);
};

break;

default:
targetStrs.WriteAsPlainText(outputPath, encoding);
break;
}

}

[GeneratedRegex(@"^\[.*\]\s*$") ]
private static partial Regex SectionRegex();
}

}
using System.IO;
using System.Linq;
using ZCore.Modules.TextProcessor.LawnStrings.Definitions.Map;
using ZCore.Serializables.ArgumentsInfo.TextProcessor.LawnStrings;
using ZCore.Serializables.ArgumentsInfo.TextProcessor.LawnStrings.Converter;

namespace ZCore.Modules.TextProcessor.LawnStrings.Methods
{
/// <summary> Allows Spliting a LawnStrings File into smaller ones for Merging then later. </summary>

public static class LawnStringsSplit
{
// Split LawnStrings into Smaller Files by Separating Strings by InitialChar

public static void SplitFile(string inputPath, LawnStringsConvertInfo config = null)
{
var encoding = EncodeHelper.GetEncodingType(config.EncodingForPlainText);
LawnStringsMap lawnStrs = LawnStringsHelper.GetMap(inputPath, true, encoding, config.InputFormat);

lawnStrs.CheckObjs();

if(lawnStrs.Objects[0].ObjData.LocStringValues.Count == 0)
return;

config ??= new();

string outputDir = inputPath;
PathHelper.AddExtension(ref outputDir, ".split");

DirManager.CheckMissingFolder(outputDir);

var groupedByInitial = lawnStrs.Objects[0].ObjData.LocStringValues
.GroupBy(pair => char.ToUpper(pair.Key[0] ) )
.ToDictionary(strGroup => strGroup.Key, strGroup => strGroup.ToList() );

string fileExt = LawnStringsHelper.GetExtension(config.OutputFormat);

foreach(var strGroup in groupedByInitial)
{
string filePath = Path.Combine(outputDir, strGroup.Key + fileExt);

LawnStringsMap fragment = new(strGroup.Value.ToDictionary(a => a.Key, a => a.Value) );
LawnStringsHelper.SaveMap(filePath, fragment, encoding, config.OutputFormat);	
}

}

// Merge Files into a bigger one

public static void MergeFiles(string inputDir, LawnStringsConvertInfo config = null)
{

if(!Directory.Exists(inputDir) )
throw new DirectoryNotFoundException($"Directory \"{inputDir}\" does not Exist.");

config ??= new();

var encoding = EncodeHelper.GetEncodingType(config.EncodingForPlainText);
LawnStringsMap mergedStrs = new();
    
string inputExt = LawnStringsHelper.GetExtension(config.InputFormat);
var files = Directory.GetFiles(inputDir, "*" + inputExt);

foreach(var filePath in files)
{
LawnStringsMap fragment = LawnStringsHelper.GetMap(filePath, true, encoding, config.InputFormat);

foreach (var pair in fragment.Objects[0].ObjData.LocStringValues)
{

if(!mergedStrs.Objects[0].ObjData.LocStringValues.ContainsKey(pair.Key) )
mergedStrs.Objects[0].ObjData.LocStringValues.Add(pair.Key, pair.Value);
            
}

}

var outputPath = inputDir.Contains("Split") ? 
inputDir.Replace("Split", "Merge") + LawnStringsHelper.GetExtension(config.OutputFormat) : 
LawnStringsHelper.BuildPath(inputDir, config.OutputFormat, "Merge");

PathHelper.CheckDuplicatedPath(ref outputPath);
 
LawnStringsHelper.SaveMap(outputPath, mergedStrs, encoding, config.OutputFormat);
}

}

}
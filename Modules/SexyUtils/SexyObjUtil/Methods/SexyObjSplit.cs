using System.IO;
using System.Linq;

namespace ZCore.Modules.SexyUtils.SexyObjUtil.Methods
{
/// <summary> Split a SexyObjTable into Smaller files or Merge them later. </summary>

public static class SexyObjSplit
{
// Get Path to Split Obj

private static string GetObjName(SexyObj fragment, bool isBaseObj, bool isUniqueObj)
{
var typeName = SexyObjHelper.HasTypeName(fragment) ? $"_{SexyObjHelper.GetTypeName(fragment)}" : string.Empty;
var aliasSuffix = fragment.Aliases?.FirstOrDefault() != null ? $"_{fragment.Aliases[0]}" : typeName;

return isBaseObj ? aliasSuffix.Trim('_') :
(isUniqueObj ? fragment.ObjClass : fragment.ObjClass + aliasSuffix);

}

/// <summary> Split SexyObjTables into Smaller files named by their ObjClass and Aliases (if they had some). </summary>

public static void SplitFile(string inputPath)
{
SexyObjTable sourceTable = new SexyObjTable().ReadObject(inputPath);

sourceTable.CheckObjs();

if(sourceTable.Objects.Count <= 1) 
return;

string outputDir = inputPath;
PathHelper.ChangeExtension(ref outputDir, ".split_obj");

DirManager.CheckMissingFolder(outputDir);

bool isBaseObj = sourceTable.Objects.GroupBy(obj => obj.ObjClass).Any(group => group.Count() > 1);

foreach(var fragment in sourceTable.Objects)
{
bool isUniqueObj = sourceTable.Objects.Count(obj => obj.ObjClass == fragment.ObjClass) == 1;

string objName = GetObjName(fragment, isBaseObj, isUniqueObj);
string filePath = Path.Combine(outputDir, objName + ".json");

fragment.WriteObject(filePath);
}

}

// Merge Files into a bigger one

public static void MergeFiles(string inputDir)
{

if(!Directory.Exists(inputDir) )
throw new DirectoryNotFoundException($"Directory \"{inputDir}\" does not Exist.");

SexyObj baseObj = new();
SexyObjTable mergedObjs = new(baseObj.ReadObjects(inputDir) );

var outputPath = inputDir.Contains("Split") ? inputDir.Replace("Split", "Merge") + ".json" : 
SexyObjHelper.BuildPath(inputDir, "Merge");

PathHelper.CheckDuplicatedPath(ref outputPath);
 
mergedObjs.WriteObject(outputPath);
}

}

}
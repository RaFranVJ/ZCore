using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ZCore.Modules.SexyUtils.SexyObjUtil
{
/// <summary> Allows some useful Tasks for SexyObjTables. </summary>

public static class SexyObjHelper
{
// Get new Path for SexyTable

public static string GetDefaultPath(string name)
{
string inputPath = LibInfo.CurrentDllDirectory + Path.DirectorySeparatorChar + name + ".json";

PathHelper.CheckDuplicatedPath(ref inputPath);

return inputPath;
}

// BuildPath

public static string BuildPath(string sourcePath, string suffix)
{
string baseDir = Path.GetDirectoryName(sourcePath);
string fileName = Path.GetFileNameWithoutExtension(sourcePath);

string outputPath = baseDir + Path.DirectorySeparatorChar + fileName + $"_{suffix}" + ".json";

PathHelper.CheckDuplicatedPath(ref outputPath);

return outputPath;
}

public static string GetFirstAliasOrDefault(IEnumerable<string> aliases)
{
return aliases != null ? aliases.FirstOrDefault() : string.Empty;
}

public static int GetAliasesLength(IEnumerable<string> aliases)
{
return aliases != null ? string.Join(", ", aliases).Length : 0;
}

// Check if ObjData has 'TypeName' field

public static bool HasTypeName(SexyObj obj)
{
return ( (IDictionary<string, object>)obj.ObjData).ContainsKey("TypeName");
}

public static string GetTypeName(SexyObj obj)
{
return ( (IDictionary<string, object>)obj.ObjData)["TypeName"].ToString();
}

}

}
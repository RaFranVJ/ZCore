using System.Collections.Generic;
using System.Linq;

namespace ZCore.Modules.SexyUtils.SexyObjUtil.Methods
{
/// <summary> Normalizes a SexyObjTable by removing Duplicates. </summary>

public static class SexyObjNormalizer
{
// Remove Duplicated Objs

public static void RemoveDuplicates(ref SexyObjTable sourceTable)
{
sourceTable.CheckObjs();

if(sourceTable.Objects.Count == 0) 
return;

Dictionary<string, SexyObj> uniqueObjs = new();

foreach(var obj in sourceTable.Objects)
{
var typeName = SexyObjHelper.HasTypeName(obj) ? $"_{SexyObjHelper.GetTypeName(obj)}" : string.Empty;
var aliases = obj.Aliases != null && obj.Aliases.Count != 0 ? $"_{obj.Aliases[0]}" : typeName;

string key = $"{obj.ObjClass}{aliases}".TrimEnd();

if(!uniqueObjs.ContainsKey(key) )
uniqueObjs[key] = obj;

}

sourceTable.Objects = uniqueObjs.Values.ToList();
}

// RemoveDuplicates from File

public static void RemoveDuplicates(string inputPath, string outputPath)
{
SexyObjTable sourceTable = new SexyObjTable().ReadObject(inputPath);

RemoveDuplicates(ref sourceTable);

sourceTable.WriteObject(outputPath);
}

}

}
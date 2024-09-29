using System;
using System.Collections.Generic;
using System.Linq;
using ZCore.Serializables.ArgumentsInfo.SexyUtils.ObjTable.Sorter;

namespace ZCore.Modules.SexyUtils.SexyObjUtil.Methods
{
/// <summary> Sorts a SexyObjTable. </summary>

public static class SexyObjSorter
{
// Sort Table Ascending

public static void OrderByAscending(ref SexyObjTable sourceTable, StringComparer comparer, SexyObjSortCriteria criteria, 
bool sortProperties)
{
sourceTable.CheckObjs();

if(sourceTable.Objects.Count == 0)
return;

var sortedObjs = criteria switch
{
SexyObjSortCriteria.SortByAliases => sourceTable.Objects.OrderBy(a => SexyObjHelper.GetFirstAliasOrDefault(a.Aliases), comparer),

SexyObjSortCriteria.SortByType => sourceTable.Objects.OrderBy(b => b.ObjClass, comparer)
.ThenBy(c => SexyObjHelper.GetFirstAliasOrDefault(c.Aliases), comparer),

_ => sourceTable.Objects.OrderBy(d => d.ObjClass, comparer)
};

if(sortProperties)
{

foreach(var obj in sortedObjs)
{
var dict = obj.ObjData as IDictionary<string, object>;
var sortedDict = dict.OrderBy(p => p.Key, comparer).ToDictionary(p => p.Key, p => p.Value);

obj.ObjData = ExpandObjPlugin.ToExpandoObject(sortedDict);   
}

}

sourceTable.Objects = sortedObjs.ToList();
}

// Sort Table Descending

public static void OrderByDescending(ref SexyObjTable sourceTable, StringComparer comparer, SexyObjSortCriteria criteria,
bool sortProperties)
{
sourceTable.CheckObjs();

if(sourceTable.Objects.Count == 0)
return;

var sortedObjs = criteria switch
{
SexyObjSortCriteria.SortByAliases => sourceTable.Objects
.OrderByDescending(a => SexyObjHelper.GetFirstAliasOrDefault(a.Aliases), comparer),

SexyObjSortCriteria.SortByType => sourceTable.Objects.OrderByDescending(b => b.ObjClass, comparer)
.ThenByDescending(c => SexyObjHelper.GetFirstAliasOrDefault(c.Aliases), comparer),

_ => sourceTable.Objects.OrderByDescending(d => d.ObjClass, comparer)
};

if(sortProperties)
{

foreach(var obj in sortedObjs)
{
var dict = obj.ObjData as IDictionary<string, object>;

var sortedDict = dict.OrderByDescending(p => p.Key, comparer)
.ToDictionary(p => p.Key, p => p.Value);

obj.ObjData = ExpandObjPlugin.ToExpandoObject(sortedDict);   
}

}

sourceTable.Objects = sortedObjs.ToList();
}

// Sort by Smaller Length

public static void OrderBySmallerLength(ref SexyObjTable sourceTable, SexyObjSortCriteria criteria, bool sortProperties)
{
sourceTable.CheckObjs();

if(sourceTable.Objects.Count == 0)
return;

var sortedObjs = criteria switch
{
SexyObjSortCriteria.SortByAliases => sourceTable.Objects.OrderBy(a => SexyObjHelper.GetAliasesLength(a.Aliases) ),

SexyObjSortCriteria.SortByType => sourceTable.Objects.OrderBy(b => b.ObjClass.Length)
.ThenBy(c => SexyObjHelper.GetAliasesLength(c.Aliases) ),

_ => sourceTable.Objects.OrderBy(d => d.ObjClass.Length)
};

if(sortProperties)
{

foreach(var obj in sortedObjs)
{
var dict = obj.ObjData as IDictionary<string, object>;

var sortedDict = dict.OrderBy(p => p.Key.Length)
.ToDictionary(p => p.Key, p => p.Value);

obj.ObjData = ExpandObjPlugin.ToExpandoObject(sortedDict);   
}

}

sourceTable.Objects = sortedObjs.ToList();
}

// Sort by Bigger Length

public static void OrderByBiggerLength(ref SexyObjTable sourceTable, SexyObjSortCriteria criteria, bool sortProperties)
{
sourceTable.CheckObjs();

if(sourceTable.Objects.Count == 0)
return;

var sortedObjs = criteria switch
{
SexyObjSortCriteria.SortByAliases => sourceTable.Objects.OrderByDescending(a => SexyObjHelper.GetAliasesLength(a.Aliases) ),

SexyObjSortCriteria.SortByType => sourceTable.Objects.OrderByDescending(b => b.ObjClass.Length)
.ThenByDescending(c => SexyObjHelper.GetAliasesLength(c.Aliases) ),

_ => sourceTable.Objects.OrderByDescending(d => d.ObjClass.Length)
};

if(sortProperties)
{

foreach(var obj in sortedObjs)
{
var dict = obj.ObjData as IDictionary<string, object>;

var sortedDict = dict.OrderByDescending(p => p.Key.Length)
.ToDictionary(p => p.Key, p => p.Value);

obj.ObjData = ExpandObjPlugin.ToExpandoObject(sortedDict);   
}

}

sourceTable.Objects = sortedObjs.ToList();
}

// Sort by Ascending Length

public static void OrderByAscendingLength(ref SexyObjTable sourceTable, StringComparer comparer,
SexyObjSortCriteria criteria, bool sortProperties)
{
sourceTable.CheckObjs();

if(sourceTable.Objects.Count == 0)
return;

var sortedObjs = criteria switch
{
SexyObjSortCriteria.SortByAliases => sourceTable.Objects.OrderBy(a1 => SexyObjHelper.GetFirstAliasOrDefault(a1.Aliases), comparer)
.ThenBy(a2 => SexyObjHelper.GetAliasesLength(a2.Aliases) ),

SexyObjSortCriteria.SortByType => sourceTable.Objects.OrderBy(b1 => SexyObjHelper.GetAliasesLength(b1.Aliases) )
.ThenBy(b2 => b2.ObjClass, comparer)
.ThenBy(b3 => b3.ObjClass.Length),
	
_ => sourceTable.Objects.OrderBy(c1 => c1.ObjClass, comparer)
.ThenBy(c2 => c2.ObjClass.Length)
};

if(sortProperties)
{

foreach(var obj in sortedObjs)
{
var dict = obj.ObjData as IDictionary<string, object>;

var sortedDict = dict.OrderBy(p => p.Key, comparer)
.ThenBy(p => p.Key.Length)
.ToDictionary(p => p.Key, p => p.Value);

obj.ObjData = ExpandObjPlugin.ToExpandoObject(sortedDict);   
}

}

sourceTable.Objects = sortedObjs.ToList();
}

// Sort by Descending Length

public static void OrderByDescendingLength(ref SexyObjTable sourceTable, StringComparer comparer,
SexyObjSortCriteria criteria, bool sortProperties)
{
sourceTable.CheckObjs();

if(sourceTable.Objects.Count == 0)
return;

var sortedObjs = criteria switch
{
SexyObjSortCriteria.SortByAliases => sourceTable.Objects
.OrderByDescending(a1 => SexyObjHelper.GetFirstAliasOrDefault(a1.Aliases), comparer)
.ThenByDescending(a2 => SexyObjHelper.GetAliasesLength(a2.Aliases) ),

SexyObjSortCriteria.SortByType => sourceTable.Objects.OrderByDescending(b1 => SexyObjHelper.GetAliasesLength(b1.Aliases) )
.ThenByDescending(b2 => b2.ObjClass, comparer)
.ThenByDescending(b3 => b3.ObjClass.Length),

_ => sourceTable.Objects.OrderByDescending(c1 => c1.ObjClass, comparer)
.ThenByDescending(c2 => c2.ObjClass.Length)
};

if(sortProperties)
{

foreach(var obj in sortedObjs)
{
var dict = obj.ObjData as IDictionary<string, object>;

var sortedDict = dict.OrderByDescending(p => p.Key, comparer)
.ThenByDescending(p => p.Key.Length)
.ToDictionary(p => p.Key, p => p.Value);

obj.ObjData = ExpandObjPlugin.ToExpandoObject(sortedDict);   
}

}

sourceTable.Objects = sortedObjs.ToList();
}
	
// Sort Obj Table

public static void SortTable(ref SexyObjTable sourceTable, SexyTableSortInfo sortInfo = null)
{
sortInfo ??= new();

var classComparer = StrComparerPlugin.GetStringComparer(sortInfo.CaseHandling);

switch(sortInfo.SortPattern)
{
case StringSortPattern.OrderByDescending:
OrderByDescending(ref sourceTable, classComparer, sortInfo.SortCriteria, sortInfo.SortPropsInObjData);
break;

case StringSortPattern.OrderBySmallerLength:
OrderBySmallerLength(ref sourceTable, sortInfo.SortCriteria, sortInfo.SortPropsInObjData);
break;

case StringSortPattern.OrderByBiggerLength:
OrderByBiggerLength(ref sourceTable, sortInfo.SortCriteria, sortInfo.SortPropsInObjData);
break;

case StringSortPattern.OrderByAscendingLength:
OrderByAscendingLength(ref sourceTable, classComparer, sortInfo.SortCriteria, sortInfo.SortPropsInObjData);
break;

case StringSortPattern.OrderByDescendingLength:
OrderByDescendingLength(ref sourceTable, classComparer, sortInfo.SortCriteria, sortInfo.SortPropsInObjData);
break;

default:
OrderByAscending(ref sourceTable, classComparer, sortInfo.SortCriteria, sortInfo.SortPropsInObjData);
break;
}

}

// Sort a JSON File that is a SexyObjTable

public static void SortFile(string inputPath, SexyTableSortInfo sortInfo = null)
{
SexyObjTable sourceTable = new SexyObjTable().ReadObject(inputPath);
SortTable(ref sourceTable, sortInfo);

string outputPath = SexyObjHelper.BuildPath(inputPath, "Sorted");
sourceTable.WriteObject(outputPath);
}

}

}
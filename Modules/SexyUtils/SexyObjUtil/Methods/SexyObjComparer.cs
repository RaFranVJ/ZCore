using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using ZCore.Serializables.ArgumentsInfo.SexyUtils.ObjTable;
using ZCore.Serializables.ArgumentsInfo.SexyUtils.ObjTable.Comparer;
using ZCore.Serializables.ArgumentsInfo.SexyUtils.ObjTable.Sorter;

namespace ZCore.Modules.SexyUtils.SexyObjUtil.Methods
{
/// <summary> Compares two different SexyObjTables. </summary>

public static class SexyObjComparer
{
// Compare Objs by TypeName

private static bool CompareByTypeName(dynamic oldObj, dynamic newObj)
{

if( ( (IDictionary<string, object>)oldObj.ObjData).ContainsKey("TypeName") &&
( (IDictionary<string, object>)newObj.ObjData).ContainsKey("TypeName"))  
return oldObj.ObjData.TypeName == newObj.ObjData.TypeName;

return false;
}   	

// Get new Objects between two Tables

public static SexyObjTable FindAddedObjs(SexyObjTable oldTable, SexyObjTable newTable, bool sortTable,
SexyTableSortInfo sortInfo = null)
{
oldTable.CheckObjs();
newTable.CheckObjs();

if(newTable.Objects.Count == 0)
return new();

var addedObjs = newTable.Objects.Where(newObj => !oldTable.Objects.Any(oldObj =>
oldObj.ObjClass == newObj.ObjClass && (oldObj.Aliases != null && newObj.Aliases != null ?
oldObj.Aliases.SequenceEqual(newObj.Aliases) : oldObj.Aliases == newObj.Aliases && 
CompareByTypeName(oldObj, newObj) ) ) ).ToList();

SexyObjTable diffTable = new(oldTable.Comment, oldTable.Version, addedObjs);

if(sortTable)
SexyObjSorter.SortTable(ref diffTable, sortInfo);

return diffTable;
}

// Compare two ObjTables

private static List<SexyObj> CompareObjects(SexyObjTable x, SexyObjTable y, SexyObjDiffCriteria diffCriteria)
{
List<SexyObj> changedObjs = new();
ExpandoObject differences = new();

foreach(var oldObj in x.Objects)
{
var newObj = y.Objects.FirstOrDefault(o => o.ObjClass == oldObj.ObjClass && 
(o.Aliases != null && oldObj.Aliases != null ? o.Aliases.SequenceEqual(oldObj.Aliases) :
o.Aliases == oldObj.Aliases) && CompareByTypeName(o, oldObj) );

newObj ??= new();

bool getChangedProps = diffCriteria.HasFlag(SexyObjDiffCriteria.FindChangesInObjData);
bool getNewProps = diffCriteria.HasFlag(SexyObjDiffCriteria.FindAddedPropsInObjData);

List<string> aliasDiff = oldObj.Aliases;

if(diffCriteria.HasFlag(SexyObjDiffCriteria.FindAliasChanges) && newObj.Aliases != null)
aliasDiff = newObj.Aliases.Except(oldObj.Aliases ?? new() ).ToList();

string commentDiff = oldObj.Comment;

if(diffCriteria.HasFlag(SexyObjDiffCriteria.FindChangedComments) )
commentDiff = newObj.Comment;

oldObj.ObjData.Compare(newObj.ObjData, getChangedProps, getNewProps, out var propsDiff);

if(propsDiff.Any() )
changedObjs.Add( new(commentDiff, aliasDiff, oldObj.ObjClass, propsDiff) );

}

return changedObjs;
}

// Get changed Objects between two Tables

public static SexyObjTable FindChangedObjs(SexyObjTable oldTable, SexyObjTable newTable,
bool checkForRecentChanges, SexyObjDiffCriteria diffCriteria, bool sortTable,
SexyTableSortInfo sortInfo = null)
{
oldTable.CheckObjs();
newTable.CheckObjs();

List<SexyObj> changedObjs;

if(checkForRecentChanges && newTable.Version >= oldTable.Version)
{

if(newTable.Objects.Count == 0)
return new();

changedObjs = CompareObjects(oldTable, newTable, diffCriteria);
}

else
{

if(oldTable.Objects.Count == 0)
return new();

changedObjs = CompareObjects(newTable, oldTable, diffCriteria);
}

SexyObjTable diffTable = new();

diffTable.Objects = changedObjs;

if(sortTable)
SexyObjSorter.SortTable(ref diffTable, sortInfo);

return diffTable;
}

// Get diff between two SexyObjTables

public static SexyObjTable FindTableDiff(SexyObjTable oldTable, SexyObjTable newTable, 
bool checkForRecentChanges, SexyObjDiffCriteria diffCriteria, bool sortTable, 
SexyTableSortInfo sortInfo = null)
{
var diffTable = FindChangedObjs(oldTable, newTable, checkForRecentChanges, diffCriteria, false);
var addedObjs = FindAddedObjs(oldTable, newTable, false);

foreach(var obj in addedObjs.Objects)
diffTable.Objects.Add(obj);

if(sortTable)
SexyObjSorter.SortTable(ref diffTable, sortInfo);

return diffTable;
}

// Compare two SexyObjTables as JSON

public static void CompareFiles(SexyTableCompareMode compareMode, bool sortTable, SexyObjDiffCriteria diffCriteria,
string filePathX, string filePathY, SexyTableSortInfo sortInfo = null)
{
SexyObjTable tableX = new SexyObjTable().ReadObject(filePathX);
SexyObjTable tableY = new SexyObjTable().ReadObject(filePathY);

SexyObjTable oldTable = null;
SexyObjTable newTable;

switch(compareMode)
{
case SexyTableCompareMode.FindChangedObjs:
oldTable = FindChangedObjs(tableX, tableY, false, diffCriteria, sortTable, sortInfo);

newTable = FindChangedObjs(tableX, tableY, true, diffCriteria, sortTable, sortInfo);
break;

case SexyTableCompareMode.FindTableDiff:
oldTable = FindTableDiff(tableX, tableY, false, diffCriteria, sortTable, sortInfo);

newTable = FindTableDiff(tableX, tableY, true, diffCriteria, sortTable, sortInfo);
break;

default:
newTable = FindAddedObjs(tableX, tableY, sortTable, sortInfo);
break;
}

// Write Changes from Old File

if(oldTable != null)
{
string oldPath = SexyObjHelper.BuildPath(filePathX, "Before");

oldTable.WriteObject(oldPath);
}

var suffix = compareMode <= SexyTableCompareMode.FindAddedObjs ? "NewObjs" : "After";
string newPath = SexyObjHelper.BuildPath(filePathY, suffix);

newTable.WriteObject(newPath);
}

}

}
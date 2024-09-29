using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using ZCore.Serializables.ArgumentsInfo.SexyUtils.ObjTable.Comparer;
using ZCore.Serializables.ArgumentsInfo.SexyUtils.ObjTable.Sorter;

namespace ZCore.Modules.SexyUtils.SexyObjUtil.Methods
{
/// <summary> Updates a SexyObjTable with a new One. </summary>

public static class SexyObjUpdater
{
// Add Missing Props to Obj

private static void AddMissingProps(ExpandoObject existingData, ExpandoObject newData)
{

if(existingData == null || newData == null)
return;

var existingDict = existingData as IDictionary<string, object>;
var newDict = newData as IDictionary<string, object>;

foreach(var prop in newDict)
{

if(!existingDict.ContainsKey(prop.Key) )
existingDict[prop.Key] = prop.Value;
        
}

}


// Update SexyObjTable instance

public static void UpdateTable(ref SexyObjTable oldTable, SexyObjTable newTable, bool sortTable,
SexyTableSortInfo sortInfo = null)
{
oldTable ??= new();

var diffTable = SexyObjComparer.FindTableDiff(oldTable, newTable, true, 
SexyObjDiffCriteria.FindAddedPropsInObjData, false);

foreach(var newObj in diffTable.Objects)
{
var existingObj = oldTable.Objects.FirstOrDefault(o => o.ObjClass == newObj.ObjClass && 
(o.Aliases != null && newObj.Aliases != null ? o.Aliases.SequenceEqual(newObj.Aliases) : 
o.Aliases == newObj.Aliases) );

if(existingObj != null)
AddMissingProps(existingObj.ObjData, newObj.ObjData);

else
oldTable.Objects.Add(newObj);

}

if(sortTable)
SexyObjSorter.SortTable(ref oldTable, sortInfo);

}

// Updates SexyObjTable File

public static void UpdateFile(bool sortTable, string filePathX, string filePathY,
SexyTableSortInfo sortInfo = null)
{
SexyObjTable tableX = new SexyObjTable().ReadObject(filePathX);
SexyObjTable tableY = new SexyObjTable().ReadObject(filePathY);

UpdateTable(ref tableX, tableY, sortTable, sortInfo);
string outputPath = SexyObjHelper.BuildPath(filePathX, "Updated");

tableX.WriteObject(outputPath);
}

}

}
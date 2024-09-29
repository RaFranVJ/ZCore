using System;
using System.Collections.Generic;
using System.Linq;
using ZCore.Modules.TextProcessor.LawnStrings.Definitions.Map;
using ZCore.Serializables.ArgumentsInfo.TextProcessor.LawnStrings;
using ZCore.Serializables.ArgumentsInfo.TextProcessor.LawnStrings.Comparer;
using ZCore.Serializables.ArgumentsInfo.TextProcessor.LawnStrings.Converter;

namespace ZCore.Modules.TextProcessor.LawnStrings.Methods
{
/// <summary> Allows Comparing two different LawnStrings. </summary>

public static class LawnStringsComparer
{
// Get new Strings between two LawnStrings

public static LawnStringsMap FindAddedStrings(LawnStringsMap oldStrs, LawnStringsMap newStrs, bool sortStrs,
LawnStringsSortInfo sortInfo = null)
{
oldStrs.CheckObjs();
newStrs.CheckObjs();

if(newStrs.Objects[0].ObjData.LocStringValues.Count == 0)
return new();

LawnStringsMap lawnStrDiff = new();

var addedStrs = newStrs.Objects[0].ObjData.LocStringValues
.Where(a => !oldStrs.Objects[0].ObjData.LocStringValues.ContainsKey(a.Key) );

lawnStrDiff.Objects[0].ObjData.LocStringValues = addedStrs.ToDictionary(b => b.Key, b => b.Value);

if(sortStrs)
LawnStringsSorter.SortStrings(ref lawnStrDiff, sortInfo);

return lawnStrDiff;
}

// Get changed Strings between two LawnStrings

public static LawnStringsMap FindChangedStrings(LawnStringsMap oldStrs, LawnStringsMap newStrs, 
bool checkForRecentChanges, bool sortStrs, LawnStringsSortInfo sortInfo = null)
{
oldStrs.CheckObjs();
newStrs.CheckObjs();

LawnStringsMap lawnStrDiff = new();
IEnumerable<KeyValuePair<string, string>> changedStrs;

if(checkForRecentChanges)
{

if(newStrs.Objects[0].ObjData.LocStringValues.Count == 0)
return new();

changedStrs = oldStrs.Objects[0].ObjData.LocStringValues
.Where(a => newStrs.Objects[0].ObjData.LocStringValues.ContainsKey(a.Key) && 
newStrs.Objects[0].ObjData.LocStringValues[a.Key] != a.Value);
}

else
{

if(oldStrs.Objects[0].ObjData.LocStringValues.Count == 0)
return new();

changedStrs = newStrs.Objects[0].ObjData.LocStringValues.
Where(b => oldStrs.Objects[0].ObjData.LocStringValues.ContainsKey(b.Key) && 
oldStrs.Objects[0].ObjData.LocStringValues[b.Key] != b.Value);
}

Func<KeyValuePair<string, string>, string> valueSelector = checkForRecentChanges ? 
(a => newStrs.Objects[0].ObjData.LocStringValues[a.Key] ) :
(b => oldStrs.Objects[0].ObjData.LocStringValues[b.Key] );

lawnStrDiff.Objects[0].ObjData.LocStringValues = changedStrs.ToDictionary(a => a.Key, valueSelector);

if(sortStrs)
LawnStringsSorter.SortStrings(ref lawnStrDiff, sortInfo);

return lawnStrDiff;
}

// Get Strings diff between two LawnStrings

public static LawnStringsMap FindStringsDiff(LawnStringsMap oldStrs, LawnStringsMap newStrs, 
bool checkForRecentChanges, bool sortStrs, LawnStringsSortInfo sortInfo = null)
{
var lawnStrDiff = FindChangedStrings(oldStrs, newStrs, checkForRecentChanges, false);
var addedStrs = FindAddedStrings(oldStrs, newStrs, false);

foreach(var pair in addedStrs.Objects[0].ObjData.LocStringValues)
lawnStrDiff.Objects[0].ObjData.LocStringValues.Add(pair.Key, pair.Value);

if(sortStrs)
LawnStringsSorter.SortStrings(ref lawnStrDiff, sortInfo);

return lawnStrDiff;
}

// Compare LawnStrings (make a Conversion if needed for both Files)

public static void CompareFiles(LawnStringsCompareMode compareMode, LawnStringsFormat outputFormat, bool sortStrs,
LawnStringsFileInfo fileInfoX = null, LawnStringsFileInfo fileInfoY = null, LawnStringsSortInfo sortInfo = null)
{
fileInfoX ??= new();
fileInfoY ??= new(LawnStringsFormat.RtonList);

var encodingX = EncodeHelper.GetEncodingType(fileInfoX.EncodingForPlainText);
LawnStringsMap lawnStrsX = LawnStringsHelper.GetMap(fileInfoX.FilePath, true, encodingX, fileInfoX.InputFormat);

var encodingY = EncodeHelper.GetEncodingType(fileInfoY.EncodingForPlainText);
LawnStringsMap lawnStrsY = LawnStringsHelper.GetMap(fileInfoY.FilePath, true, encodingY, fileInfoY.InputFormat);

LawnStringsMap oldStrs = null;
LawnStringsMap newStrs = null;

switch(compareMode)
{
case LawnStringsCompareMode.FindChangedStrings:
oldStrs = FindChangedStrings(lawnStrsX, lawnStrsY, false, sortStrs, sortInfo);

newStrs = FindChangedStrings(lawnStrsX, lawnStrsY, true, sortStrs, sortInfo);
break;

case LawnStringsCompareMode.FindAllDifferences:
oldStrs = FindStringsDiff(lawnStrsX, lawnStrsY, false, sortStrs, sortInfo);

newStrs = FindStringsDiff(lawnStrsX, lawnStrsY, true, sortStrs, sortInfo);
break;

default:
newStrs = FindAddedStrings(lawnStrsX, lawnStrsY, sortStrs, sortInfo);
break;
}

// Write Changes from Old File

if(oldStrs != null)
{
string oldPath = LawnStringsHelper.BuildPath(fileInfoX.FilePath, outputFormat, "Old");

LawnStringsHelper.SaveMap(oldPath, oldStrs, encodingX, outputFormat);
}

string newPath = LawnStringsHelper.BuildPath(fileInfoY.FilePath, outputFormat, "New");

LawnStringsHelper.SaveMap(newPath, newStrs, encodingY, outputFormat);
}

}

}
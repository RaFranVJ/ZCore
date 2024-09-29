using System;
using System.Linq;
using ZCore.Modules.TextProcessor.LawnStrings.Definitions.Map;
using ZCore.Serializables.ArgumentsInfo.TextProcessor.LawnStrings;
using ZCore.Serializables.ArgumentsInfo.TextProcessor.LawnStrings.Converter;

namespace ZCore.Modules.TextProcessor.LawnStrings.Methods
{
/// <summary> Allows Sorting the LawnStrings by a SortPattern. </summary>

public static class LawnStringsSorter
{
// Sort Strings from A to Z

public static void OrderByAscending(ref LawnStringsMap targetStrs, StringComparer strComparer)
{
targetStrs.CheckObjs();

if(targetStrs.Objects[0].ObjData.LocStringValues.Count == 0)
return;

var sortedStrs = targetStrs.Objects[0].ObjData.LocStringValues.OrderBy(a => a.Key, strComparer);

targetStrs.Objects[0].ObjData.LocStringValues = sortedStrs.ToDictionary(b => b.Key, b => b.Value);
}

// Sort Strings from Z to A

public static void OrderByDescending(ref LawnStringsMap targetStrs, StringComparer strComparer)
{
targetStrs.CheckObjs();

if(targetStrs.Objects[0].ObjData.LocStringValues.Count == 0)
return;

var sortedStrs = targetStrs.Objects[0].ObjData.LocStringValues.OrderByDescending(a => a.Key, strComparer);

targetStrs.Objects[0].ObjData.LocStringValues = sortedStrs.ToDictionary(b => b.Key, b => b.Value);
}

// Sort Strings by Smaller Length

public static void OrderBySmallerLength(ref LawnStringsMap targetStrs)
{
targetStrs.CheckObjs();

if(targetStrs.Objects[0].ObjData.LocStringValues.Count == 0)
return;

var sortedStrs = targetStrs.Objects[0].ObjData.LocStringValues.OrderBy(a => a.Value.Length);

targetStrs.Objects[0].ObjData.LocStringValues = sortedStrs.ToDictionary(b => b.Key, b => b.Value);
}

// Sort Strings by Bigger Length

public static void OrderByBiggerLength(ref LawnStringsMap targetStrs)
{
targetStrs.CheckObjs();

if(targetStrs.Objects[0].ObjData.LocStringValues.Count == 0)
return;

var sortedStrs = targetStrs.Objects[0].ObjData.LocStringValues.OrderByDescending(a => a.Value.Length);

targetStrs.Objects[0].ObjData.LocStringValues = sortedStrs.ToDictionary(b => b.Key, b => b.Value);
}

// Sort Strings from A to Z, then by Smaller Length

public static void OrderByAscendingLength(ref LawnStringsMap targetStrs, StringComparer keyComparer)
{
targetStrs.CheckObjs();

if(targetStrs.Objects[0].ObjData.LocStringValues.Count == 0)
return;

var sortedStrs = targetStrs.Objects[0].ObjData.LocStringValues.OrderBy(a => a.Key, keyComparer)
.ThenBy(b => b.Value.Length);

targetStrs.Objects[0].ObjData.LocStringValues = sortedStrs.ToDictionary(c => c.Key, c => c.Value);
}

// Sort Strings from Z to A, then by Bigger Length

public static void OrderByDescendingLength(ref LawnStringsMap targetStrs, StringComparer keyComparer)
{
targetStrs.CheckObjs();

if(targetStrs.Objects[0].ObjData.LocStringValues.Count == 0)
return;

var sortedStrs = targetStrs.Objects[0].ObjData.LocStringValues.OrderByDescending(a => a.Key, keyComparer)
.ThenByDescending(b => b.Value.Length);

targetStrs.Objects[0].ObjData.LocStringValues = sortedStrs.ToDictionary(c => c.Key, c => c.Value);
}

// Sort LawnStrings from Obj

public static void SortStrings(ref LawnStringsMap targetStrs, LawnStringsSortInfo sortInfo = null)
{
sortInfo ??= new();

var keyComparer = StrComparerPlugin.GetStringComparer(sortInfo.CaseHandling);

switch(sortInfo.SortPattern)
{
case StringSortPattern.OrderByDescending:
OrderByDescending(ref targetStrs, keyComparer);
break;

case StringSortPattern.OrderBySmallerLength:
OrderBySmallerLength(ref targetStrs);
break;

case StringSortPattern.OrderByBiggerLength:
OrderByBiggerLength(ref targetStrs);
break;

case StringSortPattern.OrderByAscendingLength:
OrderByAscendingLength(ref targetStrs, keyComparer);
break;

case StringSortPattern.OrderByDescendingLength:
OrderByDescendingLength(ref targetStrs, keyComparer);
break;

default:
OrderByAscending(ref targetStrs, keyComparer);
break;
}

}

// Sort a LawnStrings File (make a Conversion if needed)

public static void SortFile(string inputPath, LawnStringsConvertInfo convertInfo = null,
LawnStringsSortInfo sortInfo = null)
{
convertInfo ??= new();
sortInfo ??= new();

var encoding = EncodeHelper.GetEncodingType(convertInfo.EncodingForPlainText);
var lawnStrs = LawnStringsHelper.GetMap(inputPath, convertInfo.IgnoreDuplicatedKeys, encoding, convertInfo.InputFormat);

SortStrings(ref lawnStrs, sortInfo);
string outputPath = LawnStringsHelper.BuildPath(inputPath, convertInfo.OutputFormat, "Sorted");

LawnStringsHelper.SaveMap(outputPath, lawnStrs, encoding, convertInfo.OutputFormat);
}

}

}
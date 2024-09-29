using ZCore.Modules.TextProcessor.LawnStrings.Definitions.Map;
using ZCore.Serializables.ArgumentsInfo.TextProcessor.LawnStrings;
using ZCore.Serializables.ArgumentsInfo.TextProcessor.LawnStrings.Converter;

namespace ZCore.Modules.TextProcessor.LawnStrings.Methods
{
/// <summary> Allows Updating a LawnStrings File and then, making a Conversion if necessary. </summary>

public static class LawnStringsUpdater
{
// Update LawnStrings instance

public static void UpdateStrings(ref LawnStringsMap oldStrs, LawnStringsMap newStrs,
bool sortStrs, LawnStringsSortInfo sortInfo = null)
{
oldStrs ??= new();

var lawnStrDiff = LawnStringsComparer.FindAddedStrings(oldStrs, newStrs, false);

foreach(var pair in lawnStrDiff.Objects[0].ObjData.LocStringValues)
oldStrs.Objects[0].ObjData.LocStringValues.Add(pair.Key, pair.Value);

if(sortStrs)
LawnStringsSorter.SortStrings(ref oldStrs, sortInfo);

}

// Updates a LawnStrings File

public static void UpdateFile(LawnStringsFormat outputFormat, bool sortStrs, LawnStringsFileInfo fileInfoX = null,
LawnStringsFileInfo fileInfoY = null, LawnStringsSortInfo sortInfo = null)
{
fileInfoX ??= new();
fileInfoY ??= new(LawnStringsFormat.RtonList);

var encodingX = EncodeHelper.GetEncodingType(fileInfoX.EncodingForPlainText);
LawnStringsMap lawnStrsX = LawnStringsHelper.GetMap(fileInfoX.FilePath, true, encodingX, fileInfoX.InputFormat);

var encodingY = EncodeHelper.GetEncodingType(fileInfoY.EncodingForPlainText);
LawnStringsMap lawnStrsY = LawnStringsHelper.GetMap(fileInfoY.FilePath, true, encodingY, fileInfoY.InputFormat);

UpdateStrings(ref lawnStrsX, lawnStrsY, sortStrs, sortInfo);
string outputPath = LawnStringsHelper.BuildPath(fileInfoX.FilePath, outputFormat, "Updated");

LawnStringsHelper.SaveMap(outputPath, lawnStrsX, encodingX, outputFormat);
}

}

}
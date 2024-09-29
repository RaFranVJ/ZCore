using ZCore.Modules.TextProcessor.LawnStrings.Definitions.Map;
using ZCore.Serializables.ArgumentsInfo.TextProcessor.LawnStrings;
using ZCore.Serializables.ArgumentsInfo.TextProcessor.LawnStrings.Converter;

namespace ZCore.Modules.TextProcessor.LawnStrings.Methods
{
/// <summary> Allows Removing Duplicated Keys in a LawnStrings File </summary>

public static class LawnStringsNormalizer
{
// RemoveDuplicates from Files and make a Conversion if needed

public static void RemoveDuplicates(string inputPath, string outputPath, LawnStringsConvertInfo config = null)
{
var encoding = EncodeHelper.GetEncodingType(config.EncodingForPlainText);
LawnStringsMap lawnStrs = LawnStringsHelper.GetMap(inputPath, true, encoding, config.InputFormat);

lawnStrs.CheckObjs();

if(lawnStrs.Objects[0].ObjData.LocStringValues.Count == 0)
return;

config ??= new();

LawnStringsHelper.SaveMap(outputPath, lawnStrs, encoding, config.OutputFormat);	
}

}

}
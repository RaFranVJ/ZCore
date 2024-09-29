using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ZCore.Modules.TextProcessor.LawnStrings.Definitions.Map
{
/** <summary> Represents a Set of Strings used in some PvZ Games. </summary>

<remarks> This is a Variation that uses a Map of Strings. </remarks> */

public class LawnStringsMap : SexyObjTable<LawnStringsJsonMap<Dictionary<string, string>>>
{
/// <summary> Creates a new Instance of the <c>LawnStrings_Map</c>. </summary>

public LawnStringsMap()
{
Objects ??= new();

Objects.Add( new() );

Objects[0].ObjData = new()
{
LocStringValues = new()
};

}

/// <summary> Creates a new Instance of the <c>LawnStrings_Map</c>. </summary>

public LawnStringsMap(Dictionary<string, string> map)
{
Objects ??= new();

Objects.Add( new() );

Objects[0].ObjData = new()
{
LocStringValues = map ?? new()
};

}

/// <summary> Creates a new Instance of the <c>LawnStrings_Map</c>. </summary>

public LawnStringsMap(Stream sourceStream, bool ignoreDuplicatedKeys, Encoding encoding = null)
{
Objects ??= new();

Objects.Add( new() );

Objects[0].ObjData = new()
{
LocStringValues = LawnStringsHelper.GetMapFromPlainText(sourceStream, ignoreDuplicatedKeys, encoding)
};

}

// Check for null Fields

public override void CheckObjs()
{
Objects ??= new();

if(Objects.Count == 0)
Objects.Add( new() );

Objects[0] ??= new();
Objects[0].ObjData ??= new();

Objects[0].ObjData.LocStringValues ??= new();
}

/** <summary> Converts this Instance into PlainText. </summary>

<returns> A String that Represents the Sequence of Data in the LawnStrings. </returns> */

public void WriteAsPlainText(Stream targetStream, Encoding encoding = null)
{
CheckObjs();

encoding ??= Encoding.UTF8;

using StreamWriter textWriter = new(targetStream, encoding);

foreach(var key in Objects[0].ObjData.LocStringValues.Keys)
{
textWriter.WriteLine($"[{key}]");

textWriter.WriteLine($"{Objects[0].ObjData.LocStringValues[key].Replace("\\n", "\n").Replace("\r", string.Empty)}\n");
}

}

public void WriteAsPlainText(string outputPath, Encoding encoding = null)
{
using FileStream outputFile = File.OpenWrite(outputPath);

WriteAsPlainText(outputFile, encoding);
}

/** <summary> Converts this Instance into a List of Strings. </summary>

<returns> The LawnStrings converted as a List of Strings */

public LawnStrings ToList()
{
CheckObjs();

LawnStrings stringsList = new();

foreach(var pair in Objects[0].ObjData.LocStringValues)
{
stringsList.Objects[0].ObjData.LocStringValues.Add(pair.Key);
stringsList.Objects[0].ObjData.LocStringValues.Add(pair.Value);
}

return stringsList;
}

}

}
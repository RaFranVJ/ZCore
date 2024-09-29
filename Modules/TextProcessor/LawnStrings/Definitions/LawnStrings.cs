using System.Collections.Generic;
using System.IO;
using System.Text;
using ZCore.Modules.TextProcessor.LawnStrings.Definitions.Map;

namespace ZCore.Modules.TextProcessor.LawnStrings.Definitions
{
/// <summary> Represents a Set of Strings used in some PvZ Games. </summary>

public class LawnStrings : SexyObjTable<LawnStringsJsonData<List<string>>>
{
/// <summary> Creates a new Instance of the <c>LawnStrings</c>. </summary>

public LawnStrings()
{
Objects ??= new();

Objects.Add( new() );

Objects[0].ObjData = new()
{
LocStringValues = new()
};

}

/// <summary> Creates a new Instance of the <c>LawnStrings</c>. </summary>

public LawnStrings(Stream sourceStream, bool ignoreDuplicatedKeys, Encoding encoding = null)
{
Objects ??= new();

Objects.Add( new() );

Objects[0].ObjData = new()
{
LocStringValues = LawnStringsHelper.GetListFromPlainText(sourceStream, ignoreDuplicatedKeys, encoding)
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

<returns> A String that Represents the Sequence of Data in the LawnStrings */

public void WriteAsPlainText(Stream targetStream, bool ignoreDuplicatedKeys, Encoding encoding = null)
{
CheckObjs();

encoding ??= Encoding.UTF8;

using StreamWriter textWriter = new(targetStream, encoding);
HashSet<string> processedKeys = new();

for(int i = 0; i < Objects[0].ObjData.LocStringValues.Count; i += 2)
{

if(i + 1 >= Objects[0].ObjData.LocStringValues.Count)
break;

string key = Objects[0].ObjData.LocStringValues[i];

if(ignoreDuplicatedKeys)
{
	
if(processedKeys.Contains(key) )
continue;

processedKeys.Add(key);
}

textWriter.WriteLine($"[{key}]");

textWriter.WriteLine($"{Objects[0].ObjData.LocStringValues[i + 1].Replace("\\n", "\n").Replace("\r", string.Empty)}\n");
}

}

public void WriteAsPlainText(string outputPath, bool ignoreDuplicatedKeys, Encoding encoding = null)
{
using FileStream outputFile = File.OpenWrite(outputPath);

WriteAsPlainText(outputFile, ignoreDuplicatedKeys, encoding);
}

/** <summary> Converts this Instance into a Dictionary of Strings. </summary>

<returns> The LawnStrings converted as a Dictionary of Strings */

public LawnStringsMap ToMap(bool ignoreDuplicatedKeys)
{
CheckObjs();

LawnStringsMap mappedStrings = new();
HashSet<string> processedKeys = new();

for(int i = 0; i < Objects[0].ObjData.LocStringValues.Count - 1; i += 2)
{
string key = Objects[0].ObjData.LocStringValues[i];

if(ignoreDuplicatedKeys)
{

if(processedKeys.Contains(key) )
continue;

processedKeys.Add(key);
}

else
{
int suffixCount = 1;
string originalKey = key;

while(mappedStrings.Objects[0].ObjData.LocStringValues.ContainsKey(key) )
{
key = $"{originalKey}_{suffixCount}";
suffixCount++;
}

}

string value = Objects[0].ObjData.LocStringValues[i + 1];

mappedStrings.Objects[0].ObjData.LocStringValues.Add(key, value);
}

return mappedStrings;
}

}

}
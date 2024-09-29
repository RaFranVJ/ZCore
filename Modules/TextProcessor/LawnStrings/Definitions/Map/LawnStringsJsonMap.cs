using System.Collections.Generic;

namespace ZCore.Modules.TextProcessor.LawnStrings.Definitions.Map
{
/// <summary> Represents some Data for the LawnStrings File as Json. </summary>

public class LawnStringsJsonMap<T> : SexyObj<LawnStringsMapData<T>> where T : IEnumerable<KeyValuePair<string, string>>
{
/// <summary> Creates a new Instance of the <c>LawnStringsJsonData</c>. </summary>

public LawnStringsJsonMap()
{
ObjClass = "LawnStringsData";
ObjData = new();

Aliases.Add(ObjClass);
}

/// <summary> Creates a new Instance of the <c>LawnStringsJsonData</c>. </summary>

public LawnStringsJsonMap(T strs)
{
ObjClass = "LawnStringsData";
ObjData = new(strs);

Aliases.Add(ObjClass);
}

}

}
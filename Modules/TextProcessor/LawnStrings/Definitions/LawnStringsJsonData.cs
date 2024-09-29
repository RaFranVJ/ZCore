using System.Collections.Generic;

namespace ZCore.Modules.TextProcessor.LawnStrings.Definitions
{
/// <summary> Represents some Data for the LawnStrings File as JSON. </summary>

public class LawnStringsJsonData<T> : SexyObj<LawnStringsData<T>> where T : IEnumerable<string>
{
/// <summary> Creates a new Instance of the <c>LawnStringsJsonData</c>. </summary>

public LawnStringsJsonData()
{
ObjClass = "LawnStringsData";
ObjData = new();

Aliases.Add(ObjClass);
}

/// <summary> Creates a new Instance of the <c>LawnStringsJsonData</c>. </summary>

public LawnStringsJsonData(T strs)
{
ObjClass = "LawnStringsData";
ObjData = new(strs);

Aliases.Add(ObjClass);
}

}

}
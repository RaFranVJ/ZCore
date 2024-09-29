using System.Collections.Generic;

namespace ZCore.Modules.TextProcessor.LawnStrings.Definitions.Map
{
/// <summary> Represents some Data for the LawnStrings File. </summary>

public class LawnStringsMapData<T> where T : IEnumerable<KeyValuePair<string, string>>
{
/** <summary> Gets or Sets the Strings Located by Language. </summary>
<returns> The Strings. </returns> */

public T LocStringValues{ get; set; }

/// <summary> Creates a new Instance of the <c>LawnStringsData</c>. </summary>

public LawnStringsMapData()
{
}

/// <summary> Creates a new Instance of the <c>LawnStringsData</c>. </summary>

public LawnStringsMapData(T strs)
{
LocStringValues = strs;
}

}

}
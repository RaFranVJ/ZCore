using System.Collections.Generic;

namespace ZCore.Modules.TextProcessor.LawnStrings.Definitions
{
/// <summary> Represents some Data for the LawnStrings File. </summary>

public class LawnStringsData<T> where T : IEnumerable<string>
{
/** <summary> Gets or Sets the Strings Located by Language. </summary>
<returns> The Strings. </returns> */

public T LocStringValues{ get; set; }

/// <summary> Creates a new Instance of the <c>LawnStringsData</c>. </summary>

public LawnStringsData()
{
}

/// <summary> Creates a new Instance of the <c>LawnStringsData</c>. </summary>

public LawnStringsData(T strs)
{
LocStringValues = strs;
}

}

}
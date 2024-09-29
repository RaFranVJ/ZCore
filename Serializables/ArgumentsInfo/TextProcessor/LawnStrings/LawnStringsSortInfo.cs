namespace ZCore.Serializables.ArgumentsInfo.TextProcessor.LawnStrings
{
/// <summary> Groups Info related to the LawnStrings Sorter. </summary>

public class LawnStringsSortInfo : ParamGroupInfo
{
/** <summary> Gets or Sets the SortPattern for LawnStrings. </summary>
<returns> The SortPattern. </returns> */

public StringSortPattern SortPattern{ get; set; }

/** <summary> Gets or Sets the CaseHandling for the IDS of the LawnStrings. </summary>
<returns> The CaseHandling for Keys. </returns> */

public StringCaseHandling CaseHandling{ get; set; }

/// <summary> Creates a new Instance of the <c>LawnStringsSortInfo</c>. </summary>

public LawnStringsSortInfo()
{
CaseHandling = StringCaseHandling.Ordinal;
}

}

}
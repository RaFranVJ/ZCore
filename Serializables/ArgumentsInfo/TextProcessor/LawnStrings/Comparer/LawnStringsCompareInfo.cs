using ZCore.Serializables.ArgumentsInfo.TextProcessor.LawnStrings.Converter;

namespace ZCore.Serializables.ArgumentsInfo.TextProcessor.LawnStrings.Comparer
{
/// <summary> Groups Info related to the LawnStrings Comparer. </summary>

public class LawnStringsCompareInfo : ParamGroupInfo
{
/** <summary> Gets or Sets the ComparissonMode for LawnStrings. </summary>
<returns> The ComparissonMode. </returns> */

public LawnStringsCompareMode ComparissonMode{ get; set; }

/** <summary> Gets or Sets some Config for old LawnStrings. </summary>
<returns> The LawnStrings Config. </returns> */

public LawnStringsFileInfo ConfigForOldFiles{ get; set; }

/** <summary> Gets or Sets some Config for new LawnStrings. </summary>
<returns> The LawnStrings Config. </returns> */

public LawnStringsFileInfo ConfigForNewFiles{ get; set; }

/** <summary> Gets or Sets the Output Format for LawnStrings. </summary>
<returns> The OutputFormat. </returns> */

public LawnStringsFormat OutputFormat{ get; set; }

/** <summary> Gets or Sets a Boolean that determines if Generated Str should be Sorted </summary>
<returns> true or false. </returns> */

public bool SortStringsGeneratedOnComparisson{ get; set; }

/// <summary> Creates a new Instance of the <c>LawnStringsCompareInfo</c>. </summary>

public LawnStringsCompareInfo()
{
ConfigForOldFiles = new();
ConfigForNewFiles = new(LawnStringsFormat.RtonList);

OutputFormat = LawnStringsFormat.JsonList;
}

/// <summary> Checks each nullable Field of this Instance and Validates it, in case it's <c>null</c>. </summary>

public override void CheckForNullFields()
{
LawnStringsCompareInfo defaultInfo = new();

ConfigForOldFiles ??= defaultInfo.ConfigForOldFiles;
ConfigForNewFiles ??= defaultInfo.ConfigForNewFiles;

ConfigForOldFiles.CheckForNullFields();
ConfigForNewFiles.CheckForNullFields();
}

}

}
using ZCore.Serializables.ArgumentsInfo.TextProcessor.LawnStrings.Comparer;
using ZCore.Serializables.ArgumentsInfo.TextProcessor.LawnStrings.Converter;

namespace ZCore.Serializables.ArgumentsInfo.TextProcessor.LawnStrings
{
/// <summary> Groups options related to the LawnStrings Processor. </summary>

public class LawnStringsSettings : ParamGroupInfo
{
/** <summary> Gets or Sets some Options for Converting the LawnStrings File. </summary>
<returns> The Converter Params. </returns> */

public LawnStringsConvertInfo ConverterParams{ get; set; }

/** <summary> Gets or Sets some Options for Sorting Strings. </summary>
<returns> The Sorter Params. </returns> */

public LawnStringsSortInfo SorterParams{ get; set; }

/** <summary> Gets or Sets some Options for Comparing Strings. </summary>
<returns> The Comparer Params. </returns> */

public LawnStringsCompareInfo ComparerParams{ get; set; }

/** <summary> Gets or Sets some Options for Updating Strings. </summary>
<returns> The Updater Params. </returns> */

public LawnStringsUpdateInfo UpdaterParams{ get; set; }

/// <summary> Creates a new Instance of the <c>LawnStringsSettings</c>. </summary>

public LawnStringsSettings()
{
ConverterParams = new();
SorterParams = new();

ComparerParams = new();
UpdaterParams = new();
}

///<summary> Checks each nullable Field of this Instance and Validates it, in case it's <c>null</c>. </summary>

public override void CheckForNullFields()
{
LawnStringsSettings defaultOptions = new();

#region ======== Set default Values to Null Fields ========

ConverterParams ??= defaultOptions.ConverterParams;
SorterParams ??= defaultOptions.SorterParams;
ComparerParams ??= defaultOptions.ComparerParams;
UpdaterParams ??= defaultOptions.UpdaterParams;

#endregion

ConverterParams.CheckForNullFields();
SorterParams.CheckForNullFields();
ComparerParams.CheckForNullFields();
UpdaterParams.CheckForNullFields();
}

}

}
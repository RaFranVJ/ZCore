using ZCore.Serializables.ArgumentsInfo.TextProcessor.LawnStrings.Converter;

namespace ZCore.Serializables.ArgumentsInfo.TextProcessor.LawnStrings
{
/// <summary> Groups Info related to the LawnStrings Updater. </summary>

public class LawnStringsUpdateInfo : ParamGroupInfo
{
/** <summary> Gets or Sets some Config for old LawnStrings. </summary>
<returns> The LawnStrings Config. </returns> */

public LawnStringsFileInfo ConfigForOldFiles{ get; set; }

/** <summary> Gets or Sets some Config for new LawnStrings. </summary>
<returns> The LawnStrings Config. </returns> */

public LawnStringsFileInfo ConfigForNewFiles{ get; set; }

/** <summary> Gets or Sets the Output Format for LawnStrings. </summary>
<returns> The OutputFormat. </returns> */

public LawnStringsFormat OutputFormat{ get; set; }

/** <summary> Gets or Sets a Boolean that determines if Str should be Sorted on Update </summary>
<returns> true or false. </returns> */

public bool SortStringsWhenUpdating{ get; set; }

/// <summary> Creates a new Instance of the <c>LawnStringsUpdateInfo</c>. </summary>

public LawnStringsUpdateInfo()
{
ConfigForOldFiles = new();
ConfigForNewFiles = new(LawnStringsFormat.RtonList);

OutputFormat = LawnStringsFormat.JsonList;
}

/// <summary> Checks each nullable Field of this Instance and Validates it, in case it's <c>null</c>. </summary>

public override void CheckForNullFields()
{
LawnStringsUpdateInfo defaultInfo = new();

ConfigForOldFiles ??= defaultInfo.ConfigForOldFiles;
ConfigForNewFiles ??= defaultInfo.ConfigForNewFiles;

ConfigForOldFiles.CheckForNullFields();
ConfigForNewFiles.CheckForNullFields();
}

}

}
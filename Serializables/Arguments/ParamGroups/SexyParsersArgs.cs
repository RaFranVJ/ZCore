using ZCore.Serializables.ArgumentsInfo.SexyParser;
using ZCore.Serializables.ArgumentsInfo.SexyParser.Rton;

namespace ZCore.Serializables.Arguments.ParamGroups
{
/// <summary> The Arguments used in the <c>SexyParsers</c> Tasks. </summary>

public class SexyParsersArgs : ArgumentsSet
{
/** <summary> Gets or Sets the Info related to the RTON Parser. </summary>
<returns> The RTON Parse Info. </returns> */

public RtonSettings RtonConfig{ get; set; }

/** <summary> Gets or Sets the Info related to the CFW2 Parser. </summary>
<returns> The CFW2 Parse Info. </returns> */

public Cfw2Settings Cfw2Config{ get; set; }

// Add fields

/// <summary> Creates a new Instance of the <c>SexyParsersArgs</c>. </summary>

public SexyParsersArgs()
{
RtonConfig = new();
Cfw2Config = new();
}

/// <summary> Checks each nullable Field of the <c>SexyParsersArgs</c> and Validates it, in case it's <c>null</c>. </summary>

public override void CheckForNullFields()
{
SexyParsersArgs defaultArgs = new();

#region ======== Set default Values to Null Fields ========

InputPath ??= defaultArgs.InputPath;
OutputPath ??= defaultArgs.OutputPath;
RtonConfig ??= defaultArgs.RtonConfig;
Cfw2Config ??= defaultArgs.Cfw2Config;

#endregion

RtonConfig.CheckForNullFields();
Cfw2Config.CheckForNullFields();

PathHelper.CheckExistingPath(InputPath, true);
PathHelper.CheckExistingPath(OutputPath, false);
}

}

}
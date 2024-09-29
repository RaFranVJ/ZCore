using ZCore.Serializables.ArgumentsInfo.Parser.Base64;
using ZCore.Serializables.ArgumentsInfo.Parser.Json;

namespace ZCore.Serializables.Arguments.ParamGroups
{
/// <summary> The Arguments used in the <c>FileParsers</c> Tasks. </summary>

public class FileParsersArgs : ArgumentsSet
{
/** <summary> Gets or Sets the Info related to the Base64 Parser. </summary>
<returns> The Base64 Parse Info. </returns> */

public Base64Settings Base64Config{ get; set; }

/** <summary> Gets or Sets the Info related to the JSON Parser. </summary>
<returns> The JSON Parse Info. </returns> */

public JsonSettings JsonConfig{ get; set; }

// CSV Parser Fields

/// <summary> Creates a new Instance of the <c>FileParsersArgs</c>. </summary>

public FileParsersArgs()
{
Base64Config = new();
JsonConfig = new();
}

/// <summary> Checks each nullable Field of the <c>FileParsersArgs</c> instance and Validates it, in case it's <c>null</c>. </summary>

public override void CheckForNullFields()
{
FileParsersArgs defaultArgs = new();

#region ======== Set default Values to Null Fields ========

InputPath ??= defaultArgs.InputPath;
OutputPath ??= defaultArgs.OutputPath;
Base64Config ??= defaultArgs.Base64Config;
JsonConfig ??= defaultArgs.JsonConfig;

#endregion

Base64Config.CheckForNullFields();
JsonConfig.CheckForNullFields();

PathHelper.CheckExistingPath(InputPath, true);
PathHelper.CheckExistingPath(OutputPath, false);
}

}

}
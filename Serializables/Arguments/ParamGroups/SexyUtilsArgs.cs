

using ZCore.Serializables.ArgumentsInfo.SexyUtils.ObjTable;

namespace ZCore.Serializables.Arguments.ParamGroups
{
/// <summary> The Arguments used in the <c>SexyUtils</c> Tasks. </summary>

public class SexyUtilsArgs : ArgumentsSet
{
/** <summary> Gets or Sets the Info related to the ObjTables. </summary>
<returns> The ConfigForObjTables. </returns> */

public SettingsForObjTables ConfigForObjTables{ get; set; }

// Add fields

/// <summary> Creates a new Instance of the <c>SexyUtilsArgs</c>. </summary>

public SexyUtilsArgs()
{
ConfigForObjTables = new();
}

/// <summary> Checks each nullable Field of the <c>SexyUtilsArgs</c> and Validates it, in case it's <c>null</c>. </summary>

public override void CheckForNullFields()
{
SexyUtilsArgs defaultArgs = new();

#region ======== Set default Values to Null Fields ========

InputPath ??= defaultArgs.InputPath;
OutputPath ??= defaultArgs.OutputPath;
ConfigForObjTables ??= defaultArgs.ConfigForObjTables;

#endregion

PathHelper.CheckExistingPath(InputPath, true);
PathHelper.CheckExistingPath(OutputPath, false);
}

}

}
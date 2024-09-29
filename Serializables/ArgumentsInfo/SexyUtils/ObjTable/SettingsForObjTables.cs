using ZCore.Serializables.ArgumentsInfo.SexyUtils.ObjTable.Comparer;
using ZCore.Serializables.ArgumentsInfo.SexyUtils.ObjTable.Sorter;

namespace ZCore.Serializables.ArgumentsInfo.SexyUtils.ObjTable
{
/// <summary> Groups options related to the SexyObjTables Utils. </summary>

public class SettingsForObjTables : ParamGroupInfo
{
/** <summary> Gets or Sets some Options for Sorting Tables. </summary>
<returns> The Sorter Params. </returns> */

public SexyTableSortInfo SorterParams{ get; set; }

/** <summary> Gets or Sets some Options for Comparing Tables. </summary>
<returns> The Comparer Params. </returns> */

public SexyTableCompareInfo ComparerParams{ get; set; }

/** <summary> Gets or Sets some Options for Updating Tables. </summary>
<returns> The Updater Params. </returns> */

public SexyTableUpdateInfo UpdaterParams{ get; set; }

/// <summary> Creates a new Instance of the <c>SettingsForObjTables</c>. </summary>

public SettingsForObjTables()
{
SorterParams = new();
ComparerParams = new();

UpdaterParams = new();
}

/// <summary> Checks each nullable Field of this Instance and Validates it, in case it's <c>null</c>. </summary>

public override void CheckForNullFields()
{
SettingsForObjTables defaultOptions = new();

#region ======== Set default Values to Null Fields ========

SorterParams ??= defaultOptions.SorterParams;
ComparerParams ??= defaultOptions.ComparerParams;
UpdaterParams ??= defaultOptions.UpdaterParams;

#endregion

SorterParams.CheckForNullFields();
ComparerParams.CheckForNullFields();
UpdaterParams.CheckForNullFields();
}

}

}
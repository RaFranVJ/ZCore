using System.IO;
using ZCore.Modules.SexyUtils;
using ZCore.Modules.SexyUtils.SexyObjUtil;

namespace ZCore.Serializables.ArgumentsInfo.SexyUtils.ObjTable
{
/// <summary> Groups Info related to the SexyTable Updater. </summary>

public class SexyTableUpdateInfo : ParamGroupInfo
{
/** <summary> Gets or Sets a Path to the Old Table to Update. </summary>
<returns> A Path to the Old SexyObjTables File. </returns> */

public string PathToTargetFile{ get; set; }

/** <summary> Gets or Sets a Path to the New Table. </summary>
<returns> A Path to the New SexyObjTables File. </returns> */

public string PathToUpdatedFile{ get; set; }

/** <summary> Gets or Sets a Boolean that determines if Tables should be Sorted on Update </summary>
<returns> true or false. </returns> */

public bool SortTablesWhenUpdating{ get; set; }

/// <summary> Creates a new Instance of the <c>SexyTableUpdateInfo</c>. </summary>

public SexyTableUpdateInfo()
{
PathToTargetFile = SexyObjHelper.GetDefaultPath("TargetFile");
PathToUpdatedFile = SexyObjHelper.GetDefaultPath("UpdatedContent");
}

/// <summary> Checks each nullable Field of this Instance and Validates it, in case it's <c>null</c>. </summary>

public override void CheckForNullFields()
{
SexyTableUpdateInfo defaultInfo = new();

PathToTargetFile ??= defaultInfo.PathToTargetFile;
PathToUpdatedFile ??= defaultInfo.PathToUpdatedFile;
}

}

}
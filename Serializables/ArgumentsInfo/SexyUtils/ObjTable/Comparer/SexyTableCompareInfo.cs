using ZCore.Modules.SexyUtils.SexyObjUtil;

namespace ZCore.Serializables.ArgumentsInfo.SexyUtils.ObjTable.Comparer
{
/// <summary> Groups Info related to the SexyTable Comparer. </summary>

public class SexyTableCompareInfo : ParamGroupInfo
{
/** <summary> Gets or Sets a Path to the Old Table to Compare. </summary>
<returns> A Path to the Old SexyObjTables File. </returns> */

public string PathToOldTable{ get; set; }

/** <summary> Gets or Sets a Path to the New Table to Compare. </summary>
<returns> A Path to the New SexyObjTables File. </returns> */

public string PathToNewTable{ get; set; }

/** <summary> Gets or Sets the ComparissonMode for SexyObjTables. </summary>
<returns> The ComparissonMode. </returns> */

public SexyTableCompareMode ComparissonMode{ get; set; }

/** <summary> Gets or Sets the DiffCriteria for Objects in SexyObjTables. </summary>
<returns> The DiffCriteriaForObjs. </returns> */

public SexyObjDiffCriteria DiffCriteriaForObjs{ get; set; }

/** <summary> Gets or Sets a Boolean that determines if Generated Obj should be Sorted </summary>
<returns> true or false. </returns> */

public bool SortObjsGeneratedOnComparisson{ get; set; }

/// <summary> Creates a new Instance of the <c>SexyTableCompareInfo</c>. </summary>

public SexyTableCompareInfo()
{
PathToOldTable = SexyObjHelper.GetDefaultPath("OldFile");
PathToNewTable = SexyObjHelper.GetDefaultPath("NewFile");

DiffCriteriaForObjs = SexyObjDiffCriteria.DeepComparisson;
}

/// <summary> Checks each nullable Field of this Instance and Validates it, in case it's <c>null</c>. </summary>

public override void CheckForNullFields()
{
SexyTableCompareInfo defaultInfo = new();

PathToOldTable ??= defaultInfo.PathToOldTable;
PathToNewTable ??= defaultInfo.PathToNewTable;
}

}

}
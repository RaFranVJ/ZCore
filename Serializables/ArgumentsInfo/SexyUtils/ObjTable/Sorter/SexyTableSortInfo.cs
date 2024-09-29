namespace ZCore.Serializables.ArgumentsInfo.SexyUtils.ObjTable.Sorter
{
/// <summary> Groups Info related to the SexyObjs Sorter. </summary>

public class SexyTableSortInfo : ParamGroupInfo
{
/** <summary> Gets or Sets the SortPattern for Objs </summary>
<returns> The SortCriteria. </returns> */

public SexyObjSortCriteria SortCriteria{ get; set; }

/** <summary> Gets or Sets the SortPattern for ObjNames. </summary>
<returns> The SortPattern. </returns> */

public StringSortPattern SortPattern{ get; set; }

/** <summary> Gets or Sets the CaseHandling for ObjNames. </summary>
<returns> The CaseHandling for Keys. </returns> */

public StringCaseHandling CaseHandling{ get; set; }

/** <summary> Gets or Sets a Boolean that determines if Properties in ObjData should be Sorted </summary>
<returns> true or false. </returns> */

public bool SortPropsInObjData{ get; set; }

/// <summary> Creates a new Instance of the <c>SexyTableSortInfo</c>. </summary>

public SexyTableSortInfo()
{
SortCriteria = SexyObjSortCriteria.SortByType;
CaseHandling = StringCaseHandling.Ordinal;
}

}

}
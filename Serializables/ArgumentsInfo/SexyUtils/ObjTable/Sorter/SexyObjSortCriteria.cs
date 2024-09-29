using System;

namespace ZCore.Serializables.ArgumentsInfo.SexyUtils.ObjTable.Sorter
{
/// <summary> Determines how to Sort a SexyObj inside a Table. </summary>

[Flags]

public enum SexyObjSortCriteria
{
/// <summary> Default Criteria </summary>
Default,

/// <summary> Sort Objects by <c>objclass</c> </summary>
SortByClassName,

/// <summary> Sort Objects by <c>aliases</c> </summary>
SortByAliases,

/// <summary> Sort Object by <c>objclass</c> and by <c>aliases</c> </summary>
SortByType = SortByClassName | SortByAliases
}

}
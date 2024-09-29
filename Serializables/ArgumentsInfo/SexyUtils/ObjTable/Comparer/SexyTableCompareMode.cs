using System;

namespace ZCore.Serializables.ArgumentsInfo.SexyUtils.ObjTable
{
/// <summary> Determines how to Compare ObjTables. </summary>

[Flags]

public enum SexyTableCompareMode
{
Default,

/// <summary> New Objects will be Find between two SexyObjTables </summary>
FindAddedObjs,

/// <summary> Changed Objs will be Find between two SexyObjTables </summary>
FindChangedObjs,

/// <summary> Find all Differences between both SexyObjTables </summary>
FindTableDiff = FindAddedObjs | FindChangedObjs
}

}
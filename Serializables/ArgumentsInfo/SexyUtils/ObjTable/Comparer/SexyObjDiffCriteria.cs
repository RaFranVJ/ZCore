using System;

namespace ZCore.Serializables.ArgumentsInfo.SexyUtils.ObjTable.Comparer
{
/// <summary> Determines how to Find differences inside SexyObjects. </summary>

[Flags]

public enum SexyObjDiffCriteria
{
/// <summary> Default Criteria </summary>
Default,

/// <summary> Find Property Changes in <c>objdata</c> </summary>
FindChangesInObjData,

/// <summary> Find new Properties in <c>objdata</c> </summary>
FindAddedPropsInObjData,

/// <summary> Find Changes in <c>aliases</c> </summary>
FindAliasChanges = 4,

/// <summary> Find Changes in <c>#comment</c> </summary>
FindChangedComments = 8,

/// <summary> Both, Added Properties and Property Changes will be Obtained from <c>objdata</c> </summary>
CompareObjData = FindChangesInObjData | FindAddedPropsInObjData,

/// <summary> Compare Changes in <c>objdata</c> and in <c>aliases</c> </summary>
FindChangesInObjDataAndAlias = FindChangesInObjData | FindAliasChanges,

/// <summary> Compare Changes in <c>objdata</c> and in <c>#comment</c> </summary>
FindChangesInObjDataAndComment = FindChangesInObjData | FindChangedComments,

/// <summary> Get new Props in <c>objdata</c> and Changes in <c>aliases</c> </summary>
GetNewPropsAndAliasChanges = FindAddedPropsInObjData | FindAliasChanges,

/// <summary> Get new Props in <c>objdata</c> and Changes in <c>#comment</c> </summary>
GetNewPropsAndChangedComments = FindAddedPropsInObjData | FindChangedComments,

/// <summary> Compare <c>objdata</c> and <c>aliases</c> </summary>
CompareObjDataAndAliases = CompareObjData | FindAliasChanges,

/// <summary> Compare <c>objdata</c> and <c>#comment</c> </summary>
CompareObjDataAndComment = CompareObjData | FindChangedComments,

/// <summary> Compare Changes in <c>objdata</c> and in <c>#comment</c> </summary>
FindChangedAliasAndComment = FindAliasChanges | FindChangedComments,

/// <summary> Perform a Deep Comparisson in both Objects </summary>
DeepComparisson = CompareObjData | FindAliasChanges | FindChangedComments
}

}
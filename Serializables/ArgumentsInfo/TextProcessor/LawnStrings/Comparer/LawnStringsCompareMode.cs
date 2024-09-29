using System;

namespace ZCore.Serializables.ArgumentsInfo.TextProcessor.LawnStrings.Comparer
{
/// <summary> Represents how to Compare the LawnStrings File. </summary>

[Flags]

public enum LawnStringsCompareMode
{
Default,

/// <summary> New Strings will be Find between two LawnStrings </summary>
FindAddedStrings,

/// <summary> Changed Strings will be Find between two LawnStrings </summary>
FindChangedStrings,

/// <summary> Find all Differences between both LawnStrings </summary>
FindAllDifferences = FindAddedStrings | FindChangedStrings
}

}
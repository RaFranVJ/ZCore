namespace ZCore.Serializables.ArgumentsInfo.TextProcessor.LawnStrings.Converter
{
/// <summary> The Format of a LawnStrings File. </summary>

public enum LawnStringsFormat
{
/// <summary> LawnStrings shouls be Handled as PlainText </summary>
PlainText,

/// <summary> LawnStrings shouls be Handled as a List of Strings in JSON </summary>
JsonList,

/// <summary> LawnStrings shouls be Handled as a Dictionary of Strings in JSON </summary>
JsonMap,

/// <summary> LawnStrings shouls be Handled as a JsonList that later will be Parsed to RTON </summary>
RtonList,

/// <summary> LawnStrings shouls be Handled as a JsonMap that later will be Parsed to RTON </summary>
RtonMap
}

}
using System;

namespace ZCore.Serializables.ArgumentsInfo.TextProcessor.LawnStrings.Converter
{
/// <summary> Groups Info related to the LawnStrings Converter. </summary>

public class LawnStringsConvertInfo : ParamGroupInfo
{
/** <summary> Gets or Sets the InputFormat for LawnStrings. </summary>
<returns> The InputFormat. </returns> */

public LawnStringsFormat InputFormat{ get; set; }

/** <summary> Gets or Sets the OutputFormat for LawnStrings. </summary>
<returns> The OutputFormat. </returns> */

public LawnStringsFormat OutputFormat{ get; set; }

/** <summary> Gets or Sets the Encoding used for PlainText </summary>
<returns> The PlainText Encoding. </returns> */

public string EncodingForPlainText{ get; set; }

/** <summary> Gets or Sets a Boolean that Determines if duplicated Keys should be Ignored </summary>
<returns> true or false. </returns> */

public bool IgnoreDuplicatedKeys{ get; set; }

/// <summary> Creates a new Instance of the <c>LawnStringsConvertInfo</c>. </summary>

public LawnStringsConvertInfo()
{
InputFormat = LawnStringsFormat.RtonList;
EncodingForPlainText = Console.InputEncoding.ToString();

IgnoreDuplicatedKeys = true;
}

/// <summary> Creates a new Instance of the <c>LawnStringsConvertInfo</c>. </summary>

public LawnStringsConvertInfo(LawnStringsFormat inputFormat, LawnStringsFormat outputFormat)
{
InputFormat = inputFormat;
OutputFormat = outputFormat;

EncodingForPlainText = Console.InputEncoding.ToString();
IgnoreDuplicatedKeys = true;
}

/// <summary> Checks each nullable Field of this Instance and Validates it, in case it's <c>null</c>. </summary>

public override void CheckForNullFields()
{
LawnStringsConvertInfo defaultInfo = new();

EncodingForPlainText ??= defaultInfo.EncodingForPlainText;
}

}

}
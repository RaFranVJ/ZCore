using System;
using ZCore.Modules.TextProcessor.LawnStrings;
using ZCore.Serializables.ArgumentsInfo.TextProcessor.LawnStrings.Converter;

namespace ZCore.Serializables.ArgumentsInfo.TextProcessor.LawnStrings
{
/// <summary> Groups Info related to a LawnStrings that will be Compared. </summary>

public class LawnStringsFileInfo : ParamGroupInfo
{
/** <summary> Gets or Sets a Path to the LawnStrings File to Compare. </summary>
<returns> A Path to the LawnStrings File. </returns> */

public string FilePath{ get; set; }

/** <summary> Gets or Sets the Input Format for LawnStrings. </summary>
<returns> The InputFormat. </returns> */

public LawnStringsFormat InputFormat{ get; set; }

/** <summary> Gets or Sets the Encoding used for PlainText </summary>
<returns> The PlainText Encoding. </returns> */

public string EncodingForPlainText{ get; set; }

/// <summary> Creates a new Instance of the <c>LawnStringsFileInfo</c>. </summary>

public LawnStringsFileInfo()
{
FilePath = LawnStringsHelper.BuildPath(default);
EncodingForPlainText = Console.InputEncoding.ToString();
}

/// <summary> Creates a new Instance of the <c>LawnStringsFileInfo</c>. </summary>

public LawnStringsFileInfo(LawnStringsFormat format)
{
FilePath = LawnStringsHelper.BuildPath(format);
InputFormat = format;

EncodingForPlainText = Console.InputEncoding.ToString();
}

/// <summary> Checks each nullable Field of this Instance and Validates it, in case it's <c>null</c>. </summary>

public override void CheckForNullFields()
{
LawnStringsFileInfo defaultInfo = new();

FilePath ??= defaultInfo.FilePath;
EncodingForPlainText ??= defaultInfo.EncodingForPlainText;
}

}

}
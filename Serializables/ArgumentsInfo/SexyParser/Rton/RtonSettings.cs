using ZCore.Modules.SexyParsers.ReflectionObjectNotation.Parser;
using ZCore.Serializables.ArgumentsInfo.FileManager.Archive;

namespace ZCore.Serializables.ArgumentsInfo.SexyParser.Rton
{
/// <summary> Groups options related to the RTON Parser. </summary>

public class RtonSettings : ParamGroupInfo
{
/** <summary> Gets or Sets the Endian Order used when Parsing Files. </summary>
<returns> The Endian Order. </returns> */

public Endian Endianness{ get; set; }

/** <summary> Gets or Sets a Boolean that Determines if Reference Strings should be Used or not. </summary>
<returns> <b>true</b> if Reference Strings should be Use; otherwise, <b>false</b>. </returns> */

public bool UseReferenceStrings{ get; set; }

/** <summary> Gets or Sets some Options for Encoding RTON Files. </summary>
<returns> The Encoder Info. </returns> */

public RtonEncoderInfo EncoderInfo{ get; set; }

/** <summary> Gets or Sets some Options for Decoding RTON Files. </summary>
<returns> The Decoder Info. </returns> */

public RtonDecoderInfo DecoderInfo{ get; set; }

/** <summary> Gets or Sets some Options related of the RTON Parser (Default Version is 1). </summary>
<returns> The Parser Version. </returns> */

public FileVersionDetails<uint> ParserVersion{ get; set; }

/// <summary> Creates a new Instance of the <c>RtonOptions</c>. </summary>

public RtonSettings()
{
EncoderInfo = new();
DecoderInfo = new();

ParserVersion = new(RtonVersion.ExpectedVerNumber);
}

///<summary> Checks each nullable Field of the <c>RtonOptions</c> Instance and Validates it, in case it's <c>null</c>. </summary>

public override void CheckForNullFields()
{
RtonSettings defaultOptions = new();

#region ======== Set default Values to Null Fields ========

EncoderInfo ??= defaultOptions.EncoderInfo;
DecoderInfo ??= defaultOptions.DecoderInfo;
ParserVersion ??= defaultOptions.ParserVersion;

#endregion

EncoderInfo.CheckForNullFields();
DecoderInfo.CheckForNullFields();
}

}

}
using System;
using ZCore.Modules.SexyParsers.CharacterFontWidget2.Parser;
using ZCore.Serializables.ArgumentsInfo.FileManager.Archive;

namespace ZCore.Serializables.ArgumentsInfo.SexyParser
{
/// <summary> Groups options related to the CFW2 Parser. </summary>

public class Cfw2Settings : ParamGroupInfo
{
/** <summary> Gets or Sets the Endian Order used when Parsing Files. </summary>
<returns> The Endian Order. </returns> */

public Endian Endianness{ get; set; }

/** <summary> Gets or Sets some Options related of the Parser. </summary>
<returns> The Parser Version. </returns> */

public FileVersionDetails<Int128> ParserVersion{ get; set; }

/// <summary> Creates a new Instance of the <c>Cfw2Settings</c>. </summary>

public Cfw2Settings()
{
ParserVersion = new(Cfw2Version.ExpectedVerNumber);
}

///<summary> Checks each nullable Field of the <c>Cfw2Settings</c> Instance and Validates it, in case it's <c>null</c>. </summary>

public override void CheckForNullFields()
{
Cfw2Settings defaultOptions = new();

#region ======== Set default Values to Null Fields ========

ParserVersion ??= defaultOptions.ParserVersion;

#endregion
}

}

}
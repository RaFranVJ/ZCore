using System.IO;

namespace ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers
{
/// <summary> Groups some Base Settings used on the Image Parsers. </summary>

public class GenericImgSettings<T> : ParamGroupInfo where T : struct, System.Enum
{
/** <summary> Gets or Sets the Endian Order used when Parsing Files. </summary>
<returns> The Endian Order. </returns> */

public Endian Endianness{ get; set; }

/** <summary> Gets or Sets the Format used when Encoding Images. </summary>
<returns> The Image Format. </returns> */

public T ImageFormatForEncoding{ get; set; }

/** <summary> Gets or Sets some Params used for Handling PTX Info on Android. </summary>
<returns> The Info Handler for Android. </returns> */

public string PathToInfoContainer{ get; set; }

/// <summary> Creates a new Instance of the <c>GenericImgSettings</c>. </summary>

public GenericImgSettings()
{
PathToInfoContainer = GetBasePath();
}

/// <summary> Creates a new Instance of the <c>GenericImgSettings</c>. </summary>

public GenericImgSettings(T format)
{
ImageFormatForEncoding = format;
PathToInfoContainer = GetBasePath();
}

/** <summary> Gets the base Path to the Image Info basing on the CurrentAppDirectory. </summary>
<returns> The default Info Path. </returns> */

protected static string GetBasePath() => LibInfo.CurrentDllDirectory + Path.DirectorySeparatorChar + "PtxInfo" + Path.DirectorySeparatorChar;
}

}
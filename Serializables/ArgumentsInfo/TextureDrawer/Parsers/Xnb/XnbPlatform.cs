namespace ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.Xnb
{
/// <summary> Determines the Platform of a XNB File. </summary>

public enum XnbPlatform : byte
{
/// <summary> XNB File is a Resource from the Default Platform (Windows). </summary>
Default,

/** <summary> XNB File is a Resource exclusive from Windows Phone. </summary>
<remarks> Its identifier is 'm' as a Char. </remarks> */

WindowsPhone = 0x6D,

/** <summary> XNB File is a Resource exclusive from Windows. </summary>
<remarks> Its identifier is 'w' as a Char. </remarks> */

Windows = 0x77,

/** <summary> XNB File is a Resource exclusive from Xbox360. </summary>
<remarks> Its identifier is 'x' as a Char.</remarks> */

Xbox360 = 0x78
}

}
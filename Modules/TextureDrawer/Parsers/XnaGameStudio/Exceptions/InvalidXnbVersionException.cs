namespace ZCore.Modules.TextureDrawer.Parsers.XnaGameStudio.Exceptions
{
// Exception thrown when Invalid ver is Read

public class InvalidXnbVersionException(ushort ver, Limit<ushort> expected) : 
InvalidFileVersionException<ushort>(ver, $"Unknown XNB Version (v{ver}.0)\n" + 
$"Allowed Versions are: v{expected.MinValue}.0 - v{expected.MaxValue}.0", expected.MinValue, expected.MaxValue)
{
}

}
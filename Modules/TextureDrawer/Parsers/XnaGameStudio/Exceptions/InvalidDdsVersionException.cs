namespace ZCore.Modules.TextureDrawer.Parsers.XnaGameStudio.Exceptions
{
// Exception thrown when Invalid ver is Read

public class InvalidDdsVersionException(uint ver, uint expected) : 
InvalidFileVersionException<uint>(ver, $"Unknown DDS Version (v{ver}.0)\nAllowed Version is v{expected}", expected)
{
}

}
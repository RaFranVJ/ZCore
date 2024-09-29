namespace ZCore.Modules.TextureDrawer.Parsers.PackedTexture.PlayStation.PSV.Exceptions
{
// Exception thrown when Invalid ver is Read

public class Psv_InvalidPtxVersionException(uint ver, Limit<uint> expected) : 
InvalidFileVersionException<uint>(ver, $"Unknown PTX Version (v{ver})\n" + 
$"Allowed Versions are: v{expected.MinValue} - v{expected.MaxValue}", expected.MinValue, expected.MaxValue)
{
}

}
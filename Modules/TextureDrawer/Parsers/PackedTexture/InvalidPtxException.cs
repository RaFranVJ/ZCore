namespace ZCore.Modules.TextureDrawer.Parsers.PackedTexture
{
// Exception thrown when Invalid header is Read

public class InvalidPtxException(long pos, string header, string expected) : 
GenericValueMismatchException<string>(pos, header, expected, 
$"Invalid Ptx Header: \"{header}\" - Expected: \"{expected}\"")
{
}

}
namespace ZCore.Modules.TextureDrawer.Parsers.SexyTexture.Exceptions
{
// Exception thrown when Invalid header is Read

public class InvalidSexyTexException(long pos, string header, string expected) : 
GenericValueMismatchException<string>(pos, header, expected, 
$"Invalid SexyTex Header: \"{header}\" - Expected: \"{expected}\"")
{
}

}
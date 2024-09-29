namespace ZCore.Modules.TextureDrawer.Parsers.XnaGameStudio.Exceptions
{
// Exception thrown when Invalid header is Read

public class InvalidXnbException(long pos, string header, string expected) : 
GenericValueMismatchException<string>(pos, header, expected, 
$"Invalid Xnb Header: \"{header}\" - Expected: \"{expected}\"")
{
}

}
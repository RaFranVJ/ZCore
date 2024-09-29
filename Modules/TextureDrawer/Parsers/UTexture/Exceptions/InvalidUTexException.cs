namespace ZCore.Modules.TextureDrawer.Parsers.UTexture.Exceptions
{
// Exception thrown when Invalid header is Read

public class InvalidUTexException(long pos, ushort header, ushort expected) : 
GenericValueMismatchException<ushort>(pos, header, expected, 
$"Invalid UTex Magic: {header} - Expected: {expected}")
{
}

}
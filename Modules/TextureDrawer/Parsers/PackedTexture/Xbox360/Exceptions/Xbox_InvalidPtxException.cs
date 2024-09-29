namespace ZCore.Modules.TextureDrawer.Parsers.PackedTexture.Xbox360.Exceptions
{
// Exception thrown when Invalid header is Read

public class Xbox_InvalidPtxException(long pos, uint header, uint expected) : 
GenericValueMismatchException<uint>(pos, header, expected, 
$"Invalid Ptx Magic: {header}, found at {pos}.\nExpected: {expected}")
{
}

}
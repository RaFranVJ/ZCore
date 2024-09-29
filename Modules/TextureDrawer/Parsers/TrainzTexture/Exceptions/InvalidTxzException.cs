namespace ZCore.Modules.TextureDrawer.Parsers.TrainzTexture.Exceptions
{
// Exception thrown when Invalid header is Read

public class InvalidTxzException(long pos, ushort header, ushort expected) : 
GenericValueMismatchException<ushort>(pos, header, expected, 
$"Invalid Txz Magic: {header} - Expected: {expected}")
{
}

}
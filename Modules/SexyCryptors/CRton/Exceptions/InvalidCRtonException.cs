namespace ZCore.Modules.SexyCryptors.CRton.Exceptions
{
// Exception thrown when Invalid header is Read

public class InvalidCRtonException(long pos, ushort header, ushort expected) : 
GenericValueMismatchException<ushort>(pos, header, expected, 
$"Invalid C-Rton identifier: {header} - Expected: {expected}")
{
}

}
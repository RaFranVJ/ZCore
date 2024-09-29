namespace ZCore.Modules.SexyCompressors.ResourceStreamGroup.Exceptions
{
// Exception thrown when Invalid header is Read

public class InvalidRsgException(long pos, string header, string expected) : 
GenericValueMismatchException<string>(pos, header, expected, 
$"Invalid Rsg Header: \"{header}\" - Expected: \"{expected}\"")
{
}

}
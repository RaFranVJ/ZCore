namespace ZCore.Modules.SexyCompressors.MarmaladeDZ.Exceptions
{
// Exception thrown when Invalid header is Read

public class InvalidDzException(long pos, string header, string expected) : 
GenericValueMismatchException<string>(pos, header, expected, 
$"Invalid Dz Header: \"{header}\" - Expected: \"{expected}\"")
{
}

}
namespace ZCore.Modules.SexyCompressors.ResourceStreamBundle.Exceptions
{
// Exception thrown when Invalid header is Read

public class InvalidRsbException(long pos, string header, string expected) : 
GenericValueMismatchException<string>(pos, header, expected, 
$"Invalid Rsb Header: \"{header}\" - Expected: \"{expected}\"")
{
}

}
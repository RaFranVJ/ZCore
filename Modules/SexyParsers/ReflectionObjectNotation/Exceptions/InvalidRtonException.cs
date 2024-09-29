namespace ZCore.Modules.SexyParsers.ReflectionObjectNotation.Exceptions
{
// Exception thrown when Invalid header is Read

public class InvalidRtonException(long pos, string header, string expected) : 
GenericValueMismatchException<string>(pos, header, expected, 
$"Invalid Rton Header: \"{header}\" - Expected: \"{expected}\"")
{
}

}
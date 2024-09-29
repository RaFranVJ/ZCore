namespace ZCore.Modules.SexyCompressors.ArcVPackage.Exceptions
{
// Exception thrown when Invalid header is Read

public class InvalidArcvException(long pos, string header, string expected) : 
GenericValueMismatchException<string>(pos, header, expected, 
$"Invalid Arc-V Header: \"{header}\" - Expected: \"{expected}\"")
{
}

}
namespace ZCore.Modules.SexyCryptors.PopCapCipheredData.Exceptions
{
// Exception thrown when Invalid header is Read

public class InvalidCdatException(long pos, string header, string expected) : 
GenericValueMismatchException<string>(pos, header, expected, 
$"Invalid C-dat Header at {pos}.\nHeader read: \"{header}\" - Expected: \"{expected}\"")
{
}

}
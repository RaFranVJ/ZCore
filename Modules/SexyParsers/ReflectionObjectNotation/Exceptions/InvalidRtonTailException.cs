namespace ZCore.Modules.SexyParsers.ReflectionObjectNotation.Exceptions
{
// Exception thrown when Invalid tail is Read

public class InvalidRtonTailException(long pos, string tail, string expected) : 
GenericValueMismatchException<string>(pos, tail, expected, 
$"Invalid Rton Tail at: {pos}\nValue read: \"{tail}\" - Expected: \"{expected}\"")
{
}

}
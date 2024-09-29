namespace ZCore.Modules.SexyParsers.ReflectionObjectNotation.Exceptions
{
// Exception thrown when Invalid RtArray EndId is Read

public class RtArray_InvalidEndIdException(long pos, byte id, byte expected) : 
RtArray_InvalidStartIdException(pos, id, expected, false)
{
}

}
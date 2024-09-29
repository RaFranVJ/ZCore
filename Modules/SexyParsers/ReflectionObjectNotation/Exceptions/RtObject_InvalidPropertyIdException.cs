namespace ZCore.Modules.SexyParsers.ReflectionObjectNotation.Exceptions
{
// Exception thrown when Invalid Property for RtObj is Read

public class RtObject_InvalidPropertyIdException(long pos, RtTypeIdentifier id) : 
RtObject_InvalidValueIdException(pos, id, false)
{
}

}
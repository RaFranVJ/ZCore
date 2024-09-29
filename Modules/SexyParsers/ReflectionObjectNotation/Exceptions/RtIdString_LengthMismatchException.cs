using ZCore.Modules.SexyParsers.ReflectionObjectNotation.Definitions;

namespace ZCore.Modules.SexyParsers.ReflectionObjectNotation.Exceptions
{
// Exception thrown when Invalid RtIdString is Read

public class RtIdString_LengthMismatchException(long pos, RtIDType subType, string value, int expectedLen) : 
RtString_LengthMismatchException(pos, RtTypeIdentifier.IDString, value, expectedLen, subType)
{
public RtIDType SubType{ get; } = subType;
}

}
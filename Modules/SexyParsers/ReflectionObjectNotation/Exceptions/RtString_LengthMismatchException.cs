using System;
using ZCore.Modules.SexyParsers.ReflectionObjectNotation.Definitions;

namespace ZCore.Modules.SexyParsers.ReflectionObjectNotation.Exceptions
{
// Exception thrown when Invalid RtString is Read

public class RtString_LengthMismatchException(long pos, RtTypeIdentifier type, string value, int expectedLen, 
RtIDType? subType = null) : Exception(GetErrorMsg(type, value, expectedLen, pos, subType) )
{
public long Position{ get; } = pos;

public RtTypeIdentifier StringType{ get; } = type;

public string Value{ get; } = value;

public int ExpectedLength{ get; } = expectedLen;

// Get Error Msg

private static string GetErrorMsg(RtTypeIdentifier type, string value, int expectedLen, long pos, RtIDType? subType)
{
string hexType = ( (byte)type).ToString("X");

string strFlags = $"(\"{hexType}\" Type)";

if(subType != null)
{
string hexSubType = ( (byte)subType).ToString("X");

strFlags = $"(\"{hexType}\" Type - \"{hexSubType}\" SubType)";
}

return $"Invalid {type}: \"{value}\" {strFlags} at {pos}\n" +

$"Length: {value.Length} chars - Expected: {expectedLen} bytes).";
}

}

}
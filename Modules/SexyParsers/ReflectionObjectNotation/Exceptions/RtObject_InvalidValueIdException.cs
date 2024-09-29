using System;

namespace ZCore.Modules.SexyParsers.ReflectionObjectNotation.Exceptions
{
// Exception thrown when Invalid Value for RtObj is Read

public class RtObject_InvalidValueIdException(long pos, RtTypeIdentifier id, bool flags = true) : 
Exception(GetErrorMsg(pos, id, flags) )
{

public long Position{ get; } = pos;

public RtTypeIdentifier TypeIdentifier{ get; } = id;

// Get Error Msg

private static string GetErrorMsg(long pos, RtTypeIdentifier id, bool forValues)
{

string msg = forValues ? "Unknown Type Identifier \"0x{0:X2}\" at Position: {1}" :
"Unknown Type Identifier for RtObject (\"0x{0:X2}\") at: {1}, a PropertyName was Expected";

return string.Format(msg, pos, (byte)id);
}

}

}
namespace ZCore.Modules.SexyParsers.ReflectionObjectNotation.Exceptions
{
// Exception thrown when Invalid RtArray StartId is Read

public class RtArray_InvalidStartIdException(long pos, byte id, byte expected, bool flags = true) : 
GenericValueMismatchException<byte>(pos, id, expected, GetErrorMsg(pos, id, expected, flags) )
{
// Get Error Msg

private static string GetErrorMsg(long pos, byte id, byte expected, bool forEnd)
{
string idFlags = forEnd ? "End" : "Start";

string hexID = id.ToString("X");
string hexID_Expected = expected.ToString("X");

return $"Invalid {idFlags} Identifer for RtArray, found at: {pos}\n" + 
$"Value read: {hexID} - Expected: {hexID_Expected}";
}

}

}
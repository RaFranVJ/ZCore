using System;
using System.IO;

namespace ZCore.Modules.SexyCompressors.ResourceStreamGroup.Exceptions
{
// Exception thrown when PacketInfo for RSG is null

public class MissingPacketInfoException(string filePath) : Exception(GetErrorMsg(filePath) )
{

// Error MSG (should load it from LocStrings)

private static string GetErrorMsg(string filePath)
{
string fileName = string.IsNullOrEmpty(filePath) ? "RSG" : $"\"{Path.GetFileName(filePath)}\"";

return $"Missing PacketInfo for {fileName}";
}

}

}
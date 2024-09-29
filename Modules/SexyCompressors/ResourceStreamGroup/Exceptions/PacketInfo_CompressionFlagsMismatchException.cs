using System;

namespace ZCore.Modules.SexyCompressors.ResourceStreamGroup.Exceptions
{
// Exception thrown when flags does not Match in RSG and in Manifest

public class PacketInfo_CompressionFlagsMismatchException(uint flags, uint expected, string path) : 
Exception($"Invalid CompressionFlags for \"{path}\".\nFlags: {flags} (In Manifest) - Expected: {expected} (In ResGroup)")
{
}

}
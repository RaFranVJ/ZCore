using System;

namespace ZCore.Modules.SexyCompressors.ResourceStreamGroup.Exceptions
{
// Exception thrown when ver does not Match in RSG and in Manifest

public class PacketInfo_VersionMismatchException(uint ver, uint expected, string path) : 
Exception($"Invalid Version for \"{path}\".\nVersion detected: {ver} (In Manifest) - Expected: {expected} (In ResGroup)")
{
}

}
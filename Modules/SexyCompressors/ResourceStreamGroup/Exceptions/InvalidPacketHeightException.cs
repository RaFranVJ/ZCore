using System;

namespace ZCore.Modules.SexyCompressors.ResourceStreamGroup.Exceptions
{
// Exception thrown when PTX Height differs from Expected

public class InvalidPacketHeightException(int height, int expected, string path) : 
Exception($"Invalid Packet Height for \"{path}\".\nHeight: {height} (In ResGroup) - Expected: {expected} (In Manifest)")
{
}

}
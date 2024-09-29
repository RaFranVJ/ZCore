using System;

namespace ZCore.Modules.SexyCompressors.ResourceStreamGroup.Exceptions
{
// Exception thrown when Invalid CompositeName is Read

public class InvalidPacketWidthException(int width, int expected, string path) : 
Exception($"Invalid Packet Width for \"{path}\".\nWidth: {width} (In ResGroup) - Expected: {expected} (In Manifest)")
{
}

}
using System;

namespace ZCore.Modules.SexyCompressors.ResourceStreamBundle.Exceptions
{
// Exception thrown when Invalid GroupName is Read

public class InvalidGroupNameException(string name, string expected, uint index) : 
Exception($"Invalid GroupName: \"{name}\" at Packet #{index}, Expected: \"{expected}\"")
{
}

}